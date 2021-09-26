using System.ComponentModel.DataAnnotations;
using QcSupplier.Validators;

namespace QcSupplier.ViewModels {
  public class Credential {
    public int Id { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Password(ErrorMessage = "Invalid Password Format.")]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    [Compare("Password", ErrorMessage = "Password and confirm password do not match.")]
    public string ConfirmPassword { get; set; }
  }
}