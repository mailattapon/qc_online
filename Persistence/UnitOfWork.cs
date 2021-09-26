using System.Threading.Tasks;

namespace QcSupplier.Persistence {
  public class UnitOfWork {
    #region Field

    private readonly AppDbContext _context;

    #endregion

    #region Constructor

    public UnitOfWork(AppDbContext context) {
      _context = context;
    }

    #endregion

    #region Method

    public async Task CompleteAsync() {
      await _context.SaveChangesAsync();
    }

    #endregion

  }
}