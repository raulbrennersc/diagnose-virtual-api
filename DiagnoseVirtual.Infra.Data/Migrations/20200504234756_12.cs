using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace DiagnoseVirtual.Infra.Data.Migrations
{
    public partial class _12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "id_etapa",
                schema: "diagnose_virtual",
                table: "fazenda",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "etapa_fazenda",
                schema: "diagnose_virtual",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nome = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_etapa_fazenda", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_fazenda_id_etapa",
                schema: "diagnose_virtual",
                table: "fazenda",
                column: "id_etapa");

            migrationBuilder.CreateIndex(
                name: "IX_etapa_fazenda_id",
                schema: "diagnose_virtual",
                table: "etapa_fazenda",
                column: "id",
                unique: true);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_fazenda_etapa_fazenda_id_etapa",
                schema: "diagnose_virtual",
                table: "fazenda");

            migrationBuilder.DropTable(
                name: "etapa_fazenda",
                schema: "diagnose_virtual");

            migrationBuilder.DropIndex(
                name: "IX_fazenda_id_etapa",
                schema: "diagnose_virtual",
                table: "fazenda");

            migrationBuilder.DropColumn(
                name: "id_etapa",
                schema: "diagnose_virtual",
                table: "fazenda");
        }
    }
}
