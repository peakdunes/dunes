using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DUNES.API.Migrations
{
    /// <inheritdoc />
    public partial class mig202501260840 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropForeignKey(
    name: "FK_racks_companyClient_Idcompanyclient",
    table: "racks");

            migrationBuilder.DropIndex(
                name: "IX_racks_Idcompanyclient",
                table: "racks");


            migrationBuilder.DropColumn(
                name: "Idcompanyclient",
                table: "racks");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "racks",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Idlocation",
                table: "racks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdlocationNavigationId",
                table: "racks",
                type: "int",
                nullable: false,
                defaultValue: 0);

          


            migrationBuilder.CreateIndex(
                name: "IX_racks_IdlocationNavigationId",
                table: "racks",
                column: "IdlocationNavigationId");

         


         

            migrationBuilder.AddForeignKey(
                name: "FK_racks_locations_IdlocationNavigationId",
                table: "racks",
                column: "IdlocationNavigationId",
                principalTable: "locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
         
            migrationBuilder.DropForeignKey(
                name: "FK_racks_locations_IdlocationNavigationId",
                table: "racks");

            migrationBuilder.DropIndex(
                name: "IX_racks_IdlocationNavigationId",
                table: "racks");

       

            migrationBuilder.DropColumn(
                name: "Idlocation",
                table: "racks");

            migrationBuilder.DropColumn(
                name: "IdlocationNavigationId",
                table: "racks");

         

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "racks",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<string>(
                name: "Idcompanyclient",
                table: "racks",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

        

     
        }
    }
}
