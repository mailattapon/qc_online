using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace QcSupplier.Entities {
  public class Role : IdentityRole<int> {
    #region Property

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public virtual IList<UserRole> UserRoles { get; set; }

    #endregion

    #region Constructor 

    public Role() {
      UserRoles = new List<UserRole>();
    }

    #endregion

  }
}