using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HELPERLAND.Controllers;
using HELPERLAND.Models;
using System.ComponentModel.DataAnnotations;

namespace HELPERLAND.Models.ViewModels;
public class LoginViewmodel
{   
    #nullable disable

    [Required]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.EmailAddress)]
    public string Password { get; set; }

    #nullable enable
    public bool Remember { get; set; }
}