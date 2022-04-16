using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FINALIMS.Models.Extended
{
    public class SaleViewModel
    {

        public int CustomerID { get; set; }
        public int SaleID { get; set; }
        public decimal Total { get; set; }

        public decimal Discount { get; set; }

        public decimal SubTotal { get; set; }

        public SaleProductModel[] ListofSaleProductViewModel { get; set; }
    }
}