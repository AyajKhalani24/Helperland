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
public class CustomerController : Controller
{
    private readonly HelperlandContext context;

    public CustomerController(HelperlandContext context)
    {
        this.context = context;
    }
    public IActionResult servicehistory()
    {
        if (User.Identity.IsAuthenticated)
        {
            return View();
        }
        else
        {
            return BadRequest(error: "Access Denied");
        }
    }
    public IActionResult mysettings()
    {
        int UserId = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var user = context.Users.Where(u => u.UserId == UserId).FirstOrDefault();
        var detailsViewmodeltemp = new DetailsViewmodel();
        detailsViewmodeltemp.FirstName = user.FirstName;
        detailsViewmodeltemp.LastName = user.LastName;
        detailsViewmodeltemp.Email = user.Email;
        detailsViewmodeltemp.PhoneNumber = user.Mobile;
        if (user.DateOfBirth != null)
        {
            detailsViewmodeltemp.DobDate = user.DateOfBirth.Value.Day;
            detailsViewmodeltemp.DobMonth = user.DateOfBirth.Value.Month;
            detailsViewmodeltemp.DobYear = user.DateOfBirth.Value.Year;
        }
        if (user.LanguageId != null)
        {
            detailsViewmodeltemp.PreLanguage = user.LanguageId;
        }
        MysettingsViewmodel model = new MysettingsViewmodel()
        {
            detailsViewmodel = detailsViewmodeltemp
        };
        return View(model);
    }

    [HttpPost]
    public IActionResult changepassword([FromBody] ChangepasswordViewmodel model)
    {
        int UserId = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var user = context.Users.Where(u => u.UserId == UserId).FirstOrDefault();

        if (user != null)
        {
            bool verified = BCrypt.Net.BCrypt.Verify(model.OldPassword, user.Password);

            if (verified)
            {
                user.Password = BCrypto.HashPassword(model.NewPassword);
                context.Users.Attach(user);
                context.SaveChanges();
                return Json(true);
            }
        }
        return Json(false);
    }

    [HttpPost]
    public IActionResult changedetail([FromBody] DetailsViewmodel model)
    {
        try
        {
            int UserId = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var user = context.Users.Where(u => u.UserId == UserId).FirstOrDefault();

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.Mobile = model.PhoneNumber;
            user.LanguageId = model.PreLanguage;

            var dob = new DateTime(model.DobYear, model.DobMonth, model.DobDate, 0, 0, 0);
            user.DateOfBirth = dob;
            context.Users.Attach(user);
            context.SaveChanges();
            return Json(true);
        }
        catch
        {
            return Json(false);
        }
    }
    [HttpGet]
    public IActionResult dashboard()
    {
        int UserId = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var user = context.Users.Where(u => u.UserId == UserId).FirstOrDefault();

        var result = (
            from s in context.ServiceRequests
            join u in context.Users
            on s.ServiceProviderId equals u.UserId into x
            from sr in x.DefaultIfEmpty()
            where s.UserId == UserId && (s.Status == 1 || s.Status == 2)
            select new ServicehistoryViewmodel
            {
                ServiceId = s.ServiceId,
                ServiceStartTime = s.ServiceStartDate,
                ServiceHours = s.ServiceHours,
                ServiceProviderId = s.ServiceProviderId,
                ServiceProviderName = sr.FirstName + " " + sr.LastName,
                ServiceProviderProfilePicture = sr.UserProfilePicture,
                Payment = s.TotalCost,
                Status = s.Status
            }
            ).ToList();


        if (result.Count() > 0)
        {
            IEnumerable<int?> distinctServiceProviders = result.Select(x => x.ServiceProviderId).Distinct();
            if (distinctServiceProviders.Count() > 0)
            {
                foreach (var serviceProviderId in distinctServiceProviders)
                {
                    var ratings = context.Ratings.Where(rating => rating.RatingTo == serviceProviderId).ToList();
                    if (ratings.Count() > 0)
                    {
                        var avgRating = ratings.Average(r => r.Ratings);
                        // var servicesWithSP = result.Where(x => x.ServiceProviderId == serviceProviderId).ToList();
                        foreach (var model in result)
                        {
                            if (model.ServiceProviderId == serviceProviderId)
                                model.AvgRating = avgRating;
                        }
                    }
                }
            }
        }
        return View(result);
    }

    [HttpGet]
    public IActionResult ServiceHistory()
    {
        int UserId = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var user = context.Users.Where(u => u.UserId == UserId).FirstOrDefault();

        IEnumerable<ServicehistoryViewmodel> result = (
            from s in context.ServiceRequests
            join u in context.Users
            on s.ServiceProviderId equals u.UserId into x
            from sr in x.DefaultIfEmpty()
            where s.UserId == user.UserId && (s.Status == 4 || s.Status == 3)
            select new ServicehistoryViewmodel
            {
                ServiceId = s.ServiceId,
                ServiceStartTime = s.ServiceStartDate,
                ServiceHours = s.ServiceHours,
                ServiceProviderId = s.ServiceProviderId,
                ServiceProviderName = sr.FirstName + " " + sr.LastName,
                ServiceProviderProfilePicture = sr.UserProfilePicture,
                Payment = s.TotalCost,
                Status = s.Status
            }
            ).ToList();
        if (result.Count() > 0)
        {
            IEnumerable<int?> distinctServiceProviders = result.Where(x => x.ServiceProviderId != null).Select(x => x.ServiceProviderId).Distinct();
            if (distinctServiceProviders.Count() > 0)
            {
                foreach (var serviceProviderId in distinctServiceProviders)
                {
                    var ratings = context.Ratings.Where(rating => rating.RatingTo == serviceProviderId).ToList();
                    if (ratings.Count() > 0)
                    {
                        var avgRating = ratings.Average(r => r.Ratings);
                        // var servicesWithSP = result.Where(x => x.ServiceProviderId == serviceProviderId).ToList();
                        foreach (var model in result)
                        {
                            if (model.ServiceProviderId == serviceProviderId)
                                model.AvgRating = avgRating;
                        }
                    }
                }
            }
        }
        return View(result);
    }
    public IActionResult GetAddresses()
    {
        int UserId = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var addresses = context.UserAddresses.Where(a => a.UserId == UserId).ToList();
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
    public IActionResult EditAddress([FromBody] AddaddressViewmodel model)
    {
        try
        {
            int UserId = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var user = context.Users.Where(u => u.UserId == UserId).FirstOrDefault();
            if (model.AddressId != null)
            {
                var newAddress = context.UserAddresses.Where(a => a.UserId == user.UserId && a.AddressId == model.AddressId).FirstOrDefault();
                if (newAddress != null)
                {
                    newAddress.AddressLine1 = model.StreetName;
                    newAddress.AddressLine2 = model.hno;
                    newAddress.City = model.City;
                    newAddress.PostalCode = model.poc;
                    newAddress.Mobile = model.PhoneNumber;
                    var address = context.SaveChanges();
                    return Json(true);
                }
            }
            return Json(false);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return Json(false);
        }
    }
    [HttpPost]
    public IActionResult CancelService([FromBody] CancelServiceViewModel model)
    {
        try
        {
            int UserId = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var user = context.Users.Where(u => u.UserId == UserId).FirstOrDefault();
            if (user != null)
            {
                var service = context.ServiceRequests.Where(s => s.UserId == UserId && s.ServiceId == model.ServiceId).FirstOrDefault();
                if (service != null)
                {
                    service.Status = 4;
                    service.Comments = model.CancelReason;
                    context.ServiceRequests.Attach(service);
                    context.SaveChanges();
                    return Json(true);
                }
            }
            return Json(false);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return Json(false);
        }
    }

    [HttpPost]
    public IActionResult RescheduleService([FromBody] RescheduleServiceViewModel model)
    {
        try
        {
            int UserId = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var user = context.Users.Where(u => u.UserId == UserId).FirstOrDefault();
            if (user != null)
            {

                var service = context.ServiceRequests.Where(s => s.UserId == user.UserId && s.ServiceId == model.ServiceId).FirstOrDefault();
                if (service != null)
                {
                    DateTime newServiceDate = DateTime.Parse(model.NewServiceDate).AddHours((double)model.NewServiceStartTime);
                    if (newServiceDate.CompareTo(service.ServiceStartDate) == 0)
                    {
                        return Json(new { err = "This selected time is same !" });
                    }
                    DateTime newServiceEnd = newServiceDate.AddHours(service.ServiceHours);
                    var services = context.ServiceRequests.Where(s => s.ServiceProviderId == service.ServiceProviderId && s.Status == 2 && s.ServiceRequestId != service.ServiceRequestId && s.ServiceStartDate.Date == newServiceDate.Date && s.
                    ServiceStartDate.Month == newServiceDate.Month && s.ServiceStartDate.Year == newServiceDate.Year).ToList();

                    if (services.Count() > 0)
                    {
                        foreach (var s in services)
                        {
                            var tempServiceStart = s.ServiceStartDate;
                            var tempServiceEnd = s.ServiceStartDate.AddHours((double)(service.ServiceHours + 1));
                            if ((newServiceEnd.CompareTo(tempServiceStart) > 0 && newServiceEnd.CompareTo(tempServiceEnd) <= 0) || (newServiceDate.CompareTo(tempServiceStart) >= 0 && newServiceDate.CompareTo(tempServiceEnd) < 0) || ((newServiceDate.CompareTo(tempServiceStart) >= 0 && newServiceEnd.CompareTo(tempServiceEnd) < 0)))
                            {
                                return Json(new
                                {
                                    err = "Another service request has been assigned to the service provider on " + tempServiceStart.ToString("dd/MM/yyyy") +
                                " from " + tempServiceStart.ToString("HH/mm") + " to " + tempServiceEnd.AddHours(-1).ToString("HH/mm") + ". Either choose another date or pick up a different time slot."
                                });
                            }
                        }
                    }
                    service.ServiceStartDate = newServiceDate;
                    context.ServiceRequests.Attach(service);
                    context.SaveChanges();
                    return Json(new { success = "Successfully Rescheduled Service !!" });
                }
            }
            return Json(new { err = "Service Not Found !" });
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return Json(new { err = "Internal Server Error !" });
        }
    }


    [HttpGet]
    public IActionResult GetServiceDetails(int serviceId)
    {
        try
        {
            int UserId = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var user = context.Users.Where(u => u.UserId == UserId).FirstOrDefault();

            if (user != null)
            {
                var service = (
                    from s in context.ServiceRequests
                    join sa in context.ServiceRequestAddresses
                    on s.ServiceRequestId equals sa.ServiceRequestId
                    where s.ServiceId == serviceId
                    select new { s, sa }
                ).FirstOrDefault();
                if (service != null)
                {

                    var serviceExtra = context.ServiceRequestExtras.Where(se => se.ServiceRequestId == service.s.ServiceRequestId).Select(sea => sea.ServiceExtraId).ToArray();
                    var jsonData = new
                    {
                        Extras = serviceExtra,
                        Duration = service.s.ServiceHours,
                        ServiceStreetName = service.sa.AddressLine1,
                        ServiceHouseNumber = service.sa.AddressLine2,
                        PostalCode = service.sa.PostalCode,
                        City = service.sa.City,
                        PhoneNumber = service.sa.Mobile,
                        Email = service.sa.Email,
                        Comments = service.s.Comments,
                        HasPets = service.s.HasPets,
                        TotalCleaning = context.ServiceRequests.Where(s => s.ServiceProviderId == service.s.ServiceProviderId && s.Status == 3).Count()
                    };
                    return Json(jsonData);
                }
            }
            return Json(new { err = "Service Not Found !" });
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return Json(new { err = "Internal Server Error !" });
        }
    }
    [HttpGet]
    public IActionResult RatingGivenOrNot(int ServiceId, int ServiceProviderId)
    {
        try
        {
            int UserId = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var user = context.Users.Where(u => u.UserId == UserId).FirstOrDefault();
            if (user != null)
            {
                var serviceRequest = context.ServiceRequests.Where(s => s.ServiceId == ServiceId && s.UserId == user.UserId).FirstOrDefault();
                if (serviceRequest != null)
                {
                    var result = context.Ratings.Where(r => r.RatingFrom == user.UserId && r.RatingTo == ServiceProviderId && r.ServiceRequestId == serviceRequest.ServiceRequestId).FirstOrDefault();
                    if (result != null)
                    {
                        return Json(new
                        {
                            err = true,
                            OnTimeArrival = result.OnTimeArrival,
                            Friendly = result.Friendly,
                            QualityOfService = result.QualityOfService,
                            Comments = result.Comments
                        });
                    }
                    else
                    {
                        return Json(new { success = "No Rating Has Given !!" });
                    }
                }
            }
            return Json(new { err = "Service Request Not Found !!" });
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return Json(new { err = "Internal Server Error !" });
        }
    }

    [HttpPost]
    public IActionResult Rating([FromBody] RatingViewmodel model)
    {
        try
        {
            int UserId = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var user = context.Users.Where(u => u.UserId == UserId).FirstOrDefault();

            if (user != null)
            {
                var service = context.ServiceRequests.Where(s => s.ServiceId == model.ServiceId).FirstOrDefault();
                if (service != null)
                {
                    decimal AverageRating = Math.Round(((decimal)model.FriendlyRating + (decimal)model.QualityOfServiceRating + (decimal)model.OnTimeArrivalRating) / (decimal)3, 2);
                    var newRating = new Rating()
                    {
                        RatingFrom = user.UserId,
                        RatingTo = model.ServiceProviderId,
                        ServiceRequestId = service.ServiceRequestId,
                        Friendly = model.FriendlyRating,
                        QualityOfService = model.QualityOfServiceRating,
                        OnTimeArrival = model.OnTimeArrivalRating,
                        RatingDate = DateTime.Now,
                        Ratings = AverageRating
                    };
                    if (model.Comments != null)
                    {
                        newRating.Comments = model.Comments;
                    }
                    context.Add(newRating);
                    context.SaveChanges();
                    return Json(new { success = "Rating Saved Successfully !!" });
                }
            }
            return Json(new { err = "Service Not Found !" });
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return Json(new { err = "Internal Server Error !!" });
        }
    }
}


