using System;

namespace QcSupplier.Models {
  public class JobSchedule {
    #region Property

    public Type JobType { get; }

    public string CronExpression { get; }

    #endregion

    #region Constructor

    public JobSchedule(Type jobType, string cronExpression) {
      JobType = jobType;
      CronExpression = cronExpression;
    }

    #endregion
  }
}