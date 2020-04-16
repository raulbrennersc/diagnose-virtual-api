using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace DiagnoseVirtual.Infra.Data.Migrations
{
    public partial class _11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "cultura",
                schema: "diagnose_virtual",
                table: "dados_fazenda");

            migrationBuilder.AddColumn<int>(
                name: "id_cultura",
                schema: "diagnose_virtual",
                table: "dados_fazenda",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "cultura",
                schema: "diagnose_virtual",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nome = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cultura", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_dados_fazenda_id_cultura",
                schema: "diagnose_virtual",
                table: "dados_fazenda",
                column: "id_cultura");

            migrationBuilder.CreateIndex(
                name: "IX_cultura_Id",
                schema: "diagnose_virtual",
                table: "cultura",
                column: "Id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_dados_fazenda_cultura_id_cultura",
                schema: "diagnose_virtual",
                table: "dados_fazenda",
                column: "id_cultura",
                principalSchema: "diagnose_virtual",
                principalTable: "cultura",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_dados_fazenda_cultura_id_cultura",
                schema: "diagnose_virtual",
                table: "dados_fazenda");

            migrationBuilder.DropTable(
                name: "cultura",
                schema: "diagnose_virtual");

            migrationBuilder.DropIndex(
                name: "IX_dados_fazenda_id_cultura",
                schema: "diagnose_virtual",
                table: "dados_fazenda");

            migrationBuilder.DropColumn(
                name: "id_cultura",
                schema: "diagnose_virtual",
                table: "dados_fazenda");

            migrationBuilder.AddColumn<string>(
                name: "cultura",
                schema: "diagnose_virtual",
                table: "dados_fazenda",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }
    }
}
