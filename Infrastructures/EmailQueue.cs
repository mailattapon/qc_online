using System.Collections.Concurrent;
using System.Threading;
using QcSupplier.Models;

namespace QcSupplier.Infrastructures {
  public class EmailQueue {
    #region Field

    private readonly BlockingCollection<EmailTask> _tasks;

    #endregion

    #region Constructor

    public EmailQueue() {
      _tasks = new BlockingCollection<EmailTask>();
    }

    #endregion

    #region Method

    public void Enqueue(EmailTask settings) => _tasks.Add(settings);

    public EmailTask Dequeue(CancellationToken token) => _tasks.Take(token);

    #endregion
  }
}