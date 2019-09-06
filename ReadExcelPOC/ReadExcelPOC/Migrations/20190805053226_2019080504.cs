using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ReadExcelPOC.Migrations
{
    public partial class _2019080504 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "City",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CountryName = table.Column<string>(nullable: true),
                    GEOID = table.Column<string>(maxLength: 255, nullable: true),
                    RKSTCode = table.Column<string>(nullable: true),
                    CityName = table.Column<string>(nullable: true),
                    StandardName = table.Column<string>(maxLength: 255, nullable: true),
                    Alias = table.Column<string>(maxLength: 255, nullable: true),
                    Status = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_City", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "City");
        }
    }
}
