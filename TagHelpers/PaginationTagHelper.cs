using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace QcSupplier.TagHelpers {
  [HtmlTargetElement("div", Attributes = "pagination")]
  public class PaginationTagHelper : TagHelper {
    #region Fields

    private readonly string Ellipsis = "...";
    private readonly string PagePlaceHolder = ":";
    public readonly string PageClass = "btn btn-border px-3";
    public readonly string PageClassNormal = "btn-light";
    public readonly string PageClassSelected = "btn-primary active";

    #endregion

    #region Property

    public IPaginationTagHelperData Pagination { get; set; }

    #endregion

    #region Method

    #region Public

    public override void Process(TagHelperContext context, TagHelperOutput output) {
      var result = new TagBuilder("div");
      var maker = Maker();
      foreach (var i in maker) {
        var page = i.ToString();
        var tag = new TagBuilder("a");
        tag.AddCssClass(PageClass);
        if (page == Ellipsis) {
          tag.AddCssClass($"{PageClassNormal} disabled");
          tag.InnerHtml.Append(Ellipsis);
        } else {
          var url = Pagination.Url.Replace(PagePlaceHolder, page);
          tag.Attributes["href"] = url;
          tag.AddCssClass(int.Parse(page) == Pagination.Page ? PageClassSelected : PageClassNormal);
          tag.InnerHtml.Append(page);
        }

        result.InnerHtml.AppendHtml(tag);
      }
      output.Content.AppendHtml(result.InnerHtml);
    }

    #endregion

    #region Private

    private List<object> Maker() {
      int current = Pagination.Page;
      int last = Pagination.TotalPage;
      int delta = 4;
      int left = current - delta;
      int right = current + delta + 1;
      var range = new List<int>();
      var rangeWithDots = new List<object>();
      var l = 0;
      for (var i = 1; i <= last; i++) {
        if (i == 1 || i == last || i >= left && i < right) {
          range.Add(i);
        }
      }
      foreach (var item in range) {
        if (l > 0) {
          if (item - l == 2) {
            rangeWithDots.Add(l + 1);
          } else if (item - l != 1) {
            rangeWithDots.Add(Ellipsis);
          }
        }
        rangeWithDots.Add(item);
        l = item;
      }
      return rangeWithDots;
    }

    #endregion

    #endregion
  }
}