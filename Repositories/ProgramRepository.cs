using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QcSupplier.Persistence;

namespace QcSupplier.Repositories {
  public class ProgramRepository {
    #region Field

    private readonly AppDbContext _context;

    #endregion

    #region Constructors

    public ProgramRepository(AppDbContext context) {
      _context = context;
    }

    #endregion

    #region Method

    public async Task<IList<Entities.Program>> FindListAsync() {
      return await _context.Programs.OrderBy(p => p.Id).ToListAsync();
    }

    #endregion

  }
}