using System.ComponentModel.DataAnnotations;

namespace QcSupplier.ViewModels {
  public class Login {
    [Required]
    [Display(Name = "User Name")]
    public string UserName { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Display(Name = "Remember me")]
    public bool RememberMe { get; set; }
  }
}