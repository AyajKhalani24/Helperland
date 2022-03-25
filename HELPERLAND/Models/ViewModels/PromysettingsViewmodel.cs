using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HELPERLAND.Models.ViewModels;

public class PromysettingsViewmodel
{
#nullable disable
    [Required]
    public ProdetailsViewmodel detailsViewmodel { get; set; }
    public ProchangepasswordViewmodel changepasswordViewmodel { get; set; }
}

public class ProdetailsViewmodel
{
#nullable disable
    [Required(ErrorMessage = "*Please Enter Your First Name")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "*Please Enter Your Last Name")]
    public string LastName { get; set; }

    [Required]
    public string Email { get; set; }

    [Required(ErrorMessage = "*Please Enter Your Phone Number")]
    public string PhoneNumber { get; set; }

    [Required]
    public int DobDate { get; set; }

    [Required]
    public int DobMonth { get; set; }

    [Required]
    public int DobYear { get; set; }

    [Required(ErrorMessage = "*Please Enter Your Street Name")]
    public string StreetName { get; set; }

    [Required(ErrorMessage = "*Please Enter Your House Number")]
    public string HouseNumber { get; set; }

    [Required(ErrorMessage = "*Please Enter Your Postal Code")]
    public string PostalCode { get; set; }

    [Required(ErrorMessage = "*Please Enter Your City Name")]
    public string City { get; set; }

    [Required(ErrorMessage = "*Please Select Your Avatar")]
    public string ProfilePicture { get; set; }

    [Required]
    public int? Nationality { get; set; }

    [Required(ErrorMessage = "*Please Select Gender")]
    public int? Gender { get; set; }
}
public class ProchangepasswordViewmodel
{
#nullable disable
    [Required(ErrorMessage = "*Please Enter Your Old Password")]
    public string OldPassword { get; set; }


    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$", ErrorMessage = "*Create strong Password")]
    [Required(ErrorMessage = "*Please Enter Your New Password")]
    public string NewPassword { get; set; }

#nullable enable
    [Required(ErrorMessage = "*Please Enter Your Confirm Password")]
    [Compare("NewPassword", ErrorMessage = "Password is not matching")]
    public string? ConfirmPassword { get; set; }
}