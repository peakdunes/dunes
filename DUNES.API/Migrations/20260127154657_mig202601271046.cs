using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DUNES.API.Migrations
{
    /// <inheritdoc />
    public partial class mig202601271046 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RacksId",
                table: "bines",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_bines_RacksId",
                table: "bines",
                column: "RacksId");

            migrationBuilder.AddForeignKey(
                name: "FK_bines_racks_RacksId",
                table: "bines",
                column: "RacksId",
                principalTable: "racks",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_bines_racks_RacksId",
                table: "bines");

            migrationBuilder.DropIndex(
                name: "IX_bines_RacksId",
                table: "bines");

            migrationBuilder.DropColumn(
                name: "RacksId",
                table: "bines");
        }
    }
}
