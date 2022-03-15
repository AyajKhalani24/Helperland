using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HELPERLAND.Controllers;
using HELPERLAND.Models;
using System.ComponentModel.DataAnnotations;

namespace HELPERLAND.Models.ViewModels;
public class MyratingsViewModel
{
#nullable disable
    public int ServiceId { get; set; }
    public string CustomerName { get; set; }
    public double ServiceHours { get; set; }
    public DateTime ServiceDate { get; set; }
    public decimal Ratings { get; set; }
    public string Comments { get; set; }

}