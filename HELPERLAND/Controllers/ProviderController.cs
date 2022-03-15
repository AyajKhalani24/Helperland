using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HELPERLAND.Models.ViewModels;
using HELPERLAND.Models.Implementation;
using HELPERLAND.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using BCrypto = BCrypt.Net.BCrypt;

namespace HELPERLAND.Controllers;
#nullable disable

public class ProviderController : Controller
{
    private readonly HelperlandContext context;

    public ProviderController(HelperlandContext context)
    {
        this.context = context;
    }

    [HttpGet]
    public IActionResult NewServiceRequest(bool HasPets)
    {
        try
        {
            int ServiceproId = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var result = (
            from u in context.Users
            join s in context.ServiceRequests
            on u.UserId equals s.UserId
            join sa in context.ServiceRequestAddresses
            on s.ServiceRequestId equals sa.ServiceRequestId
            where s.Status == 1 && s.HasPets == HasPets
            select new UpcomingserviceViewmodel
            {
                UserId = s.UserId,
                ServiceId = s.ServiceId,
                CustomerName = u.FirstName + " " + u.LastName,
                ServiceDate = s.ServiceStartDate,
                ServiceHours = s.ServiceHours,
                StreetName = sa.AddressLine1,
                HouseNumber = sa.AddressLine2,
                City = sa.City,
                PostalCode = sa.PostalCode,
                Payment = s.TotalCost,
                RecordVersion = s.RecordVersion.ToString()
            }
         ).ToList();

            var blocked = context.FavoriteAndBlockeds.Where(fab => (fab.UserId == ServiceproId || fab.TargetUserId == ServiceproId) && fab.IsBlocked);
            foreach (var s in result)
            {
                foreach (var b in blocked)
                {
                    if ((b.UserId == ServiceproId && b.TargetUserId == s.UserId) || (b.UserId == s.UserId && b.TargetUserId == ServiceproId))
                        result.Remove(s);
                }
            }
            return View(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return Json("Internal Server Error !");
        }
    }


    public IActionResult MyRatings()
    {
        try
        {
            int ServiceproId = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var result = (
                from u in context.Users
                join s in context.ServiceRequests
                on u.UserId equals s.UserId
                join r in context.Ratings
                on s.ServiceRequestId equals r.ServiceRequestId
                where s.ServiceProviderId == ServiceproId && s.Status == 3 && r.RatingTo == ServiceproId && r.ServiceRequestId != 0
                select new MyratingsViewModel
                {
                    ServiceId = s.ServiceId,
                    ServiceDate = s.ServiceStartDate,
                    ServiceHours = s.ServiceHours,
                    CustomerName = u.FirstName + " " + u.LastName,
                    Ratings = r.Ratings,
                    Comments = r.Comments
                }
            ).ToList();

            return View(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return Json(new { err = "Internal Server Error !" });
        }
    }

    [HttpGet]
    public IActionResult MySettings()
    {
        int UserId = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var result = (
                from u in context.Users
                join uat in context.UserAddresses
                on u.UserId equals uat.UserId into x
                from ua in x.DefaultIfEmpty()
                where u.UserId == UserId
                select new { u, ua }
            ).FirstOrDefault();
        var model = new ProdetailsViewmodel()
        {
            FirstName = result.u.FirstName,
            LastName = result.u.LastName,
            Email = result.u.Email,
            PhoneNumber = result.u.Mobile,
            Nationality = result.u.NationalityId,
            Gender = result.u.Gender,
            StreetName = result.ua != null ? result.ua.AddressLine1 : "",
            HouseNumber = result.ua != null ? result.ua.AddressLine2 : "",
            PostalCode = result.u.ZipCode,
            City = result.ua != null ? result.ua.City : "",
            DobDate = result.u.DateOfBirth != null ? result.u.DateOfBirth.Value.Day : 0,
            DobMonth = result.u.DateOfBirth != null ? result.u.DateOfBirth.Value.Month : 0,
            DobYear = result.u.DateOfBirth != null ? result.u.DateOfBirth.Value.Year : 0,
            ProfilePicture = result.u.UserProfilePicture != null ? result.u.UserProfilePicture : "",
        };
        var newModel = new PromysettingsViewmodel()
        {
            detailsViewmodel = model
        };
        return View(newModel);
    }

    [HttpPost]
    public IActionResult changedetail([FromBody] ProdetailsViewmodel model)
    {
        Console.WriteLine(model.Email);
        try
        {
            int ServiceproId = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var sp = context.Users.Where(u => u.UserId == ServiceproId).FirstOrDefault();
            var sa = context.UserAddresses.Where(u => u.UserId == ServiceproId).FirstOrDefault();
            sp.FirstName = model.FirstName;
            sp.LastName = model.LastName;
            sp.Email = model.Email;
            sp.Mobile = model.PhoneNumber;
            sp.NationalityId = model.Nationality;
            sp.Gender = model.Gender;
            sp.ModifiedBy = ServiceproId;
            sp.ModifiedDate = DateTime.Now;
            sp.UserProfilePicture = model.ProfilePicture;
            sp.ZipCode = model.PostalCode;
            sp.DateOfBirth = new DateTime(model.DobYear, model.DobMonth, model.DobDate);

            if (sa != null)
            {
                sa.AddressLine1 = model.StreetName;
                sa.AddressLine2 = model.HouseNumber;
                sa.PostalCode = model.PostalCode;
                sa.City = model.City;
                sa.Mobile = model.PhoneNumber;
                context.UserAddresses.Attach(sa);
            }
            else
            {
                var userAddress = new UserAddress()
                {
                    Email = sp.Email,
                    UserId = ServiceproId,
                    AddressLine1 = model.StreetName,
                    AddressLine2 = model.HouseNumber,
                    PostalCode = model.PostalCode,
                    City = model.City,
                    Mobile = model.PhoneNumber
                };
                context.UserAddresses.Add(userAddress);
            }
            context.SaveChanges();
            return Json(true);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return Json(false);
        }
    }

    public IActionResult ServiceHistory()
    {
        int ServiceproId = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var user = context.Users.Where(u => u.UserId == ServiceproId).FirstOrDefault();

        IEnumerable<ProServicehistoryViewmodel> result = (
            from u in context.Users
            join s in context.ServiceRequests
            on u.UserId equals s.UserId
            where s.ServiceProviderId == ServiceproId && (s.Status == 3)
            select new ProServicehistoryViewmodel
            {
                ServiceId = s.ServiceId,
                ServiceRequestId = s.ServiceRequestId,
                ServiceStartTime = s.ServiceStartDate,
                ServiceHours = s.ServiceHours,
                CustomerId = s.ServiceProviderId,
                CustomerName = u.FirstName + " " + u.LastName,
                Payment = s.TotalCost,
                Status = s.Status
            }
            ).ToList();
        if (result.Count() > 0)
        {
            foreach (var add in result)
            {
                var model = context.ServiceRequestAddresses.Where(x => x.ServiceRequestId == add.ServiceRequestId).FirstOrDefault();
                add.Addressline1 = model.AddressLine1;
                add.Addressline2 = model.AddressLine2;
                add.City = model.City;
                add.PostalCode = model.PostalCode;
                add.PhoneNumber = model.Mobile;
            }
        }
        return View(result);
    }

    [HttpGet]
    public IActionResult AcceptService(int ServiceId, string RecordVersion)
    {
        try
        {
            int ServiceproId = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            Console.WriteLine(ServiceId);
            Console.WriteLine(RecordVersion);

            var service = context.ServiceRequests.Where(s => s.ServiceId == ServiceId).FirstOrDefault();
            if (service != null)
            {
                if (service.RecordVersion.ToString() == RecordVersion.ToLower())
                {
                    var result = context.ServiceRequests.Where(s => s.ServiceProviderId == ServiceproId && s.ServiceStartDate.Day == service.ServiceStartDate.Day && s.ServiceStartDate.Month == service.ServiceStartDate.Month && s.ServiceStartDate.Year == service.ServiceStartDate.Year && s.Status == 2).ToList();
                    if (result.Count() > 0)
                    {
                        var newServiceDate = service.ServiceStartDate;
                        var newServiceEnd = service.ServiceStartDate.AddHours(service.ServiceHours);
                        foreach (var s in result)
                        {
                            var tempServiceStart = s.ServiceStartDate;
                            var tempServiceEnd = s.ServiceStartDate.AddHours((double)(service.ServiceHours + 1));
                            if ((newServiceEnd.CompareTo(tempServiceStart) > 0 && newServiceEnd.CompareTo(tempServiceEnd) <= 0) || (newServiceDate.CompareTo(tempServiceStart) >= 0 && newServiceDate.CompareTo(tempServiceEnd) < 0) || ((tempServiceStart.CompareTo(newServiceDate) >= 0 && tempServiceEnd.CompareTo(newServiceEnd) < 0)))
                            {
                                return Json(new { responce = 2 });
                            }
                        }
                    }
                    service.Status = 2;
                    service.ServiceProviderId = ServiceproId;
                    service.SPAcceptedDate = DateTime.Now;
                    service.ModifiedBy = ServiceproId;
                    service.ModifiedDate = DateTime.Now;
                    service.RecordVersion = Guid.NewGuid();
                    context.ServiceRequests.Attach(service);
                    context.SaveChanges();
                    return Json(new { responce = 1 });
                }
                else
                {
                    return Json(new { responce = 3 });
                }
            }
            return Json(new { responce = 3 });
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return Json(new { err = "Internal Server Error !" });
            throw;
        }
    }

    public IActionResult CompleteRequest(int serviceId)
    {

        int ServiceproId = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        var service = context.ServiceRequests.Where(s => s.ServiceId == serviceId && s.Status == 2).FirstOrDefault();
        if (service != null)
        {
            service.Status = 3;
            context.ServiceRequests.Attach(service);
            context.SaveChanges();
            return Json(new { responce = 1, status = service.Status });
        }
        return Json(new { responce = 2 });
    }

    public IActionResult CancelRequest(int serviceId)
    {

        int ServiceproId = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        var service = context.ServiceRequests.Where(s => s.ServiceId == serviceId && s.Status == 2).FirstOrDefault();
        if (service != null)
        {
            service.Status = 1;
            service.ServiceProviderId = null;
            context.ServiceRequests.Attach(service);
            context.SaveChanges();
            return Json(new { responce = 1, status = service.Status });
        }
        return Json(new { responce = 2 });
    }
    [HttpGet]
    public IActionResult UpcomingService()
    {
        try
        {
            int ServiceproId = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var result = (
                from u in context.Users
                join s in context.ServiceRequests
                on u.UserId equals s.UserId
                join sa in context.ServiceRequestAddresses
                on s.ServiceRequestId equals sa.ServiceRequestId
                where s.ServiceProviderId == ServiceproId && s.Status == 2
                select new UpcomingserviceViewmodel
                {
                    ServiceId = s.ServiceId,
                    CustomerName = u.FirstName + " " + u.LastName,
                    PhoneNumber = u.Mobile,
                    ServiceDate = s.ServiceStartDate,
                    ServiceHours = s.ServiceHours,
                    StreetName = sa.AddressLine1,
                    HouseNumber = sa.AddressLine2,
                    City = sa.City,
                    PostalCode = sa.PostalCode,
                    Payment = s.TotalCost
                }
            ).ToList();
            return View(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return Json("Internal Server Error !");
        }
    }

    [HttpGet]
    public IActionResult BlockCustomer()
    {
        try
        {
            int ServiceproId = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var result = (
                from u in context.Users
                join s in context.ServiceRequests
                on u.UserId equals s.UserId
                join fab in context.FavoriteAndBlockeds on
                new { ServiceProviderId = s.ServiceProviderId.ToString(), UserId = s.UserId.ToString() }
                                   equals new { ServiceProviderId = fab.UserId.ToString(), UserId = fab.TargetUserId.ToString() } into x
                from fab in x.DefaultIfEmpty()
                where s.ServiceProviderId == ServiceproId && s.Status == 3
                select new BlockcustomerViewmodel
                {
                    UserId = u.UserId,
                    IsBlocked = fab.IsBlocked ? fab.IsBlocked : false,
                    CustomerName = u.FirstName + " " + u.LastName
                }
            ).Distinct().ToList();
            return View(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return Json(new { err = "Internal Server Error !" });
        }
    }
    [HttpPost]
    public IActionResult BlockCustomer([FromBody] BlockcustomerViewmodel model)
    {
        try
        {
            int ServiceproId = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var result = context.FavoriteAndBlockeds.Where(fab => fab.TargetUserId == model.UserId && fab.UserId == ServiceproId).FirstOrDefault();
            if (result != null)
            {
                result.IsBlocked = model.IsBlocked;
                context.Attach(result);
            }
            else
            {
                var newFab = new FavoriteAndBlocked()
                {
                    UserId = ServiceproId,
                    TargetUserId = model.UserId,
                    IsBlocked = model.IsBlocked,
                    IsFavorite = false
                };
                context.FavoriteAndBlockeds.Add(newFab);
            }
            context.SaveChanges();
            return Json(new { success = model.IsBlocked ? "Successfully Blocked Customer !" : "Successfully Unblocked Customer !", isBlocked = model.IsBlocked });
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return Json(new { err = "Internal Server Error !" });
        }
    }

}