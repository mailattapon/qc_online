using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QcSupplier.Entities;
using QcSupplier.Extensions;
using QcSupplier.Models;
using QcSupplier.Persistence;

namespace QcSupplier.Repositories {
  public class SupplierEvaluationRepository : BaseRepository {
    #region Field 

    private readonly AppDbContext _context;

    #endregion

    #region Constructor

    public SupplierEvaluationRepository(AppDbContext context) {
      _context = context;
    }

    #endregion

    #region Method

    public void Add(SupplierEvaluation entity) {
      _context.SupplierEvaluations.Add(entity);
    }

    public async Task<SupplierEvaluation> FindByIdAsync(int id) {
      return await _context.SupplierEvaluations.FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<SupplierEvaluation> FindByVendorAndSelectedDate(int vendorId, DateTime selectedDate) {
      return await _context.SupplierEvaluations.FirstOrDefaultAsync(s => s.VendorId == vendorId && s.SelectedDate == selectedDate);
    }

    public async Task<QueryResult<SupplierEvaluation>> FindListAsync(VendorDate param) {
      var query = _context.SupplierEvaluations.AsQueryable();
      query = query.ApplyFilter(param.VendorId, param.Start, param.End, param.UserVendorIds);
      var totalItem = await query.CountAsync();
      query = query.Include(s => s.Vendor).OrderByDescending(q => q.UpdatedAt);
      query = query.ApplyPaging(param);
      var items = await query.ToListAsync();
      return new QueryResult<SupplierEvaluation>(items, param.Page, totalItem, param.PageSize);
    }

    public async Task<IList<int>> GetVendorsForSupplierEvaluationReminderAsync() {
      return await _context.SupplierEvaluations
        .Where(s => s.SelectedDate == CurrentDate && !s.UpdaterId.HasValue)
        .Select(s => s.VendorId)
        .Distinct()
        .ToListAsync();
    }

    public void Remove(SupplierEvaluation entity) {
      _context.SupplierEvaluations.Remove(entity);
    }

    #endregion
  }
}