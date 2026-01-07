using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DUNES.API.Migrations.IdentityDb
{
    /// <inheritdoc />
    public partial class mig202601071256 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Isdefault",
                table: "userConfiguration");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Isdefault",
                table: "userConfiguration",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
