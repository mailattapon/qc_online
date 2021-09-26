using System;
using System.Collections.Generic;
using System.IO;
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
using QcSupplier.Utilities;
using QcSupplier.ViewModels;

namespace QcSupplier.Controllers {

  [Route("[Controller]")]
  public class SupplierEvaluationController : FileUploadController {
    #region Field

    private readonly IMapper _mapper;
    private readonly SupplierEvaluationRepository _supplierEvaluationRepo;
    private readonly UserManager<User> _userManager;
    private readonly UnitOfWork _unitOfWork;
    private readonly UserRepository _userRepo;
    private readonly VendorRepository _vendorRepo;
    private readonly EmailService _emailSrv;

    #endregion

    #region Property

    public override string FileNamePrefix { get { return "Supplier_Performance_Report"; } }
    public override string DetailPrefix { get { return "Supplier Performance Report"; } }

    #endregion

    #region Constructor

    public SupplierEvaluationController(
      IMapper mapper,
      SupplierEvaluationRepository supplierEvaluationRepo,
      IHostingEnvironment hostingEnv,
      FormatSetting formatSetting,
      FileUploadSetting fileUploadSetting,
      UserManager<User> userManager,
      UnitOfWork unitOfWork,
      UserRepository userRepo,
      VendorRepository vendorRepo,
      EmailService emailSrv
    ) : base(hostingEnv, fileUploadSetting, formatSetting) {
      _userManager = userManager;
      _unitOfWork = unitOfWork;
      _userRepo = userRepo;
      _vendorRepo = vendorRepo;
      _emailSrv = emailSrv;
      _supplierEvaluationRepo = supplierEvaluationRepo;
      _mapper = mapper;
    }

    #endregion

    #region Method

    #region Public

    [Route("")]
    [HttpGet]
    [Authorize(Policy = Policies.SUPPLIER_EVALUATION)]
    public async Task<IActionResult> Get(VendorDateParam param) {
      var paramModel = _mapper.Map<VendorDateParam, VendorDate>(param);
      var userId = int.Parse(_userManager.GetUserId(User));
      var user = await _userRepo.FindByIdWithVendorsAsync(userId);
      paramModel.UserVendorIds = user.UserVendors.Select(uv => uv.VendorId).ToList();
      var entities = await _supplierEvaluationRepo.FindListAsync(paramModel);
      var model = _mapper.Map<QueryResult<SupplierEvaluation>, QueryResultModel<SupplierEvaluationList>>(entities);
      var url = Url.Action(nameof(Get));
      model.Url = $"{url}?vendor={param.Vendor}&month={param.Month}&year={param.Year}&page=:";
      var vendors = _mapper.Map<List<Vendor>, List<SelectListItem>>(user.UserVendors.Select(uv => uv.Vendor).OrderBy(v => v.Name).ToList());
      vendors.ForEach(v => { v.Selected = v.Value == param.Vendor; });
      ViewBag.Vendors = vendors;
      ViewBag.Months = DateUtility.GetMonths(param.Month);
      ViewBag.Years = DateUtility.GetYears(param.Year);
      return View(model);
    }

    [Route("{id}")]
    [HttpPost]
    [Authorize(Policy = Policies.ADMIN_SUPPLIER_EVALUATION)]
    public async Task<IActionResult> Delete(int id) {
      var entity = await _supplierEvaluationRepo.FindByIdAsync(id);
      if (entity == null) {
        _ThrowNotFoundError(id);
      }
      if (!string.IsNullOrEmpty(entity.TnsFileName)) {
        DeleteFile(entity.TnsFileName);
      }
      if (!string.IsNullOrEmpty(entity.VendorFileName)) {
        DeleteFile(entity.VendorFileName);
      }
      _supplierEvaluationRepo.Remove(entity);
      await _unitOfWork.CompleteAsync();
      return RedirectToAction(nameof(Get)).WithSuccess("Success", "Supplier Evaluation have been successfully deleted");
    }

    [Route("")]
    [HttpPost]
    [Authorize(Policy = Policies.ADMIN_SUPPLIER_EVALUATION)]
    [ApiExceptionFilter]
    public async Task<IActionResult> Add(SupplierEvaluationAdd model) {
      var entry = await _supplierEvaluationRepo.FindByVendorAndSelectedDate(model.VendorId, model.SelectedDate);
      if (entry != null) {
        throw new AppException($"Vendor with this month/year has already been uploaded", StatusCodes.Status400BadRequest);
      }
      var logicalFolder = GetLogicalFolder(model.VendorId);
      var logicalFilePath = GetLogicalFilePathWithPredefinedName(logicalFolder, model.File.FileName, model.SelectedDate);
      await Upload(model.File, logicalFilePath);
      var entity = _GetEntityForCreate(model, logicalFilePath);
      _supplierEvaluationRepo.Add(entity);
      await _unitOfWork.CompleteAsync();
      await _emailSrv.SendSupplierEvaluationVendorMailAsync(entity);
      return Json(new { data = "OK" }).WithSuccess("Success", "Supplier Evaluation have been successfully created");
    }

    [Route("{id}")]
    [HttpPut]
    [Authorize(Policy = Policies.VENDOR_SUPPLIER_EVALUATION)]
    [ApiExceptionFilter]
    public async Task<IActionResult> Update(int id, IFormFile file) {
      var entity = await _supplierEvaluationRepo.FindByIdAsync(id);
      if (entity == null) {
        throw new AppException($"Supplier Evaluation with id {id} cannot be found", StatusCodes.Status400BadRequest);
      }
      var logicalFolder = GetLogicalFolder(entity.VendorId);
      var logicalFilePath = GetLogicalFilePathWithOriginalName(logicalFolder, file.FileName);
      await Upload(file, logicalFilePath);
      _SetEntityForUpdate(entity, file, logicalFilePath);
      await _unitOfWork.CompleteAsync();
      await _emailSrv.SendSupplierEvaluationTnsMailAsync(entity);
      return Json(new { data = "OK" }).WithSuccess("Success", "Supplier Evaluation have been successfully updated");
    }

    #endregion

    #region Private

    private void _SetEntityForUpdate(SupplierEvaluation entity, IFormFile file, string logicalFilePath) {
      var now = DateTime.UtcNow.ToAppDateTime();
      entity.UpdatedAt = now;
      entity.UpdaterId = int.Parse(_userManager.GetUserId(User));
      entity.ActionDate = now;
      entity.VendorFileName = logicalFilePath;
      entity.VendorFileSize = file.Length;
    }

    private SupplierEvaluation _GetEntityForCreate(SupplierEvaluationAdd model, string logicalFilePath) {
      var now = DateTime.UtcNow.ToAppDateTime();
      var entity = new SupplierEvaluation {
        VendorId = model.VendorId,
        Detail = GetDetail(model.SelectedDate),
        CreatedAt = now,
        UpdatedAt = now,
        SelectedDate = model.SelectedDate,
        DueDate = now.AddDays(7),
        CreatorId = int.Parse(_userManager.GetUserId(User)),
        TnsFileName = logicalFilePath,
        TnsFileSize = model.File.Length,
      };
      return entity;
    }

    private void _ThrowNotFoundError(int id) {
      throw new AppException($"Supplier Evaluation with Id = {id} could not be found", StatusCodes.Status404NotFound);
    }

    #endregion

    #endregion
  }
}