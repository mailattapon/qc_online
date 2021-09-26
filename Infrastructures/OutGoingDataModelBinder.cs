using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using QcSupplier.Utilities;
using QcSupplier.ViewModels;

namespace QcSupplier.Infrastructures {
  public class OutGoingDataModelBinder : IModelBinder {
    public Task BindModelAsync(ModelBindingContext context) {
      if (context == null) {
        throw new ArgumentNullException(nameof(context));
      }
      string vendor = context.HttpContext.Request.Query["vendor"];
      string month = context.HttpContext.Request.Query["month"];
      string year = context.HttpContext.Request.Query["year"];
      string page = context.HttpContext.Request.Query["page"];
      string invoice = context.HttpContext.Request.Query["invoice"];
      string FlgJudge = context.HttpContext.Request.Query["FlgJudge"];  //16/09/2021 by lattapon
      int? vendorId = ModelBinderUtility.GetVendorId(vendor);
      var (start, end) = DateUtility.GetStartEnd(ref month, ref year);
      var model = new OutGoingDataParam {
        VendorId = vendorId,
        Start = start,
        End = end,
        Vendor = vendor,
        Page = string.IsNullOrEmpty(page) ? 1 : int.Parse(page),
        Month = month,
        Year = year,
        Invoice = invoice,
        FlgJudge = FlgJudge     //16/09/2021 by lattapon
      };
      context.Result = ModelBindingResult.Success(model);
      return Task.CompletedTask;
    }
  }
}