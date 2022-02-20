using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HELPERLAND.Controllers;
using HELPERLAND.Models;
using System.ComponentModel.DataAnnotations;

namespace HELPERLAND.Models.ViewModels;
public class AddaddressViewmodel
{
#nullable disable
    public string StreetName { get; set; }
    public string hno { get; set; }
    public string poc { get; set; }
    public string City { get; set; }
    public string PhoneNumber { get; set; }
}