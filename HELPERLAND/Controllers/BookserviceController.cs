using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HELPERLAND.Models.ViewModels;
using HELPERLAND.Models.Implementation;
using HELPERLAND.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

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
        if (PostalCode != null)
        {
            var isAvailable = context.Users.Where(u => u.UserTypeId == 2 && u.ZipCode == PostalCode).FirstOrDefault();
            if (isAvailable != null)
            {
                return Json(true);
            }
        }
        return Json(false);
    }

    // [Authorize]
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
    public async Task<IActionResult> GetAddresses(string ZipCode)
    {
        Console.WriteLine(ZipCode);
        int UserId = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var addresses = context.UserAddresses.Where(a => a.UserId == UserId && a.PostalCode == ZipCode).ToList();
        IEnumerable<ReturnAddressViewModel> add = new List<ReturnAddressViewModel>();
        foreach (var a in addresses)
        {
            ReturnAddressViewModel temp = new ReturnAddressViewModel()
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
            RecordVersion = Guid.NewGuid()
        };
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
        double x = newServiceRequest.ServiceHours;
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
}



public class ReturnAddressViewModel
{
#nullable disable
    public int AddressId { get; set; }
    public string AddressLine1 { get; set; }
    public string AddressLine2 { get; set; }
    public string PostalCode { get; set; }
    public string PhoneNumber { get; set; }
    public string City { get; set; }
}
