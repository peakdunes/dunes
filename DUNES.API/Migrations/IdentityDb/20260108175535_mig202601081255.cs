using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DUNES.API.Migrations.IdentityDb
{
    /// <inheritdoc />
    public partial class mig202601081255 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Menu",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    level1 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    level2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    level3 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    level4 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    level5 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    roles = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    active = table.Column<bool>(type: "bit", nullable: false),
                    Utility = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    action = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    controller = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    order = table.Column<int>(type: "int", nullable: false),
                    title = table.Column<string>(type: "varchar(200)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menu", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Menu");
        }
    }
}
