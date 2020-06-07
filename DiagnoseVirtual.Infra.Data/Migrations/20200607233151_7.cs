using Microsoft.EntityFrameworkCore.Migrations;

namespace DiagnoseVirtual.Infra.Data.Migrations
{
    public partial class _7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "id_pdi",
                schema: "diagnose_virtual",
                table: "fazenda",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldDefaultValue: "True");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "id_pdi",
                schema: "diagnose_virtual",
                table: "fazenda",
                type: "text",
                nullable: true,
                defaultValue: "True",
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
