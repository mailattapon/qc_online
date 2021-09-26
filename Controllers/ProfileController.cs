using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QcSupplier.Entities;
using QcSupplier.Infrastructures;
using QcSupplier.Repositories;
using QcSupplier.ViewModels;

namespace QcSupplier.Controllers {
  [Route("[Controller]/[Action]")]
  [ApiExceptionFilter]
  public class ProfileController : Controller {
    #region Field

    private readonly UserRepository _repo;
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;
    private readonly ILogger<ProfileController> _logger;

    #endregion

    #region Constructor

    public ProfileController(
      UserRepository repo,
      UserManager<User> userManager,
      IMapper mapper,
      ILogger<ProfileController> logger
    ) {
      _logger = logger;
      _mapper = mapper;
      _userManager = userManager;
      _repo = repo;
    }
    #endregion

    #region Method

    [HttpGet]
    public async Task<IActionResult> Vendors() {
      var userId = int.Parse(_userManager.GetUserId(User));
      var entity = await _repo.FindByIdWithVendorsAsync(userId);
      var vendors = entity.UserVendors.Select(uv => uv.Vendor).OrderBy(v => v.Name).ToList();
      var model = _mapper.Map<IList<Vendor>, IList<VendorModel>>(vendors);
      return Json(new { Data = model });
    }

    #endregion
  }
}