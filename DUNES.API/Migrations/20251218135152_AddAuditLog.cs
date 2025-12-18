using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DUNES.API.Migrations
{
    /// <inheritdoc />
    public partial class AddAuditLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "AuditLog",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventDateUtc = table.Column<DateTime>(type: "datetime2(3)", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    EventType = table.Column<string>(type: "char(6)", nullable: false),
                    SchemaName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false, defaultValue: "dbo"),
                    TableName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    PrimaryKey = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    TraceId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IpAddress = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AppName = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    CorrelationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Module = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    BusinessKey = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    ChangedColumns = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    JsonOld = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JsonNew = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLog", x => x.Id);
                    table.CheckConstraint("CK_AuditLog_EventType", "EventType IN ('INSERT','UPDATE','DELETE')");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuditLog_BusinessKey_Date",
                schema: "dbo",
                table: "AuditLog",
                columns: new[] { "BusinessKey", "EventDateUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_AuditLog_Module_Date",
                schema: "dbo",
                table: "AuditLog",
                columns: new[] { "Module", "EventDateUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_AuditLog_Table_PK_Date",
                schema: "dbo",
                table: "AuditLog",
                columns: new[] { "SchemaName", "TableName", "PrimaryKey", "EventDateUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_AuditLog_Table_Type_Date",
                schema: "dbo",
                table: "AuditLog",
                columns: new[] { "SchemaName", "TableName", "EventType", "EventDateUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_AuditLog_TraceId",
                schema: "dbo",
                table: "AuditLog",
                column: "TraceId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLog_User_Date",
                schema: "dbo",
                table: "AuditLog",
                columns: new[] { "UserName", "EventDateUtc" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditLog",
                schema: "dbo");
        }
    }
}
