using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DiagnoseVirtual.Infra.Data.Migrations
{
    public partial class _4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "url",
                schema: "diagnose_virtual",
                table: "upload_monitoramento");

            migrationBuilder.AddColumn<byte[]>(
                name: "arquivo",
                schema: "diagnose_virtual",
                table: "upload_monitoramento",
                nullable: false,
                defaultValue: new byte[] {  });

            migrationBuilder.AddColumn<byte[][]>(
                name: "imagens",
                schema: "diagnose_virtual",
                table: "problema_monitoramento",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "url_pdi",
                schema: "diagnose_virtual",
                table: "monitoramento",
                nullable: true,
                defaultValue: "True");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "arquivo",
                schema: "diagnose_virtual",
                table: "upload_monitoramento");

            migrationBuilder.DropColumn(
                name: "imagens",
                schema: "diagnose_virtual",
                table: "problema_monitoramento");

            migrationBuilder.DropColumn(
                name: "url_pdi",
                schema: "diagnose_virtual",
                table: "monitoramento");

            migrationBuilder.AddColumn<string>(
                name: "url",
                schema: "diagnose_virtual",
                table: "upload_monitoramento",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }
    }
}
