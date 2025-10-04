using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MakeCurrentOccupancyComputedProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentOccupancy",
                table: "Stores");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(6723), new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(6724) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(6729), new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(6730) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(6733), new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(6733) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(6735), new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(6736) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(6738), new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(6738) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(6741), new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(6741) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(6743), new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(6744) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(6746), new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(7661) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(7805), new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(7805) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(7813), new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(7814) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(7816), new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(7817) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(7819), new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(7819) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(7822), new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(7822) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(7824), new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(7824) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(7826), new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(7827) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(7829), new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(7829) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(7831), new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(7832) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(7834), new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(7834) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(7836), new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(7836) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 1,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(7728));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 2,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(7732));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 3,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(7734));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 4,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(7736));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 5,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(7737));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 6,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(7739));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 7,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(7740));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 8,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(7742));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 9,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(7743));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(6390), new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(6393) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(6401), new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(6402) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(6405), new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(6405) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(6457), new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(6458) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(6460), new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(6460) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(6462), new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(6463) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(6465), new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(6465) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "CurrentOccupancy",
                table: "Stores",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 2, 10, 24, 13, 346, DateTimeKind.Utc).AddTicks(8196), new DateTime(2025, 10, 2, 10, 24, 13, 346, DateTimeKind.Utc).AddTicks(8196) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 2, 10, 24, 13, 346, DateTimeKind.Utc).AddTicks(8202), new DateTime(2025, 10, 2, 10, 24, 13, 346, DateTimeKind.Utc).AddTicks(8202) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 2, 10, 24, 13, 346, DateTimeKind.Utc).AddTicks(8205), new DateTime(2025, 10, 2, 10, 24, 13, 346, DateTimeKind.Utc).AddTicks(8205) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 2, 10, 24, 13, 346, DateTimeKind.Utc).AddTicks(8208), new DateTime(2025, 10, 2, 10, 24, 13, 346, DateTimeKind.Utc).AddTicks(8208) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 2, 10, 24, 13, 346, DateTimeKind.Utc).AddTicks(8210), new DateTime(2025, 10, 2, 10, 24, 13, 346, DateTimeKind.Utc).AddTicks(8210) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 2, 10, 24, 13, 346, DateTimeKind.Utc).AddTicks(8212), new DateTime(2025, 10, 2, 10, 24, 13, 346, DateTimeKind.Utc).AddTicks(8213) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 2, 10, 24, 13, 346, DateTimeKind.Utc).AddTicks(8214), new DateTime(2025, 10, 2, 10, 24, 13, 346, DateTimeKind.Utc).AddTicks(8215) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 2, 10, 24, 13, 346, DateTimeKind.Utc).AddTicks(8217), new DateTime(2025, 10, 2, 10, 24, 13, 346, DateTimeKind.Utc).AddTicks(8217) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 2, 10, 24, 13, 346, DateTimeKind.Utc).AddTicks(8349), new DateTime(2025, 10, 2, 10, 24, 13, 346, DateTimeKind.Utc).AddTicks(8350) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 2, 10, 24, 13, 346, DateTimeKind.Utc).AddTicks(8359), new DateTime(2025, 10, 2, 10, 24, 13, 346, DateTimeKind.Utc).AddTicks(8360) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 2, 10, 24, 13, 346, DateTimeKind.Utc).AddTicks(8362), new DateTime(2025, 10, 2, 10, 24, 13, 346, DateTimeKind.Utc).AddTicks(8363) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 2, 10, 24, 13, 346, DateTimeKind.Utc).AddTicks(8365), new DateTime(2025, 10, 2, 10, 24, 13, 346, DateTimeKind.Utc).AddTicks(8365) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 2, 10, 24, 13, 346, DateTimeKind.Utc).AddTicks(8368), new DateTime(2025, 10, 2, 10, 24, 13, 346, DateTimeKind.Utc).AddTicks(8368) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 2, 10, 24, 13, 346, DateTimeKind.Utc).AddTicks(8370), new DateTime(2025, 10, 2, 10, 24, 13, 346, DateTimeKind.Utc).AddTicks(8370) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 2, 10, 24, 13, 346, DateTimeKind.Utc).AddTicks(8373), new DateTime(2025, 10, 2, 10, 24, 13, 346, DateTimeKind.Utc).AddTicks(8373) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 2, 10, 24, 13, 346, DateTimeKind.Utc).AddTicks(8375), new DateTime(2025, 10, 2, 10, 24, 13, 346, DateTimeKind.Utc).AddTicks(8375) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 2, 10, 24, 13, 346, DateTimeKind.Utc).AddTicks(8435), new DateTime(2025, 10, 2, 10, 24, 13, 346, DateTimeKind.Utc).AddTicks(8436) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 2, 10, 24, 13, 346, DateTimeKind.Utc).AddTicks(8439), new DateTime(2025, 10, 2, 10, 24, 13, 346, DateTimeKind.Utc).AddTicks(8439) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 2, 10, 24, 13, 346, DateTimeKind.Utc).AddTicks(8441), new DateTime(2025, 10, 2, 10, 24, 13, 346, DateTimeKind.Utc).AddTicks(8442) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 1,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 2, 10, 24, 13, 346, DateTimeKind.Utc).AddTicks(8267));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 2,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 2, 10, 24, 13, 346, DateTimeKind.Utc).AddTicks(8271));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 3,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 2, 10, 24, 13, 346, DateTimeKind.Utc).AddTicks(8273));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 4,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 2, 10, 24, 13, 346, DateTimeKind.Utc).AddTicks(8275));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 5,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 2, 10, 24, 13, 346, DateTimeKind.Utc).AddTicks(8276));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 6,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 2, 10, 24, 13, 346, DateTimeKind.Utc).AddTicks(8278));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 7,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 2, 10, 24, 13, 346, DateTimeKind.Utc).AddTicks(8279));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 8,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 2, 10, 24, 13, 346, DateTimeKind.Utc).AddTicks(8281));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 9,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 2, 10, 24, 13, 346, DateTimeKind.Utc).AddTicks(8282));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 2, 10, 24, 13, 346, DateTimeKind.Utc).AddTicks(7951), new DateTime(2025, 10, 2, 10, 24, 13, 346, DateTimeKind.Utc).AddTicks(7956) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 2, 10, 24, 13, 346, DateTimeKind.Utc).AddTicks(7962), new DateTime(2025, 10, 2, 10, 24, 13, 346, DateTimeKind.Utc).AddTicks(7963) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 2, 10, 24, 13, 346, DateTimeKind.Utc).AddTicks(7965), new DateTime(2025, 10, 2, 10, 24, 13, 346, DateTimeKind.Utc).AddTicks(7966) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 2, 10, 24, 13, 346, DateTimeKind.Utc).AddTicks(7968), new DateTime(2025, 10, 2, 10, 24, 13, 346, DateTimeKind.Utc).AddTicks(7969) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 2, 10, 24, 13, 346, DateTimeKind.Utc).AddTicks(7971), new DateTime(2025, 10, 2, 10, 24, 13, 346, DateTimeKind.Utc).AddTicks(7971) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 2, 10, 24, 13, 346, DateTimeKind.Utc).AddTicks(7973), new DateTime(2025, 10, 2, 10, 24, 13, 346, DateTimeKind.Utc).AddTicks(7974) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 2, 10, 24, 13, 346, DateTimeKind.Utc).AddTicks(7976), new DateTime(2025, 10, 2, 10, 24, 13, 346, DateTimeKind.Utc).AddTicks(7976) });
        }
    }
}
