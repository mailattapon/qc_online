namespace QcSupplier.ViewModels {
  public class Pagination {
    #region Field

    private int _Page;

    #endregion

    #region Property

    public int Page {
      get { return _Page < 1 ? 1 : _Page; }
      set { _Page = value; }
    }

    #endregion
  }
}