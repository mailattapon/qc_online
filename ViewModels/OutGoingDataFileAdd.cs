using Microsoft.AspNetCore.Http;

namespace QcSupplier.ViewModels {
  public class OutGoingDataFileAdd {
    public int Id { get; set; }
    public IFormFile File { get; set; }
  }
}