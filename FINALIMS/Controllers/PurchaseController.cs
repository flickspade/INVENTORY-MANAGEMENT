using FINALIMS.Models;
using FINALIMS.Models.Extended;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IMS.Controllers
{

    public class PurchaseController : Controller
    {
        imsdbEntities dbObj = new imsdbEntities();
        [HttpGet]
     
        public ActionResult Create(bool isSuccess=false)
        {
            var data = dbObj.suppliers.ToList();
            ViewBag.GetSuppliersList = new SelectList(data, "SupplierID", "Name");

            int maxId = dbObj.purchase_order.DefaultIfEmpty().Max(p => p == null ? 0 : p.PurchaseID);
            maxId += 1;
            ViewBag.SuggestedNewPurchaseId = maxId;
            ViewBag.Success = isSuccess;
            return View();

            
        }
        [HttpGet]
        public ActionResult GetProductsBySupplierID(int id)
        {
            var productList = dbObj.products.Where(x => x.SupplierID == id).Select(x => new { ID = x.ProductID, name = x.Name }).ToList();
            return Json(new { products = productList }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProductByJquery(string term)
        {
            List<string> products;
            products = dbObj.products.Where(x => x.Name.StartsWith(term)).Select(y => y.Name).ToList();
            return Json(products, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllInfoByNameID(string SupName, int ProId)
        {
            var data = dbObj.products.Where(x => x.Name == SupName && x.SupplierID == ProId).Select(x => new { prodType = x.Product_Type, Brand =x.Brand, size=x.Size ,price = x.Sales_Price, prodID=x.ProductID }).ToList();
                return Json(new {AllData = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetPriceByName(string name , int Id)
        {
            var price = dbObj.products.Where(x => x.Name == name && x.SupplierID==Id).Select(x => new { price = x.Sales_Price }).ToList();
            return Json(new { amount = price }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Create(ProductViewModel modelData)
        {
            if (ModelState.IsValid)
            {
                purchase_order order = new purchase_order();
                order.PurchaseID = modelData.PurchaseID;
                order.supplier = modelData.SupplierID;
                order.Total = modelData.Total;
                order.Discount = modelData.Discount;
                order.SubTotal = modelData.SubTotal;
                dbObj.purchase_order.Add(order);
                dbObj.SaveChanges();


                int latestPurchaseID = order.PurchaseID;
                product_purchase obj = new product_purchase();
                //var transcationnum= Billnum(1, 999);
                foreach (var item in modelData.ListofProductViewModel)
                {
                    obj.Transaction_no = latestPurchaseID;
                    obj.product_id = item.ProductID;
                    obj.purchase_id = item.PurchaseID;
                    obj.Product_name = item.Product_Type;
                    obj.purchase_price = item.CostPrice;
                    obj.Quantity = item.Quantity;
                    obj.Item_Total = item.Total;
                    dbObj.product_purchase.Add(obj);
                    dbObj.SaveChanges();
                    
                }
            }

            return Json( modelData , JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ProductList()
        {

            var res = dbObj.purchase_order.ToList();
            return View(res);
        }

        public ActionResult Invoice(int billno)
        {
            InvoiceDetails result = new InvoiceDetails();
            List<InvoiceData> objItems = new List<InvoiceData>();

            try
            {
                var customerData = (from x in dbObj.purchase_order where x.PurchaseID == billno select x).FirstOrDefault();
                var supplierList = dbObj.suppliers.Where(x => x.SupplierID == customerData.supplier).Select(x => new { name = x.Name, address = x.Address, contact_number = x.Contact_number, email = x.Email }).ToList().Select(y => new supplier { Name = y.name, Address = y.address, Contact_number = y.contact_number, Email = y.email }).ToList();
                ViewBag.supList = supplierList;


                result.SupplierName = customerData.supplier;
                result.Discount = customerData.Discount;
                result.OrderTotal = customerData.Total;
                result.SubTotal = customerData.SubTotal;
                result.Purchase_ID = customerData.PurchaseID;



                var OrderItems = (from x in dbObj.product_purchase where x.purchase_id == customerData.PurchaseID select x).ToList();

                foreach (var item in OrderItems)
                {
                    InvoiceData obj = new InvoiceData();
                    obj.Product_Type = item.Product_name;
                    obj.CostPrice = item.purchase_price;
                    obj.Quantity = item.Quantity;
                    obj.Brand = item.Product_name;
                    obj.Size = item.Product_name;
                    obj.Total = item.Item_Total;
                    obj.Transaction_no = item.Transaction_no;

                    objItems.Add(obj);

                }
                result.InvoiceItems = objItems;
            }
            catch (Exception )
            {
                return new HttpStatusCodeResult(400, "Product Not Found! Try typing different product.");
            }
            return View(result);
        }

        public ActionResult PurchaseOrderList()
        {
            List <purchase_order> singleProducts = dbObj.purchase_order.ToList();
            List<product_purchase> PurchaseOrderList = dbObj.product_purchase.ToList();

            var data = from s in singleProducts
                                join st in PurchaseOrderList on s.PurchaseID equals st.purchase_id 
                                into table1
                                from st in table1 where st.purchase_id.Equals(s.PurchaseID)
                                select new PurchaseOrder
                                {
                                    PurchaseOrderDetail =s,
                                   ProductPurchaseDetail=st,
                                };
            return View(data);
        }
    }
}