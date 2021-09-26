using System;
using System.ComponentModel.DataAnnotations;

namespace QcSupplier.ViewModels {
  public class UserList {
    public int Id { get; set; }

    [Display(Name = "User Name")]
    public string UserName { get; set; }

    public string Email { get; set; }

    public string Department { get; set; }

    [Display(Name = "Updated At")]
    public DateTime UpdatedAt { get; set; }
  }
}