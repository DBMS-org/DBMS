using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDrillPointCompletionFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CompletedAt",
                table: "DrillPoints",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CompletedByUserId",
                table: "DrillPoints",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "DrillPoints",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 16, 5, 29, 44, 694, DateTimeKind.Utc).AddTicks(3), new DateTime(2025, 10, 16, 5, 29, 44, 694, DateTimeKind.Utc).AddTicks(3) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 16, 5, 29, 44, 694, DateTimeKind.Utc).AddTicks(9), new DateTime(2025, 10, 16, 5, 29, 44, 694, DateTimeKind.Utc).AddTicks(9) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 16, 5, 29, 44, 694, DateTimeKind.Utc).AddTicks(12), new DateTime(2025, 10, 16, 5, 29, 44, 694, DateTimeKind.Utc).AddTicks(12) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 16, 5, 29, 44, 694, DateTimeKind.Utc).AddTicks(15), new DateTime(2025, 10, 16, 5, 29, 44, 694, DateTimeKind.Utc).AddTicks(15) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 16, 5, 29, 44, 694, DateTimeKind.Utc).AddTicks(17), new DateTime(2025, 10, 16, 5, 29, 44, 694, DateTimeKind.Utc).AddTicks(17) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 16, 5, 29, 44, 694, DateTimeKind.Utc).AddTicks(19), new DateTime(2025, 10, 16, 5, 29, 44, 694, DateTimeKind.Utc).AddTicks(20) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 16, 5, 29, 44, 694, DateTimeKind.Utc).AddTicks(21), new DateTime(2025, 10, 16, 5, 29, 44, 694, DateTimeKind.Utc).AddTicks(22) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 16, 5, 29, 44, 694, DateTimeKind.Utc).AddTicks(24), new DateTime(2025, 10, 16, 5, 29, 44, 694, DateTimeKind.Utc).AddTicks(24) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 16, 5, 29, 44, 694, DateTimeKind.Utc).AddTicks(128), new DateTime(2025, 10, 16, 5, 29, 44, 694, DateTimeKind.Utc).AddTicks(128) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 16, 5, 29, 44, 694, DateTimeKind.Utc).AddTicks(138), new DateTime(2025, 10, 16, 5, 29, 44, 694, DateTimeKind.Utc).AddTicks(138) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 16, 5, 29, 44, 694, DateTimeKind.Utc).AddTicks(141), new DateTime(2025, 10, 16, 5, 29, 44, 694, DateTimeKind.Utc).AddTicks(141) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 16, 5, 29, 44, 694, DateTimeKind.Utc).AddTicks(144), new DateTime(2025, 10, 16, 5, 29, 44, 694, DateTimeKind.Utc).AddTicks(144) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 16, 5, 29, 44, 694, DateTimeKind.Utc).AddTicks(147), new DateTime(2025, 10, 16, 5, 29, 44, 694, DateTimeKind.Utc).AddTicks(147) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 16, 5, 29, 44, 694, DateTimeKind.Utc).AddTicks(150), new DateTime(2025, 10, 16, 5, 29, 44, 694, DateTimeKind.Utc).AddTicks(150) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 16, 5, 29, 44, 694, DateTimeKind.Utc).AddTicks(152), new DateTime(2025, 10, 16, 5, 29, 44, 694, DateTimeKind.Utc).AddTicks(153) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 16, 5, 29, 44, 694, DateTimeKind.Utc).AddTicks(155), new DateTime(2025, 10, 16, 5, 29, 44, 694, DateTimeKind.Utc).AddTicks(155) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 16, 5, 29, 44, 694, DateTimeKind.Utc).AddTicks(158), new DateTime(2025, 10, 16, 5, 29, 44, 694, DateTimeKind.Utc).AddTicks(158) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 16, 5, 29, 44, 694, DateTimeKind.Utc).AddTicks(160), new DateTime(2025, 10, 16, 5, 29, 44, 694, DateTimeKind.Utc).AddTicks(160) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 16, 5, 29, 44, 694, DateTimeKind.Utc).AddTicks(163), new DateTime(2025, 10, 16, 5, 29, 44, 694, DateTimeKind.Utc).AddTicks(163) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 1,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 16, 5, 29, 44, 694, DateTimeKind.Utc).AddTicks(68));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 2,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 16, 5, 29, 44, 694, DateTimeKind.Utc).AddTicks(72));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 3,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 16, 5, 29, 44, 694, DateTimeKind.Utc).AddTicks(73));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 4,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 16, 5, 29, 44, 694, DateTimeKind.Utc).AddTicks(75));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 5,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 16, 5, 29, 44, 694, DateTimeKind.Utc).AddTicks(76));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 6,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 16, 5, 29, 44, 694, DateTimeKind.Utc).AddTicks(78));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 7,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 16, 5, 29, 44, 694, DateTimeKind.Utc).AddTicks(79));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 8,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 16, 5, 29, 44, 694, DateTimeKind.Utc).AddTicks(81));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 9,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 16, 5, 29, 44, 694, DateTimeKind.Utc).AddTicks(82));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 16, 5, 29, 44, 693, DateTimeKind.Utc).AddTicks(9704), new DateTime(2025, 10, 16, 5, 29, 44, 693, DateTimeKind.Utc).AddTicks(9706) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 16, 5, 29, 44, 693, DateTimeKind.Utc).AddTicks(9711), new DateTime(2025, 10, 16, 5, 29, 44, 693, DateTimeKind.Utc).AddTicks(9712) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 16, 5, 29, 44, 693, DateTimeKind.Utc).AddTicks(9769), new DateTime(2025, 10, 16, 5, 29, 44, 693, DateTimeKind.Utc).AddTicks(9769) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 16, 5, 29, 44, 693, DateTimeKind.Utc).AddTicks(9771), new DateTime(2025, 10, 16, 5, 29, 44, 693, DateTimeKind.Utc).AddTicks(9772) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 16, 5, 29, 44, 693, DateTimeKind.Utc).AddTicks(9774), new DateTime(2025, 10, 16, 5, 29, 44, 693, DateTimeKind.Utc).AddTicks(9774) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 16, 5, 29, 44, 693, DateTimeKind.Utc).AddTicks(9776), new DateTime(2025, 10, 16, 5, 29, 44, 693, DateTimeKind.Utc).AddTicks(9776) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 16, 5, 29, 44, 693, DateTimeKind.Utc).AddTicks(9778), new DateTime(2025, 10, 16, 5, 29, 44, 693, DateTimeKind.Utc).AddTicks(9779) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompletedAt",
                table: "DrillPoints");

            migrationBuilder.DropColumn(
                name: "CompletedByUserId",
                table: "DrillPoints");

            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "DrillPoints");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(4011), new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(4012) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(4018), new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(4018) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(4021), new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(4021) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(4023), new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(4024) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(4025), new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(4026) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(4028), new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(4028) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(4030), new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(4030) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(4032), new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(4039) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(4186), new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(4187) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(4194), new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(4194) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(4197), new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(4197) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(4199), new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(4199) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(4201), new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(4202) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(4204), new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(4204) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(4206), new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(4207) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(4209), new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(4209) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(4211), new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(4211) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(4213), new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(4213) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(4217), new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(4217) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 1,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(4093));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 2,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(4097));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 3,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(4099));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 4,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(4100));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 5,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(4101));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 6,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(4103));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 7,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(4104));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 8,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(4105));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 9,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(4107));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(3733), new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(3736) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(3742), new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(3743) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(3745), new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(3746) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(3748), new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(3749) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(3751), new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(3751) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(3753), new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(3754) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(3755), new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(3756) });
        }
    }
}
