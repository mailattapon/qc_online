using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QcSupplier.Entities;
using QcSupplier.ViewModels;

namespace QcSupplier.Controllers {
  [Route("[Controller]/[Action]")]
  [AllowAnonymous]
  public class AccountController : Controller {
    #region Field

    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;

    #endregion

    #region Constructor

    public AccountController(SignInManager<User> signInManager, UserManager<User> userManager) {
      _userManager = userManager;
      _signInManager = signInManager;
    }

    #endregion

    #region Method

    [HttpGet]
    public IActionResult Login() {
      return View();
    }

    [HttpGet]
    public IActionResult AccessDenied() {
      return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(Login model, string returnUrl) {
      if (ModelState.IsValid) {
        var result = await _signInManager.PasswordSignInAsync(
          model.UserName,
          model.Password,
          model.RememberMe,
          false
        );
        if (result.Succeeded) {
          if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl)) {
            return Redirect(returnUrl);
          } else {
            return RedirectToAction("Index", "Home");
          }
        }
      }
      ModelState.Clear();
      ModelState.AddModelError(string.Empty, "Invalid UserName and/or Password");
      return View(model);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Logout() {
      await _signInManager.SignOutAsync();
      return RedirectToAction("Index", "Home");
    }

        public override bool Equals(object obj) {
            return obj is AccountController controller &&
                   EqualityComparer<UserManager<User>>.Default.Equals(_userManager, controller._userManager);
        }

        #endregion
    }
}