using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QcSupplier.Entities;
using QcSupplier.Extensions;
using QcSupplier.Models;
using QcSupplier.Persistence;

namespace QcSupplier.Repositories {
  public class DownloadRepository {
    #region Field

    private readonly AppDbContext _context;

    #endregion

    #region Constructor
    public DownloadRepository(AppDbContext context) {
      _context = context;
    }
    #endregion

    #region Method

    public void Add(Download entity) {
      _context.Downloads.Add(entity);
    }

    public async Task<Download> FindByIdAsync(int id) {
      return await _context.Downloads.FirstOrDefaultAsync(d => d.Id == id);
    }

    public async Task<QueryResult<Download>> FindListAsync(Keyword param) {
      var query = _context.Downloads.AsQueryable();
      query = query.ApplyFilter(param.Search);
      var totalItem = await query.CountAsync();
      query = query.OrderByDescending(q => q.UpdatedAt);
      query = query.ApplyPaging(param);
      var items = await query.ToListAsync();
      return new QueryResult<Download>(items, param.Page, totalItem, param.PageSize);
    }

    public void Remove(Download entity) {
      _context.Downloads.Remove(entity);
    }

    #endregion
  }
}