using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace QcSupplier.Persistence {
  public class DbInitializer {
    #region Field

    private readonly AppDbContext _context;

    #endregion

    #region Constructor

    public DbInitializer(AppDbContext context) {
      _context = context;
    }

    #endregion

    #region Method

    public void Initialize() {
      try {
        if (_context.Database.GetPendingMigrations().Count() > 0) {
          _context.Database.Migrate();
        }
      } catch (Exception) { }
    }

    #endregion
  }
}