using System;
using Microsoft.AspNetCore.Mvc;
using QcSupplier.Infrastructures;

namespace QcSupplier.ViewModels {
  [ModelBinder (BinderType = typeof (VendorDateModelBinder))]
  public class VendorDateParam : Pagination {
    public int? VendorId { get; set; }
    public DateTime? Start { get; set; }
    public DateTime? End { get; set; }
    public string Month { get; set; }
    public string Year { get; set; }
    public string Vendor { get; set; }
  }
}