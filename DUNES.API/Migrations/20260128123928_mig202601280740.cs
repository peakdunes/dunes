using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DUNES.API.Migrations
{
    /// <inheritdoc />
    public partial class mig202601280740 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_items_inventorycategories_InventoryCategoryId",
                table: "items");

            migrationBuilder.DropIndex(
                name: "IX_items_InventoryCategoryId",
                table: "items");

            migrationBuilder.DropColumn(
                name: "InventoryCategoryId",
                table: "items");

            migrationBuilder.RenameColumn(
                name: "InventorycategoriestId",
                table: "items",
                newName: "InventorycategoriesId");

            migrationBuilder.CreateIndex(
                name: "IX_items_InventorycategoriesId",
                table: "items",
                column: "InventorycategoriesId");

            migrationBuilder.AddForeignKey(
                name: "FK_items_inventorycategories_InventorycategoriesId",
                table: "items",
                column: "InventorycategoriesId",
                principalTable: "inventorycategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_items_inventorycategories_InventorycategoriesId",
                table: "items");

            migrationBuilder.DropIndex(
                name: "IX_items_InventorycategoriesId",
                table: "items");

            migrationBuilder.RenameColumn(
                name: "InventorycategoriesId",
                table: "items",
                newName: "InventorycategoriestId");

            migrationBuilder.AddColumn<int>(
                name: "InventoryCategoryId",
                table: "items",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_items_InventoryCategoryId",
                table: "items",
                column: "InventoryCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_items_inventorycategories_InventoryCategoryId",
                table: "items",
                column: "InventoryCategoryId",
                principalTable: "inventorycategories",
                principalColumn: "Id");
        }
    }
}
