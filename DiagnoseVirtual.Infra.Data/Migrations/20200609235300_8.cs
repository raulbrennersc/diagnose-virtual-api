using Microsoft.EntityFrameworkCore.Migrations;

namespace DiagnoseVirtual.Infra.Data.Migrations
{
    public partial class _8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "id_pdi",
                schema: "diagnose_virtual",
                table: "lavoura",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "id_pdi",
                schema: "diagnose_virtual",
                table: "lavoura");
        }
    }
}
