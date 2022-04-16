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
                sales_order order = new sales_order();
                order.SaleID = modelData.SaleID;
                order.customer = modelData.CustomerID;
                order.Total = modelData.Total;
                order.Discount = modelData.Discount;
                order.Sub_Total = modelData.SubTotal;
                dbObj.sales_order.Add(order);
                dbObj.SaveChanges();


                int latestPurchaseID = order.SaleID;
                product_sales obj = new product_sales();
                //var transcationnum= Billnum(1, 999);
                foreach (var item in modelData.ListofSaleProductViewModel)
                {
                    obj.Transaction_no = latestPurchaseID;
                    obj.product_id = item.ProductID;
                    obj.Sales_id = item.SaleID;
                    obj.Product_Name = item.Product_Type;
                    obj.purchase_price = item.CostPrice;
                    obj.Quantity = item.Quantity;
                    obj.ItemTotal = item.Total;
                    dbObj.product_sales.Add(obj);
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
        public ActionResult GetAllProductsInfo(string proId)
        {
            var data = dbObj.products.Where(x=>x.Name ==proId).Select(x => new { prodType = x.Product_Type, Brand = x.Brand, size = x.Size, price = x.Sales_Price, prodID = x.ProductID }).ToList();
            return Json(new { AllData = data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Invoice(int billno)
        {
            SaleInvoice result = new SaleInvoice();
            List<SaleInvoiceData> objItems = new List<SaleInvoiceData>();

            try
            {
                var customerData = (from x in dbObj.sales_order where x.SaleID == billno select x).FirstOrDefault();
                var customerList = dbObj.customers.Where(x => x.CustomerID == customerData.customer).Select(x => new { name = x.Name, address = x.Adress, contact_number = x.Contact_number, email = x.Email }).ToList().Select(y => new Models.customer { Name = y.name, Adress = y.address, Contact_number = y.contact_number, Email = y.email }).ToList();
                ViewBag.custList = customerList;


                result.CustomerName = customerData.customer;
                result.Discount = customerData.Discount;
                result.OrderTotal = customerData.Total;
                result.SubTotal = customerData.Sub_Total;
                result.Sale_ID = customerData.SaleID;



                var OrderItems = (from x in dbObj.product_sales where x.Sales_id == customerData.SaleID select x).ToList();
                foreach (var item in OrderItems)
                {
                    SaleInvoiceData obj = new SaleInvoiceData();
                    obj.Product_Type = item.Product_Name;
                    obj.CostPrice = item.purchase_price;
                    obj.Quantity = item.Quantity;
                    obj.Brand = item.Product_Name;
                    obj.Size = item.Product_Name;
                    obj.Total = item.ItemTotal;
                    obj.Transaction_no = item.Transaction_no;

                    objItems.Add(obj);

                }
                result.SalesInvoiceItems = objItems;
            }
            catch (Exception)
            {
                throw;
            }
            return View(result);
        }
        public ActionResult SalesOrderList()
        {
            List<sales_order> singleProductssale = dbObj.sales_order.ToList();
            List<product_sales> SalesOrderList = dbObj.product_sales.ToList();

            var data = from s in singleProductssale
                       join st in SalesOrderList on s.SaleID equals st.Sales_id
                       into table1
                       from st in table1
                       where st.Sales_id.Equals(s.SaleID)
                       select new SalesOrder
                       {
                           SalesOrderDetail = s,
                           ProductSalesDetail = st,
                       };
            return View(data);
        }

    }
}