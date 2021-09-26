using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using QcSupplier.Constants;
using QcSupplier.Repositories;
using QcSupplier.ViewModels;

namespace QcSupplier.Validators {
  public class UserEmailAttribute : ValidationAttribute {
    #region Field

    private readonly string USED_EMAIL = "Email '{0}' is already in use.";
    private readonly string REQUIRED_EMAIL = "The Email field is required.";

    #endregion

    #region Method

    protected override ValidationResult IsValid(object value, ValidationContext context) {
      var roles = context.ObjectType.GetProperty("Roles").GetValue(context.ObjectInstance) as IList<UserSelection>;
      var isVendorRole = roles.Any(r => r.Id == Roles.Vendor.Id && r.IsSelected);
      if (value == null) {
        return isVendorRole ? ValidationResult.Success : new ValidationResult(REQUIRED_EMAIL);
      }
      var repo = (UserRepository)context.GetService(typeof(UserRepository));
      var id = Convert.ToInt32(context.ObjectType.GetProperty("Id").GetValue(context.ObjectInstance));
      var data = value.ToString();
      var entity = repo.FindByEmail(data);
      return entity == null || entity.Id == id ? ValidationResult.Success : new ValidationResult(string.Format(USED_EMAIL, data));
    }

    #endregion

  }
}