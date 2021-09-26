using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QcSupplier.Constants;
using QcSupplier.Entities;
using QcSupplier.Extensions;
using QcSupplier.Models;
using QcSupplier.Persistence;

namespace QcSupplier.Repositories {
  public class UserRepository {
    #region Field

    private readonly AppDbContext _context;

    #endregion

    #region Constructor

    public UserRepository(AppDbContext context) {
      _context = context;
    }

    #endregion

    #region Method

    public User FindByEmail(string email) {
      return _context.Users.FirstOrDefault(u => u.Email == email);
    }

    public async Task<User> FindByIdAsync(int id) {
      return await _context.Users
        .Include(u => u.UserRoles)
        .Include(u => u.UserPrograms)
        .Include(u => u.UserVendors)
        .FirstOrDefaultAsync(u => u.Id == id);
    }

    public User FindByUserName(string username) {
      return _context.Users.FirstOrDefault(u => u.UserName == username);
    }

    public async Task<QueryResult<User>> FindListAsync(Keyword param) {
      var query = _context.Users.AsQueryable();
      query = query.ApplyFilter(param.Search);
      var totalItem = await query.CountAsync();
      query = query.Include(u => u.Department).OrderByDescending(q => q.UpdatedAt);
      query = query.ApplyPaging(param);
      var items = await query.ToListAsync();
      return new QueryResult<User>(items, param.Page, totalItem, param.PageSize);
    }

    public async Task<User> FindByIdWithVendorsAsync(int id) {
      return await _context.Users.Include(u => u.UserVendors).ThenInclude(v => v.Vendor).FirstAsync(u => u.Id == id);
    }

    public async Task<IList<User>> FindVendorListByProgramAsync(int programId) {
      return await _context.Users.Where(u =>
        u.UserRoles.Any(ur => ur.RoleId == Roles.Vendor.Id) &&
        u.UserPrograms.Any(up => up.ProgramId == programId)
      ).ToListAsync();
    }

    public async Task<IList<User>> FindTnsListByVendorAndProgramAsync(int vendorId, int programId) {
      return await _context.Users
        .Where(u =>
          u.UserRoles.Any(ur => ur.RoleId == Roles.Admin.Id) &&
          u.UserVendors.Any(uv => uv.VendorId == vendorId) &&
          u.UserPrograms.Any(up => up.ProgramId == programId)
        ).ToListAsync();
    }

    public async Task<IList<User>> FindVendorListByVendorAndProgramAsync(int vendorId, int programId) {
      return await _context.Users
        .Where(u =>
          u.UserRoles.Any(ur => ur.RoleId == Roles.Vendor.Id) &&
          u.UserVendors.Any(uv => uv.VendorId == vendorId) &&
          u.UserPrograms.Any(up => up.ProgramId == programId)
        ).ToListAsync();
    }

    public async Task<IList<User>> FindVendorListByVendorsAndProgramAsync(IList<int> vendors, int programId) {
      return await _context.Users
        .Where(u =>
          u.UserRoles.Any(ur => ur.RoleId == Roles.Vendor.Id) &&
          u.UserVendors.Any(uv => vendors.Contains(uv.VendorId)) &&
          u.UserPrograms.Any(up => up.ProgramId == programId)
        ).ToListAsync();
    }

    public async Task<bool> IsInUse(int id) {
      return await _context.Users.AnyAsync(u =>
        u.SupplierEvaluationCreators.Any(s => s.CreatorId == id) ||
        u.SupplierEvaluationUpdaters.Any(s => s.UpdaterId == id) ||
        u.DownloadCreators.Any(d => d.CreatorId == id) ||
        u.DownloadUpdaters.Any(d => d.UpdaterId == id) ||
        u.TnsFormCreators.Any(t => t.CreatorId == id) ||
        u.TnsFormUpdaters.Any(t => t.UpdaterId == id) ||
        u.SelfControlledIPPCreators.Any(s => s.CreatorId == id) ||
        u.SelfControlledIPPUpdaters.Any(s => s.UpdaterId == id)
      );
    }

    #endregion
  }
}