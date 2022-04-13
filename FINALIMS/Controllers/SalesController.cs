using FINALIMS.Models;
using FINALIMS.Models.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FINALIMS.Controllers
{
    public class SalesController : Controller
    {
        imsdbEntities dbObj = new imsdbEntities();
        [HttpGet]
        public ActionResult Create(bool isSuccess = false)
        {
            var data = dbObj.customers.ToList();
            ViewBag.GetCustomersList = new SelectList(data, "CustomerID", "Name");

            int maxId = dbObj.sales_order.DefaultIfEmpty().Max(p => p == null ? 0 : p.SaleID);
            maxId += 1;
            ViewBag.SuggestedNewSaleId = maxId;
            ViewBag.Success = isSuccess;
            return View();
        }

        [HttpPost]
        public ActionResult Create(SaleViewModel modelData)
        {
            if (ModelState.IsValid)
            {
                purchase_order order = new purchase_order();
                order.PurchaseID = modelData.SaleID;
                order.supplier = modelData.CustomerID;
                order.Total = modelData.Total;
                order.Discount = modelData.Discount;
                order.SubTotal = modelData.SubTotal;
                dbObj.purchase_order.Add(order);
                dbObj.SaveChanges();


                int latestPurchaseID = order.PurchaseID;
                product_purchase obj = new product_purchase();
                //var transcationnum= Billnum(1, 999);
                foreach (var item in modelData.ListofSaleProductViewModel)
                {
                    obj.Transaction_no = latestPurchaseID;
                    obj.product_id = item.ProductID;
                    obj.purchase_id = item.SaleID;
                    obj.Product_name = item.Product_Type;
                    obj.purchase_price = item.CostPrice;
                    obj.Quantity = item.Quantity;
                    obj.Item_Total = item.Total;
                    dbObj.product_purchase.Add(obj);
                    dbObj.SaveChanges();

                }
            }

            return Json(modelData, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetProductByJquery(string term)
        {
            List<string> products;
            products = dbObj.products.Where(x => x.Name.StartsWith(term)).Select(y => y.Name).ToList();
            return Json(products, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAllInfoByNameID(string CusName, int ProId)
        {
            var data = dbObj.products.Where(x => x.Name == CusName && x.SupplierID == ProId).Select(x => new { prodType = x.Product_Type, Brand = x.Brand, size = x.Size, price = x.Sales_Price, prodID = x.ProductID }).ToList();
            return Json(new { AllData = data }, JsonRequestBehavior.AllowGet);
        }


    }
}