using System;

namespace QcSupplier.Entities {
  public class Download {
    public int Id { get; set; }
    public string Title { get; set; }
    public string Detail { get; set; }
    public string FileName { get; set; }
    public long FileSize { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int CreatorId { get; set; }
    public int? UpdaterId { get; set; }
    public virtual User Creator { get; set; }
    public virtual User Updater { get; set; }
  }
}