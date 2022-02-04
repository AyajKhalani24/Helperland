using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HELPERLAND.Controllers;
using HELPERLAND.Models;
using System.ComponentModel.DataAnnotations;

namespace HELPERLAND.Models.ViewModels;
public class ContactusViewmodel{

    #nullable disable

    [Required]
    public string firstname { get; set; }
    [Required]
    public string lastname { get; set; }  
    [Required]
    [DataType(DataType.EmailAddress)] 
    public string email { get; set; }
    public string mono { get; set; }
    public string subject { get; set; }
    public string msg { get; set; }
    
    #nullable enable
    public IFormFile? File { get; set; }
}