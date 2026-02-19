using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DUNES.API.Migrations
{
    /// <inheritdoc />
    public partial class mig202602190920 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyClientInventoryCategories_companyClient_CompanyClientId",
                table: "CompanyClientInventoryCategories");


            migrationBuilder.DropForeignKey(
    name: "FK_CompanyClientInventoryCategories_CompaniesContract_CompaniesContractId",
    table: "CompanyClientInventoryCategories");

            // 1) Drop legacy index that depends on CompaniesContractId
            migrationBuilder.DropIndex(
                name: "IX_CompanyClientInventoryCategories_CompaniesContractId",
                table: "CompanyClientInventoryCategories");


            migrationBuilder.CreateIndex(
                name: "UX_CCInvCat_Company_Client_Category",
                table: "CompanyClientInventoryCategories",
                columns: new[] { "CompanyId", "CompanyClientId", "InventoryCategoryId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyClientInventoryCategories_companyClient_CompanyClientId",
                table: "CompanyClientInventoryCategories",
                column: "CompanyClientId",
                principalTable: "companyClient",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyClientInventoryCategories_company_CompanyId",
                table: "CompanyClientInventoryCategories",
                column: "CompanyId",
                principalTable: "company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);


            migrationBuilder.DropColumn(
    name: "CompaniesContractId",
    table: "CompanyClientInventoryCategories");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyClientInventoryCategories_companyClient_CompanyClientId",
                table: "CompanyClientInventoryCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanyClientInventoryCategories_company_CompanyId",
                table: "CompanyClientInventoryCategories");

            migrationBuilder.DropIndex(
                name: "UX_CCInvCat_Company_Client_Category",
                table: "CompanyClientInventoryCategories");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyClientInventoryCategories_companyClient_CompanyClientId",
                table: "CompanyClientInventoryCategories",
                column: "CompanyClientId",
                principalTable: "companyClient",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddColumn<int>(
    name: "CompaniesContractId",
    table: "CompanyClientInventoryCategories",
    type: "int",
    nullable: true);
        }
    }
}
