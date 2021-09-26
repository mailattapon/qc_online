using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;

namespace QcSupplier.Models {
  public class AlertDecoratorResult : IActionResult {
    #region Property

    public IActionResult Result { get; }
    public string Type { get; }
    public string Title { get; }
    public string Body { get; }

    #endregion

    #region Constructor

    public AlertDecoratorResult(IActionResult result, string type, string title, string body) {
      Result = result;
      Type = type;
      Title = title;
      Body = body;
    }

    #endregion

    #region Method

    public async Task ExecuteResultAsync(ActionContext context) {
      var factory = context.HttpContext.RequestServices.GetService<ITempDataDictionaryFactory>();
      var tempData = factory.GetTempData(context.HttpContext);
      tempData["_alert.type"] = Type;
      tempData["_alert.title"] = Title;
      tempData["_alert.body"] = Body;
      await Result.ExecuteResultAsync(context);
    }

    #endregion

  }
}