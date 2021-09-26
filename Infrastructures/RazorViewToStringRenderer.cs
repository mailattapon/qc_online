using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;

namespace QcSupplier.Infrastructures {
  public class RazorViewToStringRenderer {
    #region Field

    private IRazorViewEngine _viewEngine;
    private ITempDataProvider _tempDataProvider;
    private IServiceProvider _serviceProvider;

    [ViewData]
    public string aaaa { get; set; }
        #endregion

        #region Constructor

        public RazorViewToStringRenderer(
      IRazorViewEngine viewEngine,
      ITempDataProvider tempDataProvider,
      IServiceProvider serviceProvider) {
      _viewEngine = viewEngine;
      _tempDataProvider = tempDataProvider;
      _serviceProvider = serviceProvider;
    }

    #endregion

    #region Method

    #region Public

    public async Task<string> RenderViewToStringAsync<T>(string viewName, T model) {
       var actionContext = GetActionContext();
      var view = FindView(actionContext, viewName);

      using (var output = new StringWriter()) {
        var viewContext = new ViewContext(
          actionContext,
          view,
          new ViewDataDictionary<T>(
            metadataProvider: new EmptyModelMetadataProvider(),
            modelState: new ModelStateDictionary()) {
            Model = model,
          },

          new TempDataDictionary(
            actionContext.HttpContext,
            _tempDataProvider),
          output,
          new HtmlHelperOptions());;

        await view.RenderAsync(viewContext);

        return output.ToString();
      }
    }

    #endregion

    #region Private

    private IView FindView(ActionContext actionContext, string viewName) {
      var getViewResult = _viewEngine.GetView(executingFilePath: null, viewPath: viewName, isMainPage: true);
      if (getViewResult.Success) {
        return getViewResult.View;
      }

      var findViewResult = _viewEngine.FindView(actionContext, viewName, isMainPage: true);
      if (findViewResult.Success) {
        return findViewResult.View;
      }

      var searchedLocations = getViewResult.SearchedLocations.Concat(findViewResult.SearchedLocations);
      var errorMessage = string.Join(
        Environment.NewLine,
        new[] { $"Unable to find view '{viewName}'. The following locations were searched:" }.Concat(searchedLocations)); ;

      throw new InvalidOperationException(errorMessage);
    }

    private ActionContext GetActionContext() {
      var httpContext = new DefaultHttpContext();
      httpContext.RequestServices = _serviceProvider;
      return new ActionContext(httpContext, new RouteData(), new ActionDescriptor());
    }

    #endregion

    #endregion
  }
}