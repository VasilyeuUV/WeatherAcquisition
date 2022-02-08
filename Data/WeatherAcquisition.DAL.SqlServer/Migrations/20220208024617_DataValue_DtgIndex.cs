using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeatherAcquisition.DAL.SqlServer.Migrations
{
    public partial class DataValue_DtgIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DtgGetValue",
                table: "Values",
                newName: "GetedValueDtg");

            migrationBuilder.CreateIndex(
                name: "IX_Values_GetedValueDtg",
                table: "Values",
                column: "GetedValueDtg");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Values_GetedValueDtg",
                table: "Values");

            migrationBuilder.RenameColumn(
                name: "GetedValueDtg",
                table: "Values",
                newName: "DtgGetValue");
        }
    }
}
