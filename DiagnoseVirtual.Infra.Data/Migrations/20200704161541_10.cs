using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

namespace DiagnoseVirtual.Infra.Data.Migrations
{
    public partial class _10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_lavoura_etapa_lavoura_id_etapa",
                schema: "diagnose_virtual",
                table: "lavoura");

            migrationBuilder.AlterColumn<byte[]>(
                name: "password_salt",
                schema: "diagnose_virtual",
                table: "usuario",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "bytea");

            migrationBuilder.AlterColumn<byte[]>(
                name: "password_hash",
                schema: "diagnose_virtual",
                table: "usuario",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "bytea");

            migrationBuilder.AlterColumn<string>(
                name: "nome",
                schema: "diagnose_virtual",
                table: "usuario",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "email",
                schema: "diagnose_virtual",
                table: "usuario",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "cpf",
                schema: "diagnose_virtual",
                table: "usuario",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "nome_arquivo",
                schema: "diagnose_virtual",
                table: "upload_monitoramento",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<byte[]>(
                name: "arquivo",
                schema: "diagnose_virtual",
                table: "upload_monitoramento",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "bytea");

            migrationBuilder.AlterColumn<string>(
                name: "recomendacao",
                schema: "diagnose_virtual",
                table: "problema_monitoramento",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<Geometry>(
                name: "ponto_geom",
                schema: "diagnose_virtual",
                table: "problema_monitoramento",
                nullable: true,
                oldClrType: typeof(Geometry),
                oldType: "geometry");

            migrationBuilder.AlterColumn<string>(
                name: "descricao",
                schema: "diagnose_virtual",
                table: "problema_monitoramento",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "nome",
                schema: "diagnose_virtual",
                table: "municipio",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "telefone",
                schema: "diagnose_virtual",
                table: "localizacao_fazenda",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "proprietario",
                schema: "diagnose_virtual",
                table: "localizacao_fazenda",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "ponto_referencia",
                schema: "diagnose_virtual",
                table: "localizacao_fazenda",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "nome",
                schema: "diagnose_virtual",
                table: "localizacao_fazenda",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "gerente",
                schema: "diagnose_virtual",
                table: "localizacao_fazenda",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "email",
                schema: "diagnose_virtual",
                table: "localizacao_fazenda",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "id_etapa",
                schema: "diagnose_virtual",
                table: "lavoura",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "nome",
                schema: "diagnose_virtual",
                table: "etapa_lavoura",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "nome",
                schema: "diagnose_virtual",
                table: "etapa_fazenda",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "sigla",
                schema: "diagnose_virtual",
                table: "estado",
                maxLength: 2,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(2)",
                oldMaxLength: 2);

            migrationBuilder.AlterColumn<string>(
                name: "nome",
                schema: "diagnose_virtual",
                table: "estado",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "observacoes",
                schema: "diagnose_virtual",
                table: "dados_lavoura",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "nome",
                schema: "diagnose_virtual",
                table: "dados_lavoura",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "mes_ano_plantio",
                schema: "diagnose_virtual",
                table: "dados_lavoura",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "cultivar",
                schema: "diagnose_virtual",
                table: "dados_lavoura",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "nome",
                schema: "diagnose_virtual",
                table: "cultura",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddForeignKey(
                name: "FK_lavoura_etapa_lavoura_id_etapa",
                schema: "diagnose_virtual",
                table: "lavoura",
                column: "id_etapa",
                principalSchema: "diagnose_virtual",
                principalTable: "etapa_lavoura",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_lavoura_etapa_lavoura_id_etapa",
                schema: "diagnose_virtual",
                table: "lavoura");

            migrationBuilder.AlterColumn<byte[]>(
                name: "password_salt",
                schema: "diagnose_virtual",
                table: "usuario",
                type: "bytea",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldNullable: true);

            migrationBuilder.AlterColumn<byte[]>(
                name: "password_hash",
                schema: "diagnose_virtual",
                table: "usuario",
                type: "bytea",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "nome",
                schema: "diagnose_virtual",
                table: "usuario",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "email",
                schema: "diagnose_virtual",
                table: "usuario",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "cpf",
                schema: "diagnose_virtual",
                table: "usuario",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "nome_arquivo",
                schema: "diagnose_virtual",
                table: "upload_monitoramento",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<byte[]>(
                name: "arquivo",
                schema: "diagnose_virtual",
                table: "upload_monitoramento",
                type: "bytea",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "recomendacao",
                schema: "diagnose_virtual",
                table: "problema_monitoramento",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<Geometry>(
                name: "ponto_geom",
                schema: "diagnose_virtual",
                table: "problema_monitoramento",
                type: "geometry",
                nullable: false,
                oldClrType: typeof(Geometry),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "descricao",
                schema: "diagnose_virtual",
                table: "problema_monitoramento",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "nome",
                schema: "diagnose_virtual",
                table: "municipio",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "telefone",
                schema: "diagnose_virtual",
                table: "localizacao_fazenda",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "proprietario",
                schema: "diagnose_virtual",
                table: "localizacao_fazenda",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ponto_referencia",
                schema: "diagnose_virtual",
                table: "localizacao_fazenda",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "nome",
                schema: "diagnose_virtual",
                table: "localizacao_fazenda",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "gerente",
                schema: "diagnose_virtual",
                table: "localizacao_fazenda",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "email",
                schema: "diagnose_virtual",
                table: "localizacao_fazenda",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "id_etapa",
                schema: "diagnose_virtual",
                table: "lavoura",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "nome",
                schema: "diagnose_virtual",
                table: "etapa_lavoura",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "nome",
                schema: "diagnose_virtual",
                table: "etapa_fazenda",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "sigla",
                schema: "diagnose_virtual",
                table: "estado",
                type: "character varying(2)",
                maxLength: 2,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "nome",
                schema: "diagnose_virtual",
                table: "estado",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "observacoes",
                schema: "diagnose_virtual",
                table: "dados_lavoura",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "nome",
                schema: "diagnose_virtual",
                table: "dados_lavoura",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "mes_ano_plantio",
                schema: "diagnose_virtual",
                table: "dados_lavoura",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "cultivar",
                schema: "diagnose_virtual",
                table: "dados_lavoura",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "nome",
                schema: "diagnose_virtual",
                table: "cultura",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

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
    }
}
