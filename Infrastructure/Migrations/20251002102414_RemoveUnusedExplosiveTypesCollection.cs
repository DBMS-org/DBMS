using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUnusedExplosiveTypesCollection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(558), new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(558) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(564), new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(564) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(567), new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(567) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(570), new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(570) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(572), new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(573) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(575), new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(575) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(577), new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(578) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(580), new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(580) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(692), new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(693) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(700), new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(701) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(703), new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(704) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(706), new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(706) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(709), new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(709) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(712), new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(712) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(714), new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(715) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(717), new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(717) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(719), new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(719) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(721), new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(722) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(724), new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(724) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 1,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(626));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 2,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(631));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 3,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(632));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 4,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(634));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 5,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(636));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 6,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(637));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 7,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(638));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 8,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(639));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 9,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(641));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(283), new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(287) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(294), new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(294) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(297), new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(297) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(300), new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(300) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(302), new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(302) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(305), new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(305) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(307), new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(307) });
        }
    }
}
