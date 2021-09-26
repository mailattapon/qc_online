using System;

namespace QcSupplier.ViewModels {
  public class SupplierEvaluationList {
    public int Id { get; set; }
    public string Vendor { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string Detail { get; set; }
    public string TnsFileName { get; set; }
    public string VendorFileName { get; set; }
    public DateTime SelectedDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? ActionDate { get; set; }
    public bool isOverDue { get; set; }
  }
}