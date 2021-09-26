using System;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;

namespace QcSupplier.Infrastructures {
  public class JobFactory : IJobFactory {
    #region Field

    private readonly IServiceProvider _serviceProvider;

    #endregion

    #region Constructor

    public JobFactory(IServiceProvider serviceProvider) {
      _serviceProvider = serviceProvider;
    }

    #endregion

    #region Method

    public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler) {
      return _serviceProvider.GetRequiredService<JobRunner>();
    }

    public void ReturnJob(IJob job) { }

    #endregion

  }
}