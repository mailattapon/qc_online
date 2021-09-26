using System;

namespace QcSupplier.Repositories {
  public abstract class BaseRepository {
    public DateTime CurrentDate {
      get {
        var today = DateTime.Today;
        return new DateTime(today.Year, today.Month, 1);
      }
    }
  }
}