using System;

namespace QcSupplier.ViewModels {
  public class SelfControlledIPPList {
    public int Id { get; set; }
    public string Vendor { get; set; }
    public string Detail { get; set; }
    public string FileName { get; set; }
    public long FileSize { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool CanDelete { get; set; }
  }
}