using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FINALIMS.Models.Extended
{
    public class SalesOrder
    {
        public product_sales ProductSalesDetail { get; set; }
        public sales_order SalesOrderDetail { get; set; }
    }
}