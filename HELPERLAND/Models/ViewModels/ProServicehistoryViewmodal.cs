using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HELPERLAND.Controllers;
using HELPERLAND.Models;
using System.ComponentModel.DataAnnotations;

namespace HELPERLAND.Models.ViewModels;
public class ProServicehistoryViewmodel
{
#nullable disable
    public int ServiceId { get; set; }
    public DateTime ServiceStartTime { get; set; }
    public double ServiceHours { get; set; }
    public int? CustomerId { get; set; }
    public string CustomerName { get; set; }
    public decimal Payment { get; set; }
    public int? Status { get; set; }
    public string Addressline1 { get; set; }
    public string Addressline2 { get; set; }
    public string PostalCode { get; set; }
    public string PhoneNumber { get; set; }
    public string City { get; set; }
    public decimal AvgRating { get; set; } = 0;

    public int ServiceRequestId { get; set; }
}