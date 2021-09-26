using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace QcSupplier.ViewModels {
  public class OutGoingDataAdd {
    public string Title { get; set; }
    public string Detail { get; set; }
    public string Invoice { get; set; }
    public List<IFormFile> Files { get; set; }
  }
}