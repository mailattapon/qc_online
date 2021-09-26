using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QcSupplier.Entities;
using QcSupplier.Extensions;
using QcSupplier.Models;
using QcSupplier.Persistence;

namespace QcSupplier.Repositories {
  public class DepartmentRepository {
    #region Field

    private readonly AppDbContext _context;

    #endregion

    #region Constructor

    public DepartmentRepository(AppDbContext context) {
      _context = context;
    }

    #endregion

    #region Method

    public void Add(Department entity) {
      _context.Departments.Add(entity);
    }

    public void Remove(Department entity) {
      _context.Departments.Remove(entity);
    }

    public async Task<Department> FindByIdAsync(int id) {
      return await _context.Departments.FirstOrDefaultAsync(d => d.Id == id);
    }

    public async Task<IList<Department>> FindListAsync() {
      return await _context.Departments.ToListAsync();
    }
    public async Task<QueryResult<Department>> FindListAsync(Keyword param) {
      var query = _context.Departments.AsQueryable();
      query = query.ApplyFilter(param.Search);
      var totalItem = await query.CountAsync();
      query = query.OrderByDescending(q => q.UpdatedAt);
      query = query.ApplyPaging(param);
      var items = await query.ToListAsync();
      return new QueryResult<Department>(items, param.Page, totalItem, param.PageSize);
    }

    public Department FindByName(string name) {
      return _context.Departments.FirstOrDefault(d => d.Name == name);
    }

    public async Task<bool> IsInUse(int id) {
      return await _context.Departments.AnyAsync(d => d.Id == id && d.Users.Any(u => u.DepartmentId == id));
    }

    #endregion
  }
}