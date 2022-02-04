using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HELPERLAND.Models.ViewModels;
using HELPERLAND.Models.Implementation;
using HELPERLAND.Models;

namespace HELPERLAND.Controllers;

public class SignupController : Controller
{
    private readonly Impluser impluser;

    public SignupController(Impluser impluser)
    {
        this.impluser = impluser;
    }

    [HttpPost]
     public IActionResult Signup(SignupViewmodel user)
    {

        impluser.Add1(user);
        return RedirectToAction("Price","Home");
    }
    
    [HttpPost]
     public IActionResult Becomeprovider(SignupViewmodel user)
    {
        impluser.Add2(user);
        return RedirectToAction("index","Home");
    }

    [HttpPost]
    public IActionResult Login(LoginViewmodel login)
    {   
        int i;
        i=impluser.find(login);
        if(i==1){
            return RedirectToAction("Index","Home");
        }
        else{
            return RedirectToAction("About","Home");
        }
    }
}