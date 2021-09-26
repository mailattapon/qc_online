using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using QcSupplier.Settings;

namespace QcSupplier.Validators {
  public class PasswordAttribute : ValidationAttribute {
    protected override ValidationResult IsValid(object value, ValidationContext context) {
      if (value == null) {
        return ValidationResult.Success;
      }
      var setting = (PasswordPolicySetting)context.GetService(typeof(PasswordPolicySetting));
      var data = value.ToString();
      return Regex.IsMatch(value.ToString(), setting.PasswordRegex) ? ValidationResult.Success : new ValidationResult(ErrorMessage);
    }
  }
}