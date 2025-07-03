using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DomainRefactorCleanup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Regions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "ProjectSites",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Projects",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Machines",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(4813), new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(4814) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(4820), new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(4820) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(4823), new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(4824) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(4826), new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(4826) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(4828), new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(4829) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(4831), new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(4831) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(4833), new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(4834) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(4836), new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(4836) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(4963), new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(4963) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(5035), new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(5035) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(5039), new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(5039) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(5041), new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(5042) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(5044), new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(5044) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(5047), new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(5047) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(5049), new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(5050) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(5052), new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(5052) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(5054), new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(5054) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(5056), new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(5057) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(5058), new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(5059) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 1,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(4893));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 2,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(4897));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 3,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(4899));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 4,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(4900));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 5,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(4902));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 6,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(4903));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 7,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(4906));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 8,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(4908));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 9,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(4910));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(4438), new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(4442) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(4449), new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(4450) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(4453), new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(4453) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(4455), new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(4456) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(4458), new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(4458) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(4460), new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(4460) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(4462), new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(4463) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "ProjectSites");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Machines");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Regions",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 8, 52, 20, 76, DateTimeKind.Utc).AddTicks(7228), new DateTime(2025, 7, 2, 8, 52, 20, 76, DateTimeKind.Utc).AddTicks(7228) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 8, 52, 20, 76, DateTimeKind.Utc).AddTicks(7240), new DateTime(2025, 7, 2, 8, 52, 20, 76, DateTimeKind.Utc).AddTicks(7240) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 8, 52, 20, 76, DateTimeKind.Utc).AddTicks(7244), new DateTime(2025, 7, 2, 8, 52, 20, 76, DateTimeKind.Utc).AddTicks(7245) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 8, 52, 20, 76, DateTimeKind.Utc).AddTicks(7248), new DateTime(2025, 7, 2, 8, 52, 20, 76, DateTimeKind.Utc).AddTicks(7248) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 8, 52, 20, 76, DateTimeKind.Utc).AddTicks(7251), new DateTime(2025, 7, 2, 8, 52, 20, 76, DateTimeKind.Utc).AddTicks(7252) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 8, 52, 20, 76, DateTimeKind.Utc).AddTicks(7256), new DateTime(2025, 7, 2, 8, 52, 20, 76, DateTimeKind.Utc).AddTicks(7257) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 8, 52, 20, 76, DateTimeKind.Utc).AddTicks(7259), new DateTime(2025, 7, 2, 8, 52, 20, 76, DateTimeKind.Utc).AddTicks(7260) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 8, 52, 20, 76, DateTimeKind.Utc).AddTicks(7263), new DateTime(2025, 7, 2, 8, 52, 20, 76, DateTimeKind.Utc).AddTicks(7263) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 8, 52, 20, 76, DateTimeKind.Utc).AddTicks(7501), new DateTime(2025, 7, 2, 8, 52, 20, 76, DateTimeKind.Utc).AddTicks(7502) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 8, 52, 20, 76, DateTimeKind.Utc).AddTicks(7518), new DateTime(2025, 7, 2, 8, 52, 20, 76, DateTimeKind.Utc).AddTicks(7519) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 8, 52, 20, 76, DateTimeKind.Utc).AddTicks(7521), new DateTime(2025, 7, 2, 8, 52, 20, 76, DateTimeKind.Utc).AddTicks(7522) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 8, 52, 20, 76, DateTimeKind.Utc).AddTicks(7524), new DateTime(2025, 7, 2, 8, 52, 20, 76, DateTimeKind.Utc).AddTicks(7525) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 8, 52, 20, 76, DateTimeKind.Utc).AddTicks(7527), new DateTime(2025, 7, 2, 8, 52, 20, 76, DateTimeKind.Utc).AddTicks(7528) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 8, 52, 20, 76, DateTimeKind.Utc).AddTicks(7530), new DateTime(2025, 7, 2, 8, 52, 20, 76, DateTimeKind.Utc).AddTicks(7531) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 8, 52, 20, 76, DateTimeKind.Utc).AddTicks(7533), new DateTime(2025, 7, 2, 8, 52, 20, 76, DateTimeKind.Utc).AddTicks(7533) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 8, 52, 20, 76, DateTimeKind.Utc).AddTicks(7536), new DateTime(2025, 7, 2, 8, 52, 20, 76, DateTimeKind.Utc).AddTicks(7536) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 8, 52, 20, 76, DateTimeKind.Utc).AddTicks(7538), new DateTime(2025, 7, 2, 8, 52, 20, 76, DateTimeKind.Utc).AddTicks(7539) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 8, 52, 20, 76, DateTimeKind.Utc).AddTicks(7541), new DateTime(2025, 7, 2, 8, 52, 20, 76, DateTimeKind.Utc).AddTicks(7541) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 8, 52, 20, 76, DateTimeKind.Utc).AddTicks(7544), new DateTime(2025, 7, 2, 8, 52, 20, 76, DateTimeKind.Utc).AddTicks(7544) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 1,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 2, 8, 52, 20, 76, DateTimeKind.Utc).AddTicks(7338));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 2,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 2, 8, 52, 20, 76, DateTimeKind.Utc).AddTicks(7342));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 3,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 2, 8, 52, 20, 76, DateTimeKind.Utc).AddTicks(7344));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 4,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 2, 8, 52, 20, 76, DateTimeKind.Utc).AddTicks(7347));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 5,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 2, 8, 52, 20, 76, DateTimeKind.Utc).AddTicks(7429));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 6,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 2, 8, 52, 20, 76, DateTimeKind.Utc).AddTicks(7431));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 7,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 2, 8, 52, 20, 76, DateTimeKind.Utc).AddTicks(7433));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 8,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 2, 8, 52, 20, 76, DateTimeKind.Utc).AddTicks(7435));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 9,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 2, 8, 52, 20, 76, DateTimeKind.Utc).AddTicks(7437));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 8, 52, 20, 76, DateTimeKind.Utc).AddTicks(6626), new DateTime(2025, 7, 2, 8, 52, 20, 76, DateTimeKind.Utc).AddTicks(6634) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 8, 52, 20, 76, DateTimeKind.Utc).AddTicks(6647), new DateTime(2025, 7, 2, 8, 52, 20, 76, DateTimeKind.Utc).AddTicks(6648) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 8, 52, 20, 76, DateTimeKind.Utc).AddTicks(6651), new DateTime(2025, 7, 2, 8, 52, 20, 76, DateTimeKind.Utc).AddTicks(6652) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 8, 52, 20, 76, DateTimeKind.Utc).AddTicks(6655), new DateTime(2025, 7, 2, 8, 52, 20, 76, DateTimeKind.Utc).AddTicks(6655) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 8, 52, 20, 76, DateTimeKind.Utc).AddTicks(6658), new DateTime(2025, 7, 2, 8, 52, 20, 76, DateTimeKind.Utc).AddTicks(6658) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 8, 52, 20, 76, DateTimeKind.Utc).AddTicks(6661), new DateTime(2025, 7, 2, 8, 52, 20, 76, DateTimeKind.Utc).AddTicks(6661) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 8, 52, 20, 76, DateTimeKind.Utc).AddTicks(6664), new DateTime(2025, 7, 2, 8, 52, 20, 76, DateTimeKind.Utc).AddTicks(6665) });
        }
    }
}
