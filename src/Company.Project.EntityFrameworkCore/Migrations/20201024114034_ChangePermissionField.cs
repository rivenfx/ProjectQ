using Microsoft.EntityFrameworkCore.Migrations;

namespace Company.Project.Migrations
{
    public partial class ChangePermissionField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClaimType",
                table: "UserPermissions");

            migrationBuilder.DropColumn(
                name: "ClaimValue",
                table: "UserPermissions");

            migrationBuilder.DropColumn(
                name: "ClaimType",
                table: "RolePermissions");

            migrationBuilder.DropColumn(
                name: "ClaimValue",
                table: "RolePermissions");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "UserPermissions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "RolePermissions",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "UserPermissions");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "RolePermissions");

            migrationBuilder.AddColumn<string>(
                name: "ClaimType",
                table: "UserPermissions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClaimValue",
                table: "UserPermissions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClaimType",
                table: "RolePermissions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClaimValue",
                table: "RolePermissions",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
