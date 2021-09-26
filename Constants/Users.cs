using System;
using Microsoft.AspNetCore.Identity;
using QcSupplier.Entities;
using QcSupplier.Extensions;

namespace QcSupplier.Constants {
  public static class Users {
    public static User Admin { get; set; } = new User {
      Id = 1,
      UserName = "admin",
      NormalizedUserName = "ADMIN",
      Email = "admin@tns.com",
      NormalizedEmail = "ADMIN@TNS.COM",
      EmailConfirmed = true,
      PasswordHash = new PasswordHasher<User>().HashPassword(null, "admin"),
      SecurityStamp = string.Empty,
      CreatedAt = DateTime.UtcNow.ToAppDateTime(),
      UpdatedAt = DateTime.UtcNow.ToAppDateTime()
    };
  }
}