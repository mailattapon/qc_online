namespace QcSupplier.TagHelpers {
  public interface IPaginationTagHelperData {
    int Page { get; set; }
    int TotalPage { get; set; }
    string Url { get; set; }
  }
}