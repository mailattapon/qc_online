using Microsoft.AspNetCore.Mvc;

namespace QcSupplier.Controllers {
  [Route("[Controller]/[Action]")]
  public class HomeController : Controller {

    #region Constructor

    public HomeController() { }

    #endregion

    #region Method

    [Route("/")]
    public IActionResult Index() {
      return View();
    }

    #endregion

  }
}