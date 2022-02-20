using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HELPERLAND.Models.ViewModels;
using HELPERLAND.Models.Implementation;
using HELPERLAND.Models;

namespace HELPERLAND.Controllers;

public class BookserviceController : Controller
{
    private readonly HelperlandContext context;

    public BookserviceController(HelperlandContext context)
    {
        this.context = context;
    }
    public IActionResult setupservice()
    {
        return View();
    }
    [HttpGet]
    public IActionResult CheckAvailability(string? PostalCode)
    {
        return Json(true);
    }
    public IActionResult AddNewAddress(AddaddressViewmodel model)
    {
        try
        {
            var user = await userManager.GetUserAsync(User);
            UserAddress newAddress = new UserAddress()
            {
                UserId = 3,
                AddressLine1 = model.StreetName,
                AddressLine2 = model.hno,
                City = model.City,
                PostalCode = model.poc,
                IsDefault = false,
                IsDeleted = false,
                Mobile = model.PhoneNumber,
                Email = user.Email,
            };
            context.UserAddresses.Add(newAddress);
            var address = context.SaveChanges();
            return Json(new { addressId = newAddress.AddressId });
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return Json(new { error = "Internal Server Error !!" });
        }
    }
}

