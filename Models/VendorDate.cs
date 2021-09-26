using System;
using System.Collections.Generic;

namespace QcSupplier.Models {
  public class VendorDate : Pagination {
    public int? VendorId { get; set; }
    public DateTime? Start { get; set; }
    public DateTime? End { get; set; }
    public IList<int> UserVendorIds { get; set; }
    public IList<string> VendorAbbr { get; set; }
    }
}