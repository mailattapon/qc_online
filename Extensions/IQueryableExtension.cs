using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QcSupplier.Constants;
using QcSupplier.Entities;
using QcSupplier.Models;
using QcSupplier.Persistence;

namespace QcSupplier.Extensions {
  public static class IQueryableExtension {
    

    public static IQueryable<T> ApplyPaging<T>(this IQueryable<T> query, Pagination pagination) {
      return query.Skip((pagination.Page - 1) * pagination.PageSize).Take(pagination.PageSize);
    }

    public static IQueryable<Department> ApplyFilter(this IQueryable<Department> query, string search) {
      if (string.IsNullOrEmpty(search)) {
        return query;
      }
      query = query.Where(d => d.Name.ToLower().Contains(search.ToLower()));
      return query;
    }

    public static IQueryable<Vendor> ApplyFilter(this IQueryable<Vendor> query, string search) {
      if (string.IsNullOrEmpty(search)) {
        return query;
      }
      search = search.ToLower();
      query = query.Where(d => d.Name.ToLower().Contains(search) || d.Email.ToLower().Contains(search));
      return query;
    }

    public static IQueryable<User> ApplyFilter(this IQueryable<User> query, string search) {
      if (string.IsNullOrEmpty(search)) {
        return query;
      }
      search = search.ToLower();
      query = query.Where(d =>
        d.UserName.ToLower().Contains(search) ||
        d.Email.ToLower().Contains(search) ||
        d.Department.Name.ToLower().Contains(search)
      );
      return query;
    }

    public static IQueryable<SupplierEvaluation> ApplyFilter(
      this IQueryable<SupplierEvaluation> query,
      int? vendorId,
      DateTime? start,
      DateTime? end,
      IList<int> userVendorIds
    ) {
      if (vendorId.HasValue) {
        query = query.Where(s => s.VendorId == vendorId.Value);
      } else {
        query = query.Where(s => s.Vendor.UserVendors.Any(uv => userVendorIds.Contains(uv.VendorId)));
      }
      if (start.HasValue && end.HasValue) {
        query = query.Where(s => s.SelectedDate >= start.Value && s.SelectedDate < end.Value);
      }
      return query;
    }

    public static IQueryable<Download> ApplyFilter(this IQueryable<Download> query, string search) {
      if (string.IsNullOrEmpty(search)) {
        return query;
      }
      search = search.ToLower();
      query = query.Where(d => d.Title.ToLower().Contains(search) || d.Detail.ToLower().Contains(search));
      return query;
    }

    public static IQueryable<TnsForm> ApplyFilter(this IQueryable<TnsForm> query, string search) {
      if (string.IsNullOrEmpty(search)) {
        return query;
      }
      search = search.ToLower();
      query = query.Where(d => d.Title.ToLower().Contains(search) || d.Detail.ToLower().Contains(search));
      return query;
    }

    public static IQueryable<SelfControlledIPP> ApplyFilter(
      this IQueryable<SelfControlledIPP> query,
      int? vendorId,
      DateTime? start,
      DateTime? end,
      IList<int> userVendorIds
    ) {
      if (vendorId.HasValue) {
        query = query.Where(s => s.VendorId == vendorId.Value);
      } else {
        query = query.Where(s => s.Vendor.UserVendors.Any(uv => userVendorIds.Contains(uv.VendorId)));
      }
      if (start.HasValue && end.HasValue) {
        query = query.Where(s => s.SelectedDate >= start.Value && s.SelectedDate < end.Value);
      }
      return query;
    }

    public static IQueryable<SupplierInformation> ApplyFilter(this IQueryable<SupplierInformation> query, int? vendorId, IList<int> userVendorIds, string search) {
      if (vendorId.HasValue) {
        query = query.Where(s => s.VendorId == vendorId.Value);
      } else {
        query = query.Where(s => s.Vendor.UserVendors.Any(uv => userVendorIds.Contains(uv.VendorId)));
      }
      if (string.IsNullOrEmpty(search)) {
        return query;
      }
      search = search.ToLower();
      query = query.Where(d => d.Title.ToLower().Contains(search) || d.Detail.ToLower().Contains(search));
      return query;
    }

    public static IQueryable<OutGoingData> ApplyFilter(
      this IQueryable<OutGoingData> query,
      int? vendorId,
      IList<int> userVendorIds,
      string invoice,
      DateTime? start,
      DateTime? end
    ) {
      if (vendorId.HasValue) {
        query = query.Where(s => s.VendorId == vendorId.Value);
      } else {
        query = query.Where(s => s.Vendor.UserVendors.Any(uv => userVendorIds.Contains(uv.VendorId)));
      }
      if (!string.IsNullOrEmpty(invoice)) {
        query = query.Where(s => s.Invoice.Contains(invoice));
      }
      if (start.HasValue && end.HasValue) {
        query = query.Where(s => s.CreatedAt >= start.Value && s.CreatedAt < end.Value);
      }
      return query;
    }

    public static IQueryable<OutGoingDatas_New> ApplyFilter_New(
      this IQueryable<OutGoingDatas_New> query,
      int? vendorId,
      IList<int> userVendorIds,
      string invoice,
      DateTime? start,
      DateTime? end,
      string FlgJudge,
      IList<string> VendorAbbr,
      string flgProgram
    )
    {
        if (vendorId.HasValue)
        {
            query = query.Where(s => s.VendorAbbr == VendorAbbr[0].ToString()) ;
        }
        else
        {
            query = query.Where(s => VendorAbbr.Contains(s.VendorAbbr));
        }

        if (!string.IsNullOrEmpty(invoice))
        {
            query = query.Where(s => s.VdDocno.Contains(invoice));
        }

        if (start.HasValue && end.HasValue)
        {
            query = query.Where(s => s.CreatedAt >= start.Value && s.CreatedAt < end.Value);
        }

        if (string.IsNullOrEmpty(FlgJudge))
        {
            if (flgProgram == "admin")
            {
                query = query.Where(s => s.FlgJudge == "N" || s.FlgJudge == null);
            }

        }
        else if (FlgJudge == "Wait")
        {
            query = query.Where(s => s.FlgJudge == null && s.Filename != null);
        }
        else if (FlgJudge == "Y")
        {
            query = query.Where(s => s.FlgJudge == "Y");
        }
        else if (FlgJudge == "N")
        {
            query = query.Where(s => s.FlgJudge == "N");
        }
        else
        {

        }

        return query;
    }
  }
}