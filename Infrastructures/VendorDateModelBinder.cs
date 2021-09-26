using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using QcSupplier.Utilities;
using QcSupplier.ViewModels;

namespace QcSupplier.Infrastructures {
  public class VendorDateModelBinder : IModelBinder {
    public Task BindModelAsync(ModelBindingContext context) {
      if (context == null) {
        throw new ArgumentNullException(nameof(context));
      }
      string vendor = context.HttpContext.Request.Query["vendor"];
      string month = context.HttpContext.Request.Query["month"];
      string year = context.HttpContext.Request.Query["year"];
      string page = context.HttpContext.Request.Query["page"];
      int? vendorId = ModelBinderUtility.GetVendorId(vendor);
      var (start, end) = DateUtility.GetStartEnd(ref month, ref year);
      var model = new VendorDateParam {
        VendorId = vendorId,
        Start = start,
        End = end,
        Vendor = vendor,
        Page = string.IsNullOrEmpty(page) ? 1 : int.Parse(page),
        Month = month,
        Year = year
      };
      context.Result = ModelBindingResult.Success(model);
      return Task.CompletedTask;
    }
  }
}