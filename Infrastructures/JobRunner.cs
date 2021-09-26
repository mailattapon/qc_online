using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace QcSupplier.Infrastructures {
  public class JobRunner : IJob {
    #region Field

    private readonly IServiceProvider _serviceProvider;

    #endregion

    #region Constructure

    public JobRunner(IServiceProvider serviceProvider) {
      _serviceProvider = serviceProvider;
    }

    #endregion

    #region Method

    public async Task Execute(IJobExecutionContext context) {
      using (var scope = _serviceProvider.CreateScope()) {
        var jobType = context.JobDetail.JobType;
        var job = scope.ServiceProvider.GetRequiredService(jobType) as IJob;
        await job.Execute(context);
      }
    }

    #endregion

  }
}