using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DUNES.API.Migrations
{
    /// <inheritdoc />
    public partial class mig202602240748 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop table if exists (evita errores por columnas/renames ambiguos)
            migrationBuilder.Sql(@"
IF OBJECT_ID('dbo.CompanyClientItemStatuses', 'U') IS NOT NULL
BEGIN
    DROP TABLE dbo.CompanyClientItemStatuses;
END
");

            // Create clean table
            migrationBuilder.Sql(@"
CREATE TABLE dbo.CompanyClientItemStatuses
(
    Id INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_CompanyClientItemStatuses PRIMARY KEY,
    CompanyId INT NOT NULL,
    CompanyClientId INT NOT NULL,
    ItemStatusId INT NOT NULL,
    IsActive BIT NOT NULL
);
");

            // FKs
            migrationBuilder.Sql(@"
ALTER TABLE dbo.CompanyClientItemStatuses
ADD CONSTRAINT FK_CompanyClientItemStatuses_company_CompanyId
    FOREIGN KEY (CompanyId) REFERENCES dbo.company(Id);
");

            migrationBuilder.Sql(@"
ALTER TABLE dbo.CompanyClientItemStatuses
ADD CONSTRAINT FK_CompanyClientItemStatuses_companyClient_CompanyClientId
    FOREIGN KEY (CompanyClientId) REFERENCES dbo.companyClient(Id);
");

            migrationBuilder.Sql(@"
ALTER TABLE dbo.CompanyClientItemStatuses
ADD CONSTRAINT FK_CompanyClientItemStatuses_itemstatus_ItemStatusId
    FOREIGN KEY (ItemStatusId) REFERENCES dbo.itemstatus(Id);
");

            // Indexes
            migrationBuilder.Sql(@"
CREATE INDEX IX_CompanyClientItemStatuses_CompanyId
ON dbo.CompanyClientItemStatuses(CompanyId);
");

            migrationBuilder.Sql(@"
CREATE INDEX IX_CompanyClientItemStatuses_CompanyClientId
ON dbo.CompanyClientItemStatuses(CompanyClientId);
");

            migrationBuilder.Sql(@"
CREATE INDEX IX_CompanyClientItemStatuses_ItemStatusId
ON dbo.CompanyClientItemStatuses(ItemStatusId);
");

            migrationBuilder.Sql(@"
CREATE UNIQUE INDEX UX_CompanyClientItemStatuses_Company_Client_ItemStatus
ON dbo.CompanyClientItemStatuses(CompanyId, CompanyClientId, ItemStatusId);
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.Sql(@"
IF OBJECT_ID('dbo.CompanyClientItemStatuses', 'U') IS NOT NULL
BEGIN
    DROP TABLE dbo.CompanyClientItemStatuses;
END
");

        }
    }
}
