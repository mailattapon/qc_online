using System;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using QcSupplier.Entities;
using QcSupplier.Extensions;
using QcSupplier.Models;
using QcSupplier.Settings;
using QcSupplier.Utilities;
using QcSupplier.ViewModels;

namespace QcSupplier.Mapping {
  public class MappingProfile : Profile {
    #region Field

    private readonly PaginationSetting _paginationSetting;
    private readonly FormatSetting _formatSetting;

    #endregion

    #region Constructor

    public MappingProfile(PaginationSetting paginationSetting, FormatSetting formatSetting) {
      _paginationSetting = paginationSetting;
      _formatSetting = formatSetting;
      MapModelToViewModel();
      MapViewModelToModel();
    }

    #endregion

    #region Method

    private void MapModelToViewModel() {
      CreateMap(typeof(QueryResult<>), typeof(QueryResultModel<>));

      CreateMap<Department, SelectListItem>()
        .ForMember(d => d.Value, o => o.MapFrom(s => s.Id))
        .ForMember(d => d.Text, o => o.MapFrom(s => s.Name));

      CreateMap<Vendor, SelectListItem>()
        .ForMember(d => d.Value, o => o.MapFrom(s => s.Id))
        .ForMember(d => d.Text, o => o.MapFrom(s => s.Name));

      CreateMap<Department, DepartmentModel>();

      CreateMap<Role, Masterdata>()
        .ForMember(d => d.Name, o => o.MapFrom(s => s.NormalizedName));

      CreateMap<Vendor, VendorModel>();

      CreateMap<Vendor, UserSelection>();

      CreateMap<Entities.Program, UserSelection>()
        .AfterMap((s, d) => { d.Enabled = false; });

      CreateMap<Role, UserSelection>()
        .ForMember(d => d.Name, o => o.MapFrom(s => s.NormalizedName))
        .AfterMap((s, d) => { d.Enabled = true; });

      CreateMap<User, UserModel>();

      CreateMap<User, UserList>()
        .ForMember(d => d.Department, o => o.MapFrom(s => s.Department.Name));

      CreateMap<Download, DownloadList>();

      CreateMap<TnsForm, TnsFormList>();

      CreateMap<SupplierEvaluation, SupplierEvaluationList>()
        .ForMember(d => d.Vendor, o => o.MapFrom(s => s.Vendor.Name))
        .AfterMap((s, d) => {
          d.isOverDue = s.ActionDate.HasValue && s.ActionDate.Value > s.DueDate;
        });

      CreateMap<SelfControlledIPP, SelfControlledIPPList>()
        .ForMember(d => d.Vendor, o => o.MapFrom(s => s.Vendor.Name))
        .AfterMap((s, d) => {
          var today = DateTime.Today;
          var current = new DateTime(today.Year, today.Month, 1);
          d.CanDelete = s.SelectedDate >= current;
        });

      CreateMap<SupplierInformation, SupplierInformationList>()
        .ForMember(d => d.Vendor, o => o.MapFrom(s => s.Vendor.Name));

      CreateMap<OutGoingData, OutGoingDataList>()
        .ForMember(d => d.Vendor, o => o.MapFrom(s => s.Vendor.Name));

      CreateMap<OutGoingDataFile, OutGoingDataFileList>()
      .ForMember(d => d.Judgementor, o => o.MapFrom(s => s.Judgementor != null ? s.Judgementor.UserName : null))
      .ForMember(d => d.FileSize, o => o.MapFrom(s => FileUtility.SizeSuffix(s.FileSize)))
      .ForMember(d => d.ViewAt, o => o.MapFrom(s => s.ViewAt.ToString(_formatSetting.ListDateFormat)));

      CreateMap<OutGoingDatas_New, OutGoingDatas_NewList>()
        .ForMember(d => d.Id, o => o.MapFrom(s => s.Id));
        }

    private void MapViewModelToModel() {

      CreateMap<VendorDateParam, VendorDate>()
        .AfterMap((s, d) => {
          d.PageSize = _paginationSetting.PageSize;
        });

      CreateMap<SupplierInformationParam, SupplierInformationModel>()
      .AfterMap((s, d) => {
        d.PageSize = _paginationSetting.PageSize;
      });

      CreateMap<OutGoingDataParam, OutGoingDataModel>()
        .AfterMap((s, d) => {
          d.PageSize = _paginationSetting.PageSize;
        });

      CreateMap<KeywordParam, Keyword>()
        .AfterMap((s, d) => {
          d.PageSize = _paginationSetting.PageSize;
        });

      CreateMap<DepartmentModel, Department>();

      CreateMap<VendorModel, Vendor>();

      CreateMap<UserModel, User>()
        .ForMember(d => d.NormalizedUserName, o => o.MapFrom(s => s.UserName.ToUpper()))
        .ForMember(d => d.NormalizedEmail, o => o.MapFrom(s => s.Email.ToUpper()))
        .ForMember(d => d.EmailConfirmed, o => o.MapFrom(_ => true))
        .AfterMap((s, d) => {
          d.UserRoles.Clear();
          d.UserRoles = s.Roles.Where(r => r.IsSelected).Select(r => new UserRole { RoleId = r.Id }).ToList();

          d.UserPrograms.Clear();
          d.UserPrograms = s.Programs.Where(r => r.IsSelected).Select(r => new UserProgram { ProgramId = r.Id }).ToList();

          d.UserVendors.Clear();
          d.UserVendors = s.Vendors.Where(r => r.IsSelected).Select(r => new UserVendor { VendorId = r.Id }).ToList();
        });
    }
    #endregion

  }
}