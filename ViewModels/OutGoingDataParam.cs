using System;
using Microsoft.AspNetCore.Mvc;
using QcSupplier.Infrastructures;

namespace QcSupplier.ViewModels {
  [ModelBinder(BinderType = typeof(OutGoingDataModelBinder))]
  public class OutGoingDataParam : Pagination {
    public int? VendorId { get; set; }
    public string Vendor { get; set; }
    public DateTime? Start { get; set; }
    public DateTime? End { get; set; }
    public string Month { get; set; }
    public string Year { get; set; }
    public string Invoice { get; set; }
    
    //2021/09/16 by lattapon
    public string FlgJudge { get; set; }
    }
}