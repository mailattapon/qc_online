using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using QcSupplier.Constants;
using QcSupplier.ViewModels;

namespace QcSupplier.Validators {
  public class ProgramAttribute : ValidationAttribute {
    protected override ValidationResult IsValid(object value, ValidationContext context) {
      var roles = context.ObjectType.GetProperty("Roles").GetValue(context.ObjectInstance) as IList<UserSelection>;
      var VendorOrAdminRole = roles.Any(r => (r.Id == Roles.Vendor.Id || r.Id == Roles.Admin.Id) && r.IsSelected);
      var programs = value as IList<UserSelection>;
      var ProgramSelected = programs.Any(r => r.IsSelected);
      return VendorOrAdminRole && !ProgramSelected ? new ValidationResult(ErrorMessage) : ValidationResult.Success;
    }
  }
}