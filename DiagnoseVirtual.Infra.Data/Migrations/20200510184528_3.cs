using Microsoft.EntityFrameworkCore.Migrations;

namespace DiagnoseVirtual.Infra.Data.Migrations
{
    public partial class _3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ativa",
                schema: "diagnose_virtual",
                table: "lavoura",
                nullable: false,
                defaultValue: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ativa",
                schema: "diagnose_virtual",
                table: "lavoura");
        }
    }
}
