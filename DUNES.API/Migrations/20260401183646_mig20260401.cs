using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DUNES.API.Migrations
{
    /// <inheritdoc />
    public partial class mig20260401 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_items_companyClient_CompanyClientId",
                table: "items");

            migrationBuilder.DropForeignKey(
                name: "FK_items_company_companyId",
                table: "items");

            migrationBuilder.DropForeignKey(
                name: "FK_items_inventorycategories_InventorycategoriesId",
                table: "items");

          

            migrationBuilder.DropPrimaryKey(
                name: "PK_items",
                table: "items");

            migrationBuilder.DropIndex(
                name: "IX_items_companyId_sku",
                table: "items");

            migrationBuilder.DropColumn(
                name: "serialnumber",
                table: "items");

            migrationBuilder.RenameTable(
                name: "items",
                newName: "Items");

            migrationBuilder.RenameColumn(
                name: "sku",
                table: "Items",
                newName: "Sku");

            migrationBuilder.RenameColumn(
                name: "itemDescription",
                table: "Items",
                newName: "ItemDescription");

            migrationBuilder.RenameColumn(
                name: "isSerialized",
                table: "Items",
                newName: "IsSerialized");

            migrationBuilder.RenameColumn(
                name: "isRepairable",
                table: "Items",
                newName: "IsRepairable");

            migrationBuilder.RenameColumn(
                name: "companyId",
                table: "Items",
                newName: "CompanyId");

            migrationBuilder.RenameColumn(
                name: "active",
                table: "Items",
                newName: "Active");

            migrationBuilder.RenameColumn(
                name: "InventorycategoriesId",
                table: "Items",
                newName: "InventoryCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_items_CompanyClientId",
                table: "Items",
                newName: "IX_Items_CompanyClientId");

            migrationBuilder.RenameIndex(
                name: "IX_items_InventorycategoriesId",
                table: "Items",
                newName: "IX_Items_InventoryCategoryId");

            migrationBuilder.AlterColumn<string>(
                name: "ItemDescription",
                table: "Items",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CompanyClientId1",
                table: "Items",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PartNumber",
                table: "Items",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

                 

            migrationBuilder.AddPrimaryKey(
                name: "PK_Items",
                table: "Items",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Items_CompanyClientId1",
                table: "Items",
                column: "CompanyClientId1");

            migrationBuilder.CreateIndex(
                name: "IX_Items_CompanyId",
                table: "Items",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_PartNumber",
                table: "Items",
                column: "PartNumber",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_companyClient_CompanyClientId",
                table: "Items",
                column: "CompanyClientId",
                principalTable: "companyClient",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_companyClient_CompanyClientId1",
                table: "Items",
                column: "CompanyClientId1",
                principalTable: "companyClient",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_company_CompanyId",
                table: "Items",
                column: "CompanyId",
                principalTable: "company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_inventorycategories_InventoryCategoryId",
                table: "Items",
                column: "InventoryCategoryId",
                principalTable: "inventorycategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_companyClient_CompanyClientId",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_companyClient_CompanyClientId1",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_company_CompanyId",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_inventorycategories_InventoryCategoryId",
                table: "Items");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Items",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_CompanyClientId1",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_CompanyId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_PartNumber",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "CompanyClientId1",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "PartNumber",
                table: "Items");

            migrationBuilder.RenameTable(
                name: "Items",
                newName: "items");

            migrationBuilder.RenameColumn(
                name: "Sku",
                table: "items",
                newName: "sku");

            migrationBuilder.RenameColumn(
                name: "ItemDescription",
                table: "items",
                newName: "itemDescription");

            migrationBuilder.RenameColumn(
                name: "IsSerialized",
                table: "items",
                newName: "isSerialized");

            migrationBuilder.RenameColumn(
                name: "IsRepairable",
                table: "items",
                newName: "isRepairable");

            migrationBuilder.RenameColumn(
                name: "CompanyId",
                table: "items",
                newName: "companyId");

            migrationBuilder.RenameColumn(
                name: "Active",
                table: "items",
                newName: "active");

            migrationBuilder.RenameColumn(
                name: "InventoryCategoryId",
                table: "items",
                newName: "InventorycategoriesId");

            migrationBuilder.RenameIndex(
                name: "IX_Items_CompanyClientId",
                table: "items",
                newName: "IX_items_CompanyClientId");

            migrationBuilder.RenameIndex(
                name: "IX_Items_InventoryCategoryId",
                table: "items",
                newName: "IX_items_InventorycategoriesId");

            migrationBuilder.AlterColumn<string>(
                name: "itemDescription",
                table: "items",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AddColumn<string>(
                name: "serialnumber",
                table: "items",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_items",
                table: "items",
                column: "Id");

         
            migrationBuilder.CreateIndex(
                name: "IX_items_companyId_sku",
                table: "items",
                columns: new[] { "companyId", "sku" },
                unique: true,
                filter: "[sku] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_items_companyClient_CompanyClientId",
                table: "items",
                column: "CompanyClientId",
                principalTable: "companyClient",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_items_company_companyId",
                table: "items",
                column: "companyId",
                principalTable: "company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_items_inventorycategories_InventorycategoriesId",
                table: "items",
                column: "InventorycategoriesId",
                principalTable: "inventorycategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
