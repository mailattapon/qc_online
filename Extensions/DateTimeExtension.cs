using System;
using QcSupplier.Settings;
using TimeZoneConverter;

namespace QcSupplier.Extensions {
  public static class DateTimeExtension {
    public static string ToString(this DateTime? result, string format) {
      return result.HasValue ? result.Value.ToString(format) : "";
    }

    public static DateTime ToAppDateTime(this DateTime date) {
      return TimeZoneInfo.ConvertTimeFromUtc(date, TZConvert.GetTimeZoneInfo(AppInfoSetting.TIMEZONE));
    }
  }
}