using System;
using System.Collections.Generic;

namespace QcSupplier.Entities {
  public class OutGoingData {
    #region Property

    public int Id { get; set; }
    public int VendorId { get; set; }
    public string Title { get; set; }
    public string Detail { get; set; }
    public string Invoice { get; set; }
    public bool Judgemented { get; set; }
    public int CreatorId { get; set; }
    public int? UpdaterId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public virtual User Creator { get; set; }
    public virtual User Updater { get; set; }
    public virtual Vendor Vendor { get; set; }
    public virtual IList<OutGoingDataFile> Files { get; set; }

    #endregion

    #region Constructor

    public OutGoingData() {
      Files = new List<OutGoingDataFile>();
    }

    #endregion
  }
}