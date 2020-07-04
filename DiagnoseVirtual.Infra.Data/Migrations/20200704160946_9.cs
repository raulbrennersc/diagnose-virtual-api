using Microsoft.EntityFrameworkCore.Migrations;

namespace DiagnoseVirtual.Infra.Data.Migrations
{
    public partial class _9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "nome",
                schema: "diagnose_virtual",
                table: "usuario",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "email",
                schema: "diagnose_virtual",
                table: "usuario",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "cpf",
                schema: "diagnose_virtual",
                table: "usuario",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(11)",
                oldMaxLength: 11);

            migrationBuilder.AlterColumn<string>(
                name: "nome_arquivo",
                schema: "diagnose_virtual",
                table: "upload_monitoramento",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "recomendacao",
                schema: "diagnose_virtual",
                table: "problema_monitoramento",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(140)",
                oldMaxLength: 140);

            migrationBuilder.AlterColumn<string>(
                name: "descricao",
                schema: "diagnose_virtual",
                table: "problema_monitoramento",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(140)",
                oldMaxLength: 140);

            migrationBuilder.AlterColumn<string>(
                name: "telefone",
                schema: "diagnose_virtual",
                table: "localizacao_fazenda",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "proprietario",
                schema: "diagnose_virtual",
                table: "localizacao_fazenda",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "ponto_referencia",
                schema: "diagnose_virtual",
                table: "localizacao_fazenda",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(70)",
                oldMaxLength: 70);

            migrationBuilder.AlterColumn<string>(
                name: "nome",
                schema: "diagnose_virtual",
                table: "localizacao_fazenda",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "gerente",
                schema: "diagnose_virtual",
                table: "localizacao_fazenda",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "email",
                schema: "diagnose_virtual",
                table: "localizacao_fazenda",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "observacoes",
                schema: "diagnose_virtual",
                table: "dados_lavoura",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(250)",
                oldMaxLength: 250);

            migrationBuilder.AlterColumn<string>(
                name: "nome",
                schema: "diagnose_virtual",
                table: "dados_lavoura",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "mes_ano_plantio",
                schema: "diagnose_virtual",
                table: "dados_lavoura",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "cultivar",
                schema: "diagnose_virtual",
                table: "dados_lavoura",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(30)",
                oldMaxLength: 30);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "nome",
                schema: "diagnose_virtual",
                table: "usuario",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "email",
                schema: "diagnose_virtual",
                table: "usuario",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "cpf",
                schema: "diagnose_virtual",
                table: "usuario",
                type: "character varying(11)",
                maxLength: 11,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "nome_arquivo",
                schema: "diagnose_virtual",
                table: "upload_monitoramento",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "recomendacao",
                schema: "diagnose_virtual",
                table: "problema_monitoramento",
                type: "character varying(140)",
                maxLength: 140,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "descricao",
                schema: "diagnose_virtual",
                table: "problema_monitoramento",
                type: "character varying(140)",
                maxLength: 140,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "telefone",
                schema: "diagnose_virtual",
                table: "localizacao_fazenda",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "proprietario",
                schema: "diagnose_virtual",
                table: "localizacao_fazenda",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "ponto_referencia",
                schema: "diagnose_virtual",
                table: "localizacao_fazenda",
                type: "character varying(70)",
                maxLength: 70,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "nome",
                schema: "diagnose_virtual",
                table: "localizacao_fazenda",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "gerente",
                schema: "diagnose_virtual",
                table: "localizacao_fazenda",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "email",
                schema: "diagnose_virtual",
                table: "localizacao_fazenda",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "observacoes",
                schema: "diagnose_virtual",
                table: "dados_lavoura",
                type: "character varying(250)",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "nome",
                schema: "diagnose_virtual",
                table: "dados_lavoura",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "mes_ano_plantio",
                schema: "diagnose_virtual",
                table: "dados_lavoura",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "cultivar",
                schema: "diagnose_virtual",
                table: "dados_lavoura",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
