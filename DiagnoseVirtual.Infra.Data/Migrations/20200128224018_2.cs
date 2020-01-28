using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

namespace DiagnoseVirtual.Infra.Data.Migrations
{
    public partial class _2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Geometry>(
                name: "demarcacao_geom",
                schema: "diagnose_virtual",
                table: "lavoura",
                nullable: true,
                oldClrType: typeof(Geometry),
                oldType: "geometry");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Geometry>(
                name: "demarcacao_geom",
                schema: "diagnose_virtual",
                table: "lavoura",
                type: "geometry",
                nullable: false,
                oldClrType: typeof(Geometry),
                oldNullable: true);
        }
    }
}
