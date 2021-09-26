using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace QcSupplier.Migrations {
  public partial class init : Migration {
    protected override void Up(MigrationBuilder migrationBuilder) {
      migrationBuilder.CreateTable(
          name: "AspNetRoles",
          columns: table => new {
            Id = table.Column<int>(nullable: false)
                  .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
            Name = table.Column<string>(maxLength: 256, nullable: true),
            NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
            ConcurrencyStamp = table.Column<string>(nullable: true),
            CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
            UpdatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
          },
          constraints: table => {
            table.PrimaryKey("PK_AspNetRoles", x => x.Id);
          });

      migrationBuilder.CreateTable(
          name: "Departments",
          columns: table => new {
            Id = table.Column<int>(nullable: false)
                  .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
            Name = table.Column<string>(nullable: true),
            CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
            UpdatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
          },
          constraints: table => {
            table.PrimaryKey("PK_Departments", x => x.Id);
          });

      migrationBuilder.CreateTable(
          name: "Programs",
          columns: table => new {
            Id = table.Column<int>(nullable: false)
                  .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
            Name = table.Column<string>(nullable: true),
            PolicyName = table.Column<string>(nullable: true),
            Enabled = table.Column<bool>(nullable: false),
            CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
            UpdatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
          },
          constraints: table => {
            table.PrimaryKey("PK_Programs", x => x.Id);
          });

      migrationBuilder.CreateTable(
          name: "Vendors",
          columns: table => new {
            Id = table.Column<int>(nullable: false)
                  .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
            Name = table.Column<string>(nullable: true),
            Email = table.Column<string>(nullable: true),
            SendIPP = table.Column<bool>(nullable: false),
            CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
            UpdatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
          },
          constraints: table => {
            table.PrimaryKey("PK_Vendors", x => x.Id);
          });

      migrationBuilder.CreateTable(
          name: "AspNetRoleClaims",
          columns: table => new {
            Id = table.Column<int>(nullable: false)
                  .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
            RoleId = table.Column<int>(nullable: false),
            ClaimType = table.Column<string>(nullable: true),
            ClaimValue = table.Column<string>(nullable: true)
          },
          constraints: table => {
            table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
            table.ForeignKey(
                      name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                      column: x => x.RoleId,
                      principalTable: "AspNetRoles",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateTable(
          name: "AspNetUsers",
          columns: table => new {
            Id = table.Column<int>(nullable: false)
                  .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
            UserName = table.Column<string>(maxLength: 256, nullable: true),
            NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
            Email = table.Column<string>(maxLength: 256, nullable: true),
            NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
            EmailConfirmed = table.Column<bool>(nullable: false),
            PasswordHash = table.Column<string>(nullable: true),
            SecurityStamp = table.Column<string>(nullable: true),
            ConcurrencyStamp = table.Column<string>(nullable: true),
            PhoneNumber = table.Column<string>(nullable: true),
            PhoneNumberConfirmed = table.Column<bool>(nullable: false),
            TwoFactorEnabled = table.Column<bool>(nullable: false),
            LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
            LockoutEnabled = table.Column<bool>(nullable: false),
            AccessFailedCount = table.Column<int>(nullable: false),
            DepartmentId = table.Column<int>(nullable: true),
            CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
            UpdatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
          },
          constraints: table => {
            table.PrimaryKey("PK_AspNetUsers", x => x.Id);
            table.ForeignKey(
                      name: "FK_AspNetUsers_Departments_DepartmentId",
                      column: x => x.DepartmentId,
                      principalTable: "Departments",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Restrict);
          });

      migrationBuilder.CreateTable(
          name: "AspNetUserClaims",
          columns: table => new {
            Id = table.Column<int>(nullable: false)
                  .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
            UserId = table.Column<int>(nullable: false),
            ClaimType = table.Column<string>(nullable: true),
            ClaimValue = table.Column<string>(nullable: true)
          },
          constraints: table => {
            table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
            table.ForeignKey(
                      name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                      column: x => x.UserId,
                      principalTable: "AspNetUsers",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateTable(
          name: "AspNetUserLogins",
          columns: table => new {
            LoginProvider = table.Column<string>(nullable: false),
            ProviderKey = table.Column<string>(nullable: false),
            ProviderDisplayName = table.Column<string>(nullable: true),
            UserId = table.Column<int>(nullable: false)
          },
          constraints: table => {
            table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
            table.ForeignKey(
                      name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                      column: x => x.UserId,
                      principalTable: "AspNetUsers",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateTable(
          name: "AspNetUserRoles",
          columns: table => new {
            UserId = table.Column<int>(nullable: false),
            RoleId = table.Column<int>(nullable: false)
          },
          constraints: table => {
            table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
            table.ForeignKey(
                      name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                      column: x => x.RoleId,
                      principalTable: "AspNetRoles",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
            table.ForeignKey(
                      name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                      column: x => x.UserId,
                      principalTable: "AspNetUsers",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateTable(
          name: "AspNetUserTokens",
          columns: table => new {
            UserId = table.Column<int>(nullable: false),
            LoginProvider = table.Column<string>(nullable: false),
            Name = table.Column<string>(nullable: false),
            Value = table.Column<string>(nullable: true)
          },
          constraints: table => {
            table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
            table.ForeignKey(
                      name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                      column: x => x.UserId,
                      principalTable: "AspNetUsers",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateTable(
          name: "Downloads",
          columns: table => new {
            Id = table.Column<int>(nullable: false)
                  .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
            Title = table.Column<string>(nullable: true),
            Detail = table.Column<string>(nullable: true),
            FileName = table.Column<string>(nullable: true),
            FileSize = table.Column<long>(nullable: false),
            CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
            UpdatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
            CreatorId = table.Column<int>(nullable: false),
            UpdaterId = table.Column<int>(nullable: true)
          },
          constraints: table => {
            table.PrimaryKey("PK_Downloads", x => x.Id);
            table.ForeignKey(
                      name: "FK_Downloads_AspNetUsers_CreatorId",
                      column: x => x.CreatorId,
                      principalTable: "AspNetUsers",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Restrict);
            table.ForeignKey(
                      name: "FK_Downloads_AspNetUsers_UpdaterId",
                      column: x => x.UpdaterId,
                      principalTable: "AspNetUsers",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Restrict);
          });

      migrationBuilder.CreateTable(
          name: "OutGoingDatas",
          columns: table => new {
            Id = table.Column<int>(nullable: false)
                  .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
            VendorId = table.Column<int>(nullable: false),
            Title = table.Column<string>(nullable: true),
            Detail = table.Column<string>(nullable: true),
            Invoice = table.Column<string>(nullable: true),
            Judgemented = table.Column<bool>(nullable: false, defaultValue: false),
            CreatorId = table.Column<int>(nullable: false),
            UpdaterId = table.Column<int>(nullable: true),
            CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
            UpdatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
          },
          constraints: table => {
            table.PrimaryKey("PK_OutGoingDatas", x => x.Id);
            table.ForeignKey(
                      name: "FK_OutGoingDatas_AspNetUsers_CreatorId",
                      column: x => x.CreatorId,
                      principalTable: "AspNetUsers",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Restrict);
            table.ForeignKey(
                      name: "FK_OutGoingDatas_AspNetUsers_UpdaterId",
                      column: x => x.UpdaterId,
                      principalTable: "AspNetUsers",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Restrict);
            table.ForeignKey(
                      name: "FK_OutGoingDatas_Vendors_VendorId",
                      column: x => x.VendorId,
                      principalTable: "Vendors",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Restrict);
          });

      migrationBuilder.CreateTable(
          name: "SelfControlledIPPs",
          columns: table => new {
            Id = table.Column<int>(nullable: false)
                  .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
            VendorId = table.Column<int>(nullable: false),
            Detail = table.Column<string>(nullable: true),
            FileName = table.Column<string>(nullable: true),
            FileSize = table.Column<long>(nullable: false),
            SelectedDate = table.Column<DateTime>(nullable: false),
            CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
            UpdatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
            CreatorId = table.Column<int>(nullable: false),
            UpdaterId = table.Column<int>(nullable: true)
          },
          constraints: table => {
            table.PrimaryKey("PK_SelfControlledIPPs", x => x.Id);
            table.ForeignKey(
                      name: "FK_SelfControlledIPPs_AspNetUsers_CreatorId",
                      column: x => x.CreatorId,
                      principalTable: "AspNetUsers",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Restrict);
            table.ForeignKey(
                      name: "FK_SelfControlledIPPs_AspNetUsers_UpdaterId",
                      column: x => x.UpdaterId,
                      principalTable: "AspNetUsers",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Restrict);
            table.ForeignKey(
                      name: "FK_SelfControlledIPPs_Vendors_VendorId",
                      column: x => x.VendorId,
                      principalTable: "Vendors",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Restrict);
          });

      migrationBuilder.CreateTable(
          name: "SupplierEvaluations",
          columns: table => new {
            Id = table.Column<int>(nullable: false)
                  .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
            VendorId = table.Column<int>(nullable: false),
            Detail = table.Column<string>(nullable: true),
            SelectedDate = table.Column<DateTime>(nullable: false),
            DueDate = table.Column<DateTime>(nullable: false),
            ActionDate = table.Column<DateTime>(nullable: true),
            TnsFileName = table.Column<string>(nullable: true),
            TnsFileSize = table.Column<long>(nullable: false),
            VendorFileName = table.Column<string>(nullable: true),
            VendorFileSize = table.Column<long>(nullable: true),
            CreatorId = table.Column<int>(nullable: false),
            UpdaterId = table.Column<int>(nullable: true),
            CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
            UpdatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
          },
          constraints: table => {
            table.PrimaryKey("PK_SupplierEvaluations", x => x.Id);
            table.ForeignKey(
                      name: "FK_SupplierEvaluations_AspNetUsers_CreatorId",
                      column: x => x.CreatorId,
                      principalTable: "AspNetUsers",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Restrict);
            table.ForeignKey(
                      name: "FK_SupplierEvaluations_AspNetUsers_UpdaterId",
                      column: x => x.UpdaterId,
                      principalTable: "AspNetUsers",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Restrict);
            table.ForeignKey(
                      name: "FK_SupplierEvaluations_Vendors_VendorId",
                      column: x => x.VendorId,
                      principalTable: "Vendors",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Restrict);
          });

      migrationBuilder.CreateTable(
          name: "SupplierInformations",
          columns: table => new {
            Id = table.Column<int>(nullable: false)
                  .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
            VendorId = table.Column<int>(nullable: false),
            Title = table.Column<string>(nullable: true),
            Detail = table.Column<string>(nullable: true),
            FileName = table.Column<string>(nullable: true),
            FileSize = table.Column<long>(nullable: false),
            CreatorId = table.Column<int>(nullable: false),
            UpdaterId = table.Column<int>(nullable: true),
            CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
            UpdatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
          },
          constraints: table => {
            table.PrimaryKey("PK_SupplierInformations", x => x.Id);
            table.ForeignKey(
                      name: "FK_SupplierInformations_AspNetUsers_CreatorId",
                      column: x => x.CreatorId,
                      principalTable: "AspNetUsers",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Restrict);
            table.ForeignKey(
                      name: "FK_SupplierInformations_AspNetUsers_UpdaterId",
                      column: x => x.UpdaterId,
                      principalTable: "AspNetUsers",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Restrict);
            table.ForeignKey(
                      name: "FK_SupplierInformations_Vendors_VendorId",
                      column: x => x.VendorId,
                      principalTable: "Vendors",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Restrict);
          });

      migrationBuilder.CreateTable(
          name: "TnsForms",
          columns: table => new {
            Id = table.Column<int>(nullable: false)
                  .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
            Title = table.Column<string>(nullable: true),
            Detail = table.Column<string>(nullable: true),
            FileName = table.Column<string>(nullable: true),
            FileSize = table.Column<long>(nullable: false),
            CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
            UpdatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
            CreatorId = table.Column<int>(nullable: false),
            UpdaterId = table.Column<int>(nullable: true)
          },
          constraints: table => {
            table.PrimaryKey("PK_TnsForms", x => x.Id);
            table.ForeignKey(
                      name: "FK_TnsForms_AspNetUsers_CreatorId",
                      column: x => x.CreatorId,
                      principalTable: "AspNetUsers",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Restrict);
            table.ForeignKey(
                      name: "FK_TnsForms_AspNetUsers_UpdaterId",
                      column: x => x.UpdaterId,
                      principalTable: "AspNetUsers",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Restrict);
          });

      migrationBuilder.CreateTable(
          name: "UserPrograms",
          columns: table => new {
            UserId = table.Column<int>(nullable: false),
            ProgramId = table.Column<int>(nullable: false)
          },
          constraints: table => {
            table.PrimaryKey("PK_UserPrograms", x => new { x.UserId, x.ProgramId });
            table.ForeignKey(
                      name: "FK_UserPrograms_Programs_ProgramId",
                      column: x => x.ProgramId,
                      principalTable: "Programs",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
            table.ForeignKey(
                      name: "FK_UserPrograms_AspNetUsers_UserId",
                      column: x => x.UserId,
                      principalTable: "AspNetUsers",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateTable(
          name: "UserVendors",
          columns: table => new {
            UserId = table.Column<int>(nullable: false),
            VendorId = table.Column<int>(nullable: false)
          },
          constraints: table => {
            table.PrimaryKey("PK_UserVendors", x => new { x.UserId, x.VendorId });
            table.ForeignKey(
                      name: "FK_UserVendors_AspNetUsers_UserId",
                      column: x => x.UserId,
                      principalTable: "AspNetUsers",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
            table.ForeignKey(
                      name: "FK_UserVendors_Vendors_VendorId",
                      column: x => x.VendorId,
                      principalTable: "Vendors",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateTable(
          name: "OutGoingDataFiles",
          columns: table => new {
            Id = table.Column<int>(nullable: false)
                  .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
            OutGoingDataId = table.Column<int>(nullable: false),
            FileName = table.Column<string>(nullable: true),
            FileSize = table.Column<long>(nullable: false),
            Passed = table.Column<bool>(nullable: true),
            CreatorId = table.Column<int>(nullable: false),
            UpdaterId = table.Column<int>(nullable: true),
            ViewerId = table.Column<int>(nullable: true),
            JudgementId = table.Column<int>(nullable: true),
            CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
            UpdatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
            ViewAt = table.Column<DateTime>(nullable: true),
            JudgementAt = table.Column<DateTime>(nullable: true)
          },
          constraints: table => {
            table.PrimaryKey("PK_OutGoingDataFiles", x => x.Id);
            table.ForeignKey(
                      name: "FK_OutGoingDataFiles_AspNetUsers_CreatorId",
                      column: x => x.CreatorId,
                      principalTable: "AspNetUsers",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Restrict);
            table.ForeignKey(
                      name: "FK_OutGoingDataFiles_AspNetUsers_JudgementId",
                      column: x => x.JudgementId,
                      principalTable: "AspNetUsers",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Restrict);
            table.ForeignKey(
                      name: "FK_OutGoingDataFiles_OutGoingDatas_OutGoingDataId",
                      column: x => x.OutGoingDataId,
                      principalTable: "OutGoingDatas",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
            table.ForeignKey(
                      name: "FK_OutGoingDataFiles_AspNetUsers_UpdaterId",
                      column: x => x.UpdaterId,
                      principalTable: "AspNetUsers",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Restrict);
            table.ForeignKey(
                      name: "FK_OutGoingDataFiles_AspNetUsers_ViewerId",
                      column: x => x.ViewerId,
                      principalTable: "AspNetUsers",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Restrict);
          });

      migrationBuilder.InsertData(
          table: "AspNetRoles",
          columns: new[] { "Id", "ConcurrencyStamp", "CreatedAt", "Name", "NormalizedName", "UpdatedAt" },
          values: new object[,]
          {
                    { 1, "640a9c12-765d-45b2-aa2b-5cdfb56fe6f8", new DateTime(2020, 1, 27, 1, 1, 15, 994, DateTimeKind.Unspecified).AddTicks(8120), "Vendor", "Vendor", new DateTime(2020, 1, 27, 1, 1, 15, 994, DateTimeKind.Unspecified).AddTicks(8610) },
                    { 2, "23d2e497-a7eb-4cf3-a45c-093f654c8a85", new DateTime(2020, 1, 27, 1, 1, 15, 994, DateTimeKind.Unspecified).AddTicks(9060), "Admin", "Admin", new DateTime(2020, 1, 27, 1, 1, 15, 994, DateTimeKind.Unspecified).AddTicks(9080) },
                    { 3, "f6f99828-1863-4aa6-b658-cf507c8bd3e4", new DateTime(2020, 1, 27, 1, 1, 15, 994, DateTimeKind.Unspecified).AddTicks(9100), "SuperAdmin", "Super Admin TNS", new DateTime(2020, 1, 27, 1, 1, 15, 994, DateTimeKind.Unspecified).AddTicks(9100) }
          });

      migrationBuilder.InsertData(
          table: "AspNetUsers",
          columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreatedAt", "DepartmentId", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UpdatedAt", "UserName" },
          values: new object[] { 1, 0, "5541554a-a244-450d-a315-1b11699100ff", new DateTime(2020, 1, 27, 1, 1, 16, 15, DateTimeKind.Unspecified).AddTicks(5530), null, "admin@tns.com", true, false, null, "ADMIN@TNS.COM", "ADMIN", "AQAAAAEAACcQAAAAEOEkf8ph/OEJYEpuQAvN3/ykEPwKCGeqr/Ibsynez8RqA6MrdnBL8iaXSlAzaPxOzA==", null, false, "", false, new DateTime(2020, 1, 27, 1, 1, 16, 15, DateTimeKind.Unspecified).AddTicks(6190), "admin" });

      migrationBuilder.InsertData(
          table: "Programs",
          columns: new[] { "Id", "CreatedAt", "Enabled", "Name", "PolicyName", "UpdatedAt" },
          values: new object[,]
          {
                    { 1, new DateTime(2020, 1, 27, 1, 1, 15, 814, DateTimeKind.Unspecified).AddTicks(2650), true, "Supplier Evaluation", "SupplierEvaluation", new DateTime(2020, 1, 27, 1, 1, 15, 992, DateTimeKind.Unspecified).AddTicks(4130) },
                    { 2, new DateTime(2020, 1, 27, 1, 1, 15, 992, DateTimeKind.Unspecified).AddTicks(4680), true, "Download", "Download", new DateTime(2020, 1, 27, 1, 1, 15, 992, DateTimeKind.Unspecified).AddTicks(4700) },
                    { 3, new DateTime(2020, 1, 27, 1, 1, 15, 992, DateTimeKind.Unspecified).AddTicks(4710), true, "TNS Form", "TNSForm", new DateTime(2020, 1, 27, 1, 1, 15, 992, DateTimeKind.Unspecified).AddTicks(4720) },
                    { 4, new DateTime(2020, 1, 27, 1, 1, 15, 992, DateTimeKind.Unspecified).AddTicks(4730), true, "Supplier Information", "SupplierInformation", new DateTime(2020, 1, 27, 1, 1, 15, 992, DateTimeKind.Unspecified).AddTicks(4730) },
                    { 5, new DateTime(2020, 1, 27, 1, 1, 15, 992, DateTimeKind.Unspecified).AddTicks(4740), true, "Out Going Data", "OutGoingData", new DateTime(2020, 1, 27, 1, 1, 15, 992, DateTimeKind.Unspecified).AddTicks(4740) },
                    { 6, new DateTime(2020, 1, 27, 1, 1, 15, 992, DateTimeKind.Unspecified).AddTicks(4750), true, "Self Controlled IPP", "SelfControlledIPP", new DateTime(2020, 1, 27, 1, 1, 15, 992, DateTimeKind.Unspecified).AddTicks(4760) }
          });

      migrationBuilder.InsertData(
          table: "AspNetUserRoles",
          columns: new[] { "UserId", "RoleId" },
          values: new object[] { 1, 3 });

      migrationBuilder.CreateIndex(
          name: "IX_AspNetRoleClaims_RoleId",
          table: "AspNetRoleClaims",
          column: "RoleId");

      migrationBuilder.CreateIndex(
          name: "RoleNameIndex",
          table: "AspNetRoles",
          column: "NormalizedName",
          unique: true);

      migrationBuilder.CreateIndex(
          name: "IX_AspNetUserClaims_UserId",
          table: "AspNetUserClaims",
          column: "UserId");

      migrationBuilder.CreateIndex(
          name: "IX_AspNetUserLogins_UserId",
          table: "AspNetUserLogins",
          column: "UserId");

      migrationBuilder.CreateIndex(
          name: "IX_AspNetUserRoles_RoleId",
          table: "AspNetUserRoles",
          column: "RoleId");

      migrationBuilder.CreateIndex(
          name: "IX_AspNetUsers_DepartmentId",
          table: "AspNetUsers",
          column: "DepartmentId");

      migrationBuilder.CreateIndex(
          name: "IX_AspNetUsers_Email",
          table: "AspNetUsers",
          column: "Email",
          unique: true);

      migrationBuilder.CreateIndex(
          name: "EmailIndex",
          table: "AspNetUsers",
          column: "NormalizedEmail");

      migrationBuilder.CreateIndex(
          name: "UserNameIndex",
          table: "AspNetUsers",
          column: "NormalizedUserName",
          unique: true);

      migrationBuilder.CreateIndex(
          name: "IX_Departments_Name",
          table: "Departments",
          column: "Name",
          unique: true);

      migrationBuilder.CreateIndex(
          name: "IX_Downloads_CreatorId",
          table: "Downloads",
          column: "CreatorId");

      migrationBuilder.CreateIndex(
          name: "IX_Downloads_Detail",
          table: "Downloads",
          column: "Detail");

      migrationBuilder.CreateIndex(
          name: "IX_Downloads_Title",
          table: "Downloads",
          column: "Title");

      migrationBuilder.CreateIndex(
          name: "IX_Downloads_UpdaterId",
          table: "Downloads",
          column: "UpdaterId");

      migrationBuilder.CreateIndex(
          name: "IX_OutGoingDataFiles_CreatorId",
          table: "OutGoingDataFiles",
          column: "CreatorId");

      migrationBuilder.CreateIndex(
          name: "IX_OutGoingDataFiles_JudgementId",
          table: "OutGoingDataFiles",
          column: "JudgementId");

      migrationBuilder.CreateIndex(
          name: "IX_OutGoingDataFiles_OutGoingDataId",
          table: "OutGoingDataFiles",
          column: "OutGoingDataId");

      migrationBuilder.CreateIndex(
          name: "IX_OutGoingDataFiles_UpdaterId",
          table: "OutGoingDataFiles",
          column: "UpdaterId");

      migrationBuilder.CreateIndex(
          name: "IX_OutGoingDataFiles_ViewerId",
          table: "OutGoingDataFiles",
          column: "ViewerId");

      migrationBuilder.CreateIndex(
          name: "IX_OutGoingDatas_CreatedAt",
          table: "OutGoingDatas",
          column: "CreatedAt");

      migrationBuilder.CreateIndex(
          name: "IX_OutGoingDatas_CreatorId",
          table: "OutGoingDatas",
          column: "CreatorId");

      migrationBuilder.CreateIndex(
          name: "IX_OutGoingDatas_Invoice",
          table: "OutGoingDatas",
          column: "Invoice");

      migrationBuilder.CreateIndex(
          name: "IX_OutGoingDatas_UpdaterId",
          table: "OutGoingDatas",
          column: "UpdaterId");

      migrationBuilder.CreateIndex(
          name: "IX_OutGoingDatas_VendorId",
          table: "OutGoingDatas",
          column: "VendorId");

      migrationBuilder.CreateIndex(
          name: "IX_SelfControlledIPPs_CreatorId",
          table: "SelfControlledIPPs",
          column: "CreatorId");

      migrationBuilder.CreateIndex(
          name: "IX_SelfControlledIPPs_SelectedDate",
          table: "SelfControlledIPPs",
          column: "SelectedDate");

      migrationBuilder.CreateIndex(
          name: "IX_SelfControlledIPPs_UpdaterId",
          table: "SelfControlledIPPs",
          column: "UpdaterId");

      migrationBuilder.CreateIndex(
          name: "IX_SelfControlledIPPs_VendorId",
          table: "SelfControlledIPPs",
          column: "VendorId");

      migrationBuilder.CreateIndex(
          name: "IX_SupplierEvaluations_CreatorId",
          table: "SupplierEvaluations",
          column: "CreatorId");

      migrationBuilder.CreateIndex(
          name: "IX_SupplierEvaluations_SelectedDate",
          table: "SupplierEvaluations",
          column: "SelectedDate");

      migrationBuilder.CreateIndex(
          name: "IX_SupplierEvaluations_UpdaterId",
          table: "SupplierEvaluations",
          column: "UpdaterId");

      migrationBuilder.CreateIndex(
          name: "IX_SupplierEvaluations_VendorId",
          table: "SupplierEvaluations",
          column: "VendorId");

      migrationBuilder.CreateIndex(
          name: "IX_SupplierInformations_CreatorId",
          table: "SupplierInformations",
          column: "CreatorId");

      migrationBuilder.CreateIndex(
          name: "IX_SupplierInformations_Detail",
          table: "SupplierInformations",
          column: "Detail");

      migrationBuilder.CreateIndex(
          name: "IX_SupplierInformations_Title",
          table: "SupplierInformations",
          column: "Title");

      migrationBuilder.CreateIndex(
          name: "IX_SupplierInformations_UpdaterId",
          table: "SupplierInformations",
          column: "UpdaterId");

      migrationBuilder.CreateIndex(
          name: "IX_SupplierInformations_VendorId",
          table: "SupplierInformations",
          column: "VendorId");

      migrationBuilder.CreateIndex(
          name: "IX_TnsForms_CreatorId",
          table: "TnsForms",
          column: "CreatorId");

      migrationBuilder.CreateIndex(
          name: "IX_TnsForms_Detail",
          table: "TnsForms",
          column: "Detail");

      migrationBuilder.CreateIndex(
          name: "IX_TnsForms_Title",
          table: "TnsForms",
          column: "Title");

      migrationBuilder.CreateIndex(
          name: "IX_TnsForms_UpdaterId",
          table: "TnsForms",
          column: "UpdaterId");

      migrationBuilder.CreateIndex(
          name: "IX_UserPrograms_ProgramId",
          table: "UserPrograms",
          column: "ProgramId");

      migrationBuilder.CreateIndex(
          name: "IX_UserVendors_VendorId",
          table: "UserVendors",
          column: "VendorId");

      migrationBuilder.CreateIndex(
          name: "IX_Vendors_Email",
          table: "Vendors",
          column: "Email",
          unique: true);

      migrationBuilder.CreateIndex(
          name: "IX_Vendors_Name",
          table: "Vendors",
          column: "Name",
          unique: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder) {
      migrationBuilder.DropTable(
          name: "AspNetRoleClaims");

      migrationBuilder.DropTable(
          name: "AspNetUserClaims");

      migrationBuilder.DropTable(
          name: "AspNetUserLogins");

      migrationBuilder.DropTable(
          name: "AspNetUserRoles");

      migrationBuilder.DropTable(
          name: "AspNetUserTokens");

      migrationBuilder.DropTable(
          name: "Downloads");

      migrationBuilder.DropTable(
          name: "OutGoingDataFiles");

      migrationBuilder.DropTable(
          name: "SelfControlledIPPs");

      migrationBuilder.DropTable(
          name: "SupplierEvaluations");

      migrationBuilder.DropTable(
          name: "SupplierInformations");

      migrationBuilder.DropTable(
          name: "TnsForms");

      migrationBuilder.DropTable(
          name: "UserPrograms");

      migrationBuilder.DropTable(
          name: "UserVendors");

      migrationBuilder.DropTable(
          name: "AspNetRoles");

      migrationBuilder.DropTable(
          name: "OutGoingDatas");

      migrationBuilder.DropTable(
          name: "Programs");

      migrationBuilder.DropTable(
          name: "AspNetUsers");

      migrationBuilder.DropTable(
          name: "Vendors");

      migrationBuilder.DropTable(
          name: "Departments");
    }
  }
}
