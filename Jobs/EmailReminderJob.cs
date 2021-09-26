using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using QcSupplier.Services;
using Quartz;

namespace QcSupplier.Jobs {
  [DisallowConcurrentExecution]
  public class EmailReminderJob : IJob {
    #region Field

    private readonly EmailService _emailService;
    private readonly ILogger<EmailReminderJob> _logger;

    #endregion

    #region Constructor

    public EmailReminderJob(EmailService emailService, ILogger<EmailReminderJob> logger) {
      _logger = logger;
      _emailService = emailService;
    }

    #endregion

    #region Method

    public async Task Execute(IJobExecutionContext context) {
      _logger.LogInformation("Start sending email reminder");
      await _emailService.SendSupplierEvaluationReminderMailAsync();
      await _emailService.SendSelfControlledIPPReminderMailAsync();
      _logger.LogInformation("Finish sending email reminder");
    }

    #endregion
  }
}