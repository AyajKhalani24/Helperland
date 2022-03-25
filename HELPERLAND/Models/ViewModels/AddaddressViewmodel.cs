using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HELPERLAND.Controllers;
using HELPERLAND.Models;
using System.ComponentModel.DataAnnotations;

namespace HELPERLAND.Models.ViewModels;
public class AddaddressViewmodel
{
#nullable disable

    [Required(ErrorMessage = "*Please Enter Street Name")]
    public string StreetName { get; set; }

    [Required(ErrorMessage = "*Please Enter House Number")]
    public string hno { get; set; }

    [Required(ErrorMessage = "*Please Enter Postal Code")]
    public string poc { get; set; }

    [Required(ErrorMessage = "*Please Enter Postal Code")]
    public string City { get; set; }

    [Required(ErrorMessage = "*Please Enter Phone Number")]
    public string PhoneNumber { get; set; }

    public int? AddressId { get; set; }
}