using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HELPERLAND.Models.ViewModels;
using HELPERLAND.Models.Implementation;
using HELPERLAND.Models;

namespace HELPERLAND.Controllers;

public class HomeController : Controller
{
    private readonly Implcontactus implcontactus;

    public HomeController(Implcontactus implcontactus)
    {
        this.implcontactus = implcontactus;
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
        if(ModelState.IsValid)
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
