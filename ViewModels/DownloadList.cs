using System;

namespace QcSupplier.ViewModels {
  public class DownloadList {
    public int Id { get; set; }
    public string Title { get; set; }
    public string Detail { get; set; }
    public string FileName { get; set; }
    public long FileSize { get; set; }
    public DateTime UpdatedAt { get; set; }
  }
}