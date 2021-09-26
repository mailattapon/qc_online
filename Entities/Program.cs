using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace QcSupplier.Entities {
  public class Program {
    #region Property

    public int Id { get; set; }
    public string Name { get; set; }
    public string PolicyName { get; set; }
    public bool Enabled { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public virtual IList<UserProgram> UserPrograms { get; set; }

    #endregion

    #region Constructor

    public Program() {
      UserPrograms = new List<UserProgram>();
    }

    #endregion

  }
}