using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace DiagnoseVirtual.Infra.Data.Migrations
{
    public partial class _1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "diagnose_virtual");

            migrationBuilder.CreateTable(
                name: "usuario",
                schema: "diagnose_virtual",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nome = table.Column<string>(maxLength: 100, nullable: false),
                    cpf = table.Column<string>(maxLength: 11, nullable: false),
                    email = table.Column<string>(maxLength: 50, nullable: false),
                    password_hash = table.Column<byte[]>(nullable: false),
                    password_salt = table.Column<byte[]>(nullable: false),
                    ativo = table.Column<bool>(nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usuario", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "fazenda",
                schema: "diagnose_virtual",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    demarcacao_geom = table.Column<Geometry>(nullable: true),
                    concluida = table.Column<bool>(nullable: false, defaultValue: false),
                    ativa = table.Column<bool>(nullable: false, defaultValue: true),
                    id_usuario = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fazenda", x => x.id);
                    table.ForeignKey(
                        name: "FK_fazenda_usuario_id_usuario",
                        column: x => x.id_usuario,
                        principalSchema: "diagnose_virtual",
                        principalTable: "usuario",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "dados_fazenda",
                schema: "diagnose_virtual",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    cultura = table.Column<string>(maxLength: 100, nullable: false),
                    area_total = table.Column<double>(nullable: false),
                    quantidade_lavouras = table.Column<int>(nullable: false),
                    IdFazenda = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dados_fazenda", x => x.id);
                    table.ForeignKey(
                        name: "FK_dados_fazenda_fazenda_IdFazenda",
                        column: x => x.IdFazenda,
                        principalSchema: "diagnose_virtual",
                        principalTable: "fazenda",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "lavoura",
                schema: "diagnose_virtual",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    demarcacao_geom = table.Column<Geometry>(nullable: false),
                    concluida = table.Column<bool>(nullable: false, defaultValue: false),
                    IdFazenda = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lavoura", x => x.id);
                    table.ForeignKey(
                        name: "FK_lavoura_fazenda_IdFazenda",
                        column: x => x.IdFazenda,
                        principalSchema: "diagnose_virtual",
                        principalTable: "fazenda",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "localizacao_fazenda",
                schema: "diagnose_virtual",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nome = table.Column<string>(maxLength: 30, nullable: false),
                    estado = table.Column<string>(maxLength: 20, nullable: false),
                    municipio = table.Column<string>(maxLength: 50, nullable: false),
                    proprietario = table.Column<string>(maxLength: 50, nullable: false),
                    gerente = table.Column<string>(maxLength: 30, nullable: false),
                    contato = table.Column<string>(maxLength: 50, nullable: false),
                    ponto_referencia = table.Column<string>(maxLength: 70, nullable: false),
                    IdFazenda = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_localizacao_fazenda", x => x.id);
                    table.ForeignKey(
                        name: "FK_localizacao_fazenda_fazenda_IdFazenda",
                        column: x => x.IdFazenda,
                        principalSchema: "diagnose_virtual",
                        principalTable: "fazenda",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "dados_lavoura",
                schema: "diagnose_virtual",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nome = table.Column<string>(maxLength: 20, nullable: false),
                    mes_ano_plantio = table.Column<string>(maxLength: 7, nullable: false),
                    cultivar = table.Column<string>(maxLength: 30, nullable: false),
                    numero_plantas = table.Column<int>(nullable: false),
                    expacamento_vertical = table.Column<double>(nullable: false),
                    espacamento_horizontal = table.Column<double>(nullable: false),
                    observacoes = table.Column<string>(maxLength: 250, nullable: false),
                    IdLavoura = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dados_lavoura", x => x.id);
                    table.ForeignKey(
                        name: "FK_dados_lavoura_lavoura_IdLavoura",
                        column: x => x.IdLavoura,
                        principalSchema: "diagnose_virtual",
                        principalTable: "lavoura",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "talhao",
                schema: "diagnose_virtual",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    geometria_geom = table.Column<Geometry>(nullable: false),
                    IdLavoura = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_talhao", x => x.id);
                    table.ForeignKey(
                        name: "FK_talhao_lavoura_IdLavoura",
                        column: x => x.IdLavoura,
                        principalSchema: "diagnose_virtual",
                        principalTable: "lavoura",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_dados_fazenda_id",
                schema: "diagnose_virtual",
                table: "dados_fazenda",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_dados_fazenda_IdFazenda",
                schema: "diagnose_virtual",
                table: "dados_fazenda",
                column: "IdFazenda",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_dados_lavoura_id",
                schema: "diagnose_virtual",
                table: "dados_lavoura",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_dados_lavoura_IdLavoura",
                schema: "diagnose_virtual",
                table: "dados_lavoura",
                column: "IdLavoura",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_fazenda_id",
                schema: "diagnose_virtual",
                table: "fazenda",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_fazenda_id_usuario",
                schema: "diagnose_virtual",
                table: "fazenda",
                column: "id_usuario");

            migrationBuilder.CreateIndex(
                name: "IX_lavoura_id",
                schema: "diagnose_virtual",
                table: "lavoura",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_lavoura_IdFazenda",
                schema: "diagnose_virtual",
                table: "lavoura",
                column: "IdFazenda");

            migrationBuilder.CreateIndex(
                name: "IX_localizacao_fazenda_id",
                schema: "diagnose_virtual",
                table: "localizacao_fazenda",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_localizacao_fazenda_IdFazenda",
                schema: "diagnose_virtual",
                table: "localizacao_fazenda",
                column: "IdFazenda",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_talhao_id",
                schema: "diagnose_virtual",
                table: "talhao",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_talhao_IdLavoura",
                schema: "diagnose_virtual",
                table: "talhao",
                column: "IdLavoura");

            migrationBuilder.CreateIndex(
                name: "IX_usuario_cpf",
                schema: "diagnose_virtual",
                table: "usuario",
                column: "cpf",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_usuario_email",
                schema: "diagnose_virtual",
                table: "usuario",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_usuario_id",
                schema: "diagnose_virtual",
                table: "usuario",
                column: "id",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "dados_fazenda",
                schema: "diagnose_virtual");

            migrationBuilder.DropTable(
                name: "dados_lavoura",
                schema: "diagnose_virtual");

            migrationBuilder.DropTable(
                name: "localizacao_fazenda",
                schema: "diagnose_virtual");

            migrationBuilder.DropTable(
                name: "talhao",
                schema: "diagnose_virtual");

            migrationBuilder.DropTable(
                name: "lavoura",
                schema: "diagnose_virtual");

            migrationBuilder.DropTable(
                name: "fazenda",
                schema: "diagnose_virtual");

            migrationBuilder.DropTable(
                name: "usuario",
                schema: "diagnose_virtual");
        }
    }
}
