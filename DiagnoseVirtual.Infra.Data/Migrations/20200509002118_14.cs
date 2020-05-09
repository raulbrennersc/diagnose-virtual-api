using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace DiagnoseVirtual.Infra.Data.Migrations
{
    public partial class _14 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "talhao",
                schema: "diagnose_virtual");

            migrationBuilder.AddColumn<MultiPolygon>(
                name: "talhoes",
                schema: "diagnose_virtual",
                table: "lavoura",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "talhoes",
                schema: "diagnose_virtual",
                table: "lavoura");

            migrationBuilder.CreateTable(
                name: "talhao",
                schema: "diagnose_virtual",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    geometria_geom = table.Column<Geometry>(type: "geometry", nullable: false),
                    id_lavoura = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_talhao", x => x.id);
                    table.ForeignKey(
                        name: "FK_talhao_lavoura_id_lavoura",
                        column: x => x.id_lavoura,
                        principalSchema: "diagnose_virtual",
                        principalTable: "lavoura",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_talhao_id",
                schema: "diagnose_virtual",
                table: "talhao",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_talhao_id_lavoura",
                schema: "diagnose_virtual",
                table: "talhao",
                column: "id_lavoura");
        }
    }
}
