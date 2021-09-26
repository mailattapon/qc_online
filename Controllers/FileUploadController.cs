using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QcSupplier.Settings;

namespace QcSupplier.Controllers {
  public abstract class FileUploadController : Controller {
    #region Field

    private readonly IHostingEnvironment _hostingEnv;
    private readonly FileUploadSetting _fileUploadSetting;
    private readonly FormatSetting _formatSetting;

    #endregion

    #region Property

    public abstract string FileNamePrefix { get; }
    public abstract string DetailPrefix { get; }
    public DateTime CurrentDate {
      get {
        var today = DateTime.Today;
        return new DateTime(today.Year, today.Month, 1);
      }
    }

    #endregion

    #region Constructor

    public FileUploadController(
      IHostingEnvironment hostingEnv,
      FileUploadSetting fileUploadSetting,
      FormatSetting formatSetting
    ) {
      _hostingEnv = hostingEnv;
      _fileUploadSetting = fileUploadSetting;
      _formatSetting = formatSetting;
    }

    #endregion

    #region Method

    protected string GetLogicalFolder(int vendor) {
      return Path.Combine(_fileUploadSetting.Folder, vendor.ToString());
    }

    protected string GetLogicalFolder() {
      return Path.Combine(_fileUploadSetting.Folder);
    }

    protected bool ValidateFile(IFormFile file, out string message) {
      message = string.Empty;
      if (file == null) {
        message = "File is required";
        return false;
      }
      if (file.Length > _fileUploadSetting.MaxBytes) {
        message = "File size exceeded (10 MB limit)";
        return false;
      }
      return true;
    }

    protected string GetLogicalFilePathWithPredefinedName(string logicalFolder, string originalFileName, DateTime date) {
      var physicalFolder = GetPhysicalFolder(logicalFolder);
      var suffix = date.ToString(_formatSetting.FileDateFormat);
      var extension = Path.GetExtension(originalFileName);
      var fileName = $"{FileNamePrefix}_{suffix}{extension}";
      var filePath = Path.Combine(physicalFolder, fileName);
      if (System.IO.File.Exists(filePath)) {
        var guid = Guid.NewGuid();
        fileName = $"{FileNamePrefix}_{suffix}_{guid}{extension}";
      }
      return Path.Combine(logicalFolder, fileName);
    }

    protected string GetLogicalFilePathWithOriginalName(string logicalFolder, string fileName) {
      var physicalFolder = GetPhysicalFolder(logicalFolder);
      var filePath = Path.Combine(physicalFolder, fileName);
      if (System.IO.File.Exists(filePath)) {
        var guid = Guid.NewGuid();
        var name = Path.GetFileNameWithoutExtension(fileName);
        var extension = Path.GetExtension(fileName);
        fileName = $"{name}_{guid}{extension}";
      }
      return Path.Combine(logicalFolder, fileName);
    }

    protected async Task Upload(IFormFile file, string logicalFilePath) {
      var physicalFilePath = Path.Combine(_hostingEnv.WebRootPath, logicalFilePath);
      using (var fileStream = new FileStream(physicalFilePath, FileMode.Create)) {
        await file.CopyToAsync(fileStream);
      }
    }

    protected void DeleteFile(string logicalFilePath) {
      var physicalFilePath = Path.Combine(_hostingEnv.WebRootPath, logicalFilePath);
      if (System.IO.File.Exists(physicalFilePath)) {
        System.IO.File.Delete(physicalFilePath);
      }
    }

    protected string GetPhysicalFolder(string logicalFolder) {
      var folder = Path.Combine(_hostingEnv.WebRootPath, logicalFolder);
      if (!Directory.Exists(folder)) {
        Directory.CreateDirectory(folder);
      }
      return folder;
    }

    protected string GetDetail(DateTime date) {
      if (string.IsNullOrEmpty(DetailPrefix)) {
        return string.Empty;
      }
      var suffix = date.ToString(_formatSetting.NameDateFormat);
      return $"{DetailPrefix} {suffix}";
    }

    #endregion
  }
}