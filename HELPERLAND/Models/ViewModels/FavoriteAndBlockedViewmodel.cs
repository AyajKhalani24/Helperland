using System.ComponentModel.DataAnnotations;

namespace HELPERLAND.Models.ViewModels;
public class FavoriteAndBlockedViewmodel
{
#nullable disable
    public bool? IsBlocked { get; set; }
    public int TotalCleaning { get; set; }
    public bool? IsFavorite { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string SPId { get; set; }
    public string ProfilePhoto { get; set; }
    public decimal AvgRatings { get; set; }

}