using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DUNES.API.Migrations
{
    /// <inheritdoc />
    public partial class mig202601280718 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_items_company_IdcompanyNavigationId",
                table: "items");

            migrationBuilder.DropIndex(
                name: "IX_items_IdcompanyNavigationId",
                table: "items");

            migrationBuilder.DropColumn(
                name: "IdcompanyNavigationId",
                table: "items");

            migrationBuilder.RenameColumn(
                name: "Idcompany",
                table: "items",
                newName: "companyId");

            migrationBuilder.RenameIndex(
                name: "IX_items_Idcompany_sku",
                table: "items",
                newName: "IX_items_companyId_sku");

            migrationBuilder.AddForeignKey(
                name: "FK_items_company_companyId",
                table: "items",
                column: "companyId",
                principalTable: "company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_items_company_companyId",
                table: "items");

            migrationBuilder.RenameColumn(
                name: "companyId",
                table: "items",
                newName: "Idcompany");

            migrationBuilder.RenameIndex(
                name: "IX_items_companyId_sku",
                table: "items",
                newName: "IX_items_Idcompany_sku");

            migrationBuilder.AddColumn<int>(
                name: "IdcompanyNavigationId",
                table: "items",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_items_IdcompanyNavigationId",
                table: "items",
                column: "IdcompanyNavigationId");

            migrationBuilder.AddForeignKey(
                name: "FK_items_company_IdcompanyNavigationId",
                table: "items",
                column: "IdcompanyNavigationId",
                principalTable: "company",
                principalColumn: "Id");
        }
    }
}
