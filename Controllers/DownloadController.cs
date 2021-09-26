using System;
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
  public class DownloadController : FileUploadController {

    #region Field

    private readonly IMapper _mapper;
    private readonly FileUploadSetting _fileUploadSetting;
    private readonly UserManager<User> _userManager;
    private readonly UnitOfWork _unitOfWork;
    private readonly DownloadRepository _downloadRepo;
    private readonly EmailService _emailSvc;

    #endregion

    #region Property

    public override string FileNamePrefix { get { throw new NotImplementedException(); } }
    public override string DetailPrefix { get { throw new NotImplementedException(); } }

    #endregion

    #region Constructor

    public DownloadController(
      IMapper mapper,
      IHostingEnvironment hostingEnv,
      FormatSetting formatSetting,
      FileUploadSetting fileUploadSetting,
      UserManager<User> userManager,
      UnitOfWork unitOfWork,
      DownloadRepository downloadRepo,
      EmailService emailSvc
    ) : base(hostingEnv, fileUploadSetting, formatSetting) {
      _mapper = mapper;
      _fileUploadSetting = fileUploadSetting;
      _userManager = userManager;
      _unitOfWork = unitOfWork;
      _downloadRepo = downloadRepo;
      _emailSvc = emailSvc;
    }

    #endregion

    #region Method

    #region Public

    [Route("")]
    [HttpGet]
    [Authorize(Policy = Policies.DOWNLOAD)]
    public async Task<IActionResult> Get(KeywordParam param) {
      var paramModel = _mapper.Map<KeywordParam, Keyword>(param);
      var entities = await _downloadRepo.FindListAsync(paramModel);
      var model = _mapper.Map<QueryResult<Download>, QueryResultModel<DownloadList>>(entities);
      var url = Url.Action(nameof(Get));
      model.Url = $"{url}?search={param.Search}&page=:";
      ViewBag.Keyword = param.Search;
      return View(model);
    }

    [Route("")]
    [HttpPost]
    [Authorize(Policy = Policies.ADMIN_DOWNLOAD)]
    [ApiExceptionFilter]
    public async Task<IActionResult> Add(DownloadAdd model) {
      var logicalFolder = GetLogicalFolder();
      var logicalFilePath = GetLogicalFilePathWithOriginalName(logicalFolder, model.File.FileName);
      await Upload(model.File, logicalFilePath);
      var entity = _GetEntityForCreate(model, logicalFilePath);
      _downloadRepo.Add(entity);
      await _unitOfWork.CompleteAsync();
      await _emailSvc.SendDownloadMailAsync(entity);
      return Json(new { data = "OK" }).WithSuccess("Success", "Download have been successfully created");
    }

    [Route("{id}")]
    [HttpPost]
    [Authorize(Policy = Policies.ADMIN_DOWNLOAD)]
    public async Task<IActionResult> Delete(int id) {
      var entity = await _downloadRepo.FindByIdAsync(id);
      if (entity == null) {
        _ThrowNotFoundError(id);
      }
      if (!string.IsNullOrEmpty(entity.FileName)) {
        DeleteFile(entity.FileName);
      }
      _downloadRepo.Remove(entity);
      await _unitOfWork.CompleteAsync();
      return RedirectToAction(nameof(Get)).WithSuccess("Success", "Download have been successfully deleted");
    }

    #endregion

    #region Private

    private Download _GetEntityForCreate(DownloadAdd model, string logicalFilePath) {
      var now = DateTime.UtcNow.ToAppDateTime();
      var entity = new Download {
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
      throw new AppException($"Download with Id = {id} could not be found", StatusCodes.Status404NotFound);
    }

    #endregion

    #endregion

  }
}