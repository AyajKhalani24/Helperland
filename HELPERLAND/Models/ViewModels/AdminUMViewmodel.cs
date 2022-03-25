using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HELPERLAND.Controllers;
using HELPERLAND.Models;
using System.ComponentModel.DataAnnotations;

namespace HELPERLAND.Models.ViewModels;

public class AdminUMViewmodel
{
#nullable disable

    public List<string> Names { get; set; }
    // public AdminRescheduleViewModel AdminRescheduleViewModel { get; set; }
    public IEnumerable<AdminUMViewmodel> viewModels { get; set; }
    public int UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int UserTypeId { get; set; }
    public DateTime RegisteredDate { get; set; }
    public string PhoneNumber { get; set; }
    public string ZipCode { get; set; }
    public bool IsApproved { get; set; }
    public bool IsRegisteredUser { get; set; }
    public string City { get; set; }
#nullable enable
    public int? SP_Id { get; set; }

}