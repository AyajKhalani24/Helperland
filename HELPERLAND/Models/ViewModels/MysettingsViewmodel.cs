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
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public string PhoneNumber { get; set; }
    [Required]
    public int DobDate { get; set; }
    [Required]
    public int DobMonth { get; set; }
    [Required]
    public int DobYear { get; set; }
    public int? PreLanguage { get; set; }

}
public class ChangepasswordViewmodel
{
#nullable disable
    [Required(ErrorMessage = "*Please Enter Your Old Password")]
    public string OldPassword { get; set; }

    [Required(ErrorMessage = "*Please Enter Your New Password")]
    public string NewPassword { get; set; }

    [Required(ErrorMessage = "*Please Enter Your Confirm Password")]
    [Compare("NewPassword", ErrorMessage = "Password is not matching")]

#nullable enable
    public string? ConfirmPassword { get; set; }
}