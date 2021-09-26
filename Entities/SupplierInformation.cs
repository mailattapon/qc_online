using System;

namespace QcSupplier.Entities {
  public class SupplierInformation {
    public int Id { get; set; }
    public int VendorId { get; set; }
    public string Title { get; set; }
    public string Detail { get; set; }
    public string FileName { get; set; }
    public long FileSize { get; set; }
    public int CreatorId { get; set; }
    public int? UpdaterId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public virtual User Creator { get; set; }
    public virtual User Updater { get; set; }
    public virtual Vendor Vendor { get; set; }
  }
}