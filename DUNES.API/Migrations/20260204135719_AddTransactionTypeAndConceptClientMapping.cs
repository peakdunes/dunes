using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DUNES.API.Migrations
{
    /// <inheritdoc />
    public partial class AddTransactionTypeAndConceptClientMapping : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TransactionConceptClients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    CompanyClientId = table.Column<int>(type: "int", nullable: false),
                    TransactionConceptId = table.Column<int>(type: "int", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionConceptClients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransactionConceptClients_Transactionconcepts_TransactionConceptId",
                        column: x => x.TransactionConceptId,
                        principalTable: "Transactionconcepts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransactionConceptClients_companyClient_CompanyClientId",
                        column: x => x.CompanyClientId,
                        principalTable: "companyClient",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransactionConceptClients_company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TransactionTypeClients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    CompanyClientId = table.Column<int>(type: "int", nullable: false),
                    TransactionTypeId = table.Column<int>(type: "int", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionTypeClients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransactionTypeClients_companyClient_CompanyClientId",
                        column: x => x.CompanyClientId,
                        principalTable: "companyClient",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransactionTypeClients_company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransactionTypeClients_transactiontypes_TransactionTypeId",
                        column: x => x.TransactionTypeId,
                        principalTable: "transactiontypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TransactionConceptClients_CompanyClientId",
                table: "TransactionConceptClients",
                column: "CompanyClientId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionConceptClients_CompanyId_CompanyClientId_TransactionConceptId",
                table: "TransactionConceptClients",
                columns: new[] { "CompanyId", "CompanyClientId", "TransactionConceptId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TransactionConceptClients_TransactionConceptId",
                table: "TransactionConceptClients",
                column: "TransactionConceptId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionTypeClients_CompanyClientId",
                table: "TransactionTypeClients",
                column: "CompanyClientId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionTypeClients_CompanyId_CompanyClientId_TransactionTypeId",
                table: "TransactionTypeClients",
                columns: new[] { "CompanyId", "CompanyClientId", "TransactionTypeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TransactionTypeClients_TransactionTypeId",
                table: "TransactionTypeClients",
                column: "TransactionTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TransactionConceptClients");

            migrationBuilder.DropTable(
                name: "TransactionTypeClients");
        }
    }
}
