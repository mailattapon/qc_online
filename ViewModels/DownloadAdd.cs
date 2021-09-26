using Microsoft.AspNetCore.Http;

namespace QcSupplier.ViewModels {
  public class DownloadAdd {
    public string Title { get; set; }
    public string Detail { get; set; }
    public IFormFile File { get; set; }
  }
}