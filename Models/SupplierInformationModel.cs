using System.Collections.Generic;

namespace QcSupplier.Models {
  public class SupplierInformationModel : Keyword {
    public int? VendorId { get; set; }
    public IList<int> UserVendorIds { get; set; }
  }
}