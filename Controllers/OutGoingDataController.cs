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
  public class OutGoingDataController : FileUploadController {
    #region Field

    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;
    private readonly UserRepository _userRepo;
    private readonly OutGoingDataRepository _outGoingDataRepo;
    private readonly UnitOfWork _unitOfWork;
    private readonly EmailService _emailSvc;
    private readonly OutGoingDataFileRepository _outGoingDataFileRepo;

    #endregion

    #region Property

    public override string FileNamePrefix { get { return ""; } }
    public override string DetailPrefix { get { return ""; } }

    #endregion

    #region Constructor

    public OutGoingDataController(
      IHostingEnvironment hostingEnv,
      FileUploadSetting fileUploadSetting,
      FormatSetting formatSetting,
      IMapper mapper,
      UserManager<User> userManager,
      UserRepository userRepo,
      OutGoingDataRepository outGoingDataRepo,
      UnitOfWork unitOfWork,
      EmailService emailSvc,
      OutGoingDataFileRepository outGoingDataFileRepo
    ) : base(hostingEnv, fileUploadSetting, formatSetting) {
      _mapper = mapper;
      _userManager = userManager;
      _userRepo = userRepo;
      _outGoingDataRepo = outGoingDataRepo;
      _unitOfWork = unitOfWork;
      _emailSvc = emailSvc;
      _outGoingDataFileRepo = outGoingDataFileRepo;
    }

    #endregion

    #region Method

    #region Public

    [Route("{id}")]
    [HttpPost]
    [Authorize(Policy = Policies.VENDOR_OUT_GOING_DATA)]
    public async Task<IActionResult> Delete(int id) {
      var entity = await _outGoingDataRepo.FindByIdAsync(id);
      if (entity == null) {
        _ThrowNotFoundError(id);
      }
      foreach (var file in entity.Files) {
        DeleteFile(file.FileName);
      }
      _outGoingDataRepo.Remove(entity);
      await _unitOfWork.CompleteAsync();
      return RedirectToAction(nameof(Get)).WithSuccess("Success", "Out Going Data have been successfully deleted");
    }

    [Route("")]
    [HttpPost]
    [Authorize(Policy = Policies.VENDOR_OUT_GOING_DATA)]
    [ApiExceptionFilter]
    public async Task<IActionResult> Add(OutGoingDataAdd model) {
      var userId = int.Parse(_userManager.GetUserId(User));
      var user = await _userRepo.FindByIdAsync(userId);
      var vendorId = user.UserVendors.First().VendorId;
      var logicalFolder = GetLogicalFolder(vendorId);
      var entity = _GetEntityForCreate(model, vendorId);
      foreach (var file in model.Files) {
        var logicalFilePath = GetLogicalFilePathWithOriginalName(logicalFolder, file.FileName);
        await Upload(file, logicalFilePath);
        entity.Files.Add(_GetFileEntityForCreate(logicalFilePath, file.Length));
      }
      _outGoingDataRepo.Add(entity);
      await _unitOfWork.CompleteAsync();
      await _emailSvc.SendOutGoingDataTnsMailAsync(entity);
      return Json(new { data = "OK" }).WithSuccess("Success", "Out Going Data have been successfully created");
    }

    [Route("{id}/files")]
    [HttpGet]
    [Authorize(Policy = Policies.OUT_GOING_DATA)]
    public async Task<IActionResult> GetFilesById(int id) {
      var entities = await _outGoingDataFileRepo.FindListAsync(id);
      var model = _mapper.Map<IList<OutGoingDataFile>, IList<OutGoingDataFileList>>(entities);
      return Json(new { data = model });
    }

    [Route("")]
    [HttpGet]
    [Authorize(Policy = Policies.OUT_GOING_DATA)]
    public async Task<IActionResult> Get(OutGoingDataParam param) {
      var paramModel = _mapper.Map<OutGoingDataParam, OutGoingDataModel>(param);
      var userId = int.Parse(_userManager.GetUserId(User));
      var user = await _userRepo.FindByIdWithVendorsAsync(userId);
      paramModel.UserVendorIds = user.UserVendors.Select(uv => uv.VendorId).ToList();

      var idVendor = user.UserVendors.Select(uv => uv.VendorId).ToList();
      paramModel.VendorAbbr = await _outGoingDataRepo.FindVendorAbbr(idVendor);


      //delete 14/09/2021 by lattapon     
      //var entities = await _outGoingDataRepo.FindListAsync(paramModel);
      //var model = _mapper.Map<QueryResult<OutGoingData>, QueryResultModel<OutGoingDataList>>(entities);

      var date_now = DateTime.UtcNow.ToAppDateTime();
      date_now = date_now.AddDays(-7);
      string date_cal = date_now.ToString("yyyyMMdd");

      //2021/09/14 lattapon
      var entities = await _outGoingDataRepo.FindListAsync_vendor(paramModel, date_cal);
      var model = _mapper.Map<QueryResult<OutGoingDatas_New>, QueryResultModel<OutGoingDatas_NewList>>(entities);

      if (User.IsInRole(Roles.VENDOR))
      {
        entities = await _outGoingDataRepo.FindListAsync_vendor(paramModel, date_cal);
        model = _mapper.Map<QueryResult<OutGoingDatas_New>, QueryResultModel<OutGoingDatas_NewList>>(entities);
      }
      else
      {
        entities = await _outGoingDataRepo.FindListAsync_admin(paramModel, date_cal);
        model = _mapper.Map<QueryResult<OutGoingDatas_New>, QueryResultModel<OutGoingDatas_NewList>>(entities);
      }


      var url = Url.Action(nameof(Get));
      //model.Url = $"{url}?vendor={param.Vendor}&invoice={param.Invoice}&month={param.Month}&year={param.Year}&page=:";   delete 16/09/2021 by lattapon
      model.Url = $"{url}?vendor={param.Vendor}&invoice={param.Invoice}&month={param.Month}&year={param.Year}&FlgJudge={param.FlgJudge}&page=:"; // add 16/09/2021 by lattapon
      var vendors = _mapper.Map<List<Vendor>, List<SelectListItem>>(user.UserVendors.Select(uv => uv.Vendor).OrderBy(v => v.Name).ToList());
      vendors.ForEach(v => { v.Selected = v.Value == param.Vendor; });
      ViewBag.Vendors = vendors;
      ViewBag.Invoice = param.Invoice;
      ViewBag.Months = DateUtility.GetMonths(param.Month);
      ViewBag.Years = DateUtility.GetYears(param.Year);
      ViewBag.FlgJudge = DateUtility.GetFlg(param.FlgJudge);
      return View(model);
    }

    [Route("{id}/files_new")]
    [HttpGet]
    [Authorize(Policy = Policies.OUT_GOING_DATA)]
    public async Task<IActionResult> GetFilesById_new(int id)
    {
        var entities = await _outGoingDataFileRepo.FindListAsync_new(id);
        var model = _mapper.Map<IList<OutGoingDatas_New>, IList<OutGoingDatas_NewList>>(entities);
        return Json(new { data = model });
    }

        #endregion

        #region Private

    private OutGoingDataFile _GetFileEntityForCreate(string fileName, long fileSize) {
      var now = DateTime.UtcNow.ToAppDateTime();
      var entity = new OutGoingDataFile {
        FileName = fileName,
        FileSize = fileSize,
        CreatedAt = now,
        UpdatedAt = now,
        CreatorId = int.Parse(_userManager.GetUserId(User))
      };
      return entity;
    }

    private OutGoingData _GetEntityForCreate(OutGoingDataAdd model, int vendorId) {
      var now = DateTime.UtcNow.ToAppDateTime();
      var entity = new OutGoingData {
        VendorId = vendorId,
        Title = model.Title,
        Detail = model.Detail,
        Invoice = model.Invoice,
        Judgemented = false,
        CreatedAt = now,
        UpdatedAt = now,
        CreatorId = int.Parse(_userManager.GetUserId(User))
      };
      return entity;
    }

    private void _ThrowNotFoundError(int id) {
      throw new AppException($"Out Going Data with Id = {id} could not be found", StatusCodes.Status404NotFound);
    }

    #endregion

    #endregion
  }
}