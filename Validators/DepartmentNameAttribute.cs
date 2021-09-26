using System;
using System.ComponentModel.DataAnnotations;
using QcSupplier.Repositories;

namespace QcSupplier.Validators {
  public class DepartmentNameAttribute : ValidationAttribute {
    protected override ValidationResult IsValid(object value, ValidationContext context) {
      if (value == null) {
        return ValidationResult.Success;
      }
      var repo = (DepartmentRepository)context.GetService(typeof(DepartmentRepository));
      var id = Convert.ToInt32(context.ObjectType.GetProperty("Id").GetValue(context.ObjectInstance));
      var data = value.ToString();
      var entity = repo.FindByName(data);
      return entity == null || entity.Id == id ? ValidationResult.Success : new ValidationResult(string.Format(ErrorMessage, data));
    }
  }
}