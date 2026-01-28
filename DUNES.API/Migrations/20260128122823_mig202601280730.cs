using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DUNES.API.Migrations
{
    /// <inheritdoc />
    public partial class mig202601280730 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
    name: "IX_inventorycategories_Idcompanyclient",
    table: "inventorycategories");

            migrationBuilder.DropForeignKey(
             name: "FK_inventorycategories_companyClient_Idcompanyclient",
             table: "inventorycategories");


            migrationBuilder.DropForeignKey(
                name: "FK_inventorycategories_company_Idcompany",
                table: "inventorycategories");

            migrationBuilder.DropColumn(
                name: "Idcompanyclient",
                table: "inventorycategories");

            migrationBuilder.RenameColumn(
                name: "Idcompany",
                table: "inventorycategories",
                newName: "companyId");

            migrationBuilder.AddColumn<int>(
                name: "InventoryCategoryId",
                table: "items",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InventorycategoriestId",
                table: "items",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_items_InventoryCategoryId",
                table: "items",
                column: "InventoryCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_inventorycategories_company_companyId",
                table: "inventorycategories",
                column: "companyId",
                principalTable: "company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_items_inventorycategories_InventoryCategoryId",
                table: "items",
                column: "InventoryCategoryId",
                principalTable: "inventorycategories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_inventorycategories_company_companyId",
                table: "inventorycategories");

            migrationBuilder.DropForeignKey(
                name: "FK_items_inventorycategories_InventoryCategoryId",
                table: "items");

            migrationBuilder.DropIndex(
                name: "IX_items_InventoryCategoryId",
                table: "items");

            migrationBuilder.DropColumn(
                name: "InventoryCategoryId",
                table: "items");

            migrationBuilder.DropColumn(
                name: "InventorycategoriestId",
                table: "items");

            migrationBuilder.RenameColumn(
                name: "companyId",
                table: "inventorycategories",
                newName: "Idcompany");

            migrationBuilder.AddColumn<string>(
                name: "Idcompanyclient",
                table: "inventorycategories",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_inventorycategories_company_Idcompany",
                table: "inventorycategories",
                column: "Idcompany",
                principalTable: "company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
