using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HELPERLAND.Models.ViewModels;

public class MysettingsViewmodel
{
#nullable disable
    [Required]
    public DetailsViewmodel detailsViewmodel { get; set; }
    public AddaddressViewmodel addaddressViewmodel { get; set; }
    public ChangepasswordViewmodel changepasswordViewmodel { get; set; }
}

public class DetailsViewmodel
{
#nullable disable
    [Required(ErrorMessage = "*Please enter firstname")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "*Please enter lastname")]
    public string LastName { get; set; }
    [Required]
    public string Email { get; set; }
    [Required(ErrorMessage = "*Please enter phonenumber")]
    public string PhoneNumber { get; set; }
    [Required]
    public int DobDate { get; set; }
    [Required]
    public int DobMonth { get; set; }
    [Required]
    public int DobYear { get; set; }
    [Required]
    public int? PreLanguage { get; set; }

}
public class ChangepasswordViewmodel
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