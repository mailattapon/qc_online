using System;
using Microsoft.AspNetCore.Http;

namespace QcSupplier.ViewModels {
  public class SupplierEvaluationAdd {
    public int VendorId { get; set; }
    public DateTime SelectedDate { get; set; }
    public IFormFile File { get; set; }
  }
}