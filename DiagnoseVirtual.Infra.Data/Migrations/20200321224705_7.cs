using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace DiagnoseVirtual.Infra.Data.Migrations
{
    public partial class _7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "email",
                schema: "diagnose_virtual",
                table: "localizacao_fazenda");

            migrationBuilder.DropColumn(
                name: "estado",
                schema: "diagnose_virtual",
                table: "localizacao_fazenda");

            migrationBuilder.DropColumn(
                name: "municipio",
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
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "id_municipio",
                schema: "diagnose_virtual",
                table: "localizacao_fazenda",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "estado",
                schema: "diagnose_virtual",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nome = table.Column<string>(maxLength: 30, nullable: false),
                    sigla = table.Column<string>(maxLength: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_estado", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "municipio",
                schema: "diagnose_virtual",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    codigo_ibge = table.Column<int>(nullable: false),
                    nome = table.Column<string>(nullable: false),
                    id_estado = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_municipio", x => x.id);
                    table.ForeignKey(
                        name: "FK_municipio_estado_id_estado",
                        column: x => x.id_estado,
                        principalSchema: "diagnose_virtual",
                        principalTable: "estado",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_localizacao_fazenda_id_municipio",
                schema: "diagnose_virtual",
                table: "localizacao_fazenda",
                column: "id_municipio");

            migrationBuilder.CreateIndex(
                name: "IX_estado_id",
                schema: "diagnose_virtual",
                table: "estado",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_municipio_id",
                schema: "diagnose_virtual",
                table: "municipio",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_municipio_id_estado",
                schema: "diagnose_virtual",
                table: "municipio",
                column: "id_estado");

            migrationBuilder.AddForeignKey(
                name: "FK_localizacao_fazenda_municipio_id_municipio",
                schema: "diagnose_virtual",
                table: "localizacao_fazenda",
                column: "id_municipio",
                principalSchema: "diagnose_virtual",
                principalTable: "municipio",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_localizacao_fazenda_municipio_id_municipio",
                schema: "diagnose_virtual",
                table: "localizacao_fazenda");

            migrationBuilder.DropTable(
                name: "municipio",
                schema: "diagnose_virtual");

            migrationBuilder.DropTable(
                name: "estado",
                schema: "diagnose_virtual");

            migrationBuilder.DropIndex(
                name: "IX_localizacao_fazenda_id_municipio",
                schema: "diagnose_virtual",
                table: "localizacao_fazenda");

            migrationBuilder.DropColumn(
                name: "contato",
                schema: "diagnose_virtual",
                table: "localizacao_fazenda");

            migrationBuilder.DropColumn(
                name: "id_municipio",
                schema: "diagnose_virtual",
                table: "localizacao_fazenda");

            migrationBuilder.AddColumn<string>(
                name: "email",
                schema: "diagnose_virtual",
                table: "localizacao_fazenda",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "estado",
                schema: "diagnose_virtual",
                table: "localizacao_fazenda",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "municipio",
                schema: "diagnose_virtual",
                table: "localizacao_fazenda",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "telefone",
                schema: "diagnose_virtual",
                table: "localizacao_fazenda",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }
    }
}
