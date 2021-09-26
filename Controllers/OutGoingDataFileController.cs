using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
  public class OutGoingDataFileController : FileUploadController {
    #region Field`

    private readonly OutGoingDataFileRepository _outgoingDataFileRepo;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;
    private readonly UnitOfWork _unitOfWork;
    private readonly EmailService _emailSvc;
    private readonly UserRepository _userRepo;

    //2021/09/15 lattapon
    private readonly VendorRepository _vendorRepo;

    #endregion

    #region Property

    public override string FileNamePrefix { get { return ""; } }
    public override string DetailPrefix { get { return ""; } }

    #endregion

    #region Constructor

    public OutGoingDataFileController(
      IHostingEnvironment hostingEnv,
      FileUploadSetting fileUploadSetting,
      FormatSetting formatSetting,
      OutGoingDataFileRepository outgoingDataFileRepo,
      IMapper mapper,
      UserManager<User> userManager,
      UnitOfWork unitOfWork,
      EmailService emailSvc,
      UserRepository userRepo,
      VendorRepository vendorRepo       //2021/09/15 lattapon
    ) : base(hostingEnv, fileUploadSetting, formatSetting) {
      _outgoingDataFileRepo = outgoingDataFileRepo;
      _mapper = mapper;
      _userManager = userManager;
      _unitOfWork = unitOfWork;
      _emailSvc = emailSvc;
      _userRepo = userRepo;
      _vendorRepo = vendorRepo;      //2021/09/15 lattapon
        }

    #endregion

    #region Method

    #region Public

    [Route("{id}")]
    [HttpDelete]
    [Authorize(Policy = Policies.VENDOR_OUT_GOING_DATA)]
    [ApiExceptionFilter]
    public async Task<IActionResult> Delete(int id) {
      var entity = await _outgoingDataFileRepo.FindByIdAsync(id);
      if (entity == null) {
        _ThrowNotFoundError(id);
      }
      DeleteFile(entity.FileName);
      _outgoingDataFileRepo.Remove(entity);
      await _unitOfWork.CompleteAsync();
      return Json(new { data = "OK" });
    }

    [Route("{id}/judge")]
    [HttpPut]
    [Authorize(Policy = Policies.ADMIN_OUT_GOING_DATA)]
    [ApiExceptionFilter]
    public async Task<IActionResult> Judge([FromBody] OutGoingDataFileJudge param) {
      var entity = await _outgoingDataFileRepo.FindByIdAsync(param.Id);
      if (entity == null) {
        _ThrowNotFoundError(param.Id);
      }
      _UpdateEntityForJudgement(entity, param.Passed);
      await _unitOfWork.CompleteAsync();
      if (!param.Passed) {
        await _emailSvc.SentOutGoingDataVendorMailAsync(entity.OutGoingDataId);
      }
      entity = await _outgoingDataFileRepo.FindByIdAsync(param.Id);
      var model = _mapper.Map<OutGoingDataFile, OutGoingDataFileList>(entity);
      return Json(new { data = model });
    }

    [Route("{id}/view")]
    [HttpPut]
    [Authorize(Policy = Policies.ADMIN_OUT_GOING_DATA)]
    [ApiExceptionFilter]
    public async Task<IActionResult> View(int id) {
      var entity = await _outgoingDataFileRepo.FindByIdAsync(id);
      if (entity == null) {
        _ThrowNotFoundError(id);
      }
      _UpdateEntityForView(entity);
      await _unitOfWork.CompleteAsync();
      entity = await _outgoingDataFileRepo.FindByIdAsync(id);
      var model = _mapper.Map<OutGoingDataFile, OutGoingDataFileList>(entity);
      return Json(new { data = model });
    }

    [Route("")]
    [HttpPost]
    [Authorize(Policy = Policies.VENDOR_OUT_GOING_DATA)]
    [ApiExceptionFilter]
    public async Task<IActionResult> Add(OutGoingDataFileAdd param) {
      var userId = int.Parse(_userManager.GetUserId(User));
      var user = await _userRepo.FindByIdAsync(userId);
      var vendorId = user.UserVendors.First().VendorId;
      var logicalFolder = GetLogicalFolder(vendorId);
      var logicalFilePath = GetLogicalFilePathWithOriginalName(logicalFolder, param.File.FileName);
      await Upload(param.File, logicalFilePath);
      var entity = _GetEntityForCreate(param.Id, logicalFilePath, param.File.Length);
      _outgoingDataFileRepo.Add(entity);
      await _unitOfWork.CompleteAsync();
      entity = await _outgoingDataFileRepo.FindByIdAsync(entity.Id);
      var model = _mapper.Map<OutGoingDataFile, OutGoingDataFileList>(entity);
      return Json(new { data = model });
    }

    //2021/09/15 lattapon
    [Route("{id}/upload_file")]
    [HttpPut]
    [Authorize(Policy = Policies.VENDOR_OUT_GOING_DATA)]
    [ApiExceptionFilter]
    public async Task<IActionResult> upload_file(OutGoingDataFileAdd form)
    {
        var entity = await _outgoingDataFileRepo.FindByIdAsync_new(form.Id);
        if (entity == null)
        {
            _ThrowNotFoundError(form.Id);
        }

        var userId = int.Parse(_userManager.GetUserId(User));
        var user = await _userRepo.FindByIdAsync(userId);
        var vendorId = user.UserVendors.First().VendorId;
        var logicalFolder = GetLogicalFolder(vendorId);
        var logicalFilePath = GetLogicalFilePathWithOriginalName(logicalFolder, form.File.FileName);
        await Upload(form.File, logicalFilePath);

        _UpdateEntityForView_new(entity, logicalFilePath);
        await _unitOfWork.CompleteAsync();
        entity = await _outgoingDataFileRepo.FindByIdAsync_new(form.Id);
        var model = _mapper.Map<OutGoingDatas_New, OutGoingDatas_NewList>(entity);
        return Json(new { data = model });
    }
    [Route("{id}/update_flg")]
    [HttpPut]
    [Authorize(Policy = Policies.ADMIN_OUT_GOING_DATA)]
    [ApiExceptionFilter]
    public async Task<IActionResult> update_flg(OutGoingDataFileJudge form)
    {
        var entity = await _outgoingDataFileRepo.FindByIdAsync_new(form.Id);
        if (entity == null)
        {
            _ThrowNotFoundError(form.Id);
        }
        var userId = int.Parse(_userManager.GetUserId(User));
        var user = await _userRepo.FindByIdAsync(userId);
        string flg = "";
        if (form.Passed)
        {
            flg = "Y";
        }
        else
        {
            flg = "N";
            var vendor = await _vendorRepo.FindByIdAsync_new(entity.VendorAbbr);
            foreach (var file in vendor)
            {
                var model1 = _mapper.Map<OutGoingDatas_New, OutGoingDatas_NewList>(entity);
                    model1.VendorName = file.Name;
                await _emailSvc.SendOutGoingDataTnsMailAsync_new(model1, file.Name, file.Email);
            }
        }
        _UpdateEntityForView_flg(entity, flg);
        await _unitOfWork.CompleteAsync();
        entity = await _outgoingDataFileRepo.FindByIdAsync_new(form.Id);
        var model = _mapper.Map<OutGoingDatas_New, OutGoingDatas_NewList>(entity);
        return Json(new { data = model });
    }

        #endregion

    #region Private

    private void _UpdateEntityForJudgement(OutGoingDataFile entity, bool passed) {
      var now = DateTime.UtcNow.ToAppDateTime();
      var userId = int.Parse(_userManager.GetUserId(User));
      entity.Passed = passed;
      entity.UpdatedAt = now;
      entity.UpdaterId = userId;
      entity.JudgementAt = now;
      entity.JudgementId = userId;
      entity.OutGoingData.UpdatedAt = now;
      entity.OutGoingData.UpdaterId = userId;
      entity.OutGoingData.Judgemented = true;
    }

    private void _UpdateEntityForView(OutGoingDataFile entity) {
      var now = DateTime.UtcNow.ToAppDateTime();
      var userId = int.Parse(_userManager.GetUserId(User));
      entity.UpdatedAt = now;
      entity.UpdaterId = userId;
      entity.ViewAt = now;
      entity.ViewerId = userId;
    }

    private OutGoingDataFile _GetEntityForCreate(int id, string fileName, long fileSize) {
      var now = DateTime.UtcNow.ToAppDateTime();
      var entity = new OutGoingDataFile {
        OutGoingDataId = id,
        FileName = fileName,
        FileSize = fileSize,
        CreatedAt = now,
        UpdatedAt = now,
        CreatorId = int.Parse(_userManager.GetUserId(User))
      };
      return entity;
    }

    private void _ThrowNotFoundError(int id) {
      throw new AppException($"Out Going Data File with Id = {id} could not be found", StatusCodes.Status404NotFound);
    }


    //2021/09/15 lattapon
    private void _UpdateEntityForView_new(OutGoingDatas_New entity, string fileName)
    {
        var now = DateTime.UtcNow.ToAppDateTime();
        var userId = int.Parse(_userManager.GetUserId(User));
        entity.UpdatedAt = now;
        entity.Filename = fileName;
        entity.FlgJudge = null;
        entity.ReviewId = null;

    }


    private void _UpdateEntityForView_flg(OutGoingDatas_New entity, string flg)
    {
        var now = DateTime.UtcNow.ToAppDateTime();
        var userId = int.Parse(_userManager.GetUserId(User));
        entity.UpdatedAt = now;
        entity.FlgJudge = flg;
        entity.ReviewId = userId;
        if(flg == "N")
        {
            entity.Filename = null;
        }
    }

        #endregion

        #endregion
    }
}