using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HELPERLAND.Controllers;
using HELPERLAND.Models;
using System.ComponentModel.DataAnnotations;

namespace HELPERLAND.Models.ViewModels;
public class RatingViewmodel
{
#nullable disable
    public int FriendlyRating { get; set; }
    public int OnTimeArrivalRating { get; set; }
    public int QualityOfServiceRating { get; set; }
    public int ServiceId { get; set; }
    public int ServiceProviderId { get; set; }
#nullable enable
    public string? Comments { get; set; }

}