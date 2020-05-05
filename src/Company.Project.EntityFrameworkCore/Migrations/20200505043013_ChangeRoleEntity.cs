using Microsoft.EntityFrameworkCore.Migrations;

namespace Company.Project.Migrations
{
    public partial class ChangeRoleEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "Roles");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Roles",
                maxLength: 1024,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DispayName",
                table: "Roles",
                maxLength: 512,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "DispayName",
                table: "Roles");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Roles",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: true);
        }
    }
}
