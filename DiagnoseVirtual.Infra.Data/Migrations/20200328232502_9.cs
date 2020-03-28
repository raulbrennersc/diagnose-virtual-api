using Microsoft.EntityFrameworkCore.Migrations;

namespace DiagnoseVirtual.Infra.Data.Migrations
{
    public partial class _9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "primeiro_acesso",
                schema: "diagnose_virtual",
                table: "usuario",
                nullable: false,
                defaultValue: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "primeiro_acesso",
                schema: "diagnose_virtual",
                table: "usuario");
        }
    }
}
