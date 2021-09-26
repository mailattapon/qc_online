using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QcSupplier.Entities;
using QcSupplier.Extensions;
using QcSupplier.Models;
using QcSupplier.Persistence;

namespace QcSupplier.Repositories {
  public class TnsFormRepository {
    #region Field

    private readonly AppDbContext _context;

    #endregion

    #region Constructor 

    public TnsFormRepository(AppDbContext context) {
      _context = context;
    }
    #endregion

    #region Method

    public void Add(TnsForm entity) {
      _context.TnsForms.Add(entity);
    }

    public async Task<TnsForm> FindByIdAsync(int id) {
      return await _context.TnsForms.FirstOrDefaultAsync(d => d.Id == id);
    }

    public async Task<QueryResult<TnsForm>> FindListAsync(Keyword param) {
      var query = _context.TnsForms.AsQueryable();
      query = query.ApplyFilter(param.Search);
      var totalItem = await query.CountAsync();
      query = query.OrderByDescending(q => q.UpdatedAt);
      query = query.ApplyPaging(param);
      var items = await query.ToListAsync();
      return new QueryResult<TnsForm>(items, param.Page, totalItem, param.PageSize);
    }

    public void Remove(TnsForm entity) {
      _context.TnsForms.Remove(entity);
    }

    #endregion
  }
}