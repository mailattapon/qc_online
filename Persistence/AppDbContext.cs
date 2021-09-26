using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QcSupplier.Entities;
using QcSupplier.Extensions;

namespace QcSupplier.Persistence {
  public class AppDbContext : IdentityDbContext<User, Role, int, IdentityUserClaim<int>, UserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>> {
    #region Property

    public DbSet<Department> Departments { get; set; }
    public DbSet<Vendor> Vendors { get; set; }
    public DbSet<Entities.Program> Programs { get; set; }
    public DbSet<UserVendor> UserVendors { get; set; }
    public DbSet<UserProgram> UserPrograms { get; set; }
    public DbSet<SupplierEvaluation> SupplierEvaluations { get; set; }
    public DbSet<Download> Downloads { get; set; }
    public DbSet<TnsForm> TnsForms { get; set; }
    public DbSet<SelfControlledIPP> SelfControlledIPPs { get; set; }
    public DbSet<SupplierInformation> SupplierInformations { get; set; }
    public DbSet<OutGoingData> OutGoingDatas { get; set; }
    public DbSet<OutGoingDataFile> OutGoingDataFiles { get; set; }

    //14/09/2021 lattapon 
    public DbSet<OutGoingDatas_New> OutGoingDatas_New { get; set; }
    #endregion

    #region Constructor

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    #endregion

    #region Method

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      base.OnModelCreating(modelBuilder);

      modelBuilder.Entity<User>(e => {
        e.HasKey(u => u.Id);
        e.HasIndex(u => u.Email).IsUnique();
        e.Property(u => u.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        e.Property(u => u.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        e.HasMany(u => u.UserRoles).WithOne(ur => ur.User).HasForeignKey(ur => ur.UserId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        e.HasMany(u => u.UserVendors).WithOne(uv => uv.User).HasForeignKey(uv => uv.UserId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        e.HasMany(u => u.UserPrograms).WithOne(up => up.User).HasForeignKey(up => up.UserId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        e.HasMany(u => u.SupplierEvaluationCreators).WithOne(s => s.Creator).HasForeignKey(s => s.CreatorId).IsRequired().OnDelete(DeleteBehavior.Restrict);
        e.HasMany(u => u.SupplierEvaluationUpdaters).WithOne(s => s.Updater).HasForeignKey(s => s.UpdaterId).IsRequired(false).OnDelete(DeleteBehavior.Restrict); ;
        e.HasMany(u => u.DownloadCreators).WithOne(s => s.Creator).HasForeignKey(s => s.CreatorId).IsRequired().OnDelete(DeleteBehavior.Restrict); ;
        e.HasMany(u => u.DownloadUpdaters).WithOne(s => s.Updater).HasForeignKey(s => s.UpdaterId).IsRequired(false).OnDelete(DeleteBehavior.Restrict); ;
        e.HasMany(u => u.TnsFormCreators).WithOne(s => s.Creator).HasForeignKey(s => s.CreatorId).IsRequired().OnDelete(DeleteBehavior.Restrict); ;
        e.HasMany(u => u.TnsFormUpdaters).WithOne(s => s.Updater).HasForeignKey(s => s.UpdaterId).IsRequired(false).OnDelete(DeleteBehavior.Restrict); ;
        e.HasMany(u => u.SelfControlledIPPCreators).WithOne(s => s.Creator).HasForeignKey(s => s.CreatorId).IsRequired().OnDelete(DeleteBehavior.Restrict); ;
        e.HasMany(u => u.SelfControlledIPPUpdaters).WithOne(s => s.Updater).HasForeignKey(s => s.UpdaterId).IsRequired(false).OnDelete(DeleteBehavior.Restrict); ;
        e.HasMany(u => u.SupplierInformationCreators).WithOne(s => s.Creator).HasForeignKey(s => s.CreatorId).IsRequired().OnDelete(DeleteBehavior.Restrict); ;
        e.HasMany(u => u.SupplierInformationUpdaters).WithOne(s => s.Updater).HasForeignKey(s => s.UpdaterId).IsRequired(false).OnDelete(DeleteBehavior.Restrict); ;
        e.HasMany(u => u.OutGoingDataCreators).WithOne(s => s.Creator).HasForeignKey(s => s.CreatorId).IsRequired().OnDelete(DeleteBehavior.Restrict); ;
        e.HasMany(u => u.OutGoingDataUpdaters).WithOne(s => s.Updater).HasForeignKey(s => s.UpdaterId).IsRequired(false).OnDelete(DeleteBehavior.Restrict);
        e.HasMany(u => u.OutGoingDataFileCreators).WithOne(s => s.Creator).HasForeignKey(s => s.CreatorId).IsRequired().OnDelete(DeleteBehavior.Restrict);
        e.HasMany(u => u.OutGoingDataFileUpdaters).WithOne(s => s.Updater).HasForeignKey(s => s.UpdaterId).IsRequired(false).OnDelete(DeleteBehavior.Restrict);
        e.HasMany(u => u.OutGoingDataFileViewers).WithOne(s => s.Viewer).HasForeignKey(s => s.ViewerId).IsRequired(false).OnDelete(DeleteBehavior.Restrict);
        e.HasMany(u => u.OutGoingDataFileJudgementors).WithOne(s => s.Judgementor).HasForeignKey(s => s.JudgementId).IsRequired(false).OnDelete(DeleteBehavior.Restrict);

        //2021/09/14 lattapon
        e.HasMany(u => u.OutGoingDatasNew).WithOne(s => s.CreatorId).HasForeignKey(s => s.ReviewId).IsRequired(false).OnDelete(DeleteBehavior.Restrict);
      });

      modelBuilder.Entity<Vendor>(e => {
        e.HasKey(v => v.Id);
        e.HasIndex(v => v.Name).IsUnique();
        e.HasIndex(v => v.Email).IsUnique();
        e.Property(v => v.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        e.Property(v => v.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        e.HasMany(v => v.UserVendors).WithOne(uv => uv.Vendor).HasForeignKey(uv => uv.VendorId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        e.HasMany(v => v.SupplierEvaluations).WithOne(s => s.Vendor).HasForeignKey(s => s.VendorId).IsRequired().OnDelete(DeleteBehavior.Restrict);
        e.HasMany(v => v.SelfControlledIPPs).WithOne(s => s.Vendor).HasForeignKey(s => s.VendorId).IsRequired().OnDelete(DeleteBehavior.Restrict);
        e.HasMany(v => v.SupplierInformations).WithOne(s => s.Vendor).HasForeignKey(s => s.VendorId).IsRequired().OnDelete(DeleteBehavior.Restrict);
        e.HasMany(v => v.OutGoingDatas).WithOne(s => s.Vendor).HasForeignKey(s => s.VendorId).IsRequired().OnDelete(DeleteBehavior.Restrict);
      });

      modelBuilder.Entity<Department>(e => {
        e.HasKey(d => d.Id);
        e.HasIndex(d => d.Name).IsUnique();
        e.Property(d => d.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        e.Property(d => d.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        e.HasMany(d => d.Users).WithOne(u => u.Department).HasForeignKey(u => u.DepartmentId).IsRequired(false).OnDelete(DeleteBehavior.Restrict);
      });

      modelBuilder.Entity<Role>(e => {
        e.HasKey(r => r.Id);
        e.Property(r => r.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        e.Property(r => r.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        e.HasMany(r => r.UserRoles).WithOne(ur => ur.Role).HasForeignKey(ur => ur.RoleId).IsRequired().OnDelete(DeleteBehavior.Cascade);
      });

      modelBuilder.Entity<Entities.Program>(e => {
        e.HasKey(p => p.Id);
        e.Property(p => p.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        e.Property(p => p.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        e.HasMany(p => p.UserPrograms).WithOne(up => up.Program).HasForeignKey(up => up.ProgramId).IsRequired().OnDelete(DeleteBehavior.Cascade); ;
      });

      modelBuilder.Entity<SupplierEvaluation>(e => {
        e.HasKey(s => s.Id);
        e.HasIndex(s => s.SelectedDate);
        e.Property(s => s.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        e.Property(s => s.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
      });

      modelBuilder.Entity<Download>(e => {
        e.HasKey(d => d.Id);
        e.HasIndex(d => d.Title);
        e.HasIndex(d => d.Detail);
        e.Property(d => d.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        e.Property(d => d.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
      });

      modelBuilder.Entity<TnsForm>(e => {
        e.HasKey(t => t.Id);
        e.HasIndex(t => t.Title);
        e.HasIndex(t => t.Detail);
        e.Property(t => t.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        e.Property(t => t.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
      });

      modelBuilder.Entity<SelfControlledIPP>(e => {
        e.HasKey(s => s.Id);
        e.HasIndex(s => s.SelectedDate);
        e.Property(s => s.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        e.Property(s => s.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
      });

      modelBuilder.Entity<SupplierInformation>(e => {
        e.HasKey(s => s.Id);
        e.HasIndex(s => s.Title);
        e.HasIndex(s => s.Detail);
        e.Property(s => s.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        e.Property(s => s.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
      });

      modelBuilder.Entity<OutGoingData>(e => {
        e.HasKey(o => o.Id);
        e.HasIndex(o => o.Invoice);
        e.HasIndex(o => o.CreatedAt);
        e.Property(o => o.Judgemented).HasDefaultValue(false);
        e.Property(o => o.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        e.Property(o => o.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        e.HasMany(o => o.Files).WithOne(f => f.OutGoingData).HasForeignKey(f => f.OutGoingDataId).IsRequired().OnDelete(DeleteBehavior.Cascade);
      });

      //lattapon
      modelBuilder.Entity<OutGoingDatas_New>(e => {
        e.HasKey(o => o.Id);
        e.Property(o => o.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        e.Property(o => o.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
      });

      modelBuilder.Entity<OutGoingDataFile>(e => {
        e.HasKey(o => o.Id);
        e.Property(o => o.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        e.Property(o => o.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
      });

      modelBuilder.Entity<UserRole>(e => {
        e.HasKey(ur => new { ur.UserId, ur.RoleId });
      });

      modelBuilder.Entity<UserProgram>(e => {
        e.HasKey(up => new { up.UserId, up.ProgramId });
      });

      modelBuilder.Entity<UserVendor>(e => {
        e.HasKey(uv => new { uv.UserId, uv.VendorId });
      });

      //lattapon 14/09/2021 
      modelBuilder.Entity<OutGoingDatas_New>(e => {
        e.HasKey(o => o.Id);
        e.Property(o => o.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        e.Property(o => o.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
      });

      modelBuilder.Seed();
    }

    #endregion
  }
}