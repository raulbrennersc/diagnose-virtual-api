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
                    ativo = table.Column<bool>(nullable: false, defaultValue: true),
                    primeiro_acesso = table.Column<bool>(nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usuario", x => x.id);
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

            migrationBuilder.CreateTable(
                name: "fazenda",
                schema: "diagnose_virtual",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_usuario = table.Column<int>(nullable: true),
                    id_etapa = table.Column<int>(nullable: false),
                    demarcacao_geom = table.Column<Geometry>(nullable: true),
                    concluida = table.Column<bool>(nullable: false, defaultValue: false),
                    ativa = table.Column<bool>(nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fazenda", x => x.id);
                    table.ForeignKey(
                        name: "FK_fazenda_etapa_fazenda_id_etapa",
                        column: x => x.id_etapa,
                        principalSchema: "diagnose_virtual",
                        principalTable: "etapa_fazenda",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_fazenda_usuario_id_usuario",
                        column: x => x.id_usuario,
                        principalSchema: "diagnose_virtual",
                        principalTable: "usuario",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "dados_fazenda",
                schema: "diagnose_virtual",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_cultura = table.Column<int>(nullable: true),
                    area_total = table.Column<double>(nullable: false),
                    quantidade_lavouras = table.Column<int>(nullable: false),
                    id_fazenda = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dados_fazenda", x => x.id);
                    table.ForeignKey(
                        name: "FK_dados_fazenda_cultura_id_cultura",
                        column: x => x.id_cultura,
                        principalSchema: "diagnose_virtual",
                        principalTable: "cultura",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dados_fazenda_fazenda_id_fazenda",
                        column: x => x.id_fazenda,
                        principalSchema: "diagnose_virtual",
                        principalTable: "fazenda",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "lavoura",
                schema: "diagnose_virtual",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_etapa = table.Column<int>(nullable: false),
                    id_fazenda = table.Column<int>(nullable: true),
                    demarcacao_geom = table.Column<Geometry>(nullable: true),
                    concluida = table.Column<bool>(nullable: false, defaultValue: false),
                    talhoes = table.Column<Geometry[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lavoura", x => x.id);
                    table.ForeignKey(
                        name: "FK_lavoura_etapa_lavoura_id_etapa",
                        column: x => x.id_etapa,
                        principalSchema: "diagnose_virtual",
                        principalTable: "etapa_lavoura",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_lavoura_fazenda_id_fazenda",
                        column: x => x.id_fazenda,
                        principalSchema: "diagnose_virtual",
                        principalTable: "fazenda",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "localizacao_fazenda",
                schema: "diagnose_virtual",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nome = table.Column<string>(maxLength: 30, nullable: false),
                    id_municipio = table.Column<int>(nullable: true),
                    proprietario = table.Column<string>(maxLength: 50, nullable: false),
                    gerente = table.Column<string>(maxLength: 30, nullable: false),
                    telefone = table.Column<string>(maxLength: 20, nullable: false),
                    email = table.Column<string>(maxLength: 50, nullable: false),
                    ponto_referencia = table.Column<string>(maxLength: 70, nullable: false),
                    id_fazenda = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_localizacao_fazenda", x => x.id);
                    table.ForeignKey(
                        name: "FK_localizacao_fazenda_fazenda_id_fazenda",
                        column: x => x.id_fazenda,
                        principalSchema: "diagnose_virtual",
                        principalTable: "fazenda",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_localizacao_fazenda_municipio_id_municipio",
                        column: x => x.id_municipio,
                        principalSchema: "diagnose_virtual",
                        principalTable: "municipio",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "monitoramento",
                schema: "diagnose_virtual",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_fazenda = table.Column<int>(nullable: true),
                    data_monitoramento = table.Column<DateTime>(nullable: false),
                    ativo = table.Column<bool>(nullable: false, defaultValue: true)
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
                name: "dados_lavoura",
                schema: "diagnose_virtual",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nome = table.Column<string>(maxLength: 20, nullable: false),
                    mes_ano_plantio = table.Column<string>(maxLength: 20, nullable: false),
                    cultivar = table.Column<string>(maxLength: 30, nullable: false),
                    numero_plantas = table.Column<int>(nullable: false),
                    expacamento_vertical = table.Column<double>(nullable: false),
                    espacamento_horizontal = table.Column<double>(nullable: false),
                    observacoes = table.Column<string>(maxLength: 250, nullable: false),
                    id_lavoura = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dados_lavoura", x => x.id);
                    table.ForeignKey(
                        name: "FK_dados_lavoura_lavoura_id_lavoura",
                        column: x => x.id_lavoura,
                        principalSchema: "diagnose_virtual",
                        principalTable: "lavoura",
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
                    ponto_geom = table.Column<Point>(nullable: false),
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
                    nome_arquivo = table.Column<string>(maxLength: 50, nullable: false),
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
                name: "IX_cultura_Id",
                schema: "diagnose_virtual",
                table: "cultura",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_dados_fazenda_id",
                schema: "diagnose_virtual",
                table: "dados_fazenda",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_dados_fazenda_id_cultura",
                schema: "diagnose_virtual",
                table: "dados_fazenda",
                column: "id_cultura");

            migrationBuilder.CreateIndex(
                name: "IX_dados_fazenda_id_fazenda",
                schema: "diagnose_virtual",
                table: "dados_fazenda",
                column: "id_fazenda",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_dados_lavoura_id",
                schema: "diagnose_virtual",
                table: "dados_lavoura",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_dados_lavoura_id_lavoura",
                schema: "diagnose_virtual",
                table: "dados_lavoura",
                column: "id_lavoura",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_estado_id",
                schema: "diagnose_virtual",
                table: "estado",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_etapa_fazenda_id",
                schema: "diagnose_virtual",
                table: "etapa_fazenda",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_etapa_lavoura_id",
                schema: "diagnose_virtual",
                table: "etapa_lavoura",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_fazenda_id",
                schema: "diagnose_virtual",
                table: "fazenda",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_fazenda_id_etapa",
                schema: "diagnose_virtual",
                table: "fazenda",
                column: "id_etapa");

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
                name: "IX_lavoura_id_etapa",
                schema: "diagnose_virtual",
                table: "lavoura",
                column: "id_etapa");

            migrationBuilder.CreateIndex(
                name: "IX_lavoura_id_fazenda",
                schema: "diagnose_virtual",
                table: "lavoura",
                column: "id_fazenda");

            migrationBuilder.CreateIndex(
                name: "IX_localizacao_fazenda_id",
                schema: "diagnose_virtual",
                table: "localizacao_fazenda",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_localizacao_fazenda_id_fazenda",
                schema: "diagnose_virtual",
                table: "localizacao_fazenda",
                column: "id_fazenda",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_localizacao_fazenda_id_municipio",
                schema: "diagnose_virtual",
                table: "localizacao_fazenda",
                column: "id_municipio");

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
                name: "problema_monitoramento",
                schema: "diagnose_virtual");

            migrationBuilder.DropTable(
                name: "upload_monitoramento",
                schema: "diagnose_virtual");

            migrationBuilder.DropTable(
                name: "cultura",
                schema: "diagnose_virtual");

            migrationBuilder.DropTable(
                name: "lavoura",
                schema: "diagnose_virtual");

            migrationBuilder.DropTable(
                name: "municipio",
                schema: "diagnose_virtual");

            migrationBuilder.DropTable(
                name: "monitoramento",
                schema: "diagnose_virtual");

            migrationBuilder.DropTable(
                name: "etapa_lavoura",
                schema: "diagnose_virtual");

            migrationBuilder.DropTable(
                name: "estado",
                schema: "diagnose_virtual");

            migrationBuilder.DropTable(
                name: "fazenda",
                schema: "diagnose_virtual");

            migrationBuilder.DropTable(
                name: "etapa_fazenda",
                schema: "diagnose_virtual");

            migrationBuilder.DropTable(
                name: "usuario",
                schema: "diagnose_virtual");
        }
    }
}
