using Microsoft.EntityFrameworkCore.Migrations;

namespace Company.Project.Migrations
{
    public partial class RoleRenameDispayNameToDisplayName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DispayName",
                table: "Roles");

            migrationBuilder.AddColumn<string>(
                name: "DisplayName",
                table: "Roles",
                maxLength: 512,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisplayName",
                table: "Roles");

            migrationBuilder.AddColumn<string>(
                name: "DispayName",
                table: "Roles",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: true);
        }
    }
}
