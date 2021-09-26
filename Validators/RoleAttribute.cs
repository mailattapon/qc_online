using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using QcSupplier.ViewModels;

namespace QcSupplier.Validators {
  public class RoleAttribute : ValidationAttribute {
    protected override ValidationResult IsValid(object value, ValidationContext context) {
      var roles = value as IList<UserSelection>;
      var count = roles.Any(r => r.IsSelected);
      return roles.Any(r => r.IsSelected) ? ValidationResult.Success : new ValidationResult(ErrorMessage);
    }
  }
}