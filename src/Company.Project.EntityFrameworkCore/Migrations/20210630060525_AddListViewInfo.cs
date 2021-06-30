using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Company.Project.Migrations
{
    public partial class AddListViewInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateIndex(
                name: "IX_PageColumnItem_Name_Creator_TenantName",
                table: "PageColumnItem",
                columns: new[] { "Name", "Creator", "TenantName" });

            migrationBuilder.CreateIndex(
                name: "IX_PageFilterItem_Name_Creator_TenantName",
                table: "PageFilterItem",
                columns: new[] { "Name", "Creator", "TenantName" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PageColumnItem");

            migrationBuilder.DropTable(
                name: "PageFilterItem");
        }
    }
}
