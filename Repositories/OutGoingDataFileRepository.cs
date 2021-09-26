using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QcSupplier.Entities;
using QcSupplier.Persistence;

namespace QcSupplier.Repositories {
  public class OutGoingDataFileRepository {
    #region Field

    private readonly AppDbContext _context;

    #endregion

    #region Constructor

    public OutGoingDataFileRepository(AppDbContext context) {
      this._context = context;
    }

    #endregion

    #region Method

    public async Task<IList<OutGoingDataFile>> FindListAsync(int id) {
      return await _context.OutGoingDataFiles
        .Include(o => o.Judgementor)
        .Where(o => o.OutGoingDataId == id)
        .OrderBy(o => o.Id)
        .ToListAsync();
    }

    public async Task<OutGoingDataFile> FindByIdAsync(int id) {
      return await _context.OutGoingDataFiles
        .Include(o => o.Judgementor)
        .Include(o => o.OutGoingData)
        .FirstOrDefaultAsync(o => o.Id == id);
    }

    public void Remove(OutGoingDataFile entity) {
      _context.OutGoingDataFiles.Remove(entity);
    }

    public void Add(OutGoingDataFile entity) {
      _context.OutGoingDataFiles.Add(entity);
    }

    //2021/09/15 lattapon
    public async Task<OutGoingDatas_New> FindByIdAsync_new(int id)
    {
        return await _context.OutGoingDatas_New
            .FirstOrDefaultAsync(o => o.Id == id);
    }
        public async Task<IList<OutGoingDatas_New>> FindListAsync_new(int id)
        {
            //return await _context.OutGoingDatas_New
            //    .Include(o => o.CreatorId)
            //    .Where(q => q.Id == id)
            //    .OrderBy(q => q.VdDocdt)
            //    .ThenByDescending(q => q.DsSheetno)
            //    .ThenBy(q => q.VdDocno)
            //    .ThenBy(q => q.PartNo)
            //.ToListAsync();
            return await (from t1 in _context.OutGoingDatas_New
                          join t2 in _context.Users on t1.ReviewId equals t2.Id into t3
                          from m in t3.DefaultIfEmpty()
                          where t1.Id == id
                          orderby t1
                          select new OutGoingDatas_New
                          {
                              Id = t1.Id,  // add ใส่ model 
                              VendorAbbr = t1.VendorAbbr,
                              VdDocdt = t1.VdDocdt,
                              DsSheetno = t1.DsSheetno,
                              VdDocno = t1.VdDocno,
                              PartNo = t1.PartNo,
                              PartName = t1.PartName,
                              Status = t1.Status,
                              Filename = t1.Filename,
                              FlgJudge = t1.FlgJudge,
                              ReviewId = t1.ReviewId,
                              CreatedAt = t1.CreatedAt,
                              UpdatedAt = t1.UpdatedAt,
                              //reviewName = m.UserName
                          }).ToListAsync();
        }

        #endregion

    }
}