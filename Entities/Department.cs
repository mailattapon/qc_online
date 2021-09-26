using System;
using System.Collections.Generic;

namespace QcSupplier.Entities {
  public class Department {
    #region Property

    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public virtual IList<User> Users { get; set; }

    #endregion

    #region Constructor

    public Department() {
      Users = new List<User>();
    }

    #endregion

  }
}