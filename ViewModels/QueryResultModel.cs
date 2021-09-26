using System.Collections.Generic;
using QcSupplier.TagHelpers;

namespace QcSupplier.ViewModels {
  public class QueryResultModel<T> : IPaginationTagHelperData {
    public int Page { get; set; }
    public IList<T> Items { get; set; }
    public int TotalItem { get; set; }
    public int TotalPage { get; set; }
    public string Url { get; set; }
  }
}