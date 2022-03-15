using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HELPERLAND.Controllers;
using HELPERLAND.Models;
using System.ComponentModel.DataAnnotations;

namespace HELPERLAND.Models.ViewModels;
public class BlockcustomerViewmodel
{
    public bool IsBlocked { get; set; }
    public string? CustomerName { get; set; }
    public int UserId { get; set; }
}