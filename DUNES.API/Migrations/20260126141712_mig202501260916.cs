using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DUNES.API.Migrations
{
    /// <inheritdoc />
    public partial class mig202501260916 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_racks_locations_IdlocationNavigationId",
                table: "racks");

            migrationBuilder.RenameColumn(
                name: "IdlocationNavigationId",
                table: "racks",
                newName: "LocationsId");

            migrationBuilder.RenameIndex(
                name: "IX_racks_IdlocationNavigationId",
                table: "racks",
                newName: "IX_racks_LocationsId");

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

            migrationBuilder.RenameColumn(
                name: "LocationsId",
                table: "racks",
                newName: "IdlocationNavigationId");

            migrationBuilder.RenameIndex(
                name: "IX_racks_LocationsId",
                table: "racks",
                newName: "IX_racks_IdlocationNavigationId");

            migrationBuilder.AddForeignKey(
                name: "FK_racks_locations_IdlocationNavigationId",
                table: "racks",
                column: "IdlocationNavigationId",
                principalTable: "locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }
    }
}
