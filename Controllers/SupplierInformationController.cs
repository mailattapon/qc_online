using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using QcSupplier.Constants;
using QcSupplier.Entities;
using QcSupplier.Extensions;
using QcSupplier.Infrastructures;
using QcSupplier.Models;
using QcSupplier.Persistence;
using QcSupplier.Repositories;
using QcSupplier.Services;
using QcSupplier.Settings;
using QcSupplier.ViewModels;

namespace QcSupplier.Controllers {
  [Route("[Controller]")]
  public class SupplierInformationController : FileUploadController {
    #region Field

    private readonly UserManager<User> _userManager;
    private readonly UserRepository _userRepo;
    private readonly IMapper _mapper;
    private readonly SupplierInformationRepository _supplierInformationRepo;
    private readonly UnitOfWork _unitOfWork;
    private readonly EmailService _emailSvc;

    #endregion

    #region Property

    public override string FileNamePrefix { get { return ""; } }
    public override string DetailPrefix { get { return ""; } }

    #endregion

    #region Constructor

    public SupplierInformationController(
      IHostingEnvironment hostingEnv,
      FileUploadSetting fileUploadSetting,
      FormatSetting formatSetting,
      UserManager<User> userManager,
      UserRepository userRepo,
      IMapper mapper,
      SupplierInformationRepository supplierInformationRepo,
      UnitOfWork unitOfWork,
      EmailService emailSvc
      ) : base(hostingEnv, fileUploadSetting, formatSetting) {
      _userManager = userManager;
      _userRepo = userRepo;
      _mapper = mapper;
      _supplierInformationRepo = supplierInformationRepo;
      _unitOfWork = unitOfWork;
      _emailSvc = emailSvc;
    }

    #region Method

    #region Public

    [Route("{id}")]
    [HttpPost]
    [Authorize(Policy = Policies.VENDOR_SUPPLIER_INFORMATION)]
    public async Task<IActionResult> Delete(int id) {
      var entity = await _supplierInformationRepo.FindByIdAsync(id);
      if (entity == null) {
        _ThrowNotFoundError(id);
      }
      if (!string.IsNullOrEmpty(entity.FileName)) {
        DeleteFile(entity.FileName);
      }
      _supplierInformationRepo.Remove(entity);
      await _unitOfWork.CompleteAsync();
      return RedirectToAction(nameof(Get)).WithSuccess("Success", "Out Going Data have been successfully deleted");
    }

    [Route("")]
    [HttpGet]
    [Authorize(Policy = Policies.SUPPLIER_INFORMATION)]
    public async Task<IActionResult> Get(SupplierInformationParam param) {
      var paramModel = _mapper.Map<SupplierInformationParam, SupplierInformationModel>(param);
      var userId = int.Parse(_userManager.GetUserId(User));
      var user = await _userRepo.FindByIdWithVendorsAsync(userId);
      paramModel.UserVendorIds = user.UserVendors.Select(uv => uv.VendorId).ToList();
      var entities = await _supplierInformationRepo.FindListAsync(paramModel);
      var model = _mapper.Map<QueryResult<SupplierInformation>, QueryResultModel<SupplierInformationList>>(entities);
      var url = Url.Action(nameof(Get));
      model.Url = $"{url}?vendor={param.Vendor}&search={param.Search}&page=:";
      var vendors = _mapper.Map<List<Vendor>, List<SelectListItem>>(user.UserVendors.Select(uv => uv.Vendor).OrderBy(v => v.Name).ToList());
      vendors.ForEach(v => { v.Selected = v.Value == param.Vendor; });
      ViewBag.Vendors = vendors;
      ViewBag.Keyword = param.Search;
      return View(model);
    }


    [Route("")]
    [HttpPost]
    [Authorize(Policy = Policies.VENDOR_SUPPLIER_INFORMATION)]
    [ApiExceptionFilter]
    public async Task<IActionResult> Add(SupplierInformationAdd model) {
      var userId = int.Parse(_userManager.GetUserId(User));
      var user = await _userRepo.FindByIdAsync(userId);
      var vendorId = user.UserVendors.First().VendorId;
      var logicalFolder = GetLogicalFolder(vendorId);
      var logicalFilePath = GetLogicalFilePathWithOriginalName(logicalFolder, model.File.FileName);
      await Upload(model.File, logicalFilePath);
      var entity = _GetEntityForCreate(model, vendorId, logicalFilePath);
      _supplierInformationRepo.Add(entity);
      await _unitOfWork.CompleteAsync();
      await _emailSvc.SendSupplierInformationTnsMailAsync(entity);
      return Json(new { data = "OK" }).WithSuccess("Success", "Supplier Information have been successfully created");
    }

    #endregion

    #region Private

    private SupplierInformation _GetEntityForCreate(SupplierInformationAdd model, int vendorId, string logicalFilePath) {
      var now = DateTime.UtcNow.ToAppDateTime();
      var entity = new SupplierInformation {
        VendorId = vendorId,
        Title = model.Title,
        Detail = model.Detail,
        FileName = logicalFilePath,
        FileSize = model.File.Length,
        CreatedAt = now,
        UpdatedAt = now,
        CreatorId = int.Parse(_userManager.GetUserId(User))
      };
      return entity;
    }

    private void _ThrowNotFoundError(int id) {
      throw new AppException($"Supplier Information with Id = {id} could not be found", StatusCodes.Status404NotFound);
    }

    #endregion

    #endregion

    #endregion

  }
}