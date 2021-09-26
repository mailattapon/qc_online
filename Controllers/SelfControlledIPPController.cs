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
using QcSupplier.Utilities;
using QcSupplier.ViewModels;

namespace QcSupplier.Controllers {
  [Route("[Controller]")]
  public class SelfControlledIPPController : FileUploadController {
    #region Field

    private readonly IMapper _mapper;
    private readonly SelfControlledIPPRepository _selfControlledIPPRepo;
    private readonly UserManager<User> _userManager;
    private readonly UnitOfWork _unitOfWork;
    private readonly UserRepository _userRepo;
    private readonly VendorRepository _vendorRepo;
    private readonly EmailService _emailSvc;

    #endregion

    #region Property

    public override string FileNamePrefix { get { return "Self_control_IPP"; } }
    public override string DetailPrefix { get { return "Self-control IPP"; } }

    #endregion

    #region Constructor

    public SelfControlledIPPController(
      IMapper mapper,
      SelfControlledIPPRepository selfControlledIPPRepo,
      IHostingEnvironment hostingEnv,
      FormatSetting formatSetting,
      FileUploadSetting fileUploadSetting,
      UserManager<User> userManager,
      UnitOfWork unitOfWork,
      UserRepository userRepo,
      VendorRepository vendorRepo,
      EmailService emailSvc
    ) : base(hostingEnv, fileUploadSetting, formatSetting) {
      _userManager = userManager;
      _unitOfWork = unitOfWork;
      _userRepo = userRepo;
      _vendorRepo = vendorRepo;
      _emailSvc = emailSvc;
      _selfControlledIPPRepo = selfControlledIPPRepo;
      _mapper = mapper;
    }

    #endregion

    #region Method

    #region Public

    [Route("")]
    [HttpGet]
    [Authorize(Policy = Policies.SELF_CONTROLLED_IPP)]
    public async Task<IActionResult> Get(VendorDateParam param) {
      var paramModel = _mapper.Map<VendorDateParam, VendorDate>(param);
      var userId = int.Parse(_userManager.GetUserId(User));
      var user = await _userRepo.FindByIdWithVendorsAsync(userId);
      paramModel.UserVendorIds = user.UserVendors.Select(uv => uv.VendorId).ToList();
      var entities = await _selfControlledIPPRepo.FindListAsync(paramModel);
      var model = _mapper.Map<QueryResult<SelfControlledIPP>, QueryResultModel<SelfControlledIPPList>>(entities);
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
    [Authorize(Policy = Policies.VENDOR_SELF_CONTROLLED_IPP)]
    public async Task<IActionResult> Delete(int id) {
      var entity = await _selfControlledIPPRepo.FindByIdAsync(id);
      if (entity == null) {
        _ThrowNotFoundError(id);
      }
      if (!string.IsNullOrEmpty(entity.FileName)) {
        DeleteFile(entity.FileName);
      }
      _selfControlledIPPRepo.Remove(entity);
      await _unitOfWork.CompleteAsync();
      return RedirectToAction(nameof(Get)).WithSuccess("Success", "Self Controlled IPP have been successfully deleted");
    }

    [Route("")]
    [HttpPost]
    [Authorize(Policy = Policies.VENDOR_SELF_CONTROLLED_IPP)]
    [ApiExceptionFilter]
    public async Task<IActionResult> Add(SelfControlledIPPAdd model) {
      var userId = int.Parse(_userManager.GetUserId(User));
      var user = await _userRepo.FindByIdAsync(userId);
      var vendorId = user.UserVendors.First().VendorId;
      var logicalFolder = GetLogicalFolder(vendorId);
      var logicalFilePath = GetLogicalFilePathWithPredefinedName(logicalFolder, model.File.FileName, model.SelectedDate);
      await Upload(model.File, logicalFilePath);
      var entity = _GetEntityForCreate(model, vendorId, logicalFilePath);
      _selfControlledIPPRepo.Add(entity);
      await _unitOfWork.CompleteAsync();
      await _emailSvc.SendSelfControlledIPPMailAsync(entity);
      return Json(new { data = "OK" }).WithSuccess("Success", "Self Controlled have been successfully created");
    }

    #endregion

    #region Private

    private SelfControlledIPP _GetEntityForCreate(SelfControlledIPPAdd model, int vendorId, string logicalFilePath) {
      var now = DateTime.UtcNow.ToAppDateTime();
      var entity = new SelfControlledIPP {
        VendorId = vendorId,
        Detail = GetDetail(model.SelectedDate),
        CreatedAt = now,
        UpdatedAt = now,
        SelectedDate = model.SelectedDate,
        CreatorId = int.Parse(_userManager.GetUserId(User)),
        FileName = logicalFilePath,
        FileSize = model.File.Length,
      };
      return entity;
    }

    private void _ThrowNotFoundError(int id) {
      throw new AppException($"Self Controlled IPP with Id = {id} could not be found", StatusCodes.Status404NotFound);
    }

    #endregion

    #endregion
  }
}