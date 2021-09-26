using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QcSupplier.Entities;
using QcSupplier.Extensions;
using QcSupplier.Models;
using QcSupplier.Persistence;

namespace QcSupplier.Repositories {
  public class VendorRepository : BaseRepository {
    #region Field

    private readonly AppDbContext _context;

    #endregion

    #region Constructor 

    public VendorRepository(AppDbContext context) {
      _context = context;
    }

    #endregion

    #region Method

    public void Add(Vendor entity) {
      _context.Vendors.Add(entity);
    }

    public Vendor FindByEmail(string email) {
      return _context.Vendors.FirstOrDefault(v => v.Email == email);
    }

    public async Task<Vendor> FindByIdAsync(int id) {
      return await _context.Vendors.FirstOrDefaultAsync(v => v.Id == id);
    }

    public async Task<List<Vendor>> FindByIdsAsync(IList<int> ids) {
      return await _context.Vendors.Where(v => ids.Contains(v.Id)).ToListAsync();
    }

    public Vendor FindByName(string name) {
      return _context.Vendors.FirstOrDefault(v => v.Name == name);
    }

    public async Task<IList<Vendor>> FindListAsync() {
      return await _context.Vendors.OrderBy(v => v.Name).ToListAsync();
    }

    public async Task<QueryResult<Vendor>> FindListAsync(Keyword param) {
      var query = _context.Vendors.AsQueryable();
      query = query.ApplyFilter(param.Search);
      var totalItem = await query.CountAsync();
      query = query.OrderByDescending(q => q.UpdatedAt);
      query = query.ApplyPaging(param);
      var items = await query.ToListAsync();
      return new QueryResult<Vendor>(items, param.Page, totalItem, param.PageSize);
    }

    public async Task<IList<int>> GetListForSelfControlledIPPReminderAsync() {
      return await _context.Vendors
        .Where(v => v.SendIPP && !v.SelfControlledIPPs.Any(s => s.SelectedDate == CurrentDate))
        .Select(v => v.Id)
        .ToListAsync();
    }

    public async Task<bool> IsInUse(int id) {
      return await _context.Vendors.AnyAsync(
        v => v.Id == id && (
          v.UserVendors.Any(uv => uv.VendorId == id) ||
          v.SupplierEvaluations.Any(s => s.VendorId == id) ||
          v.SelfControlledIPPs.Any(s => s.VendorId == id)
        )
      );
    }

    public void Remove(Vendor entity) {
      _context.Vendors.Remove(entity);
    }

    //2021/09/15 lattapon
    public async Task<IList<Vendor>> FindByIdAsync_new(string VendorAbbr)
    {
        return await _context.Vendors.Where(v => v.VendorAbbr == VendorAbbr).ToListAsync();
    }


        #endregion
    }
}