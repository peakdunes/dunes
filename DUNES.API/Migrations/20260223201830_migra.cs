using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DUNES.API.Migrations
{
    /// <inheritdoc />
    public partial class migra : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyClientItemStatuses_itemstatus_ItemStatusId",
                table: "CompanyClientItemStatuses");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyClientItemStatuses_itemstatus_ItemStatusId",
                table: "CompanyClientItemStatuses",
                column: "ItemStatusId",
                principalTable: "itemstatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

         

            migrationBuilder.DropForeignKey(
                name: "FK_CompanyClientItemStatuses_itemstatus_ItemStatusId",
                table: "CompanyClientItemStatuses");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyClientItemStatuses_itemstatus_ItemStatusId",
                table: "CompanyClientItemStatuses",
                column: "ItemStatusId",
                principalTable: "itemstatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);


        }
    }
}
