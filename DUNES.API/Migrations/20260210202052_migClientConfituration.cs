using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DUNES.API.Migrations
{
    /// <inheritdoc />
    public partial class migClientConfituration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompaniesContract_companyClient_CompanyClientId",
                table: "CompaniesContract");

            migrationBuilder.DropForeignKey(
                name: "FK_CompaniesContract_company_CompanyId",
                table: "CompaniesContract");

            migrationBuilder.DropIndex(
                name: "IX_CompaniesContract_CompanyId",
                table: "CompaniesContract");

            migrationBuilder.CreateTable(
                name: "CompanyClientInventoryCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompaniesContractId = table.Column<int>(type: "int", nullable: false),
                    InventoryCategoryId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyClientInventoryCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyClientInventoryCategories_CompaniesContract_CompaniesContractId",
                        column: x => x.CompaniesContractId,
                        principalTable: "CompaniesContract",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompanyClientInventoryCategories_inventorycategories_InventoryCategoryId",
                        column: x => x.InventoryCategoryId,
                        principalTable: "inventorycategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CompanyClientInventoryTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompaniesContractId = table.Column<int>(type: "int", nullable: false),
                    InventoryTypeId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyClientInventoryTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyClientInventoryTypes_CompaniesContract_CompaniesContractId",
                        column: x => x.CompaniesContractId,
                        principalTable: "CompaniesContract",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompanyClientInventoryTypes_inventoryTypes_InventoryTypeId",
                        column: x => x.InventoryTypeId,
                        principalTable: "inventoryTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CompanyClientItemStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompaniesContractId = table.Column<int>(type: "int", nullable: false),
                    ItemStatusId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyClientItemStatuses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyClientItemStatuses_CompaniesContract_CompaniesContractId",
                        column: x => x.CompaniesContractId,
                        principalTable: "CompaniesContract",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompanyClientItemStatuses_itemstatus_ItemStatusId",
                        column: x => x.ItemStatusId,
                        principalTable: "itemstatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompaniesContract_CompanyId_CompanyClientId",
                table: "CompaniesContract",
                columns: new[] { "CompanyId", "CompanyClientId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CompanyClientInventoryCategories_CompaniesContractId_InventoryCategoryId",
                table: "CompanyClientInventoryCategories",
                columns: new[] { "CompaniesContractId", "InventoryCategoryId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CompanyClientInventoryCategories_CompaniesContractId_IsActive",
                table: "CompanyClientInventoryCategories",
                columns: new[] { "CompaniesContractId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_CompanyClientInventoryCategories_InventoryCategoryId",
                table: "CompanyClientInventoryCategories",
                column: "InventoryCategoryId");

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
                name: "IX_CompanyClientInventoryTypes_InventoryTypeId",
                table: "CompanyClientInventoryTypes",
                column: "InventoryTypeId");

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
                name: "IX_CompanyClientItemStatuses_ItemStatusId",
                table: "CompanyClientItemStatuses",
                column: "ItemStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_CompaniesContract_companyClient_CompanyClientId",
                table: "CompaniesContract",
                column: "CompanyClientId",
                principalTable: "companyClient",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CompaniesContract_company_CompanyId",
                table: "CompaniesContract",
                column: "CompanyId",
                principalTable: "company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompaniesContract_companyClient_CompanyClientId",
                table: "CompaniesContract");

            migrationBuilder.DropForeignKey(
                name: "FK_CompaniesContract_company_CompanyId",
                table: "CompaniesContract");

            migrationBuilder.DropTable(
                name: "CompanyClientInventoryCategories");

            migrationBuilder.DropTable(
                name: "CompanyClientInventoryTypes");

            migrationBuilder.DropTable(
                name: "CompanyClientItemStatuses");

            migrationBuilder.DropIndex(
                name: "IX_CompaniesContract_CompanyId_CompanyClientId",
                table: "CompaniesContract");

            migrationBuilder.CreateIndex(
                name: "IX_CompaniesContract_CompanyId",
                table: "CompaniesContract",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_CompaniesContract_companyClient_CompanyClientId",
                table: "CompaniesContract",
                column: "CompanyClientId",
                principalTable: "companyClient",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CompaniesContract_company_CompanyId",
                table: "CompaniesContract",
                column: "CompanyId",
                principalTable: "company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
