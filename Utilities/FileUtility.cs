using System;

namespace QcSupplier.Utilities {
  public static class FileUtility {
    #region Field

    private static readonly string[] SizeSuffixes = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
    private static readonly int decimalPlaces = 1;

    #endregion

    #region Method

    public static string SizeSuffix(long value) {
      if (value < 0) { return "-" + SizeSuffix(-value); }
      if (value == 0) { return string.Format("{0:n" + decimalPlaces + "} bytes", 0); }
      int mag = (int)Math.Log(value, 1024);
      decimal adjustedSize = (decimal)value / (1L << (mag * 10));
      if (Math.Round(adjustedSize, decimalPlaces) >= 1000) {
        mag += 1;
        adjustedSize /= 1024;
      }
      return string.Format("{0:n" + decimalPlaces + "} {1}", adjustedSize, SizeSuffixes[mag]);
    }

    #endregion

  }
}