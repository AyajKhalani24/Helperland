using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HELPERLAND.Controllers;
using HELPERLAND.Models;
using System.ComponentModel.DataAnnotations;

namespace HELPERLAND.Models.ViewModels;

public class SignupViewmodel
{
    #nullable disable

    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }  
    [Required]
    [DataType(DataType.EmailAddress)] 
    public string Email { get; set; }
    public string Password { get; set; }
    public string Mobile { get; set; }
    
}