using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Company.Project.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppToken",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(maxLength: 64, nullable: false),
                    Type = table.Column<string>(maxLength: 64, nullable: false),
                    UserId = table.Column<string>(maxLength: 128, nullable: false),
                    Scheme = table.Column<string>(maxLength: 64, nullable: false),
                    Claims = table.Column<string>(maxLength: 1024, nullable: false),
                    CreateTime = table.Column<DateTimeOffset>(nullable: false),
                    Expiration = table.Column<DateTimeOffset>(nullable: false),
                    TenantName = table.Column<string>(maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppToken", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PageColumnItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    Creator = table.Column<string>(maxLength: 256, nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Field = table.Column<string>(maxLength: 128, nullable: false),
                    Order = table.Column<int>(nullable: true),
                    Width = table.Column<int>(nullable: true),
                    Hidden = table.Column<byte>(nullable: false),
                    TenantName = table.Column<string>(maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PageColumnItem", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PageFilterItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    Creator = table.Column<string>(maxLength: 256, nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Field = table.Column<string>(maxLength: 128, nullable: false),
                    Order = table.Column<int>(nullable: true),
                    Hidden = table.Column<byte>(nullable: false),
                    Width = table.Column<int>(nullable: false),
                    XsWidth = table.Column<int>(nullable: true),
                    SmWidth = table.Column<int>(nullable: true),
                    MdWidth = table.Column<int>(nullable: true),
                    LgWidth = table.Column<int>(nullable: true),
                    XlWidth = table.Column<int>(nullable: true),
                    XxlWidth = table.Column<int>(nullable: true),
                    TenantName = table.Column<string>(maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PageFilterItem", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 32, nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Type = table.Column<string>(maxLength: 16, nullable: false),
                    Provider = table.Column<string>(maxLength: 64, nullable: false),
                    TenantName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    DisplayName = table.Column<string>(maxLength: 512, nullable: true),
                    Description = table.Column<string>(maxLength: 1024, nullable: true),
                    IsStatic = table.Column<bool>(nullable: false),
                    Creator = table.Column<string>(nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    LastModifier = table.Column<string>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    Deleter = table.Column<string>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    TenantName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SampleEntitys",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DisplayName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SampleEntitys", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tenants",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    Creator = table.Column<string>(maxLength: 128, nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifier = table.Column<string>(maxLength: 128, nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Deleter = table.Column<string>(maxLength: 128, nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(maxLength: 64, nullable: true),
                    DisplayName = table.Column<string>(maxLength: 128, nullable: true),
                    Description = table.Column<string>(maxLength: 512, nullable: true),
                    ConnectionString = table.Column<string>(maxLength: 1024, nullable: true),
                    IsStatic = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
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
                    Nickname = table.Column<string>(maxLength: 512, nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    IsStatic = table.Column<bool>(nullable: false),
                    Creator = table.Column<string>(nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    LastModifier = table.Column<string>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    Deleter = table.Column<string>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    TenantName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<Guid>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true),
                    TenantName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleClaims_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true),
                    TenantName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserClaims_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    TenantName = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<Guid>(nullable: false),
                    Id = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => new { x.LoginProvider, x.ProviderKey, x.TenantName });
                    table.ForeignKey(
                        name: "FK_UserLogins_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    RoleId = table.Column<Guid>(nullable: false),
                    TenantName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true),
                    TenantName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PageColumnItem_Name_Creator_TenantName",
                table: "PageColumnItem",
                columns: new[] { "Name", "Creator", "TenantName" });

            migrationBuilder.CreateIndex(
                name: "IX_PageFilterItem_Name_Creator_TenantName",
                table: "PageFilterItem",
                columns: new[] { "Name", "Creator", "TenantName" });

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaims_RoleId",
                table: "RoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_NormalizedName_TenantName",
                table: "Roles",
                columns: new[] { "NormalizedName", "TenantName" },
                unique: true,
                filter: "[NormalizedName] IS NOT NULL AND [TenantName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Tenants_Name",
                table: "Tenants",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UserClaims_UserId",
                table: "UserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_UserId",
                table: "UserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UserId_RoleId_TenantName",
                table: "UserRoles",
                columns: new[] { "UserId", "RoleId", "TenantName" },
                unique: true,
                filter: "[TenantName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Nickname_TenantName",
                table: "Users",
                columns: new[] { "Nickname", "TenantName" },
                unique: true,
                filter: "[TenantName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Users_NormalizedUserName_NormalizedEmail_TenantName",
                table: "Users",
                columns: new[] { "NormalizedUserName", "NormalizedEmail", "TenantName" },
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL AND [NormalizedEmail] IS NOT NULL AND [TenantName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UserTokens_UserId_LoginProvider_Name_TenantName",
                table: "UserTokens",
                columns: new[] { "UserId", "LoginProvider", "Name", "TenantName" },
                unique: true,
                filter: "[LoginProvider] IS NOT NULL AND [Name] IS NOT NULL AND [TenantName] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppToken");

            migrationBuilder.DropTable(
                name: "PageColumnItem");

            migrationBuilder.DropTable(
                name: "PageFilterItem");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "RoleClaims");

            migrationBuilder.DropTable(
                name: "SampleEntitys");

            migrationBuilder.DropTable(
                name: "Tenants");

            migrationBuilder.DropTable(
                name: "UserClaims");

            migrationBuilder.DropTable(
                name: "UserLogins");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "UserTokens");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
