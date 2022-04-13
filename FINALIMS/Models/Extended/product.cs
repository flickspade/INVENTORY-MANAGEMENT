using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FINALIMS.Models
{
    [MetadataType(typeof(ProductMetadata))]
    public partial class product
    {
    }
    public class ProductMetadata
    {
        public string ProductID { get; set; }
        public string Name { get; set; }
        [Required(ErrorMessage = "Sales Price is required.")]
        [RegularExpression(@"^[0-9]+(\.[0-9]{1,2})$", ErrorMessage = "Valid Decimal number with maximum 2 decimal places.")]
        public decimal Sales_Price { get; set; }
        [Required(ErrorMessage = "Purchased Price is required.")]
        [RegularExpression(@"^[0-9]+(\.[0-9]{1,2})$", ErrorMessage = "Valid Decimal number with maximum 2 decimal places.")]
        public decimal Purchase_Price { get; set; }
        public int SupplierID { get; set; }
        public string Product_Type { get; set; }
        public string Size { get; set; }
        public string Brand { get; set; }
    }
}