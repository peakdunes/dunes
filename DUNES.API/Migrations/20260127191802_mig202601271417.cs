using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DUNES.API.Migrations
{
    /// <inheritdoc />
    public partial class mig202601271417 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropForeignKey(
    name: "FK_itemstatus_companyClient_Idcompanyclient",
    table: "itemstatus");


            migrationBuilder.DropIndex(
    name: "IX_itemstatus_Idcompanyclient",
    table: "itemstatus");

            migrationBuilder.DropIndex(
    name: "IX_inventoryTypes_Idcompanyclient",
    table: "inventoryTypes");

            //            migrationBuilder.DropForeignKey(
            //name: "IX_inventoryTypes_Idcompanyclient",
            //table: "inventoryTypes");

            migrationBuilder.DropForeignKey(
name: "FK_inventoryTypes_companyClient_Idcompanyclient",
table: "inventoryTypes");

            

            migrationBuilder.DropForeignKey(
                name: "FK_inventorydetail_bines_IdbinNavigationId",
                table: "inventorydetail");

            migrationBuilder.DropForeignKey(
                name: "FK_inventorydetail_inventoryTypes_Idtype",
                table: "inventorydetail");

            migrationBuilder.DropForeignKey(
                name: "FK_inventorydetail_itemstatus_Idstatus",
                table: "inventorydetail");

           

            migrationBuilder.DropForeignKey(
                name: "FK_inventorymovement_bines_IdbinNavigationId",
                table: "inventorymovement");

            migrationBuilder.DropForeignKey(
                name: "FK_inventorymovement_inventoryTypes_Idtype",
                table: "inventorymovement");

            migrationBuilder.DropForeignKey(
                name: "FK_inventorymovement_itemstatus_Idstatus",
                table: "inventorymovement");

          

            migrationBuilder.DropForeignKey(
                name: "FK_inventorytransactionDetail_bines_IdbinNavigationId",
                table: "inventorytransactionDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_inventorytransactionDetail_inventoryTypes_Idtype",
                table: "inventorytransactionDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_inventorytransactionDetail_itemstatus_Idstatus",
                table: "inventorytransactionDetail");

           

            migrationBuilder.DropColumn(
                name: "Idcompanyclient",
                table: "itemstatus");

            migrationBuilder.DropColumn(
                name: "Idcompanyclient",
                table: "inventoryTypes");

            migrationBuilder.DropColumn(
                name: "itemstatusid",
                table: "inventoryTypes");

            migrationBuilder.DropColumn(
                name: "updateitemstatus",
                table: "inventoryTypes");

            
          

         

            migrationBuilder.CreateTable(
                name: "items",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Idcompany = table.Column<int>(type: "int", nullable: false),
                    CompanyClientId = table.Column<int>(type: "int", nullable: true),
                    sku = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    itemDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Barcode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    serialnumber = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    isRepairable = table.Column<bool>(type: "bit", nullable: false),
                    isSerialized = table.Column<bool>(type: "bit", nullable: false),
                    active = table.Column<bool>(type: "bit", nullable: false),
                    IdcompanyNavigationId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_items_companyClient_CompanyClientId",
                        column: x => x.CompanyClientId,
                        principalTable: "companyClient",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_items_company_IdcompanyNavigationId",
                        column: x => x.IdcompanyNavigationId,
                        principalTable: "company",
                        principalColumn: "Id");
                });

       

         

            migrationBuilder.CreateIndex(
                name: "IX_items_CompanyClientId",
                table: "items",
                column: "CompanyClientId");

            migrationBuilder.CreateIndex(
                name: "IX_items_Idcompany_sku",
                table: "items",
                columns: new[] { "Idcompany", "sku" },
                unique: true,
                filter: "[sku] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_items_IdcompanyNavigationId",
                table: "items",
                column: "IdcompanyNavigationId");

       

        
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
          
          
         

            migrationBuilder.DropTable(
                name: "items");

        




            migrationBuilder.AddColumn<string>(
                name: "Idcompanyclient",
                table: "itemstatus",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "tagName",
                table: "itemsbybin",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "itemid",
                table: "itemsbybin",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true,
                oldDefaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Idcompanyclient",
                table: "itemsbybin",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true,
                oldDefaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Idcompanyclient",
                table: "inventoryTypes",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "itemstatusid",
                table: "inventoryTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "updateitemstatus",
                table: "inventoryTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

           
        }
    }
}
