using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using QcSupplier.Constants;
using QcSupplier.ViewModels;

namespace QcSupplier.Validators {
  public class VendorAttribute : ValidationAttribute {
    protected override ValidationResult IsValid(object value, ValidationContext context) {
      var roles = context.ObjectType.GetProperty("Roles").GetValue(context.ObjectInstance) as IList<UserSelection>;
      var vendorOrAdminRole = roles.Any(r => (r.Id == Roles.Vendor.Id || r.Id == Roles.Admin.Id) && r.IsSelected);
      var vendors = value as IList<UserSelection>;
      var vendorSelected = vendors.Any(r => r.IsSelected);
      return vendorOrAdminRole && !vendorSelected ? new ValidationResult(ErrorMessage) : ValidationResult.Success;
    }
  }
}