using System;
using System.ComponentModel.DataAnnotations;
using QcSupplier.Validators;

namespace QcSupplier.ViewModels {
  public class VendorModel {
    public int Id { get; set; }

    [Required]
    [VendorName (ErrorMessage = "Name '{0}' is already in use.")]
    public string Name { get; set; }

    [Required]
    [EmailAddress]
    [VendorEmail (ErrorMessage = "Email '{0}' is already in use.")]
    public string Email { get; set; }

    [Display (Name = "Updated At")]
    public DateTime UpdatedAt { get; set; }

    [Display (Name = "Send Self IPP")]
    public bool SendIPP { get; set; }

    //2021/09/15 lattapon
    [Required]
    [Display(Name = "Vendor Abbr")]
    public string VendorAbbr { get; set; }
    }
}