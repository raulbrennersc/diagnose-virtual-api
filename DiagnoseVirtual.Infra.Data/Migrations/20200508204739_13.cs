using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace DiagnoseVirtual.Infra.Data.Migrations
{
    public partial class _13 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "id_etapa",
                schema: "diagnose_virtual",
                table: "lavoura",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "etapa_lavoura",
                schema: "diagnose_virtual",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nome = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_etapa_lavoura", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_lavoura_id_etapa",
                schema: "diagnose_virtual",
                table: "lavoura",
                column: "id_etapa");

            migrationBuilder.CreateIndex(
                name: "IX_etapa_lavoura_id",
                schema: "diagnose_virtual",
                table: "etapa_lavoura",
                column: "id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_lavoura_etapa_lavoura_id_etapa",
                schema: "diagnose_virtual",
                table: "lavoura",
                column: "id_etapa",
                principalSchema: "diagnose_virtual",
                principalTable: "etapa_lavoura",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_lavoura_etapa_lavoura_id_etapa",
                schema: "diagnose_virtual",
                table: "lavoura");

            migrationBuilder.DropTable(
                name: "etapa_lavoura",
                schema: "diagnose_virtual");

            migrationBuilder.DropIndex(
                name: "IX_lavoura_id_etapa",
                schema: "diagnose_virtual",
                table: "lavoura");

            migrationBuilder.DropColumn(
                name: "id_etapa",
                schema: "diagnose_virtual",
                table: "lavoura");
        }
    }
}
