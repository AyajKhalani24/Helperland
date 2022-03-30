using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HELPERLAND.Controllers;
using HELPERLAND.Models;
using System.ComponentModel.DataAnnotations;

namespace HELPERLAND.Models.ViewModels;

public class AdminSRViewmodel
{
#nullable disable

    public List<string> SP_Names { get; set; }
    public List<string> Cust_Names { get; set; }
    public AdminRescheduleViewModel AdminRescheduleViewModel { get; set; }
    public AdminRefundViewModel AdminRefundViewModel { get; set; }
    public IEnumerable<AdminSRViewmodel> viewModels { get; set; }
    public int ServiceId { get; set; }
    public DateTime ServiceDate { get; set; }
    public double TotalHours { get; set; }
    public string User_First_Name { get; set; }
    public string User_Last_Name { get; set; }
    public string StreetName { get; set; }
    public string HouseNumber { get; set; }
    public string PostalCode { get; set; }
    public string City { get; set; }
    public decimal SP_Avg_Ratings { get; set; }
    public int ServiceStatus { get; set; }
    public decimal? Refund { get; set; }
    public decimal Payment { get; set; }

#nullable enable
    public int? SP_Id { get; set; }
    public string? ProfilePhoto { get; set; }
    public string? SP_First_Name { get; set; }
    public string? SP_Last_Name { get; set; }

}