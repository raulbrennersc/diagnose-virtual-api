using Microsoft.EntityFrameworkCore.Migrations;

namespace DiagnoseVirtual.Infra.Data.Migrations
{
    public partial class _5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "nome_arquivo",
                schema: "diagnose_virtual",
                table: "upload_monitoramento",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(25)",
                oldMaxLength: 25);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "nome_arquivo",
                schema: "diagnose_virtual",
                table: "upload_monitoramento",
                type: "character varying(25)",
                maxLength: 25,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 50);
        }
    }
}
