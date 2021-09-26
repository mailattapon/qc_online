using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using QcSupplier.Models;
using QcSupplier.Settings;
using Quartz;
using Quartz.Spi;
using TimeZoneConverter;

namespace QcSupplier.Services {
  public class QuartzHostedService : IHostedService {
    #region Field

    private readonly ISchedulerFactory _schedulerFactory;
    private readonly IJobFactory _jobFactory;
    private readonly IEnumerable<JobSchedule> _jobSchedules;

    #endregion

    #region Property

    public IScheduler Scheduler { get; set; }

    #endregion

    #region Constructor

    public QuartzHostedService(
      ISchedulerFactory schedulerFactory,
      IJobFactory jobFactory,
      IEnumerable<JobSchedule> jobSchedules
    ) {
      _schedulerFactory = schedulerFactory;
      _jobSchedules = jobSchedules;
      _jobFactory = jobFactory;
    }

    #endregion

    #region Method

    public async Task StartAsync(CancellationToken cancellationToken) {
      Scheduler = await _schedulerFactory.GetScheduler(cancellationToken);
      Scheduler.JobFactory = _jobFactory;
      foreach (var jobSchedule in _jobSchedules) {
        var job = CreateJob(jobSchedule);
        var trigger = CreateTrigger(jobSchedule);
        await Scheduler.ScheduleJob(job, trigger, cancellationToken);
      }
      await Scheduler.Start(cancellationToken);
    }

    public async Task StopAsync(CancellationToken cancellationToken) {
      await Scheduler?.Shutdown(cancellationToken);
    }

    private static IJobDetail CreateJob(JobSchedule schedule) {
      var jobType = schedule.JobType;
      return JobBuilder
        .Create(jobType)
        .WithIdentity(jobType.FullName)
        .WithDescription(jobType.Name)
        .Build();
    }

    private static ITrigger CreateTrigger(JobSchedule schedule) {
      return TriggerBuilder
        .Create()
        .WithIdentity($"{schedule.JobType.FullName}.trigger")
        .WithCronSchedule(schedule.CronExpression, cron => cron.InTimeZone(TZConvert.GetTimeZoneInfo(AppInfoSetting.TIMEZONE)))
        .WithDescription(schedule.CronExpression)
        .Build();
    }

    #endregion
  }
}