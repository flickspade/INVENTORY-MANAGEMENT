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
    public class ProductController : Controller
    {
        //defing an object of model class to acess model data.
        imsdbEntities dbObj = new imsdbEntities();
        // POST: Add Product
        [HttpPost]
        public ActionResult AddProduct(product model)
        {
            if (ModelState.IsValid)
            {
                product data = new product();
                data.ProductID = model.ProductID;
                data.Name = model.Name;
                data.Sales_Price = model.Sales_Price ;
                data.Purchase_Price = model.Purchase_Price;
                data.SupplierID = model.SupplierID;
                data.Product_Type = model.Product_Type;
                data.Size = model.Size;
                data.Brand = model.Brand;

                //saving information on database table supplier.
                dbObj.products.Add(data);
                dbObj.SaveChanges();
            }
            return RedirectToAction("Product");
          
        }

        [HttpGet]
        //GET: Product
        public ActionResult Product() {

            Random random = new Random();
            const string numchars = "0123456789";
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            List<string> randStr = new List<string>();
            string output = "";
            for (int i = 0; i <= 10; i++)
            {
                string AlphaRandom = new string(Enumerable.Repeat(chars, 1)
                  .Select(s => s[random.Next(s.Length)]).ToArray());
                string AlphaRandom1 = new string(Enumerable.Repeat(chars, 1)
                  .Select(s => s[random.Next(s.Length)]).ToArray());
                string NumberRandom = new string(Enumerable.Repeat(numchars, 1)
                  .Select(s => s[random.Next(s.Length)]).ToArray());
                string NumberRandom1 = new string(Enumerable.Repeat(numchars, 1)
                  .Select(s => s[random.Next(s.Length)]).ToArray());

                if (randStr.Contains(NumberRandom + NumberRandom1 + AlphaRandom + AlphaRandom1))
                {
                    i--;
                }
                else
                {
                    randStr.Add( NumberRandom + NumberRandom1 + AlphaRandom+ AlphaRandom1);
                    output = (randStr[i]); 
                }
            }
            ViewBag.SuggestedNewProductID = output;
            //getting all the product present in database

            var data = dbObj.suppliers.ToList();
            ViewBag.GetList = new SelectList(data, "SupplierID", "Name");
            return View();

        }
        [HttpGet]
        public ActionResult ProductList()
        {

            var res = dbObj.products.ToList();
            return View(res);
        }
        // GET: Product/Delete/5
        public ActionResult Delete(string id)
        {
            var res = dbObj.products.Where(x => x.ProductID == id).First();
            dbObj.products.Remove(res);
            dbObj.SaveChanges();


            var list = dbObj.products.ToList();

            return View("ProductList", list);
        }
        //GET: product/Edit/5
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            product prod = dbObj.products.Find(id);
            if (prod == null)
            {
                return HttpNotFound();
            }
            return View(prod);
        }

        [HttpPost]
        public ActionResult Edit(product prodinfo)
        {
            if (ModelState.IsValid)
            {
                dbObj.Entry(prodinfo).State = EntityState.Modified;
                dbObj.SaveChanges();

                var list = dbObj.products.ToList();
                return View("ProductList", list);
            }
            return View(prodinfo);

        }


    }
}