using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FINALIMS.Models.Extended
{
    public class SaleInvoice
    {
        public int CustomerName { get; set; }
        public decimal Sale_ID { get; set; }
        public decimal OrderTotal { get; set; }
        public decimal Discount { get; set; }
        public decimal SubTotal { get; set; }

        public List<SaleInvoiceData> SalesInvoiceItems { get; set; }
    }
    public class SaleInvoiceData
    {
        public string Product_Type { get; set; }
        public decimal CostPrice { get; set; }
        public int Quantity { get; set; }
        public string Brand { get; set; }
        public string Size { get; set; }
        public decimal Total { get; set; }
        public int Transaction_no { get; set; }
    }
}