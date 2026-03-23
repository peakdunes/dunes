using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DUNES.API.Migrations
{
    /// <inheritdoc />
    public partial class migAddFieldsPermission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.AddColumn<string>(
                name: "ActionName",
                table: "AuthPermissions",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "DisplayOrder",
                table: "AuthPermissions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "GroupName",
                table: "AuthPermissions",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ModuleName",
                table: "AuthPermissions",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            // Opcional pero recomendable:
            // Poblar valores iniciales para registros existentes
            migrationBuilder.Sql(@"
                UPDATE AuthPermissions
                SET GroupName = 'General'
                WHERE ISNULL(GroupName, '') = ''
            ");

            migrationBuilder.Sql(@"
                UPDATE AuthPermissions
                SET ModuleName =
                    CASE
                        WHEN PermissionKey LIKE 'Locations.%' THEN 'Locations'
                        WHEN PermissionKey LIKE 'TransactionConcepts.%' THEN 'Transaction Concepts'
                        WHEN PermissionKey LIKE 'Users.%' THEN 'Users'
                        WHEN PermissionKey LIKE 'Roles.%' THEN 'Roles'
                        ELSE 'General'
                    END
                WHERE ISNULL(ModuleName, '') = ''
            ");

            migrationBuilder.Sql(@"
                UPDATE AuthPermissions
                SET ActionName =
                    CASE
                        WHEN PermissionKey LIKE '%.Access' THEN 'Access'
                        WHEN PermissionKey LIKE '%.Create' THEN 'Create'
                        WHEN PermissionKey LIKE '%.Update' THEN 'Update'
                        WHEN PermissionKey LIKE '%.Delete' THEN 'Delete'
                        WHEN PermissionKey LIKE '%.Read'   THEN 'Read'
                        ELSE 'Custom'
                    END
                WHERE ISNULL(ActionName, '') = ''
            ");

            migrationBuilder.Sql(@"
                UPDATE AuthPermissions
                SET DisplayOrder =
                    CASE
                        WHEN ActionName = 'Access' THEN 1
                        WHEN ActionName = 'Read' THEN 1
                        WHEN ActionName = 'Create' THEN 2
                        WHEN ActionName = 'Update' THEN 3
                        WHEN ActionName = 'Delete' THEN 4
                        ELSE 99
                    END
                WHERE DisplayOrder = 0
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActionName",
                table: "AuthPermissions");

            migrationBuilder.DropColumn(
                name: "DisplayOrder",
                table: "AuthPermissions");

            migrationBuilder.DropColumn(
                name: "GroupName",
                table: "AuthPermissions");

            migrationBuilder.DropColumn(
                name: "ModuleName",
                table: "AuthPermissions");

        }
    }
}
