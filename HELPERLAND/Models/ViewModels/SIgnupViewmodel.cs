using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HELPERLAND.Controllers;
using HELPERLAND.Models;
using System.ComponentModel.DataAnnotations;

namespace HELPERLAND.Models.ViewModels;

public class SignupViewmodel
{
#nullable disable

    [Required(ErrorMessage = "*Please Enter Your Name")]
    [StringLength(15)]
    public string FirstName { get; set; }
    [Required(ErrorMessage = "*Please Enter Your Surname")]
    [StringLength(15)]
    public string LastName { get; set; }
    [Required(ErrorMessage = "*Please Enter Your Email Address")]
    [RegularExpression(@"^\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$", ErrorMessage = "*Enter Valid Email Address")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }

    [Required(ErrorMessage = "*Please Enter Your Password")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$", ErrorMessage = "*Create strong Password")]
    public string Password { get; set; }

    [Required(ErrorMessage = "*Please Enter Confirm Password")]
    [Compare("Password", ErrorMessage = "Password is not matching")]
    public string CPassword { get; set; }

    [StringLength(12)]
    public string Mobile { get; set; }

}