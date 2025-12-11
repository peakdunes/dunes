using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DUNES.API.Migrations
{
    /// <inheritdoc />
    public partial class mig202512111700 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
                name: "FK_companyClient_cities_IdcityB",
                table: "companyClient",
                column: "Idcity",
                principalTable: "cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_companyClient_countries_IdcountryB",
                table: "companyClient",
                column: "Idcountry",
                principalTable: "countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_companyClient_statesCountries_IdstateB",
                table: "companyClient",
                column: "Idstate",
                principalTable: "statesCountries",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_companyClient_cities_IdcityB",
                table: "companyClient");

            migrationBuilder.DropForeignKey(
                name: "FK_companyClient_countries_IdcountryB",
                table: "companyClient");

            migrationBuilder.DropForeignKey(
                name: "FK_companyClient_statesCountries_IdstateB",
                table: "companyClient");
        }
    }
}
