using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DUNES.API.Migrations
{
    /// <inheritdoc />
    public partial class mig202501260922 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Idlocation",
                table: "racks",
                newName: "LocationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LocationId",
                table: "racks",
                newName: "Idlocation");
        }
    }
}
