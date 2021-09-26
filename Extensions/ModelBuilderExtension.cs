using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QcSupplier.Constants;
using QcSupplier.Entities;

namespace QcSupplier.Extensions {
  public static class ModelBuilderExtensions {
    public static void Seed(this ModelBuilder modelBuilder) {
      modelBuilder.Entity<Entities.Program>().HasData(
        Programs.SupplierEvaluation,
        Programs.Download,
        Programs.TnsForm,
        Programs.SupplierInformation,
        Programs.OutGoingData,
        Programs.SelfControlledIpp
      );

      modelBuilder.Entity<Role>().HasData(
        Roles.Vendor,
        Roles.Admin,
        Roles.SuperAdmin
      );

      var hasher = new PasswordHasher<User>();

      modelBuilder.Entity<User>().HasData(
        Users.Admin
      );

      modelBuilder.Entity<UserRole>().HasData(
        new UserRole {
          UserId = 1,
          RoleId = Roles.SuperAdmin.Id
        }
      );
    }
  }
}