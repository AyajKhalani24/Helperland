using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HELPERLAND.Controllers;
using HELPERLAND.Models;
using System.ComponentModel.DataAnnotations;

namespace HELPERLAND.Models.ViewModels;
public class ContactusViewmodel
{

#nullable disable

    [Required(ErrorMessage = "*Please Enter Your Name")]
    public string firstname { get; set; }
    [Required(ErrorMessage = "*Please Enter Your Surname")]
    public string lastname { get; set; }

    [Required(ErrorMessage = "*Please Enter Your Email Address")]
    [RegularExpression(@"^\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$", ErrorMessage = "*Enter Valid Email Address")]
    [DataType(DataType.EmailAddress)]
    public string email { get; set; }

    [Phone]
    [StringLength(12)]

    [Required(ErrorMessage = "*Please Enter Your phone number")]
    public string mono { get; set; }
    public string subject { get; set; }
    public string msg { get; set; }

#nullable enable
    public IFormFile? File { get; set; }
}