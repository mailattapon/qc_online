using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using QcSupplier.Infrastructures;
using QcSupplier.Models;
using QcSupplier.Settings;

namespace QcSupplier.Services {
  public class EmailHostedService : IHostedService, IDisposable {
    #region Field

    private CancellationTokenSource _source;
    private readonly EmailQueue _queue;
    private readonly EmailSetting _emailSetting;
    private SmtpClient _client;
    private Task _current;
    private readonly ILogger<EmailHostedService> _logger;

    #endregion

    #region Constructor

    public EmailHostedService(EmailQueue queue, EmailSetting emailSetting, ILogger<EmailHostedService> logger) {
      _logger = logger;
      _queue = queue;
      _emailSetting = emailSetting;
      _client = CreateSmtpClient();
    }

    #endregion

    #region Method

    #region Public Method

    public void Dispose() {
      _client?.Dispose();
      _client = null;
    }

    public Task StartAsync(CancellationToken cancellationToken) {
      _ = Task.Run(() => StartProcess(cancellationToken));
      _logger.LogInformation("Email service has started");
      return Task.CompletedTask;
    }

    public async Task StartProcess(CancellationToken cancellationToken) {
      _source = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
      while (!cancellationToken.IsCancellationRequested) {
        try {
          var task = _queue.Dequeue(_source.Token);
          _current = _SendMailAsync(task);
          await _current;
        } catch (OperationCanceledException ex) {
          _logger.LogError(ex, "sending email has been cancelled");
        } catch (Exception ex) {
          _logger.LogError(ex, "error occurred when sending email");
          _logger.LogError(ex, ex.Message);
        }
      }
    }

    public async Task StopAsync(CancellationToken cancellationToken) {
      _source.Cancel();
      if (_current != null) {
        await Task.WhenAny(_current, Task.Delay(Timeout.Infinite, cancellationToken));
      }
      _logger.LogInformation("Email service has stopped");
    }

    #endregion

    #region Private

    private async Task _SendMailAsync(EmailTask task) {
      var from = _emailSetting.From;
      var to = string.Join(',', task.Recipients);
      var subject = task.Subject;
      var body = task.Body;
      _logger.LogInformation("------------------------------");
      _logger.LogInformation("Sending email...");
      _logger.LogInformation($"from: {from}");
      _logger.LogInformation($"to: {to}");
      _logger.LogInformation($"subject: {subject}");
      _logger.LogInformation($"body: {body}");
      _logger.LogInformation("------------------------------");
      var options = new MailMessage(
        from: from,
        to: to
      ) {
        Subject = subject,
        Body = body,
        IsBodyHtml = true,
      };
      await _client.SendMailAsync(options);
    }

    private SmtpClient CreateSmtpClient() {
      var client = new SmtpClient {
        Host = _emailSetting.Host,
        //Port = _emailSetting.Port,
        EnableSsl = _emailSetting.EnableSsl,
        UseDefaultCredentials = false,
        Credentials = new NetworkCredential(userName: _emailSetting.UserName, password: _emailSetting.Password)
      };
      return client;
    }

    #endregion

    #endregion
  }
}