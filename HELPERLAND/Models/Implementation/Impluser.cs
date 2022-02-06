using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HELPERLAND.Controllers;
using HELPERLAND.Models.ViewModels;
using System.Security.Cryptography;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using BCrypto = BCrypt.Net.BCrypt;

namespace HELPERLAND.Models.Implementation;

public class Impluser
{
    private readonly HelperlandContext context;

    public Impluser(HelperlandContext context)
    {
        this.context = context;
    }
    public void Add1(SignupViewmodel user)
    {
        var passwordHash = BCrypto.HashPassword(user.Password);

        User newuser = new User()
        {
            UserTypeId = 1,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Password = passwordHash,
            Mobile = user.Mobile,
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now
        };

        context.Users.Add(newuser);
        context.SaveChanges();
    }
    public void Add2(SignupViewmodel user)
    {
        var passwordHash = BCrypto.HashPassword(user.Password);
        User newuser = new User()
        {
            UserTypeId = 2,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Password = passwordHash,
            Mobile = user.Mobile,
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now
        };

        context.Users.Add(newuser);
        context.SaveChanges();
    }
}