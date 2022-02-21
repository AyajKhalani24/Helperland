using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace HELPERLAND.Models.ViewModels
{

    public class SetupserviceViewmodel
    {
#nullable disable
        public string Zipcode { get; set; }
        public string servicedate { get; set; }
        public float servicetime { get; set; }
        public float servicehours { get; set; }
        public int AddressId { get; set; }
        public bool HasPets { get; set; }

#nullable enable
        public IEnumerable<int>? ExtraServices { get; set; }
        public string? Comments { get; set; }
    }
}