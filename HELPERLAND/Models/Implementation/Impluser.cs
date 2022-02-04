using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HELPERLAND.Controllers;
using HELPERLAND.Models.ViewModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

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
        User newuser = new User(){
            UserTypeId=1,
            FirstName=user.FirstName,
            LastName=user.LastName,
            Email=user.Email,
            Password=user.Password,
            Mobile=user.Mobile,
            CreatedDate=DateTime.Now,
            ModifiedDate=DateTime.Now
        };
        
        context.Users.Add(newuser);
        context.SaveChanges();
    }
    public void Add2(SignupViewmodel user)
    {   
        User newuser = new User(){
            UserTypeId=2,
            FirstName=user.FirstName,
            LastName=user.LastName,
            Email=user.Email,
            Password=user.Password,
            Mobile=user.Mobile,
            CreatedDate=DateTime.Now,
            ModifiedDate=DateTime.Now
        };
        
        context.Users.Add(newuser);
        context.SaveChanges();
    }

    public int find(LoginViewmodel login)
    {   
        var user= context.Users.Where(x => x.Email == login.Email).FirstOrDefault();
        if(login.Password==user.Password)
        {
            return 1;   
        }
        else
        {
            return 0;
        }
            
    }
}