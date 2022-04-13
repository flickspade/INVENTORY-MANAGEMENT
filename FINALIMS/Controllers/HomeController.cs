using FINALIMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FINALIMS.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        //[Authorize]
        public ActionResult Dashboard()
        {
            imsdbEntities dbObj = new imsdbEntities();
            ViewBag.Message = "DashBoard.";
            decimal CustomerCount = dbObj.customers.Count();
            ViewBag.CusCount = CustomerCount;

            decimal SupplierCount = dbObj.suppliers.Count();
            ViewBag.SupCount = SupplierCount;

            return View();
        }
    }
}