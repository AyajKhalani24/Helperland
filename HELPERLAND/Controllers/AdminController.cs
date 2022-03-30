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
public class AdminController : Controller
{
    private readonly HelperlandContext context;
    private readonly SendEmail sendEmail;

    public AdminController(HelperlandContext context, SendEmail sendEmail)
    {
        this.context = context;
        this.sendEmail = sendEmail;
    }
    public IActionResult ServiceRequest()
    {
        var result = (
            from sa in context.ServiceRequestAddresses
            join s in context.Users on new { ServiceProviderId = sa.ServiceRequest.ServiceProviderId.ToString() } equals new { ServiceProviderId = s.UserId.ToString() } into s_join
            from s in s_join.DefaultIfEmpty()
            join r in (
                (
                    from rt in context.Ratings
                    group rt by new
                    {
                        rt.RatingTo
                    } into g
                    select new
                    {
                        AvgRatings = (decimal?)g.Average(p => p.Ratings),
                        g.Key.RatingTo
                    })
            ) on new { ServiceProviderId = sa.ServiceRequest.ServiceProviderId.ToString() } equals new { ServiceProviderId = r.RatingTo.ToString() } into r_join
            from r in r_join.DefaultIfEmpty()
            select new AdminSRViewmodel
            {
                ServiceId = sa.ServiceRequest.ServiceId,
                Refund = sa.ServiceRequest.RefundedAmount,
                Payment = sa.ServiceRequest.TotalCost,
                User_First_Name = sa.ServiceRequest.User.FirstName,
                User_Last_Name = sa.ServiceRequest.User.LastName,
                SP_First_Name = s.FirstName,
                SP_Last_Name = s.LastName,
                SP_Id = s.UserId,
                ServiceDate = sa.ServiceRequest.ServiceStartDate,
                TotalHours = sa.ServiceRequest.ServiceHours,
                StreetName = sa.AddressLine1,
                HouseNumber = sa.AddressLine2,
                PostalCode = sa.PostalCode,
                City = sa.City,
                ProfilePhoto = s.UserProfilePicture,
                SP_Avg_Ratings = r.AvgRatings != null ? (decimal)r.AvgRatings : 0,
                ServiceStatus = sa.ServiceRequest.Status != null ? Convert.ToInt32(sa.ServiceRequest.Status) : 0
            }).ToList();

        var SP_Names = (
            from s in context.ServiceRequests
            join u in context.Users
            on s.ServiceProviderId equals u.UserId
            select u.FirstName + " " + u.LastName
        ).Distinct().ToList();
        var User_Names = (
            from u in context.Users
            join s in context.ServiceRequests
            on u.UserId equals s.UserId
            select u.FirstName + " " + u.LastName
        ).Distinct().ToList();
        var model = new AdminSRViewmodel()
        {
            viewModels = result,
            SP_Names = SP_Names,
            Cust_Names = User_Names
        };
        return View(model);
    }

    public IActionResult UserManagement()
    {
        var result = (
            from u in context.Users
            join z in (
                (
                    from zt in context.Zipcodes
                    select new
                    {
                        ZipCode = zt.ZipcodeValue,
                        City = zt.City.CityName
                    })
            ) on u.ZipCode equals z.ZipCode into z_join
            from z in z_join.DefaultIfEmpty()
            select new AdminUMViewmodel
            {
                UserId = u.UserId,
                FirstName = u.FirstName,
                LastName = u.LastName,
                UserTypeId = u.UserTypeId,
                RegisteredDate = u.CreatedDate,
                PhoneNumber = u.Mobile,
                ZipCode = u.ZipCode,
                IsApproved = u.IsApproved,
                IsRegisteredUser = u.IsRegisteredUser,
                City = z.City
            }).ToList();
        var model = new AdminUMViewmodel()
        {
            viewModels = result,
            Names = context.Users.Select(s => s.FirstName + " " + s.LastName).ToList()
        };
        return View(model);
    }
    public IActionResult Activate(int UserId, bool Isapprove)
    {
        try
        {
            var user = context.Users.Where(a => a.UserId == UserId).FirstOrDefault();
            if (user != null)
            {
                user.IsApproved = !Isapprove;
                context.Users.Attach(user);
                context.SaveChanges();
                return Json(new { msg = "Successfully Changed" });
            }
            return Json(new { msg = "Enternal Server error" });
        }
        catch
        {
            return Json(false);
        }
    }

    [HttpPost]
    public IActionResult EditOrRescheduleService([FromBody] AdminRescheduleViewModel model)
    {
        //For who Edited service(ModifiedBy)
        int UserId = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        if (UserId != 0)
        {
            var service = context.ServiceRequests.Where(s => s.ServiceId == model.ServiceId).FirstOrDefault();

            //For sending Mail To Customer
            var user = context.Users.Where(u => u.UserId == service.UserId).FirstOrDefault();
            if (service != null)
            {
                DateTime newServiceDate = DateTime.Parse(model.RescheduleDate).AddHours((double)model.RescheduleTime);
                DateTime newServiceEnd = newServiceDate.AddHours(service.ServiceHours);
                if (newServiceEnd.Hour + newServiceEnd.Minute / 60 > 21)
                {
                    return Json(new { err = "Rescheduled Time must not be greater than 9:00 PM !" });
                }
                else if (DateTime.Now.CompareTo(newServiceDate) > 0)
                {
                    return Json(new { err = "Rescheduled Time is is already passed !" });
                }
                else if (service.ServiceProviderId != null)
                {
                    var services = context.ServiceRequests.Where(s => s.ServiceProviderId == service.ServiceProviderId && s.Status == 2 && s.ServiceRequestId != service.ServiceRequestId && s.ServiceStartDate.Date == newServiceDate.Date && s.ServiceStartDate.Month == newServiceDate.Month && s.ServiceStartDate.Year == newServiceDate.Year).ToList();
                    if (services.Count() > 0)
                    {
                        foreach (var s in services)
                        {
                            var tempServiceStart = s.ServiceStartDate;
                            var tempServiceEnd = s.ServiceStartDate.AddHours((double)(service.ServiceHours + 1));
                            if ((newServiceEnd.CompareTo(tempServiceStart) > 0 && newServiceEnd.CompareTo(tempServiceEnd) <= 0) || (newServiceDate.CompareTo(tempServiceStart) >= 0 && newServiceDate.CompareTo(tempServiceEnd) < 0) || ((tempServiceStart.CompareTo(newServiceDate) >= 0 && tempServiceEnd.CompareTo(newServiceEnd) < 0)))
                            {
                                return Json(new
                                {
                                    err = "Another service request has been assigned to the provider on " + tempServiceStart.ToString("dd/MM/yyyy") +
                                " from " + tempServiceStart.ToString("HH/mm") + " to " + tempServiceEnd.AddHours(-1).ToString("HH/mm") + ". Either choose another date or pick up a different time slot."
                                });
                            }
                        }
                    }
                }
                service.ServiceStartDate = newServiceDate;
                service.RecordVersion = Guid.NewGuid();
                service.ModifiedBy = UserId;
                service.ModifiedDate = DateTime.Now;
                service.Comments = model.RescheduleReason;
                context.ServiceRequests.Attach(service);
                var sa = context.ServiceRequestAddresses.Where(a => a.ServiceRequestId == service.ServiceRequestId).FirstOrDefault();
                if (sa != null)
                {
                    sa.AddressLine1 = model.StreetName;
                    sa.AddressLine2 = model.HouseNumber;
                    sa.City = model.City;
                    sa.PostalCode = model.PostalCode;
                    context.ServiceRequestAddresses.Attach(sa);
                }
                else return Json(new { err = "Address Not Found !" });

                context.SaveChanges();
                var email = new EmailViewmodel()
                {
                    To = user.Email,
                    Subject = "Reschedule Service",
                    isHTML = true,
                    Body = $"<p>The Service {service.ServiceId} that you want to Reschedule is Reschedule by Admin on {service.ServiceStartDate.ToString("MM/dd/yyyy")} at {service.ServiceStartDate.ToString("HH:mm")}</p>",
                };
                bool result = sendEmail.sendMail(email);
                if (result)
                {
                    return Json(new { success = "Successfully Rescheduled Service !!" });
                }
                else
                {
                    return BadRequest(error: "Internal Server Error");
                }
            }
        }
        return Json(new { err = "Service Not Found !" });
    }

    public IActionResult RefundPayment([FromBody] AdminRefundViewModel model)
    {
        try
        {
            var service = context.ServiceRequests.Where(s => s.ServiceId == model.ServiceId).FirstOrDefault();
            if (service.RefundedAmount == null)
            {
                service.RefundedAmount = model.RefundAmount;
            }
            else
            {
                service.RefundedAmount += model.RefundAmount;
            }
            context.ServiceRequests.Attach(service);
            context.SaveChanges();
            return Json(true);
        }
        catch
        {
            return Json(false);
        }
    }
}