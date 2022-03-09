using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HELPERLAND.Controllers;
using HELPERLAND.Models;
using System.ComponentModel.DataAnnotations;

namespace HELPERLAND.Models.ViewModels;
public class AddaddressViewmodel
{
#nullable disable

    [Required(ErrorMessage = "Enter street name")]
    public string StreetName { get; set; }

    [Required(ErrorMessage = "Enter house number")]
    public string hno { get; set; }
    public string poc { get; set; }
    public string City { get; set; }

    [Required(ErrorMessage = "Enter ")]
    public string PhoneNumber { get; set; }

    public int? AddressId { get; set; }
}