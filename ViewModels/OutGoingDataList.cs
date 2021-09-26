using System;

namespace QcSupplier.ViewModels {
  public class OutGoingDataList {
    public int Id { get; set; }
    public string Vendor { get; set; }
    public string Title { get; set; }
    public string Detail { get; set; }
    public string Invoice { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool Judgemented { get; set; }
  }
}