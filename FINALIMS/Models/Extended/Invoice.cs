using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FINALIMS.Models.Extended
{
    public class InvoiceDetails
    {
        public int SupplierName { get; set; }
        public decimal Purchase_ID { get; set; }
        public decimal OrderTotal { get; set; }
        public decimal Discount { get; set; }
        public decimal SubTotal { get; set; }
        
        public List<InvoiceData> InvoiceItems { get; set; }
}

    public class InvoiceData
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