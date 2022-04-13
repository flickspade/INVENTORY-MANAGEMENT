using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FINALIMS.Models.Extended
{
    public class ProductViewModel
    {
        public int SupplierID { get; set; }
        public int PurchaseID { get; set; }

        public decimal Total { get; set; }

        public decimal Discount { get; set; }

        public decimal SubTotal { get; set; }
        public PurchaseProductModel[] ListofProductViewModel { get; set; }
    }
}