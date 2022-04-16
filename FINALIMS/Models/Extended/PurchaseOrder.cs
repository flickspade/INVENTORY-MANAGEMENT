using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FINALIMS.Models.Extended
{
    public class PurchaseOrder
    {
        public product_purchase ProductPurchaseDetail { get; set; }
        public purchase_order PurchaseOrderDetail { get; set; }
    }
}