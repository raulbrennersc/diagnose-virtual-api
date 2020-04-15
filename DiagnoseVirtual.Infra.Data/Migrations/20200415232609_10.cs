using Microsoft.EntityFrameworkCore.Migrations;

namespace DiagnoseVirtual.Infra.Data.Migrations
{
    public partial class _10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ativo",
                schema: "diagnose_virtual",
                table: "monitoramento",
                nullable: false,
                defaultValue: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ativo",
                schema: "diagnose_virtual",
                table: "monitoramento");
        }
    }
}
