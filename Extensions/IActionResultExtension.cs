using Microsoft.AspNetCore.Mvc;
using QcSupplier.Models;

namespace QcSupplier.Extensions {
  public static class IActionResultExtension {
    #region Method

    #region Public

    public static IActionResult WithSuccess(this IActionResult result, string title, string body) {
      return Alert(result, "success", title, body);
    }

    public static IActionResult WithInfo(this IActionResult result, string title, string body) {
      return Alert(result, "info", title, body);
    }

    public static IActionResult WithWarning(this IActionResult result, string title, string body) {
      return Alert(result, "warning", title, body);
    }

    public static IActionResult WithDanger(this IActionResult result, string title, string body) {
      return Alert(result, "danger", title, body);
    }

    #endregion

    #endregion

    #region Private Methods

    private static IActionResult Alert(IActionResult result, string type, string title, string body) {
      return new AlertDecoratorResult(result, type, title, body);
    }

    #endregion
  }
}