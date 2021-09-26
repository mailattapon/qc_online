using System.Collections.Generic;

namespace QcSupplier.Models {
  public class EmailTask {
    public string Subject { get; set; }
    public string Body { get; set; }
    public IList<string> Recipients { get; set; }
  }
}