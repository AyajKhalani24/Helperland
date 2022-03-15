using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HELPERLAND.Models.ViewModels
{
    public class ReturnaddressViewmodel
    {
        #nullable disable

        public int AddressId { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string PostalCode { get; set; }
        public string PhoneNumber { get; set; }
        public string City { get; set; }
    }
}