using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DUNES.API.Migrations
{
    /// <inheritdoc />
    public partial class _2026022312102 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            // 1) Drop FK
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyClientInventoryTypes_CompaniesContract_CompaniesContractId",
                table: "CompanyClientInventoryTypes");

            // 2) Drop index
            migrationBuilder.DropIndex(
                name: "IX_CompanyClientInventoryTypes_CompaniesContractId",
                table: "CompanyClientInventoryTypes");

            // 3) Drop legacy column
            migrationBuilder.DropColumn(
                name: "CompaniesContractId",
                table: "CompanyClientInventoryTypes");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                 name: "CompaniesContractId",
                 table: "CompanyClientInventoryTypes",
                 type: "int",
                 nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CompanyClientInventoryTypes_CompaniesContractId",
                table: "CompanyClientInventoryTypes",
                column: "CompaniesContractId");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyClientInventoryTypes_CompaniesContract_CompaniesContractId",
                table: "CompanyClientInventoryTypes",
                column: "CompaniesContractId",
                principalTable: "CompaniesContract",
                principalColumn: "Id");



        }
    }
}
