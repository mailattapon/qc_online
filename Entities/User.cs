using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace QcSupplier.Entities {
  public class User : IdentityUser<int> {
    #region Property

    public int? DepartmentId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public virtual Department Department { get; set; }
    public virtual IList<UserRole> UserRoles { get; set; }
    public virtual IList<UserProgram> UserPrograms { get; set; }
    public virtual IList<UserVendor> UserVendors { get; set; }
    public virtual IList<SupplierEvaluation> SupplierEvaluationCreators { get; set; }
    public virtual IList<SupplierEvaluation> SupplierEvaluationUpdaters { get; set; }
    public virtual IList<Download> DownloadCreators { get; set; }
    public virtual IList<Download> DownloadUpdaters { get; set; }
    public virtual IList<TnsForm> TnsFormCreators { get; set; }
    public virtual IList<TnsForm> TnsFormUpdaters { get; set; }
    public virtual IList<SelfControlledIPP> SelfControlledIPPCreators { get; set; }
    public virtual IList<SelfControlledIPP> SelfControlledIPPUpdaters { get; set; }
    public virtual IList<SupplierInformation> SupplierInformationCreators { get; set; }
    public virtual IList<SupplierInformation> SupplierInformationUpdaters { get; set; }
    public virtual IList<OutGoingData> OutGoingDataCreators { get; set; }
    public virtual IList<OutGoingData> OutGoingDataUpdaters { get; set; }
    public virtual IList<OutGoingDataFile> OutGoingDataFileCreators { get; set; }
    public virtual IList<OutGoingDataFile> OutGoingDataFileUpdaters { get; set; }
    public virtual IList<OutGoingDataFile> OutGoingDataFileViewers { get; set; }
    public virtual IList<OutGoingDataFile> OutGoingDataFileJudgementors { get; set; }
    //2021/09/14 lattapon
    public virtual IList<OutGoingDatas_New> OutGoingDatasNew { get; set; }

        #endregion

        #region Constructor

    public User() {
      UserRoles = new List<UserRole>();
      UserPrograms = new List<UserProgram>();
      UserVendors = new List<UserVendor>();
      SupplierEvaluationCreators = new List<SupplierEvaluation>();
      SupplierEvaluationUpdaters = new List<SupplierEvaluation>();
      DownloadCreators = new List<Download>();
      DownloadUpdaters = new List<Download>();
      TnsFormCreators = new List<TnsForm>();
      TnsFormUpdaters = new List<TnsForm>();
      SelfControlledIPPCreators = new List<SelfControlledIPP>();
      SelfControlledIPPUpdaters = new List<SelfControlledIPP>();
      SupplierInformationCreators = new List<SupplierInformation>();
      SupplierInformationUpdaters = new List<SupplierInformation>();
      OutGoingDataCreators = new List<OutGoingData>();
      OutGoingDataUpdaters = new List<OutGoingData>();
      OutGoingDataFileCreators = new List<OutGoingDataFile>();
      OutGoingDataFileUpdaters = new List<OutGoingDataFile>();
      OutGoingDataFileViewers = new List<OutGoingDataFile>();
      OutGoingDataFileJudgementors = new List<OutGoingDataFile>();
      OutGoingDatasNew = new List<OutGoingDatas_New>();
    }

    #endregion

  }
}