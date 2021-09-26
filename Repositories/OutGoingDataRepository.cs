using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QcSupplier.Entities;
using QcSupplier.Extensions;
using QcSupplier.Models;
using QcSupplier.Persistence;

namespace QcSupplier.Repositories {
  public class OutGoingDataRepository {
    #region Field

    private readonly AppDbContext _context;

    #endregion

    #region Constructor

    public OutGoingDataRepository(AppDbContext context) {
      _context = context;
    }

    #endregion

    #region Method

    public void Remove(OutGoingData entity) {
      _context.OutGoingDatas.Remove(entity);
    }

    public void Add(OutGoingData entity) {
      _context.OutGoingDatas.Add(entity);
    }

    public async Task<OutGoingData> FindByIdAsync(int id) {
      return await _context.OutGoingDatas
        .Include(o => o.Files)
        .Include(o => o.Vendor)
        .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<QueryResult<OutGoingData>> FindListAsync(OutGoingDataModel param) {
      var query = _context.OutGoingDatas.AsQueryable();
      query = query.ApplyFilter(param.VendorId, param.UserVendorIds, param.Invoice, param.Start, param.End);
      var totalItem = await query.CountAsync();
      query = query
        .Include(o => o.Vendor)
        .OrderByDescending(q => q.Title)
          .ThenByDescending(q => q.Invoice);
      query = query.ApplyPaging(param);
      var items = await query.ToListAsync();
      return new QueryResult<OutGoingData>(items, param.Page, totalItem, param.PageSize);
    }

        //2021/09/14 lattapon
        public async Task<QueryResult<OutGoingDatas_New>> FindListAsync_vendor(OutGoingDataModel param, string datenow)
        {
            var query = _context.OutGoingDatas_New.AsQueryable();

            if (!string.IsNullOrEmpty(param.VendorId.ToString()))
            {
                var vendorAbbr = await FindVendorAbbr_id(int.Parse(param.VendorId.ToString()));
                IList<string> ilist = new List<string>();
                ilist.Add(vendorAbbr);
                query = query.ApplyFilter_New(param.VendorId, param.UserVendorIds, param.Invoice, param.Start, param.End, param.FlgJudge, ilist,"vendor");
            }
            else
            {
                query = query.ApplyFilter_New(param.VendorId, param.UserVendorIds, param.Invoice, param.Start, param.End, param.FlgJudge, param.VendorAbbr,"vendor");
            }

            query = query
                .Include(o => o.CreatorId)
                .Where(q => q.Status == "N" )
                //&& q.VdDocdt.CompareTo(datenow) >= 0)
                .OrderBy(q => q.VdDocdt)
                .ThenByDescending(q => q.DsSheetno)
                .ThenBy(q => q.VdDocno)
                .ThenBy(q => q.PartNo);
            query = query.ApplyPaging(param);
            var totalItem = await query.CountAsync();
            var items = await query.ToListAsync();
            return new QueryResult<OutGoingDatas_New>(items, param.Page, totalItem, param.PageSize);
        }

        public async Task<QueryResult<OutGoingDatas_New>> FindListAsync_admin(OutGoingDataModel param, string datenow)
        {
            var query = _context.OutGoingDatas_New.AsQueryable();
            //query = query.ApplyFilter_New(param.VendorId, param.UserVendorIds, param.Invoice, param.Start, param.End, param.FlgJudge ,param.VendorAbbr);
            if (!string.IsNullOrEmpty(param.VendorId.ToString()))
            {
                var vendorAbbr = await FindVendorAbbr_id(int.Parse(param.VendorId.ToString()));
                IList<string> ilist = new List<string>();
                ilist.Add(vendorAbbr);
                query = query.ApplyFilter_New(param.VendorId, param.UserVendorIds, param.Invoice, param.Start, param.End, param.FlgJudge, ilist,"admin");
            }
            else
            {
                query = query.ApplyFilter_New(param.VendorId, param.UserVendorIds, param.Invoice, param.Start, param.End, param.FlgJudge, param.VendorAbbr,"admin");
            }
            query = query
                .Include(o => o.CreatorId)
                .Where(q => q.Status == "N")
                .OrderBy(q => q.VdDocdt)
                .ThenByDescending(q => q.DsSheetno)
                .ThenBy(q => q.VdDocno)
                .ThenBy(q => q.PartNo);
            query = query.ApplyPaging(param);
            var totalItem = await query.CountAsync();
            var items = await query.ToListAsync();
            return new QueryResult<OutGoingDatas_New>(items, param.Page, totalItem, param.PageSize);
        }

        public async Task<List<string>> FindVendorAbbr(IList<int> id)
        {
            return await _context.Vendors
                .Where(c => id.Contains(c.Id))
                .Select(c => c.VendorAbbr)
                .ToListAsync();
        }
        public async Task<string> FindVendorAbbr_id(int id)
        {
            return await _context.Vendors
                .Where(c => c.Id == id)
                .Select(c => c.VendorAbbr)
                .FirstOrDefaultAsync();
        }

        #endregion
    }
}