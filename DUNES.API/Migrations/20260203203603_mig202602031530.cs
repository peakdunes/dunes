using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DUNES.API.Migrations
{
    /// <inheritdoc />
    public partial class mig202602031530 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // FK
            migrationBuilder.DropForeignKey(
                name: "FK_transactiontypes_companyClient_Idcompanyclient",
                table: "transactiontypes");

            // Index
            migrationBuilder.DropIndex(
                name: "IX_transactiontypes_Idcompanyclient",
                table: "transactiontypes");


            migrationBuilder.DropForeignKey(
                name: "FK_inventorymovement_transactiontypes_Idtransactiontype",
                table: "inventorymovement");

            migrationBuilder.DropForeignKey(
                name: "FK_inventorytransactionDetail_transactiontypes_Idtypetransaction",
                table: "inventorytransactionDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_transactiontypes_company_Idcompany",
                table: "transactiontypes");

            migrationBuilder.DropColumn(
                name: "Idcompanyclient",
                table: "transactiontypes");

            migrationBuilder.DropColumn(
                name: "ispreconsumption",
                table: "transactiontypes");

            migrationBuilder.RenameColumn(
                name: "Idcompany",
                table: "transactiontypes",
                newName: "companyId");

            migrationBuilder.RenameIndex(
                name: "IX_transactiontypes_Idcompany",
                table: "transactiontypes",
                newName: "IX_transactiontypes_companyId");

            migrationBuilder.AddColumn<int>(
                name: "IdtypetransactionNavigationId",
                table: "inventorytransactionDetail",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdtransactiontypeNavigationId",
                table: "inventorymovement",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_inventorytransactionDetail_IdtypetransactionNavigationId",
                table: "inventorytransactionDetail",
                column: "IdtypetransactionNavigationId");

            migrationBuilder.CreateIndex(
                name: "IX_inventorymovement_IdtransactiontypeNavigationId",
                table: "inventorymovement",
                column: "IdtransactiontypeNavigationId");

            migrationBuilder.AddForeignKey(
                name: "FK_inventorymovement_transactiontypes_IdtransactiontypeNavigationId",
                table: "inventorymovement",
                column: "IdtransactiontypeNavigationId",
                principalTable: "transactiontypes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_inventorytransactionDetail_transactiontypes_IdtypetransactionNavigationId",
                table: "inventorytransactionDetail",
                column: "IdtypetransactionNavigationId",
                principalTable: "transactiontypes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_transactiontypes_company_companyId",
                table: "transactiontypes",
                column: "companyId",
                principalTable: "company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_inventorymovement_transactiontypes_IdtransactiontypeNavigationId",
                table: "inventorymovement");

            migrationBuilder.DropForeignKey(
                name: "FK_inventorytransactionDetail_transactiontypes_IdtypetransactionNavigationId",
                table: "inventorytransactionDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_transactiontypes_company_companyId",
                table: "transactiontypes");

            migrationBuilder.DropIndex(
                name: "IX_inventorytransactionDetail_IdtypetransactionNavigationId",
                table: "inventorytransactionDetail");

            migrationBuilder.DropIndex(
                name: "IX_inventorymovement_IdtransactiontypeNavigationId",
                table: "inventorymovement");

            migrationBuilder.DropColumn(
                name: "IdtypetransactionNavigationId",
                table: "inventorytransactionDetail");

            migrationBuilder.DropColumn(
                name: "IdtransactiontypeNavigationId",
                table: "inventorymovement");

            migrationBuilder.RenameColumn(
                name: "companyId",
                table: "transactiontypes",
                newName: "Idcompany");

            migrationBuilder.RenameIndex(
                name: "IX_transactiontypes_companyId",
                table: "transactiontypes",
                newName: "IX_transactiontypes_Idcompany");

            migrationBuilder.AddColumn<string>(
                name: "Idcompanyclient",
                table: "transactiontypes",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ispreconsumption",
                table: "transactiontypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_inventorymovement_transactiontypes_Idtransactiontype",
                table: "inventorymovement",
                column: "Idtransactiontype",
                principalTable: "transactiontypes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_inventorytransactionDetail_transactiontypes_Idtypetransaction",
                table: "inventorytransactionDetail",
                column: "Idtypetransaction",
                principalTable: "transactiontypes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_transactiontypes_company_Idcompany",
                table: "transactiontypes",
                column: "Idcompany",
                principalTable: "company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
