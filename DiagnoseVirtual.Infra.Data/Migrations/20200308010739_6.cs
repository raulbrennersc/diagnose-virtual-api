using Microsoft.EntityFrameworkCore.Migrations;

namespace DiagnoseVirtual.Infra.Data.Migrations
{
    public partial class _6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "contato",
                schema: "diagnose_virtual",
                table: "localizacao_fazenda");

            migrationBuilder.AddColumn<string>(
                name: "email",
                schema: "diagnose_virtual",
                table: "localizacao_fazenda",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "telefone",
                schema: "diagnose_virtual",
                table: "localizacao_fazenda",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "email",
                schema: "diagnose_virtual",
                table: "localizacao_fazenda");

            migrationBuilder.DropColumn(
                name: "telefone",
                schema: "diagnose_virtual",
                table: "localizacao_fazenda");

            migrationBuilder.AddColumn<string>(
                name: "contato",
                schema: "diagnose_virtual",
                table: "localizacao_fazenda",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }
    }
}
