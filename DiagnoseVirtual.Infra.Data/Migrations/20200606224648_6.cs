using Microsoft.EntityFrameworkCore.Migrations;

namespace DiagnoseVirtual.Infra.Data.Migrations
{
    public partial class _6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_fazenda_etapa_fazenda_id_etapa",
                schema: "diagnose_virtual",
                table: "fazenda");

            migrationBuilder.AlterColumn<int>(
                name: "id_etapa",
                schema: "diagnose_virtual",
                table: "fazenda",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<string>(
                name: "id_pdi",
                schema: "diagnose_virtual",
                table: "fazenda",
                nullable: true,
                defaultValue: "True");

            migrationBuilder.AddForeignKey(
                name: "FK_fazenda_etapa_fazenda_id_etapa",
                schema: "diagnose_virtual",
                table: "fazenda",
                column: "id_etapa",
                principalSchema: "diagnose_virtual",
                principalTable: "etapa_fazenda",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_fazenda_etapa_fazenda_id_etapa",
                schema: "diagnose_virtual",
                table: "fazenda");

            migrationBuilder.DropColumn(
                name: "id_pdi",
                schema: "diagnose_virtual",
                table: "fazenda");

            migrationBuilder.AlterColumn<int>(
                name: "id_etapa",
                schema: "diagnose_virtual",
                table: "fazenda",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_fazenda_etapa_fazenda_id_etapa",
                schema: "diagnose_virtual",
                table: "fazenda",
                column: "id_etapa",
                principalSchema: "diagnose_virtual",
                principalTable: "etapa_fazenda",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
