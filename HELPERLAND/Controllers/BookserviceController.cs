using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HELPERLAND.Models.ViewModels;
using HELPERLAND.Models.Implementation;
using HELPERLAND.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace HELPERLAND.Controllers;
#nullable disable

public class BookserviceController : Controller
{
    private readonly HelperlandContext context;
    private readonly SendEmail sendEmail;

    public BookserviceController(HelperlandContext context, SendEmail sendEmail)
    {
        this.context = context;
        this.sendEmail = sendEmail;
    }
    public IActionResult setupservice()
    {
        if (User.Identity.IsAuthenticated)
        {
            return View();
        }
        else
        {
            return BadRequest();
        }
    }

    [HttpGet]
    public IActionResult CheckAvailability(string PostalCode)
    {
        if (PostalCode != null)
        {
            int UserId = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var user = context.Users.Where(u => u.UserId == UserId).FirstOrDefault();
            // var isAvailable = context.Users.Where(u => u.UserTypeId == 2 && u.ZipCode == PostalCode).FirstOrDefault();
            var isAvailable = (from u in context.Users.AsEnumerable()
                               where
                               !(
                                   from fabu in context.FavoriteAndBlockeds.AsEnumerable()
                                   where
                                     fabu.UserId == user.UserId &&
                                     fabu.IsBlocked == true
                                   select new
                                   {
                                       TargetUserId = fabu.TargetUserId
                                   }
                               ).Union
                               (
                                   from fabt in context.FavoriteAndBlockeds.AsEnumerable()
                                   where
                                     fabt.TargetUserId == user.UserId &&
                                     fabt.IsBlocked == true
                                   select new
                                   {
                                       TargetUserId = fabt.UserId
                                   }
                               ).Contains(new { TargetUserId = u.UserId }) &&
                                 u.UserTypeId == 2
                               select new
                               {
                                   u.Email,
                                   u.UserId
                               }).ToList();

            if (isAvailable.Count() > 0)
            {
                return Json(true);
            }
        }
        return Json(false);
    }

    [HttpPost]
    public IActionResult AddNewAddress([FromBody] AddaddressViewmodel model)
    {
        try
        {
            int UserId = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var user = context.Users.Where(u => u.UserId == UserId).FirstOrDefault();

            UserAddress newAddress = new UserAddress()
            {
                UserId = UserId,
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
    public IActionResult GetAddresses(string ZipCode)
    {
        // Console.WriteLine(ZipCode);
        int UserId = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var addresses = context.UserAddresses.Where(a => a.UserId == UserId && a.PostalCode == ZipCode).ToList();

        IEnumerable<ReturnaddressViewmodel> add = new List<ReturnaddressViewmodel>();
        foreach (var a in addresses)
        {
            ReturnaddressViewmodel temp = new ReturnaddressViewmodel()
            {
                AddressId = a.AddressId,
                AddressLine1 = a.AddressLine1,
                AddressLine2 = a.AddressLine2,
                PostalCode = a.PostalCode,
                PhoneNumber = a.Mobile,
                City = a.City

            };
            add = add.Append(temp);
        }
        return Json(add);
    }

    [HttpPost]
    public IActionResult Index([FromBody] SetupserviceViewmodel model)
    {
        int random = new Random().Next(1000, 9999);
        var isExist = context.ServiceRequests.Where(x => x.ServiceId == random).FirstOrDefault();

        while (isExist != null)
        {
            random = new Random().Next(1000, 9999);
            isExist = context.ServiceRequests.Where(x => x.ServiceId == random).FirstOrDefault();
        }

        int UserId = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var user = context.Users.Where(u => u.UserId == UserId).FirstOrDefault();

        var newServiceRequest = new ServiceRequest()
        {
            ServiceId = random,
            UserId = user.UserId,
            ZipCode = model.Zipcode,
            PaymentDue = false,
            ServiceHours = model.servicehours,
            HasPets = model.HasPets,
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now,
            Distance = 0,
            Status = 1,
            RecordVersion = Guid.NewGuid()
        };

        if (model.ServiceProId != null)
        {
            newServiceRequest.ServiceProviderId = model.ServiceProId;
            newServiceRequest.Status = 2;
            var Provider = context.Users.Where(a => a.UserId == model.ServiceProId).FirstOrDefault();
            if (Provider == null)
                Console.WriteLine("No Malyo");
            else
                Console.WriteLine("malyo");

            Console.WriteLine(Provider.Email);
            var email = new EmailViewmodel()
            {
                To = Provider.Email,
                Subject = "Service Alert",
                isHTML = true,
                Body = $"<p>The Service {random} has been directly assign to you.</p>",
            };
            bool result = sendEmail.sendMail(email);
            Console.WriteLine(result);
        }

        if (model.ExtraServices != null)
        {
            newServiceRequest.ServiceHours += model.ExtraServices.Count() * 0.5;
        }
        newServiceRequest.SubTotal = ((decimal)model.servicehours) * 18;
        newServiceRequest.TotalCost = newServiceRequest.SubTotal;

        if (model.Comments != null)
        {
            newServiceRequest.Comments = model.Comments;
        }
        var s = model.servicedate.Split("/");
        double x = model.servicetime;

        int hours = ((int)Math.Floor(x));
        int min = ((int)Math.Ceiling((x - hours) * 60));

        var d = new DateTime(Int32.Parse(s[2]), Int32.Parse(s[1]), Int32.Parse(s[0]), hours, min, 0);
        newServiceRequest.ServiceStartDate = d;

        context.ServiceRequests.Add(newServiceRequest);
        context.SaveChanges();

        var custAddress = context.UserAddresses.Where(a => a.AddressId == model.AddressId && a.UserId == user.UserId).FirstOrDefault();
        if (custAddress != null)
        {
            var newServiceRequestAddress = new ServiceRequestAddress()
            {
                ServiceRequestId = newServiceRequest.ServiceRequestId,
                AddressLine1 = custAddress.AddressLine1,
                AddressLine2 = custAddress.AddressLine2,
                City = custAddress.City,
                State = custAddress.State,
                PostalCode = custAddress.PostalCode,
                Mobile = custAddress.Mobile,
                Email = custAddress.Email
            };
            context.ServiceRequestAddresses.Add(newServiceRequestAddress);
            context.SaveChanges();
        }
        if (model.ExtraServices != null)
        {
            foreach (var extra in model.ExtraServices)
            {
                var extraService = new ServiceRequestExtra()
                {
                    ServiceRequestId = newServiceRequest.ServiceRequestId,
                    ServiceExtraId = extra
                };
                context.ServiceRequestExtras.Add(extraService);
                context.SaveChanges();
            }
        }
        return Json(new { ServiceId = newServiceRequest.ServiceId });
    }

    public IActionResult SendMailToProvider(string ZipCode, string serviceId)
    {
        var Service = context.ServiceRequests.Where(a => a.ServiceId == Convert.ToInt32(serviceId) && a.Status == 2).FirstOrDefault();
        if (Service == null)
        {
            int flag = 0;
            var Provider = context.Users.Where(a => a.ZipCode == ZipCode && a.UserTypeId == 2).ToList();

            int UserId = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var user = context.Users.Where(u => u.UserId == UserId).FirstOrDefault();

            var blocked = context.FavoriteAndBlockeds.Where(fab => (fab.UserId == user.UserId || fab.TargetUserId == user.UserId) && fab.IsBlocked == true).ToList();
            foreach (var s in Provider.ToList())
            {
                foreach (var b in blocked)
                {
                    if ((b.UserId == s.UserId && b.TargetUserId == user.UserId) || (b.UserId == user.UserId && b.TargetUserId == s.UserId))
                    {
                        Provider.Remove(s);
                        continue;
                    }
                }
            }
            foreach (var s in Provider)
            {
                var email = new EmailViewmodel()
                {
                    To = s.Email,
                    Subject = "Service Alert",
                    isHTML = true,
                    Body = $"<p>The Service {serviceId} has been assign in your area go and check your dashboard.</p>",
                };
                bool result = sendEmail.sendMail(email);
                flag += 1;
            }
            return Json(true);
        }
        return Json(true);
    }
}
