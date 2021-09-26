using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using QcSupplier.Utilities;
using QcSupplier.ViewModels;

namespace QcSupplier.Infrastructures {
  public class SupplierInformationModelBinder : IModelBinder {
    public Task BindModelAsync(ModelBindingContext context) {
      if (context == null) {
        throw new ArgumentNullException(nameof(context));
      }
      string vendor = context.HttpContext.Request.Query["vendor"];
      string search = context.HttpContext.Request.Query["search"];
      string page = context.HttpContext.Request.Query["page"];
      int? vendorId = ModelBinderUtility.GetVendorId(vendor);
      var model = new SupplierInformationParam {
        VendorId = vendorId,
        Vendor = vendor,
        Search = search,
        Page = string.IsNullOrEmpty(page) ? 1 : int.Parse(page)
      };
      context.Result = ModelBindingResult.Success(model);
      return Task.CompletedTask;
    }
  }
}