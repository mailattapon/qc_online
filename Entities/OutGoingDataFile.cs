using System;

namespace QcSupplier.Entities {
  public class OutGoingDataFile {
    public int Id { get; set; }
    public int OutGoingDataId { get; set; }
    public string FileName { get; set; }
    public long FileSize { get; set; }
    public bool? Passed { get; set; }
    public int CreatorId { get; set; }
    public int? UpdaterId { get; set; }
    public int? ViewerId { get; set; }
    public int? JudgementId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? ViewAt { get; set; }
    public DateTime? JudgementAt { get; set; }
    public virtual User Creator { get; set; }
    public virtual User Updater { get; set; }
    public virtual User Viewer { get; set; }
    public virtual User Judgementor { get; set; }
    public virtual OutGoingData OutGoingData { get; set; }
  }
}