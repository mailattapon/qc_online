using System;
using Microsoft.AspNetCore.Http;

namespace QcSupplier.ViewModels {
  public class SelfControlledIPPAdd {
    public DateTime SelectedDate { get; set; }
    public IFormFile File { get; set; }
  }
}