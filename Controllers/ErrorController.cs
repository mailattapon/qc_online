using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QcSupplier.Models;

namespace QcSupplier.Controllers {
  [Route("[Controller]")]
  public class ErrorController : Controller {
    #region Field

    private readonly ILogger<ErrorController> _logger;

    #endregion

    #region Constructor

    public ErrorController(ILogger<ErrorController> logger) {
      _logger = logger;
    }

    #endregion

    #region Method

    [Route("{statusCode}")]
    public IActionResult HttpStatusCodeHandler(int statusCode) {
      var statusCodeResult = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
      switch (statusCode) {
        case 404:
        default:
          return View("NotFound");
      }
    }

    [Route("")]
    public IActionResult Error() {
      var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
      var ex = exceptionHandlerPathFeature.Error;
      var message = "There was a problem, please try again.";
      var statusCode = StatusCodes.Status500InternalServerError;
      if (ex is AppException) {
        message = ex.Message;
        statusCode = (ex as AppException).StatusCode;
      }
      ViewBag.Message = message;
      ViewBag.StatusCode = statusCode;
      _logger.LogError(ex, "\n\tMessage: {0}\n\tStackTrace: {1}", ex.Message, ex.StackTrace);
      return View("Error");
    }

    #endregion
  }
}