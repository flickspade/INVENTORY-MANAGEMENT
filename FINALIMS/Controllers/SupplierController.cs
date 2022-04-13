using FINALIMS.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace FINALIMS.Controllers
{
    public class SupplierController : Controller
    {
        imsdbEntities dbObj = new imsdbEntities();
        [HttpPost]
        public ActionResult AddSupplier( supplier model)
        {
            if (ModelState.IsValid)
            {
                supplier data = new supplier();
                data.SupplierID = model.SupplierID;
                data.Name = model.Name;
                data.Address = model.Address;
                data.Contact_number = model.Contact_number;
                //saving information on database table supplier.
                dbObj.suppliers.Add(data);
                dbObj.SaveChanges();
            }
            return RedirectToAction("Supplier");
            

        }
        [HttpGet]
        public  ActionResult Supplier()
        {
                //sugggest a new ID whenever new supplier is being created
                int maxId = dbObj.suppliers.DefaultIfEmpty().Max(p => p == null ? 0 : p.SupplierID);
                maxId += 1;
                ViewBag.SuggestedNewSuppId = maxId;
               return View();

        }


        public ActionResult SupplierList()
        {
            
           var res = dbObj.suppliers.ToList();
            return View(res);
        }
        // GET: Supplier/Delete/5
        public ActionResult Delete(int id)
        {
            var res = dbObj.suppliers.Where(x => x.SupplierID == id).First();
            dbObj.suppliers.Remove(res);
            dbObj.SaveChanges();

            
            var list = dbObj.suppliers.ToList();

            return View("SupplierList",list);
        }
        //GET: customers/Edit/5
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            supplier supplier = dbObj.suppliers.Find(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View(supplier);
        }

        [HttpPost]
        public ActionResult Edit(supplier supinfo)
        {
            if (ModelState.IsValid)
            {
                dbObj.Entry(supinfo).State = EntityState.Modified;
                dbObj.SaveChanges();

                var list = dbObj.suppliers.ToList();
                return View("SupplierList", list);
            }
            return View(supinfo);

        }


        //// POST: Supplier/Delete/5
        //[HttpPost]
        //public ActionResult Delete(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add delete logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
