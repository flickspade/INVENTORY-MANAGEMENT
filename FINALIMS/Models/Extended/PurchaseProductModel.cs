using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FINALIMS.Models.Extended
{
    public class PurchaseProductModel
    {
        public int Transaction_no { get; set; }
        public int PurchaseID { get; set; }
        public string ProductID { get; set; }
        public string Product_Type { get; set; }
        public decimal Total { get; set; }
        public int Quantity { get; set; }
        public decimal CostPrice { get; set; }
      
    }
}