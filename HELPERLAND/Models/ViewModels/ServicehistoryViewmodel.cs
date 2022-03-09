using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HELPERLAND.Controllers;
using HELPERLAND.Models;
using System.ComponentModel.DataAnnotations;

namespace HELPERLAND.Models.ViewModels;

public class ServicehistoryViewmodel
{
#nullable disable
    public int ServiceId { get; set; }
    public DateTime ServiceStartTime { get; set; }
    public double ServiceHours { get; set; }
    public int? ServiceProviderId { get; set; }
    public string ServiceProviderName { get; set; }
    public string ServiceProviderProfilePicture { get; set; }
    public decimal Payment { get; set; }
    public int? Status { get; set; }
    public decimal AvgRating { get; set; } = 0;
}

public class CancelServiceViewModel
{
    [Required]
    public int ServiceId { get; set; }
    [Required(ErrorMessage = "Enter Cancellation Reason !")]
    public string CancelReason { get; set; }
}

public class RescheduleServiceViewModel
{
#nullable disable
    public int ServiceId { get; set; }
    public string NewServiceDate { get; set; }
    public decimal NewServiceStartTime { get; set; }

}