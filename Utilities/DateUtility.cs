using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace QcSupplier.Utilities {
  public static class DateUtility {
    private static readonly Dictionary<string, string> _months = new Dictionary<string, string> { { "01", "Jan" },
      { "02", "Feb" },
      { "03", "Mar" },
      { "04", "Apr" },
      { "05", "May" },
      { "06", "Jun" },
      { "07", "Jul" },
      { "08", "Aug" },
      { "09", "Sep" },
      { "10", "Oct" },
      { "11", "Nov" },
      { "12", "Dec" },
    };

    public static IList<SelectListItem> GetMonths(string value) {
      var data = new List<SelectListItem>();
      foreach (var kvp in _months) {
        data.Add(new SelectListItem { Value = kvp.Key, Text = kvp.Value, Selected = kvp.Value == value });
      }
      foreach (var d in data) {
        d.Selected = d.Value == value;
      }
      return data;
    }

    public static IList<SelectListItem> GetYears(string value) {
      var selectedYear = DateTime.Today.Year;
      var startYear = selectedYear - 4;
      var endYear = selectedYear + 5;
      var data = new List<SelectListItem>();
      for (var i = startYear; i <= endYear; i++) {
        var year = i.ToString();
        data.Add(new SelectListItem { Value = year, Text = year, Selected = year == value });
      }
      return data;
    }

    public static Tuple<DateTime?, DateTime?> GetStartEnd(ref string month, ref string year) {
      DateTime? start = null;
      DateTime? end = null;
      if (!string.IsNullOrEmpty(month) && !string.IsNullOrEmpty(year)) {
        start = DateTime.Parse($"{year}-{month}-01");
        end = start.Value.AddMonths(1);
      } else if (string.IsNullOrEmpty(month) && !string.IsNullOrEmpty(year)) {
        start = DateTime.Parse($"{year}-01-01");
        end = start.Value.AddYears(1);
      } else {
        month = string.Empty;
        year = string.Empty;
      }
      return new Tuple<DateTime?, DateTime?>(start, end);
    }

    //2021/09/16 by lattapon
    public static IList<SelectListItem> GetFlg(string value)
    {
        var data = new List<SelectListItem>();
        data.Add(new SelectListItem { Value = "", Text = "Select", Selected = "Select" == value });
        data.Add(new SelectListItem { Value = "Wait", Text = "Waiting..", Selected = "" == value });
        data.Add(new SelectListItem { Value = "Y", Text = "OK", Selected = "Y" == value });
        data.Add(new SelectListItem { Value = "N", Text = "NG", Selected = "N" == value });
        data.Add(new SelectListItem { Value = "All", Text = "All", Selected = "All" == value });
        foreach (var d in data)
        {
            d.Selected = d.Value == value;
        }
        return data;
    }

    }
}