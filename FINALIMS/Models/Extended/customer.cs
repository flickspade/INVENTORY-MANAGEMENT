using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FINALIMS.Models.Extended
{
    [MetadataType(typeof(CustomerMetaData))]
    public partial class customer
    {
    }
    public class CustomerMetaData
    {

        [Display(Name = "Customer ID")]
        [Required(AllowEmptyStrings = false)] //placeholder ==> model.id.value ==> //set editable false
        public int CustomerID { get; set; }

        [Display(Name = "Customer Name ")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Customer Name Required")]
        public string Name { get; set; }

        [Display(Name = "Address")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Address required")]
        public string Address { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "Contact Number is Required")]
        [Display(Name = "Contact Number")]
        public int Contact_number { get; set; }


        [Display(Name = "Email ")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email is Required")]
        public string Email { get; set; }
    }
}