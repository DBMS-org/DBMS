using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDrillPointExplosiveProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "ANFO",
                table: "DrillPoints",
                type: "float(18)",
                precision: 18,
                scale: 6,
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Emulsion",
                table: "DrillPoints",
                type: "float(18)",
                precision: 18,
                scale: 6,
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Subdrill",
                table: "DrillPoints",
                type: "float(18)",
                precision: 18,
                scale: 6,
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Volume",
                table: "DrillPoints",
                type: "float(18)",
                precision: 18,
                scale: 6,
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(9078), new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(9078) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(9084), new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(9085) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(9087), new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(9088) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(9090), new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(9090) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(9092), new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(9092) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(9094), new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(9095) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(9096), new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(9097) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(9099), new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(9100) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(9207), new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(9208) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(9215), new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(9215) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(9218), new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(9218) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(9220), new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(9222) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(9224), new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(9224) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(9226), new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(9226) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(9228), new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(9229) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(9231), new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(9231) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(9233), new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(9233) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(9235), new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(9235) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(9237), new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(9238) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 1,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(9142));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 2,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(9145));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 3,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(9147));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 4,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(9148));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 5,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(9150));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 6,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(9151));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 7,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(9153));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 8,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(9154));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 9,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(9156));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(8826), new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(8829) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(8836), new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(8836) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(8839), new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(8839) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(8841), new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(8842) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(8844), new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(8844) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(8846), new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(8847) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(8848), new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(8849) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ANFO",
                table: "DrillPoints");

            migrationBuilder.DropColumn(
                name: "Emulsion",
                table: "DrillPoints");

            migrationBuilder.DropColumn(
                name: "Subdrill",
                table: "DrillPoints");

            migrationBuilder.DropColumn(
                name: "Volume",
                table: "DrillPoints");

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
    }
}
