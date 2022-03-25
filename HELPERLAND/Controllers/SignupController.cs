using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HELPERLAND.Models.ViewModels;
using HELPERLAND.Models.Implementation;
using HELPERLAND.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Authorization;
using BCrypto = BCrypt.Net.BCrypt;

namespace HELPERLAND.Controllers;

public class SignupController : Controller
{
#nullable disable
    private readonly Impluser impluser;
    private readonly HelperlandContext context;
    private readonly SendEmail sendEmail;
    public readonly IDataProtector protector;
    public readonly string Key = "Ayaj@2001@Khalani";

    public SignupController(Impluser impluser, HelperlandContext context, IDataProtectionProvider protector, SendEmail sendEmail)
    {
        this.impluser = impluser;
        this.context = context;
        this.sendEmail = sendEmail;
        this.protector = protector.CreateProtector(Key);
    }

    [HttpPost]
    public IActionResult Signup(SignupViewmodel user)
    {
        impluser.Add1(user);
        return RedirectToAction("index", "Home");
    }

    [HttpPost]
    public IActionResult Becomeprovider(SignupViewmodel user)
    {
        impluser.Add2(user);
        return RedirectToAction("index", "Home");
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewmodel login)
    {
        var user = context.Users.Where(x => x.Email.Equals(login.Email)).FirstOrDefault();

        if (user == null)
        {
            ModelState.AddModelError("Email", "Authentication Failed !!");
            Response.Cookies.Append("loginmodal", "true");
            return View("../Home/Index");
        }
        else
        {
            bool verified = BCrypt.Net.BCrypt.Verify(login.Password, user.Password);
            if (!verified)
            {
                ModelState.AddModelError("Email", "Authentication Failed !!");
                Response.Cookies.Append("loginmodal", "true");
                return View("../Home/Index");
            }
            else
            {
                var claims = new List<Claim>();
                claims.Add(new Claim("Email", user.Email));
                claims.Add(new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()));
                claims.Add(new Claim(ClaimTypes.Name, user.FirstName + " " + user.LastName));
                claims.Add(new Claim(ClaimTypes.SerialNumber, user.UserTypeId.ToString()));
                if (user.UserTypeId == 1)
                {
                    claims.Add(new Claim(ClaimTypes.Role, "Customer"));
                }
                else if (user.UserTypeId == 2)
                {
                    claims.Add(new Claim(ClaimTypes.Role, "Provider"));
                }
                else if (user.UserTypeId == 3)
                {
                    claims.Add(new Claim(ClaimTypes.Role, "Admin"));
                }
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                await HttpContext.SignInAsync(claimsPrincipal);
                if (user.UserTypeId == 3)
                {
                    return RedirectToAction("ServiceRequest", "Admin");
                }
            }
        }
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
    [HttpGet]
    public IActionResult ResetPassword(string token)
    {
        try
        {
            var decryptId = protector.Unprotect(token);
            DateTime expiryDate = DateTime.Parse(decryptId.Split("%")[2]).AddHours(1);
            DateTime current = DateTime.UtcNow;
            int isvalid = current.CompareTo(expiryDate);

            if (isvalid > 0)
            {
                throw new Exception();
            }
            return View("~/Views/Home/ResetPassword.cshtml");

        }
        catch
        {
            return BadRequest(error: "Invalid Link");
        }
    }

    [HttpPost]
    public IActionResult ResetPassword(SignupViewmodel model, string token)
    {
        string decryptId = protector.Unprotect(token);
        if (decryptId != null)
        {
            int userId = Convert.ToInt32(decryptId.Split("%")[1]);
            var user = context.Users.Where(e => e.UserId == userId).FirstOrDefault();
            user.Password = BCrypto.HashPassword(model.Password);
            context.Users.Attach(user);
            context.SaveChanges();
            return RedirectToAction("index", "Home");
        }
        return BadRequest(error: "Invalid Link");
    }

    [HttpPost]
    public IActionResult ResetLink(LoginViewmodel model)
    {
        var user = context.Users.Where(x => x.Email.Equals(model.Email)).FirstOrDefault();

        if (user != null)
        {
            string Token = protector.Protect(model.Email + "%" + user.UserId + "%" + DateTime.UtcNow);
            string ResetURL = Url.Action("ResetPassword", "Signup", new { token = Token }, Request.Scheme);
            var email = new EmailViewmodel()
            {
                To = model.Email,
                Subject = "Password reset of your account in helperland",
                isHTML = true,
                Body = $"To reset your password of Helperland.<p><a href='{ResetURL}'>Click Here</a></p>",
            };
            bool result = sendEmail.sendMail(email);
            if (result)
            {
                return RedirectToAction("index", "Home");
            }
            else
            {
                return BadRequest(error: "Internal Server Error");
            }
        }
        return BadRequest(error: "Email is not registered");
    }
}