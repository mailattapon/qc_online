using System;

namespace QcSupplier.ViewModels {
  public class OutGoingDataFileList {
    public int Id { get; set; }
    public string FileName { get; set; }
    public string FileSize { get; set; }
    public string ViewAt { get; set; }
    public bool? Passed { get; set; }
    public string Judgementor { get; set; }
  }
}