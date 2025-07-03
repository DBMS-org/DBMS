using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixPatternSettingsFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatternSettings_Projects_ProjectId",
                table: "PatternSettings");

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

            migrationBuilder.AddForeignKey(
                name: "FK_PatternSettings_Projects_ProjectId",
                table: "PatternSettings",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatternSettings_Projects_ProjectId",
                table: "PatternSettings");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(8894), new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(8895) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(8902), new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(8902) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(8906), new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(8907) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(8910), new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(8910) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(8913), new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(8914) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(8916), new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(8917) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(8920), new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(8921) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(8923), new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(8924) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(9071), new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(9072) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(9079), new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(9079) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(9082), new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(9083) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(9085), new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(9085) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(9087), new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(9088) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(9090), new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(9091) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(9093), new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(9094) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(9096), new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(9097) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(9099), new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(9099) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(9102), new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(9102) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(9104), new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(9105) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 1,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(8993));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 2,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(8998));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 3,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(9000));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 4,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(9002));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 5,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(9004));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 6,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(9006));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 7,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(9008));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 8,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(9010));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 9,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(9012));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(8333), new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(8338) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(8347), new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(8347) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(8351), new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(8352) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(8355), new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(8355) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(8358), new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(8359) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(8361), new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(8362) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(8364), new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(8365) });

            migrationBuilder.AddForeignKey(
                name: "FK_PatternSettings_Projects_ProjectId",
                table: "PatternSettings",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
