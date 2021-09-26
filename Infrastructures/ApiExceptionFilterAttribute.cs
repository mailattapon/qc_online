using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using QcSupplier.Models;

namespace QcSupplier.Infrastructures {
  public class ApiExceptionFilterAttribute : ExceptionFilterAttribute {
    public override void OnException(ExceptionContext context) {
      var logger = (ILogger<ApiExceptionFilterAttribute>)context.HttpContext.RequestServices.GetService<ILogger<ApiExceptionFilterAttribute>>();
      var ex = context.Exception;
      logger.LogError(ex, "\n\tMessage: {0}\n\tStackTrace: {1}", ex.Message, ex.StackTrace);
      context.ExceptionHandled = true;
      var message = "There was a problem, please try again.";
      var statusCode = StatusCodes.Status500InternalServerError;
      if (ex is AppException) {
        message = ex.Message;
        statusCode = (ex as AppException).StatusCode;
      }
      context.Result = new JsonResult(new { Message = message }) { StatusCode = statusCode };
    }
  }
}