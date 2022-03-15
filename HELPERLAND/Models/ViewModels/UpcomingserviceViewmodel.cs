using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HELPERLAND.Controllers;
using HELPERLAND.Models;
using System.ComponentModel.DataAnnotations;

namespace HELPERLAND.Models.ViewModels;
public class UpcomingserviceViewmodel
{
    public int ServiceId { get; set; }
    public DateTime ServiceDate { get; set; }
    public double ServiceHours { get; set; }
    public decimal Payment { get; set; }
    public int UserId { get; set; }
    public string? RecordVersion { get; set; }

#nullable disable
    public string PhoneNumber { get; set; }
    public string StreetName { get; set; }
    public string CustomerName { get; set; }
    public string HouseNumber { get; set; }
    public string City { get; set; }
    public string PostalCode { get; set; }
}