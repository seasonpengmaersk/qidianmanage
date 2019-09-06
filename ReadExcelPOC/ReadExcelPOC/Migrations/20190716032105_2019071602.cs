using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ReadExcelPOC.Migrations
{
    public partial class _2019071602 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Terminal",
                table: "Terminal");

            migrationBuilder.AlterColumn<string>(
                name: "TerminalGEOID",
                table: "Terminal",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Terminal",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Terminal",
                table: "Terminal",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Terminal",
                table: "Terminal");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Terminal");

            migrationBuilder.AlterColumn<string>(
                name: "TerminalGEOID",
                table: "Terminal",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Terminal",
                table: "Terminal",
                column: "TerminalGEOID");
        }
    }
}
