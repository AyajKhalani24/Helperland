using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HELPERLAND.Models.ViewModels;
using HELPERLAND.Models.Implementation;
using HELPERLAND.Models;

namespace HELPERLAND.Controllers;

public class HomeController : Controller
{
    private readonly Implcontactus implcontactus;
    private readonly HelperlandContext context;

    public HomeController(Implcontactus implcontactus, HelperlandContext context)
    {
        this.implcontactus = implcontactus;
        this.context = context;
    }

    [HttpGet]
    public IActionResult GetCityByPostalCode(string PostalCode)
    {
        try
        {
            if (PostalCode != null)
            {
                var city = (
                    from z in context.Zipcodes
                    join c in context.Cities
                    on z.CityId equals c.Id
                    where z.ZipcodeValue == PostalCode
                    select c.CityName
                ).FirstOrDefault();
                if (city != null)
                {
                    return Json(new { cityName = city });
                }
            }
            return Json(new { err = "City Not Found !" });
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return Json(new { err = "Internal Server Error !" });
        }
    }
    public IActionResult Index()
    {
        return View();
    }
    public IActionResult About()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Contact()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Contact(ContactusViewmodel contact)
    {
        if (ModelState.IsValid)
        {
            implcontactus.Add(contact);
            return RedirectToAction("Contact");
        }
        return RedirectToAction("Contact");
    }
    public IActionResult Facq()
    {
        return View();
    }
    public IActionResult Price()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Signup()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Becomeprovider()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }
}
