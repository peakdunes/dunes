using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DUNES.API.Migrations.IdentityDb
{
    /// <inheritdoc />
    public partial class migPermissionData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.AddColumn<string>(
                name: "ButtonCss",
                table: "AuthPermissions",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ButtonOrder",
                table: "AuthPermissions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ButtonText",
                table: "AuthPermissions",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConfirmationMessage",
                table: "AuthPermissions",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

         


            migrationBuilder.AddColumn<string>(
                name: "IconCss",
                table: "AuthPermissions",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

         

            migrationBuilder.AddColumn<string>(
                name: "MvcActionName",
                table: "AuthPermissions",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "RequiresConfirmation",
                table: "AuthPermissions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ShowAsRowAction",
                table: "AuthPermissions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ShowAsToolbarAction",
                table: "AuthPermissions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "TextCss",
                table: "AuthPermissions",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.DropColumn(
                name: "ButtonCss",
                table: "AuthPermissions");

            migrationBuilder.DropColumn(
                name: "ButtonOrder",
                table: "AuthPermissions");

            migrationBuilder.DropColumn(
                name: "ButtonText",
                table: "AuthPermissions");

            migrationBuilder.DropColumn(
                name: "ConfirmationMessage",
                table: "AuthPermissions");

         


            migrationBuilder.DropColumn(
                name: "IconCss",
                table: "AuthPermissions");

          

            migrationBuilder.DropColumn(
                name: "MvcActionName",
                table: "AuthPermissions");

            migrationBuilder.DropColumn(
                name: "RequiresConfirmation",
                table: "AuthPermissions");

            migrationBuilder.DropColumn(
                name: "ShowAsRowAction",
                table: "AuthPermissions");

            migrationBuilder.DropColumn(
                name: "ShowAsToolbarAction",
                table: "AuthPermissions");

            migrationBuilder.DropColumn(
                name: "TextCss",
                table: "AuthPermissions");
        }
    }
}
