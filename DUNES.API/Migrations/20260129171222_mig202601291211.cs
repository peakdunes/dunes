using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DUNES.API.Migrations
{
    /// <inheritdoc />
    public partial class mig202601291211 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            
            migrationBuilder.DropForeignKey(
                name: "FK_preconsumptionconfiguration_transactionconcepts_conceptid",
                table: "preconsumptionconfiguration");

            
            migrationBuilder.DropIndex(
                name: "IX_preconsumptionconfiguration_conceptid",
                table: "preconsumptionconfiguration");

            // 1️⃣ DROP FK
            migrationBuilder.DropForeignKey(
                name: "FK_transactionconcepts_companyClient_Idcompanyclient",
                table: "transactionconcepts");

            // 2️⃣ DROP INDEX
            migrationBuilder.DropIndex(
                name: "IX_transactionconcepts_Idcompanyclient",
                table: "transactionconcepts");

            // 3️⃣ DROP COLUMN
          


            migrationBuilder.DropForeignKey(
                name: "FK_inventorymovement_transactionconcepts_Idtransactionconcept",
                table: "inventorymovement");

            migrationBuilder.DropForeignKey(
                name: "FK_inventorytransactionHdr_transactionconcepts_Idtransactionconcept",
                table: "inventorytransactionHdr");

            migrationBuilder.DropForeignKey(
                name: "FK_transactionconcepts_company_Idcompany",
                table: "transactionconcepts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_transactionconcepts",
                table: "transactionconcepts");

            migrationBuilder.DropIndex(
                name: "IX_transactionconcepts_Idcompany",
                table: "transactionconcepts");

            migrationBuilder.DropColumn(
                name: "Idcompany",
                table: "transactionconcepts");

            migrationBuilder.DropColumn(
                name: "Idcompanyclient",
                table: "transactionconcepts");

            migrationBuilder.DropColumn(
                name: "callType",
                table: "transactionconcepts");

            migrationBuilder.DropColumn(
                name: "createZebraCall",
                table: "transactionconcepts");

            migrationBuilder.DropColumn(
                name: "createZebraInvTran",
                table: "transactionconcepts");

            migrationBuilder.DropColumn(
                name: "isInternal",
                table: "transactionconcepts");

            migrationBuilder.RenameTable(
                name: "transactionconcepts",
                newName: "Transactionconcepts");

            migrationBuilder.RenameColumn(
                name: "active",
                table: "Transactionconcepts",
                newName: "Active");

            migrationBuilder.RenameColumn(
                name: "zebraInventoryAssociated",
                table: "Transactionconcepts",
                newName: "companyId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Transactionconcepts",
                type: "nvarchar(200)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Observations",
                table: "Transactionconcepts",
                type: "nvarchar(1000)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdtransactionconceptNavigationId",
                table: "inventorytransactionHdr",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdtransactionconceptNavigationId",
                table: "inventorymovement",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Transactionconcepts",
                table: "Transactionconcepts",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Transactionconcepts_companyId",
                table: "Transactionconcepts",
                column: "companyId");

            migrationBuilder.CreateIndex(
                name: "IX_inventorytransactionHdr_IdtransactionconceptNavigationId",
                table: "inventorytransactionHdr",
                column: "IdtransactionconceptNavigationId");

            migrationBuilder.CreateIndex(
                name: "IX_inventorymovement_IdtransactionconceptNavigationId",
                table: "inventorymovement",
                column: "IdtransactionconceptNavigationId");

            migrationBuilder.AddForeignKey(
                name: "FK_inventorymovement_Transactionconcepts_IdtransactionconceptNavigationId",
                table: "inventorymovement",
                column: "IdtransactionconceptNavigationId",
                principalTable: "Transactionconcepts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_inventorytransactionHdr_Transactionconcepts_IdtransactionconceptNavigationId",
                table: "inventorytransactionHdr",
                column: "IdtransactionconceptNavigationId",
                principalTable: "Transactionconcepts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactionconcepts_company_companyId",
                table: "Transactionconcepts",
                column: "companyId",
                principalTable: "company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_inventorymovement_Transactionconcepts_IdtransactionconceptNavigationId",
                table: "inventorymovement");

            migrationBuilder.DropForeignKey(
                name: "FK_inventorytransactionHdr_Transactionconcepts_IdtransactionconceptNavigationId",
                table: "inventorytransactionHdr");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactionconcepts_company_companyId",
                table: "Transactionconcepts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Transactionconcepts",
                table: "Transactionconcepts");

            migrationBuilder.DropIndex(
                name: "IX_Transactionconcepts_companyId",
                table: "Transactionconcepts");

            migrationBuilder.DropIndex(
                name: "IX_inventorytransactionHdr_IdtransactionconceptNavigationId",
                table: "inventorytransactionHdr");

            migrationBuilder.DropIndex(
                name: "IX_inventorymovement_IdtransactionconceptNavigationId",
                table: "inventorymovement");

            migrationBuilder.DropColumn(
                name: "Observations",
                table: "Transactionconcepts");

            migrationBuilder.DropColumn(
                name: "IdtransactionconceptNavigationId",
                table: "inventorytransactionHdr");

            migrationBuilder.DropColumn(
                name: "IdtransactionconceptNavigationId",
                table: "inventorymovement");

            migrationBuilder.RenameTable(
                name: "Transactionconcepts",
                newName: "transactionconcepts");

            migrationBuilder.RenameColumn(
                name: "Active",
                table: "transactionconcepts",
                newName: "active");

            migrationBuilder.RenameColumn(
                name: "companyId",
                table: "transactionconcepts",
                newName: "zebraInventoryAssociated");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "transactionconcepts",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Idcompany",
                table: "transactionconcepts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Idcompanyclient",
                table: "transactionconcepts",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "callType",
                table: "transactionconcepts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "createZebraCall",
                table: "transactionconcepts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "createZebraInvTran",
                table: "transactionconcepts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isInternal",
                table: "transactionconcepts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_transactionconcepts",
                table: "transactionconcepts",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_transactionconcepts_Idcompany",
                table: "transactionconcepts",
                column: "Idcompany");

            migrationBuilder.AddForeignKey(
                name: "FK_inventorymovement_transactionconcepts_Idtransactionconcept",
                table: "inventorymovement",
                column: "Idtransactionconcept",
                principalTable: "transactionconcepts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_inventorytransactionHdr_transactionconcepts_Idtransactionconcept",
                table: "inventorytransactionHdr",
                column: "Idtransactionconcept",
                principalTable: "transactionconcepts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_transactionconcepts_company_Idcompany",
                table: "transactionconcepts",
                column: "Idcompany",
                principalTable: "company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
