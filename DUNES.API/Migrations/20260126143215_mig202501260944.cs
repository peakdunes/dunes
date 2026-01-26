using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DUNES.API.Migrations
{
    /// <inheritdoc />
    public partial class mig202501260944 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LocationsId",
                table: "racks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_racks_LocationsId",
                table: "racks",
                column: "LocationsId");

            migrationBuilder.AddForeignKey(
                name: "FK_racks_locations_LocationsId",
                table: "racks",
                column: "LocationsId",
                principalTable: "locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_racks_locations_LocationsId",
                table: "racks");

            migrationBuilder.DropIndex(
                name: "IX_racks_LocationsId",
                table: "racks");

            migrationBuilder.DropColumn(
                name: "LocationsId",
                table: "racks");
        }
    }
}
