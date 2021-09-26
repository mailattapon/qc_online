using System;

namespace QcSupplier.Entities {
  public class SupplierEvaluation {
    public int Id { get; set; }
    public int VendorId { get; set; }
    public string Detail { get; set; }
    public DateTime SelectedDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? ActionDate { get; set; }
    public string TnsFileName { get; set; }
    public long TnsFileSize { get; set; }
    public string VendorFileName { get; set; }
    public long? VendorFileSize { get; set; }
    public int CreatorId { get; set; }
    public int? UpdaterId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public virtual User Creator { get; set; }
    public virtual User Updater { get; set; }
    public virtual Vendor Vendor { get; set; }
  }
}