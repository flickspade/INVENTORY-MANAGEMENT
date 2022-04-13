using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FINALIMS.Models
{
    [MetadataType(typeof(SupplierMetadata))]
    public partial class supplier
    {
    }
    public class SupplierMetadata
    {

        [Display(Name = "Supplier ID")]
        [Required(AllowEmptyStrings = false)] //placeholder ==> model.id.value ==> //set editable false
        public int SupplierID { get; set; }

        [Display(Name = "Supplier Name ")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Supplier Name Required")]
        public string Name { get; set; }

        [Display(Name = "Address ")]
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