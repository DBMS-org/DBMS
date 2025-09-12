using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSchema_20250909202829 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 9, 15, 28, 38, 258, DateTimeKind.Utc).AddTicks(2052), new DateTime(2025, 9, 9, 15, 28, 38, 258, DateTimeKind.Utc).AddTicks(2053) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 9, 15, 28, 38, 258, DateTimeKind.Utc).AddTicks(2058), new DateTime(2025, 9, 9, 15, 28, 38, 258, DateTimeKind.Utc).AddTicks(2058) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 9, 15, 28, 38, 258, DateTimeKind.Utc).AddTicks(2061), new DateTime(2025, 9, 9, 15, 28, 38, 258, DateTimeKind.Utc).AddTicks(2061) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 9, 15, 28, 38, 258, DateTimeKind.Utc).AddTicks(2064), new DateTime(2025, 9, 9, 15, 28, 38, 258, DateTimeKind.Utc).AddTicks(2064) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 9, 15, 28, 38, 258, DateTimeKind.Utc).AddTicks(2066), new DateTime(2025, 9, 9, 15, 28, 38, 258, DateTimeKind.Utc).AddTicks(2067) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 9, 15, 28, 38, 258, DateTimeKind.Utc).AddTicks(2069), new DateTime(2025, 9, 9, 15, 28, 38, 258, DateTimeKind.Utc).AddTicks(2069) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 9, 15, 28, 38, 258, DateTimeKind.Utc).AddTicks(2071), new DateTime(2025, 9, 9, 15, 28, 38, 258, DateTimeKind.Utc).AddTicks(2072) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 9, 15, 28, 38, 258, DateTimeKind.Utc).AddTicks(2074), new DateTime(2025, 9, 9, 15, 28, 38, 258, DateTimeKind.Utc).AddTicks(2074) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 9, 15, 28, 38, 258, DateTimeKind.Utc).AddTicks(2181), new DateTime(2025, 9, 9, 15, 28, 38, 258, DateTimeKind.Utc).AddTicks(2181) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 9, 15, 28, 38, 258, DateTimeKind.Utc).AddTicks(2188), new DateTime(2025, 9, 9, 15, 28, 38, 258, DateTimeKind.Utc).AddTicks(2189) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 9, 15, 28, 38, 258, DateTimeKind.Utc).AddTicks(2191), new DateTime(2025, 9, 9, 15, 28, 38, 258, DateTimeKind.Utc).AddTicks(2192) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 9, 15, 28, 38, 258, DateTimeKind.Utc).AddTicks(2194), new DateTime(2025, 9, 9, 15, 28, 38, 258, DateTimeKind.Utc).AddTicks(2194) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 9, 15, 28, 38, 258, DateTimeKind.Utc).AddTicks(2197), new DateTime(2025, 9, 9, 15, 28, 38, 258, DateTimeKind.Utc).AddTicks(2197) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 9, 15, 28, 38, 258, DateTimeKind.Utc).AddTicks(2199), new DateTime(2025, 9, 9, 15, 28, 38, 258, DateTimeKind.Utc).AddTicks(2199) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 9, 15, 28, 38, 258, DateTimeKind.Utc).AddTicks(2202), new DateTime(2025, 9, 9, 15, 28, 38, 258, DateTimeKind.Utc).AddTicks(2202) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 9, 15, 28, 38, 258, DateTimeKind.Utc).AddTicks(2204), new DateTime(2025, 9, 9, 15, 28, 38, 258, DateTimeKind.Utc).AddTicks(2204) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 9, 15, 28, 38, 258, DateTimeKind.Utc).AddTicks(2206), new DateTime(2025, 9, 9, 15, 28, 38, 258, DateTimeKind.Utc).AddTicks(2207) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 9, 15, 28, 38, 258, DateTimeKind.Utc).AddTicks(2348), new DateTime(2025, 9, 9, 15, 28, 38, 258, DateTimeKind.Utc).AddTicks(2349) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 9, 15, 28, 38, 258, DateTimeKind.Utc).AddTicks(2351), new DateTime(2025, 9, 9, 15, 28, 38, 258, DateTimeKind.Utc).AddTicks(2352) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 1,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 9, 15, 28, 38, 258, DateTimeKind.Utc).AddTicks(2120));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 2,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 9, 15, 28, 38, 258, DateTimeKind.Utc).AddTicks(2123));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 3,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 9, 15, 28, 38, 258, DateTimeKind.Utc).AddTicks(2125));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 4,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 9, 15, 28, 38, 258, DateTimeKind.Utc).AddTicks(2127));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 5,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 9, 15, 28, 38, 258, DateTimeKind.Utc).AddTicks(2128));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 6,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 9, 15, 28, 38, 258, DateTimeKind.Utc).AddTicks(2129));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 7,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 9, 15, 28, 38, 258, DateTimeKind.Utc).AddTicks(2131));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 8,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 9, 15, 28, 38, 258, DateTimeKind.Utc).AddTicks(2132));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 9,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 9, 15, 28, 38, 258, DateTimeKind.Utc).AddTicks(2133));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 9, 15, 28, 38, 258, DateTimeKind.Utc).AddTicks(1810), new DateTime(2025, 9, 9, 15, 28, 38, 258, DateTimeKind.Utc).AddTicks(1814) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 9, 15, 28, 38, 258, DateTimeKind.Utc).AddTicks(1821), new DateTime(2025, 9, 9, 15, 28, 38, 258, DateTimeKind.Utc).AddTicks(1821) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 9, 15, 28, 38, 258, DateTimeKind.Utc).AddTicks(1824), new DateTime(2025, 9, 9, 15, 28, 38, 258, DateTimeKind.Utc).AddTicks(1825) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 9, 15, 28, 38, 258, DateTimeKind.Utc).AddTicks(1827), new DateTime(2025, 9, 9, 15, 28, 38, 258, DateTimeKind.Utc).AddTicks(1827) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 9, 15, 28, 38, 258, DateTimeKind.Utc).AddTicks(1829), new DateTime(2025, 9, 9, 15, 28, 38, 258, DateTimeKind.Utc).AddTicks(1830) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 9, 15, 28, 38, 258, DateTimeKind.Utc).AddTicks(1832), new DateTime(2025, 9, 9, 15, 28, 38, 258, DateTimeKind.Utc).AddTicks(1832) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 9, 15, 28, 38, 258, DateTimeKind.Utc).AddTicks(1834), new DateTime(2025, 9, 9, 15, 28, 38, 258, DateTimeKind.Utc).AddTicks(1835) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(9066), new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(9067) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(9071), new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(9072) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(9074), new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(9075) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(9077), new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(9078) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(9080), new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(9080) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(9082), new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(9082) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(9084), new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(9085) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(9087), new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(9087) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(9212), new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(9213) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(9224), new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(9225) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(9227), new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(9228) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(9230), new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(9230) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(9232), new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(9233) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(9235), new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(9235) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(9237), new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(9238) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(9239), new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(9240) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(9242), new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(9242) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(9244), new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(9244) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(9246), new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(9247) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 1,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(9136));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 2,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(9142));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 3,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(9144));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 4,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(9145));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 5,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(9147));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 6,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(9148));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 7,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(9149));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 8,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(9151));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 9,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(9152));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(8770), new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(8774) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(8786), new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(8786) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(8789), new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(8789) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(8792), new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(8792) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(8794), new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(8795) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(8797), new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(8797) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(8799), new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(8799) });
        }
    }
}
