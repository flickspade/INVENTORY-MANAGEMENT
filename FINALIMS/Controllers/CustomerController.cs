using FINALIMS.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace FINALIMS.Controllers
{
    public class CustomerController : Controller
    {
        // POST: Customer
        imsdbEntities dbObj = new imsdbEntities();
        [HttpPost]
        public ActionResult AddCustomer(customer model)
        {
            if (ModelState.IsValid)
            {
                customer data = new customer();
                data.CustomerID = model.CustomerID;
                data.Name = model.Name;
                data.Adress = model.Adress;
                data.Contact_number = model.Contact_number;
                //saving information on database table supplier.
                dbObj.customers.Add(data);
                dbObj.SaveChanges();
            }
            return RedirectToAction("Customer");
        }
        [HttpGet]
        public ActionResult Customer()
        {
            //sugggest a new ID whenever new supplier is being created
            int maxId = dbObj.customers.DefaultIfEmpty().Max(p => p == null ? 0 : p.CustomerID);
            maxId += 1;
            ViewBag.SuggestedNewSuppId = maxId;
           
            return View();

        }
        public ActionResult CustomerList()
        {

            var res = dbObj.customers.ToList();
            return View(res);
        }


        public ActionResult Delete(int id)
        {
            var res = dbObj.customers.Where(x => x.CustomerID == id).First();
            dbObj.customers.Remove(res);
            dbObj.SaveChanges();


            var list = dbObj.customers.ToList();

            return View("CustomerList", list);
        }
        //GET: customers/Edit/5
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            customer customer = dbObj.customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        [HttpPost]
        public ActionResult Edit(customer cusinfo)
        {
            if (ModelState.IsValid)
            {
                dbObj.Entry(cusinfo).State = EntityState.Modified;
                dbObj.SaveChanges();

                var list = dbObj.customers.ToList();
                return View("CustomerList", list);
            }
            return View(cusinfo);

        }
    }
}