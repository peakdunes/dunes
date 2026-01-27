using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DUNES.API.Migrations
{
    /// <inheritdoc />
    public partial class mig202601270858 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

          
    migrationBuilder.DropForeignKey(
        name: "FK_bines_companyClient_Idcompanyclient",
        table: "bines");

            
            migrationBuilder.DropIndex(
                name: "IX_bines_Idcompanyclient",
                table: "bines");


            migrationBuilder.DropForeignKey(
                name: "FK_inventorydetail_bines_Idbin",
                table: "inventorydetail");

            migrationBuilder.DropForeignKey(
                name: "FK_inventorymovement_bines_Idbin",
                table: "inventorymovement");

            migrationBuilder.DropForeignKey(
                name: "FK_inventorytransactionDetail_bines_Idbin",
                table: "inventorytransactionDetail");

            migrationBuilder.DropColumn(
                name: "Idcompanyclient",
                table: "bines");

            migrationBuilder.DropColumn(
                name: "Observations",
                table: "bines");

            migrationBuilder.DropColumn(
                name: "TagName",
                table: "bines");

            migrationBuilder.DropColumn(
                name: "include_in_consumption",
                table: "bines");

            migrationBuilder.AddColumn<int>(
                name: "IdbinNavigationId",
                table: "inventorytransactionDetail",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdbinNavigationId",
                table: "inventorymovement",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdbinNavigationId",
                table: "inventorydetail",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LocationsId",
                table: "bines",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "bines",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_inventorytransactionDetail_IdbinNavigationId",
                table: "inventorytransactionDetail",
                column: "IdbinNavigationId");

            migrationBuilder.CreateIndex(
                name: "IX_inventorymovement_IdbinNavigationId",
                table: "inventorymovement",
                column: "IdbinNavigationId");

            migrationBuilder.CreateIndex(
                name: "IX_inventorydetail_IdbinNavigationId",
                table: "inventorydetail",
                column: "IdbinNavigationId");

            migrationBuilder.CreateIndex(
                name: "IX_bines_LocationsId",
                table: "bines",
                column: "LocationsId");

            migrationBuilder.AddForeignKey(
                name: "FK_bines_locations_LocationsId",
                table: "bines",
                column: "LocationsId",
                principalTable: "locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_inventorydetail_bines_IdbinNavigationId",
                table: "inventorydetail",
                column: "IdbinNavigationId",
                principalTable: "bines",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_inventorymovement_bines_IdbinNavigationId",
                table: "inventorymovement",
                column: "IdbinNavigationId",
                principalTable: "bines",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_inventorytransactionDetail_bines_IdbinNavigationId",
                table: "inventorytransactionDetail",
                column: "IdbinNavigationId",
                principalTable: "bines",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_bines_locations_LocationsId",
                table: "bines");

            migrationBuilder.DropForeignKey(
                name: "FK_inventorydetail_bines_IdbinNavigationId",
                table: "inventorydetail");

            migrationBuilder.DropForeignKey(
                name: "FK_inventorymovement_bines_IdbinNavigationId",
                table: "inventorymovement");

            migrationBuilder.DropForeignKey(
                name: "FK_inventorytransactionDetail_bines_IdbinNavigationId",
                table: "inventorytransactionDetail");

            migrationBuilder.DropIndex(
                name: "IX_inventorytransactionDetail_IdbinNavigationId",
                table: "inventorytransactionDetail");

            migrationBuilder.DropIndex(
                name: "IX_inventorymovement_IdbinNavigationId",
                table: "inventorymovement");

            migrationBuilder.DropIndex(
                name: "IX_inventorydetail_IdbinNavigationId",
                table: "inventorydetail");

            migrationBuilder.DropIndex(
                name: "IX_bines_LocationsId",
                table: "bines");

            migrationBuilder.DropColumn(
                name: "IdbinNavigationId",
                table: "inventorytransactionDetail");

            migrationBuilder.DropColumn(
                name: "IdbinNavigationId",
                table: "inventorymovement");

            migrationBuilder.DropColumn(
                name: "IdbinNavigationId",
                table: "inventorydetail");

            migrationBuilder.DropColumn(
                name: "LocationsId",
                table: "bines");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "bines");

            migrationBuilder.AddColumn<string>(
                name: "Idcompanyclient",
                table: "bines",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Observations",
                table: "bines",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TagName",
                table: "bines",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "include_in_consumption",
                table: "bines",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_inventorydetail_bines_Idbin",
                table: "inventorydetail",
                column: "Idbin",
                principalTable: "bines",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_inventorymovement_bines_Idbin",
                table: "inventorymovement",
                column: "Idbin",
                principalTable: "bines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_inventorytransactionDetail_bines_Idbin",
                table: "inventorytransactionDetail",
                column: "Idbin",
                principalTable: "bines",
                principalColumn: "Id");
        }
    }
}
