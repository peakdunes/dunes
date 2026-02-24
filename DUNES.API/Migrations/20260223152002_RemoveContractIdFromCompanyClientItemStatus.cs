using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DUNES.API.Migrations
{
    /// <inheritdoc />
    public partial class RemoveContractIdFromCompanyClientItemStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            // 1) Drop FK that depends on CompaniesContractId
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyClientItemStatuses_CompaniesContract_CompaniesContractId",
                table: "CompanyClientItemStatuses");

            // 2) Drop index that depends on CompaniesContractId
            migrationBuilder.DropIndex(
                name: "IX_CompanyClientItemStatuses_CompaniesContractId",
                table: "CompanyClientItemStatuses");

            // 3) Drop legacy column
            migrationBuilder.DropColumn(
                name: "CompaniesContractId",
                table: "CompanyClientItemStatuses");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
        name: "CompaniesContractId",
        table: "CompanyClientItemStatuses",
        type: "int",
        nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CompanyClientItemStatuses_CompaniesContractId",
                table: "CompanyClientItemStatuses",
                column: "CompaniesContractId");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyClientItemStatuses_CompaniesContract_CompaniesContractId",
                table: "CompanyClientItemStatuses",
                column: "CompaniesContractId",
                principalTable: "CompaniesContract",
                principalColumn: "Id");
        }
    }
}
