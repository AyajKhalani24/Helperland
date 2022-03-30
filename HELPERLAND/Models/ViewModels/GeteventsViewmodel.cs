using System.ComponentModel.DataAnnotations;

namespace HELPERLAND.Models.ViewModels;
public class GeteventsViewmodel
{
#nullable disable
    public int id { get; set; }
    public string start { get; set; }
    public string title { get; set; }
    public string backgroundColor { get; set; }
    public bool allDay { get; set; }
}