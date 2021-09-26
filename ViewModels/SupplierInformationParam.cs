using Microsoft.AspNetCore.Mvc;
using QcSupplier.Infrastructures;

namespace QcSupplier.ViewModels {
  [ModelBinder(BinderType = typeof(SupplierInformationModelBinder))]
  public class SupplierInformationParam : Pagination {
    public int? VendorId { get; set; }
    public string Vendor { get; set; }
    public string Search { get; set; }
  }
}