using FINALIMS.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;

namespace FINALIMS.Controllers
{
    public class UserController : Controller
    {
        //[ValidateAntiForgeryToken]
        //[Authorize(Roles = "Admin")]
        //Registration Action
        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }
        //Registration POST action 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration([Bind(Exclude = "IsEmailVerified,ActivatorCode")] user info)
        {
            bool Status = false;
            string message = "";
            //
            // Model Validation 
            if (ModelState.IsValid)
            {

                #region //Email already Exists 
                var isExist = IsEmailExist(info.Email);
                if (isExist)
                {
                    ModelState.AddModelError("EmailExist", "Email already exist");
                    return View(info);
                }
                #endregion

                #region Generate Activation Code 
                //var actcode = Guid.NewGuid();
                //info.ActivatorCode = Convert.ToChar(actcode);
                info.ActivatorCode = Guid.NewGuid();
                #endregion

                #region  Password Hashing 
                info.Password = Crypto.Hash(info.Password);
                info.ConfirmPassword = Crypto.Hash(info.ConfirmPassword); //
                #endregion
                info.IsEmailVerified = "False";

                #region Save to Database
                using (imsdbEntities dc = new imsdbEntities())
                {                   
                        dc.users.Add(info);                                                         
                        dc.SaveChanges();                    
                    //Send Email to User
                    SendVerificationLinkEmail(info.Email, info.ActivatorCode.ToString());
                    message = "Registration successfully done. Account activation link " +
                        " has been sent to your email id:" + info.Email;
                    Status = true;
                }
                #endregion
            }
            else
            {
                message = "Invalid Request";
            }

            ViewBag.Message = message;
            ViewBag.Status = Status;
            return View(info);
        }
        //Verify Account  

        [HttpGet]
        public ActionResult VerifyAccount(string id)
        {
            bool Status = false;
            using (imsdbEntities dc = new imsdbEntities())
            {
                dc.Configuration.ValidateOnSaveEnabled = false; // This line I have added here to avoid 
                                                                    // Confirm password does not match issue on save changes
                var v = dc.users.Where(a => a.ActivatorCode == new Guid(id)).FirstOrDefault();
                if (v != null)
                {
                    v.IsEmailVerified = "True";
                    dc.SaveChanges();
                    Status = true;
                }
                else
                {
                    ViewBag.Message = "Invalid Request";
                }
            }
            ViewBag.Status = Status;
            return View();
        }

        //Login 
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        //Login POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserLogin login, string ReturnUrl = "")
        {
            string message = "";
            try{
                using (imsdbEntities dc = new imsdbEntities())
                {
                    var v = dc.users.Where(a => a.Email == login.Email).FirstOrDefault();
                    if (v != null)
                    {
                        if (v.IsEmailVerified == "False")
                        {
                            ViewBag.Message = "Please verify your email first";
                            return View();
                        }

                        if (string.Compare(Crypto.Hash(login.Password), v.Password) == 0)
                        {
                            int timeout = login.RememberMe ? 525600 : 20; // 525600 min = 1 year
                            var ticket = new FormsAuthenticationTicket(login.Email, login.RememberMe, timeout);
                            string encrypted = FormsAuthentication.Encrypt(ticket);
                            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                            cookie.Expires = DateTime.Now.AddMinutes(timeout);
                            cookie.HttpOnly = true;
                            Response.Cookies.Add(cookie);


                            if (Url.IsLocalUrl(ReturnUrl))
                            {
                                return Redirect(ReturnUrl);
                            }
                            else
                            {
                                return RedirectToAction("Dashboard", "Home");
                            }
                        }
                        else
                        {
                            message = "Invalid credential provided";
                        }
                    }
                    else
                    {
                        message = "Invalid credential provided";
                    }
                }
                

            }
            catch (Exception e)
            {
                if (e.Data == null)
                {
                    ViewBag.Message = "Please Enter correct Credentials";
                    return View();
                }
            }
            ModelState.AddModelError("password", "Invalid Username or Password");
            ViewBag.Message = message;
            return View();

        }

        //Logout
        //[Authorize]
        //[HttpPost]
        //public ActionResult Logout()
        //{
        //    FormsAuthentication.SignOut();
        //    Session.Abandon();
        //    return RedirectToAction("Login");
        //}
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Login");
        }

        [NonAction]
        public bool IsEmailExist(string emailID)
        {
            using (imsdbEntities dc = new imsdbEntities())
            {
                var v = dc.users.Where(a => a.Email == emailID).FirstOrDefault();
                return v != null;
            }
        }

        [NonAction]
        public void SendVerificationLinkEmail(string emailID, string activatorCode)
        {
            var verifyUrl = "/User/VerifyAccount/" + activatorCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            var fromEmail = new MailAddress("aayushlion8426@gmail.com","N.STraders");
            var toEmail = new MailAddress(emailID);
            var fromEmailPassword = "proxfootballer"; /*password*/
            string subject = "Your account is successfully created!";

            string body = "<br/><br/>We are excited to tell you that your N.S Traders User Account is" +
                " successfully created. Please click on the below link to verify your account" +
                " <br/><br/><a href='" + link + "'>" + link + "</a> ";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(message);
        }
    }
}