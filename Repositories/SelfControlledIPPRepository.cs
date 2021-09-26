using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QcSupplier.Entities;
using QcSupplier.Extensions;
using QcSupplier.Models;
using QcSupplier.Persistence;

namespace QcSupplier.Repositories {
  public class SelfControlledIPPRepository {
    #region Field

    private readonly AppDbContext _context;

    #endregion

    #region Constructor 

    public SelfControlledIPPRepository(AppDbContext context) {
      this._context = context;
    }

    #endregion

    #region Method

    public void Add(SelfControlledIPP entity) {
      _context.SelfControlledIPPs.Add(entity);
    }

    public async Task<SelfControlledIPP> FindByIdAsync(int id) {
      return await _context.SelfControlledIPPs.FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<QueryResult<SelfControlledIPP>> FindListAsync(VendorDate param) {
      var query = _context.SelfControlledIPPs.AsQueryable();
      query = query.ApplyFilter(param.VendorId, param.Start, param.End, param.UserVendorIds);
      var totalItem = await query.CountAsync();
      query = query.Include(s => s.Vendor).OrderByDescending(q => q.UpdatedAt);
      query = query.ApplyPaging(param);
      var items = await query.ToListAsync();
      return new QueryResult<SelfControlledIPP>(items, param.Page, totalItem, param.PageSize);
    }

    public void Remove(SelfControlledIPP entity) {
      _context.SelfControlledIPPs.Remove(entity);
    }

    #endregion
  }
}