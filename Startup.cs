using System;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QcSupplier.Entities;
using QcSupplier.Extensions;
using QcSupplier.Mapping;
using QcSupplier.Persistence;
using QcSupplier.Services;
using QcSupplier.Settings;

namespace QcSupplier {
  public class Startup {

    #region Field

    private readonly IConfiguration _config;

    #endregion

    #region Property

    private string ConnectionString {
      get { return _config.GetConnectionString("DefaultConnection"); }
    }

    #endregion

    #region Constructor

    public Startup(IConfiguration configuration) {
      _config = configuration;
    }

    #endregion

    #region Method

    public void ConfigureServices(IServiceCollection services) {
      services.AddSettings(_config);
      services.AddDependencyInjections();

      // Create Service Provider
      var sp = services.BuildServiceProvider();
      var paginationSetting = sp.GetService<PaginationSetting>();
      var emailSetting = sp.GetService<EmailSetting>();
      var formatSetting = sp.GetService<FormatSetting>();

      //Hosted Service
      services.AddHostedService<QuartzHostedService>();
      if (emailSetting.Enabled) {
        services.AddHostedService<EmailHostedService>();
      }

      // Auto Mapper
      var mappingConfig = new MapperConfiguration(o => {
        o.AddProfile(new MappingProfile(paginationSetting, formatSetting));
      });
      var mapper = mappingConfig.CreateMapper();
      services.AddSingleton(mapper);

      // Entity Framework Core
      services.AddDbContextPool<AppDbContext>(o => o.UseNpgsql(ConnectionString));

      var passwordPolicySetting = sp.GetService<PasswordPolicySetting>();

      // Identity Management
      services.AddIdentity<User, Role>(o => {
        o.Password.RequiredLength = passwordPolicySetting.RequiredLength;
        o.Password.RequireUppercase = passwordPolicySetting.RequireUppercase;
        o.Password.RequireDigit = passwordPolicySetting.RequireDigit;
        o.Password.RequireNonAlphanumeric = passwordPolicySetting.RequireNonAlphanumeric;
      }).AddEntityFrameworkStores<AppDbContext>();

      services.AddPolicies();

      // MVC
      services.AddSession(o => o.IdleTimeout = TimeSpan.FromMinutes(20));
      services.AddMvc(o => {
        var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
        o.Filters.Add(new AuthorizeFilter(policy));
      })
        .AddSessionStateTempDataProvider()
        .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
    }

    public void Configure(IApplicationBuilder app, IHostingEnvironment env, DbInitializer dbInitializer) {
      if (!env.IsDevelopment()) {
        dbInitializer.Initialize();
      }

      // app.UseDeveloperExceptionPage ();
      app.UseStatusCodePagesWithReExecute("/Error/{0}");
      app.UseExceptionHandler("/Error");

      app.UseStaticFiles();
      app.UseAuthentication();
      app.UseSession();
      app.UseMvc();
    }

    #endregion
  }
}