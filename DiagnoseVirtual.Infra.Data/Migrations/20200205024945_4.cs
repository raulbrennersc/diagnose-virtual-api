using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace DiagnoseVirtual.Infra.Data.Migrations
{
    public partial class _4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "monitoramento",
                schema: "diagnose_virtual",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_fazenda = table.Column<int>(nullable: true),
                    data_monitoramento = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_monitoramento", x => x.id);
                    table.ForeignKey(
                        name: "FK_monitoramento_fazenda_id_fazenda",
                        column: x => x.id_fazenda,
                        principalSchema: "diagnose_virtual",
                        principalTable: "fazenda",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "problema_monitoramento",
                schema: "diagnose_virtual",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_monitoramento = table.Column<int>(nullable: true),
                    ponto_geom = table.Column<Geometry>(nullable: false),
                    descricao = table.Column<string>(maxLength: 140, nullable: false),
                    recomendacao = table.Column<string>(maxLength: 140, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_problema_monitoramento", x => x.id);
                    table.ForeignKey(
                        name: "FK_problema_monitoramento_monitoramento_id_monitoramento",
                        column: x => x.id_monitoramento,
                        principalSchema: "diagnose_virtual",
                        principalTable: "monitoramento",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "upload_monitoramento",
                schema: "diagnose_virtual",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_monitoramento = table.Column<int>(nullable: true),
                    nome_arquivo = table.Column<string>(maxLength: 25, nullable: false),
                    url = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_upload_monitoramento", x => x.id);
                    table.ForeignKey(
                        name: "FK_upload_monitoramento_monitoramento_id_monitoramento",
                        column: x => x.id_monitoramento,
                        principalSchema: "diagnose_virtual",
                        principalTable: "monitoramento",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_monitoramento_id",
                schema: "diagnose_virtual",
                table: "monitoramento",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_monitoramento_id_fazenda",
                schema: "diagnose_virtual",
                table: "monitoramento",
                column: "id_fazenda");

            migrationBuilder.CreateIndex(
                name: "IX_problema_monitoramento_id",
                schema: "diagnose_virtual",
                table: "problema_monitoramento",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_problema_monitoramento_id_monitoramento",
                schema: "diagnose_virtual",
                table: "problema_monitoramento",
                column: "id_monitoramento");

            migrationBuilder.CreateIndex(
                name: "IX_upload_monitoramento_id",
                schema: "diagnose_virtual",
                table: "upload_monitoramento",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_upload_monitoramento_id_monitoramento",
                schema: "diagnose_virtual",
                table: "upload_monitoramento",
                column: "id_monitoramento");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "problema_monitoramento",
                schema: "diagnose_virtual");

            migrationBuilder.DropTable(
                name: "upload_monitoramento",
                schema: "diagnose_virtual");

            migrationBuilder.DropTable(
                name: "monitoramento",
                schema: "diagnose_virtual");
        }
    }
}
