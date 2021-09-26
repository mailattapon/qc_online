using System;
using QcSupplier.Entities;
using QcSupplier.Extensions;

namespace QcSupplier.Constants {
  public static class Roles {
    public const string VENDOR = "Vendor";
    public const string ADMIN = "Admin";
    public const string SUPER_ADMIN = "SuperAdmin";

    public static Role Vendor { get; private set; } = new Role {
      Id = 1,
      Name = VENDOR,
      NormalizedName = "Vendor",
      CreatedAt = DateTime.UtcNow.ToAppDateTime(),
      UpdatedAt = DateTime.UtcNow.ToAppDateTime()
    };

    private static object ToAppDateTime() {
      throw new NotImplementedException();
    }

    public static Role Admin { get; private set; } = new Role {
      Id = 2,
      Name = ADMIN,
      NormalizedName = "Admin",
      CreatedAt = DateTime.UtcNow.ToAppDateTime(),
      UpdatedAt = DateTime.UtcNow.ToAppDateTime()
    };
    public static Role SuperAdmin { get; private set; } = new Role {
      Id = 3,
      Name = SUPER_ADMIN,
      NormalizedName = "Super Admin TNS",
      CreatedAt = DateTime.UtcNow.ToAppDateTime(),
      UpdatedAt = DateTime.UtcNow.ToAppDateTime()
    };
  }
}