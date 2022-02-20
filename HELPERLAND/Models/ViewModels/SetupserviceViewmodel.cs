using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace HELPERLAND.Models.ViewModels
{

    public class SetupserviceViewmodel
    {
        [Required(ErrorMessage = "Zipcode is required")]
        [RegularExpression(@"[0-9]{5}", ErrorMessage = "Enter Valid Zipcode")]
        public string Zipcode { get; set; }

        [Required(ErrorMessage = "Please enter service date")]
        [RegularExpression(@"[0-9]{5}", ErrorMessage = "Enter Valid Date")]
        public string servicedate { get; set; }

        [Required(ErrorMessage = "Please select service time")]
        public float servicetime { get; set; }


        [Required(ErrorMessage = "Please select service hours")]
        public float servicehours { get; set; }
    }
}