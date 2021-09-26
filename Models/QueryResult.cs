using System;
using System.Collections.Generic;

namespace QcSupplier.Models {
  public class QueryResult<T> {
    #region Property

    public int Page { get; private set; }
    public IList<T> Items { get; private set; }
    public int TotalItem { get; private set; }
    public int TotalPage { get; private set; }
    public string Url { get; set; }

    #endregion

    #region Constructor

    public QueryResult(IList<T> items, int page, int totalItem, int pageSize) {
      Items = items;
      Page = page;
      TotalItem = totalItem;
      TotalPage = (int)Math.Ceiling((decimal)TotalItem / pageSize);
    }

    #endregion

  }
}