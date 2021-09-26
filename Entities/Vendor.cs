using System;
using System.Collections.Generic;

namespace QcSupplier.Entities {
  public class Vendor {
    #region Property

    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public bool SendIPP { get; set; }

    //2021/09/15 lattapon
    public string VendorAbbr { get; set; }

    public virtual DateTime CreatedAt { get; set; }
    public virtual DateTime UpdatedAt { get; set; }
    public virtual IList<UserVendor> UserVendors { get; set; }
    public virtual IList<SupplierEvaluation> SupplierEvaluations { get; set; }
    public virtual IList<SelfControlledIPP> SelfControlledIPPs { get; set; }
    public virtual IList<SupplierInformation> SupplierInformations { get; set; }
    public virtual IList<OutGoingData> OutGoingDatas { get; set; }

    #endregion

    #region Constructor 

    public Vendor() {
      UserVendors = new List<UserVendor>();
      SupplierEvaluations = new List<SupplierEvaluation>();
      SelfControlledIPPs = new List<SelfControlledIPP>();
      SupplierInformations = new List<SupplierInformation>();
      OutGoingDatas = new List<OutGoingData>();
    }

    #endregion

  }
}