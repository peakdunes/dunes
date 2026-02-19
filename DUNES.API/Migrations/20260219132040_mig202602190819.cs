using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DUNES.API.Migrations
{
    /// <inheritdoc />
    public partial class mig202602190819 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyClientInventoryCategories_CompaniesContract_CompaniesContractId",
                table: "CompanyClientInventoryCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanyClientInventoryTypes_CompaniesContract_CompaniesContractId",
                table: "CompanyClientInventoryTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanyClientItemStatuses_CompaniesContract_CompaniesContractId",
                table: "CompanyClientItemStatuses");

            migrationBuilder.DropIndex(
                name: "IX_CompanyClientItemStatuses_CompaniesContractId_IsActive",
                table: "CompanyClientItemStatuses");

            migrationBuilder.DropIndex(
                name: "IX_CompanyClientItemStatuses_CompaniesContractId_ItemStatusId",
                table: "CompanyClientItemStatuses");

            migrationBuilder.DropIndex(
                name: "IX_CompanyClientInventoryTypes_CompaniesContractId_InventoryTypeId",
                table: "CompanyClientInventoryTypes");

            migrationBuilder.DropIndex(
                name: "IX_CompanyClientInventoryTypes_CompaniesContractId_IsActive",
                table: "CompanyClientInventoryTypes");

            migrationBuilder.DropIndex(
                name: "IX_CompanyClientInventoryCategories_CompaniesContractId_InventoryCategoryId",
                table: "CompanyClientInventoryCategories");

            migrationBuilder.DropIndex(
                name: "IX_CompanyClientInventoryCategories_CompaniesContractId_IsActive",
                table: "CompanyClientInventoryCategories");

            migrationBuilder.AlterColumn<int>(
                name: "CompaniesContractId",
                table: "CompanyClientItemStatuses",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "CompanyClientId",
                table: "CompanyClientItemStatuses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "CompanyClientItemStatuses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "CompaniesContractId",
                table: "CompanyClientInventoryTypes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "CompanyClientId",
                table: "CompanyClientInventoryTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "CompanyClientInventoryTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "CompaniesContractId",
                table: "CompanyClientInventoryCategories",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "CompanyClientId",
                table: "CompanyClientInventoryCategories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "CompanyClientInventoryCategories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CompanyClientItemStatuses_CompaniesContractId",
                table: "CompanyClientItemStatuses",
                column: "CompaniesContractId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyClientItemStatuses_CompanyClientId",
                table: "CompanyClientItemStatuses",
                column: "CompanyClientId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyClientInventoryTypes_CompaniesContractId",
                table: "CompanyClientInventoryTypes",
                column: "CompaniesContractId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyClientInventoryTypes_CompanyClientId",
                table: "CompanyClientInventoryTypes",
                column: "CompanyClientId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyClientInventoryCategories_CompaniesContractId",
                table: "CompanyClientInventoryCategories",
                column: "CompaniesContractId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyClientInventoryCategories_CompanyClientId",
                table: "CompanyClientInventoryCategories",
                column: "CompanyClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyClientInventoryCategories_CompaniesContract_CompaniesContractId",
                table: "CompanyClientInventoryCategories",
                column: "CompaniesContractId",
                principalTable: "CompaniesContract",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyClientInventoryCategories_companyClient_CompanyClientId",
                table: "CompanyClientInventoryCategories",
                column: "CompanyClientId",
                principalTable: "companyClient",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyClientInventoryTypes_CompaniesContract_CompaniesContractId",
                table: "CompanyClientInventoryTypes",
                column: "CompaniesContractId",
                principalTable: "CompaniesContract",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyClientInventoryTypes_companyClient_CompanyClientId",
                table: "CompanyClientInventoryTypes",
                column: "CompanyClientId",
                principalTable: "companyClient",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyClientItemStatuses_CompaniesContract_CompaniesContractId",
                table: "CompanyClientItemStatuses",
                column: "CompaniesContractId",
                principalTable: "CompaniesContract",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyClientItemStatuses_companyClient_CompanyClientId",
                table: "CompanyClientItemStatuses",
                column: "CompanyClientId",
                principalTable: "companyClient",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyClientInventoryCategories_CompaniesContract_CompaniesContractId",
                table: "CompanyClientInventoryCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanyClientInventoryCategories_companyClient_CompanyClientId",
                table: "CompanyClientInventoryCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanyClientInventoryTypes_CompaniesContract_CompaniesContractId",
                table: "CompanyClientInventoryTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanyClientInventoryTypes_companyClient_CompanyClientId",
                table: "CompanyClientInventoryTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanyClientItemStatuses_CompaniesContract_CompaniesContractId",
                table: "CompanyClientItemStatuses");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanyClientItemStatuses_companyClient_CompanyClientId",
                table: "CompanyClientItemStatuses");

            migrationBuilder.DropIndex(
                name: "IX_CompanyClientItemStatuses_CompaniesContractId",
                table: "CompanyClientItemStatuses");

            migrationBuilder.DropIndex(
                name: "IX_CompanyClientItemStatuses_CompanyClientId",
                table: "CompanyClientItemStatuses");

            migrationBuilder.DropIndex(
                name: "IX_CompanyClientInventoryTypes_CompaniesContractId",
                table: "CompanyClientInventoryTypes");

            migrationBuilder.DropIndex(
                name: "IX_CompanyClientInventoryTypes_CompanyClientId",
                table: "CompanyClientInventoryTypes");

            migrationBuilder.DropIndex(
                name: "IX_CompanyClientInventoryCategories_CompaniesContractId",
                table: "CompanyClientInventoryCategories");

            migrationBuilder.DropIndex(
                name: "IX_CompanyClientInventoryCategories_CompanyClientId",
                table: "CompanyClientInventoryCategories");

            migrationBuilder.DropColumn(
                name: "CompanyClientId",
                table: "CompanyClientItemStatuses");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "CompanyClientItemStatuses");

            migrationBuilder.DropColumn(
                name: "CompanyClientId",
                table: "CompanyClientInventoryTypes");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "CompanyClientInventoryTypes");

            migrationBuilder.DropColumn(
                name: "CompanyClientId",
                table: "CompanyClientInventoryCategories");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "CompanyClientInventoryCategories");

            migrationBuilder.AlterColumn<int>(
                name: "CompaniesContractId",
                table: "CompanyClientItemStatuses",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CompaniesContractId",
                table: "CompanyClientInventoryTypes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CompaniesContractId",
                table: "CompanyClientInventoryCategories",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CompanyClientItemStatuses_CompaniesContractId_IsActive",
                table: "CompanyClientItemStatuses",
                columns: new[] { "CompaniesContractId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_CompanyClientItemStatuses_CompaniesContractId_ItemStatusId",
                table: "CompanyClientItemStatuses",
                columns: new[] { "CompaniesContractId", "ItemStatusId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CompanyClientInventoryTypes_CompaniesContractId_InventoryTypeId",
                table: "CompanyClientInventoryTypes",
                columns: new[] { "CompaniesContractId", "InventoryTypeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CompanyClientInventoryTypes_CompaniesContractId_IsActive",
                table: "CompanyClientInventoryTypes",
                columns: new[] { "CompaniesContractId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_CompanyClientInventoryCategories_CompaniesContractId_InventoryCategoryId",
                table: "CompanyClientInventoryCategories",
                columns: new[] { "CompaniesContractId", "InventoryCategoryId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CompanyClientInventoryCategories_CompaniesContractId_IsActive",
                table: "CompanyClientInventoryCategories",
                columns: new[] { "CompaniesContractId", "IsActive" });

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyClientInventoryCategories_CompaniesContract_CompaniesContractId",
                table: "CompanyClientInventoryCategories",
                column: "CompaniesContractId",
                principalTable: "CompaniesContract",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyClientInventoryTypes_CompaniesContract_CompaniesContractId",
                table: "CompanyClientInventoryTypes",
                column: "CompaniesContractId",
                principalTable: "CompaniesContract",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyClientItemStatuses_CompaniesContract_CompaniesContractId",
                table: "CompanyClientItemStatuses",
                column: "CompaniesContractId",
                principalTable: "CompaniesContract",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
