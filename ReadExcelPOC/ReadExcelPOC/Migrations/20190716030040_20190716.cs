using Microsoft.EntityFrameworkCore.Migrations;

namespace ReadExcelPOC.Migrations
{
    public partial class _20190716 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Terminal",
                columns: table => new
                {
                    TerminalGEOID = table.Column<string>(nullable: false),
                    SubArea = table.Column<string>(name: "Sub Area", nullable: true),
                    TerminalRKSTCode = table.Column<string>(nullable: true),
                    PortRKST = table.Column<string>(nullable: true),
                    PortGEOID = table.Column<string>(nullable: true),
                    TerminalName = table.Column<string>(nullable: true),
                    PortName = table.Column<string>(nullable: true),
                    ExtendField1 = table.Column<string>(nullable: true),
                    ExtendField2 = table.Column<string>(nullable: true),
                    ExtendField3 = table.Column<string>(nullable: true),
                    ExtendField4 = table.Column<string>(nullable: true),
                    ExtendField5 = table.Column<string>(nullable: true),
                    ExtendField6 = table.Column<string>(nullable: true),
                    ExtendField7 = table.Column<string>(nullable: true),
                    ExtendField8 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Terminal", x => x.TerminalGEOID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Terminal");
        }
    }
}
