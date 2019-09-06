using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ReadExcelPOC.Migrations
{
    public partial class _2019080501 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExcelObject");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExcelObject",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Key = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExcelObject", x => x.id);
                });
        }
    }
}
