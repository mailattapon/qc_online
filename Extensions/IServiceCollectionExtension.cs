using Microsoft.Extensions.DependencyInjection;
using QcSupplier.Constants;
using QcSupplier.Settings;
using Microsoft.Extensions.Configuration;
using Quartz.Spi;
using QcSupplier.Infrastructures;
using Quartz;
using Quartz.Impl;
using QcSupplier.Models;
using QcSupplier.Jobs;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;
using QcSupplier.Persistence;
using QcSupplier.Repositories;
using QcSupplier.Services;
using QcSupplier.Entities;

namespace QcSupplier.Extensions {
  public static class IServiceCollectionExtension {
    public static IServiceCollection AddPolicies(this IServiceCollection services) {
      return services.AddAuthorization(e => {
        e.AddPolicy(Policies.SUPPLIER_EVALUATION, o => o.RequireClaim(Programs.SUPPLIER_EVALUATION));
        e.AddPolicy(Policies.ADMIN_SUPPLIER_EVALUATION, o => o.RequireClaim(Programs.SUPPLIER_EVALUATION).RequireRole(Roles.ADMIN));
        e.AddPolicy(Policies.VENDOR_SUPPLIER_EVALUATION, o => o.RequireClaim(Programs.SUPPLIER_EVALUATION).RequireRole(Roles.VENDOR));
        e.AddPolicy(Policies.DOWNLOAD, o => o.RequireClaim(Programs.DOWNLOAD));
        e.AddPolicy(Policies.ADMIN_DOWNLOAD, o => o.RequireClaim(Programs.DOWNLOAD).RequireRole(Roles.ADMIN));
        e.AddPolicy(Policies.TNS_FORM, o => o.RequireClaim(Programs.TNS_FORM));
        e.AddPolicy(Policies.ADMIN_TNS_FORM, o => o.RequireClaim(Programs.TNS_FORM).RequireRole(Roles.ADMIN));
        e.AddPolicy(Policies.SELF_CONTROLLED_IPP, o => o.RequireClaim(Programs.SELF_CONTROLLED_IPP));
        e.AddPolicy(Policies.VENDOR_SELF_CONTROLLED_IPP, o => o.RequireClaim(Programs.SELF_CONTROLLED_IPP).RequireRole(Roles.VENDOR));
        e.AddPolicy(Policies.SUPPLIER_INFORMATION, o => o.RequireClaim(Programs.SUPPLIER_INFORMATION));
        e.AddPolicy(Policies.VENDOR_SUPPLIER_INFORMATION, o => o.RequireClaim(Programs.SUPPLIER_INFORMATION).RequireRole(Roles.VENDOR));
        e.AddPolicy(Policies.OUT_GOING_DATA, o => o.RequireClaim(Programs.OUT_GOING_DATA));
        e.AddPolicy(Policies.ADMIN_OUT_GOING_DATA, o => o.RequireClaim(Programs.OUT_GOING_DATA).RequireRole(Roles.ADMIN));
        e.AddPolicy(Policies.VENDOR_OUT_GOING_DATA, o => o.RequireClaim(Programs.OUT_GOING_DATA).RequireRole(Roles.VENDOR));
      });
    }

    public static IServiceCollection AddSettings(this IServiceCollection services, IConfiguration config) {
      return services
        .Configure<PaginationSetting>(config.GetSection("Pagination"))
        .Configure<PasswordPolicySetting>(config.GetSection("PasswordPolicy"))
        .Configure<FormatSetting>(config.GetSection("Format"))
        .Configure<AppInfoSetting>(config.GetSection("AppInfo"))
        .Configure<FileUploadSetting>(config.GetSection("FileUpload"))
        .Configure<EmailSetting>(config.GetSection("Email"));
    }

    public static IServiceCollection AddDependencyInjections(this IServiceCollection services) {
      return services
        .AddSingleton<IJobFactory, JobFactory>()
        .AddSingleton<ISchedulerFactory, StdSchedulerFactory>()
        .AddSingleton<JobRunner>()
        .AddSingleton(new JobSchedule(jobType: typeof(EmailReminderJob), cronExpression: "0 0 0 24 * ?"))
        .AddSingleton<PaginationSetting>(o => o.GetRequiredService<IOptions<PaginationSetting>>().Value)
        .AddSingleton<PasswordPolicySetting>(o => o.GetRequiredService<IOptions<PasswordPolicySetting>>().Value)
        .AddSingleton<FormatSetting>(o => o.GetRequiredService<IOptions<FormatSetting>>().Value)
        .AddSingleton<AppInfoSetting>(o => o.GetRequiredService<IOptions<AppInfoSetting>>().Value)
        .AddSingleton<FileUploadSetting>(o => o.GetRequiredService<IOptions<FileUploadSetting>>().Value)
        .AddSingleton<EmailSetting>(o => o.GetRequiredService<IOptions<EmailSetting>>().Value)
        .AddSingleton<EmailQueue>()
        .AddScoped<IUserClaimsPrincipalFactory<User>, AppUserClaimsPrincipalFactory>()
        .AddScoped<RazorViewToStringRenderer>()
        .AddScoped<UnitOfWork>()
        .AddScoped<UserRepository>()
        .AddScoped<DepartmentRepository>()
        .AddScoped<VendorRepository>()
        .AddScoped<ProgramRepository>()
        .AddScoped<SupplierEvaluationRepository>()
        .AddScoped<DownloadRepository>()
        .AddScoped<TnsFormRepository>()
        .AddScoped<SelfControlledIPPRepository>()
        .AddScoped<SupplierInformationRepository>()
        .AddScoped<OutGoingDataRepository>()
        .AddScoped<OutGoingDataFileRepository>()
        .AddScoped<EmailService>()
        .AddScoped<EmailReminderJob>()
        .AddScoped<DbInitializer>();
    }
  }
}