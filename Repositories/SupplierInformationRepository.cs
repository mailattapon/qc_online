using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QcSupplier.Entities;
using QcSupplier.Extensions;
using QcSupplier.Models;
using QcSupplier.Persistence;

namespace QcSupplier.Repositories {
  public class SupplierInformationRepository {
    #region Field

    private readonly AppDbContext _context;

    #endregion

    #region Constructor

    public SupplierInformationRepository(AppDbContext context) {
      _context = context;
    }

    #endregion 

    #region Method

    public void Remove(SupplierInformation entity) {
      _context.SupplierInformations.Remove(entity);
    }

    public async Task<SupplierInformation> FindByIdAsync(int id) {
      return await _context.SupplierInformations.FirstOrDefaultAsync(s => s.Id == id);
    }

    public void Add(SupplierInformation entity) {
      _context.SupplierInformations.Add(entity);
    }

    public async Task<QueryResult<SupplierInformation>> FindListAsync(SupplierInformationModel param) {
      var query = _context.SupplierInformations.AsQueryable();
      query = query.ApplyFilter(param.VendorId, param.UserVendorIds, param.Search);
      var totalItem = await query.CountAsync();
      query = query.Include(s => s.Vendor).OrderByDescending(q => q.UpdatedAt);
      query = query.ApplyPaging(param);
      var items = await query.ToListAsync();
      return new QueryResult<SupplierInformation>(items, param.Page, totalItem, param.PageSize);
    }

    #endregion
  }
}