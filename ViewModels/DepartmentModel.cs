using System;
using System.ComponentModel.DataAnnotations;
using QcSupplier.Validators;

namespace QcSupplier.ViewModels {
  public class DepartmentModel {
    public int Id { get; set; }

    [Required]
    [DepartmentName(ErrorMessage = "Name '{0}' is already in use.")]
    public string Name { get; set; }

    [Display(Name = "Updated At")]
    public DateTime UpdatedAt { get; set; }
  }
}