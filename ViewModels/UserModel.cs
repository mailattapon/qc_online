using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using QcSupplier.Validators;

namespace QcSupplier.ViewModels {
  public class UserModel {
    #region Property

    public int Id { get; set; }

    [Required]
    [Display (Name = "User Name")]
    [UserName (ErrorMessage = "User Name '{0}' is already in use.")]
    public string UserName { get; set; }

    [EmailAddress]
    [UserEmail]
    public string Email { get; set; }

    [Display (Name = "Department")]
    public int? DepartmentId { get; set; }

    [Role (ErrorMessage = "At least 1 role must be selected.")]
    public IList<UserSelection> Roles { get; set; }

    [Program (ErrorMessage = "At least 1 program must be selected.")]
    public IList<UserSelection> Programs { get; set; }

    [Vendor (ErrorMessage = "At least 1 vendor must be selected.")]
    public IList<UserSelection> Vendors { get; set; }

    public IList<SelectListItem> Departments { get; set; }

    #endregion

    #region Constructor

    public UserModel () {
      Roles = new List<UserSelection> ();
      Programs = new List<UserSelection> ();
      Vendors = new List<UserSelection> ();
      Departments = new List<SelectListItem> ();
    }

    #endregion

  }
}