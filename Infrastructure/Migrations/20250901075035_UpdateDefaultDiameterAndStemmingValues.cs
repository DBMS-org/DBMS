using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDefaultDiameterAndStemmingValues : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Stemming",
                table: "PatternSettings",
                type: "float(18)",
                precision: 18,
                scale: 6,
                nullable: false,
                defaultValue: 3.0,
                oldClrType: typeof(double),
                oldType: "float(18)",
                oldPrecision: 18,
                oldScale: 6,
                oldDefaultValue: 2.0);

            migrationBuilder.AlterColumn<double>(
                name: "Diameter",
                table: "PatternSettings",
                type: "float(18)",
                precision: 18,
                scale: 6,
                nullable: false,
                defaultValue: 0.89000000000000001,
                oldClrType: typeof(double),
                oldType: "float(18)",
                oldPrecision: 18,
                oldScale: 6,
                oldDefaultValue: 0.14999999999999999);

            migrationBuilder.AlterColumn<double>(
                name: "Stemming",
                table: "DrillPoints",
                type: "float(18)",
                precision: 18,
                scale: 6,
                nullable: false,
                defaultValue: 3.0,
                oldClrType: typeof(double),
                oldType: "float(18)",
                oldPrecision: 18,
                oldScale: 6,
                oldDefaultValue: 2.0);

            migrationBuilder.AlterColumn<double>(
                name: "Diameter",
                table: "DrillPoints",
                type: "float(18)",
                precision: 18,
                scale: 6,
                nullable: false,
                defaultValue: 0.89000000000000001,
                oldClrType: typeof(double),
                oldType: "float(18)",
                oldPrecision: 18,
                oldScale: 6,
                oldDefaultValue: 0.14999999999999999);

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(8153), new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(8154) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(8160), new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(8160) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(8163), new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(8163) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(8165), new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(8166) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(8168), new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(8168) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(8170), new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(8170) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(8172), new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(8173) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(8174), new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(8175) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(8292), new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(8293) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(8302), new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(8302) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(8305), new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(8305) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(8307), new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(8308) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(8310), new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(8310) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(8313), new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(8313) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(8315), new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(8316) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(8318), new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(8318) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(8320), new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(8320) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(8322), new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(8323) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(8324), new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(8325) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 1,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(8216));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 2,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(8221));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 3,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(8223));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 4,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(8224));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 5,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(8226));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 6,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(8228));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 7,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(8229));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 8,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(8231));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 9,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(8232));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(7915), new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(7920) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(7927), new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(7927) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(7930), new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(7930) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(7932), new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(7933) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(7935), new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(7935) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(7937), new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(7938) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(7940), new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(7940) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Stemming",
                table: "PatternSettings",
                type: "float(18)",
                precision: 18,
                scale: 6,
                nullable: false,
                defaultValue: 2.0,
                oldClrType: typeof(double),
                oldType: "float(18)",
                oldPrecision: 18,
                oldScale: 6,
                oldDefaultValue: 3.0);

            migrationBuilder.AlterColumn<double>(
                name: "Diameter",
                table: "PatternSettings",
                type: "float(18)",
                precision: 18,
                scale: 6,
                nullable: false,
                defaultValue: 0.14999999999999999,
                oldClrType: typeof(double),
                oldType: "float(18)",
                oldPrecision: 18,
                oldScale: 6,
                oldDefaultValue: 0.89000000000000001);

            migrationBuilder.AlterColumn<double>(
                name: "Stemming",
                table: "DrillPoints",
                type: "float(18)",
                precision: 18,
                scale: 6,
                nullable: false,
                defaultValue: 2.0,
                oldClrType: typeof(double),
                oldType: "float(18)",
                oldPrecision: 18,
                oldScale: 6,
                oldDefaultValue: 3.0);

            migrationBuilder.AlterColumn<double>(
                name: "Diameter",
                table: "DrillPoints",
                type: "float(18)",
                precision: 18,
                scale: 6,
                nullable: false,
                defaultValue: 0.14999999999999999,
                oldClrType: typeof(double),
                oldType: "float(18)",
                oldPrecision: 18,
                oldScale: 6,
                oldDefaultValue: 0.89000000000000001);

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 1, 7, 37, 41, 194, DateTimeKind.Utc).AddTicks(9830), new DateTime(2025, 9, 1, 7, 37, 41, 194, DateTimeKind.Utc).AddTicks(9830) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 1, 7, 37, 41, 194, DateTimeKind.Utc).AddTicks(9838), new DateTime(2025, 9, 1, 7, 37, 41, 194, DateTimeKind.Utc).AddTicks(9838) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 1, 7, 37, 41, 194, DateTimeKind.Utc).AddTicks(9842), new DateTime(2025, 9, 1, 7, 37, 41, 194, DateTimeKind.Utc).AddTicks(9843) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 1, 7, 37, 41, 194, DateTimeKind.Utc).AddTicks(9846), new DateTime(2025, 9, 1, 7, 37, 41, 194, DateTimeKind.Utc).AddTicks(9846) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 1, 7, 37, 41, 194, DateTimeKind.Utc).AddTicks(9849), new DateTime(2025, 9, 1, 7, 37, 41, 194, DateTimeKind.Utc).AddTicks(9850) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 1, 7, 37, 41, 194, DateTimeKind.Utc).AddTicks(9852), new DateTime(2025, 9, 1, 7, 37, 41, 194, DateTimeKind.Utc).AddTicks(9853) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 1, 7, 37, 41, 194, DateTimeKind.Utc).AddTicks(9856), new DateTime(2025, 9, 1, 7, 37, 41, 194, DateTimeKind.Utc).AddTicks(9856) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 1, 7, 37, 41, 194, DateTimeKind.Utc).AddTicks(9859), new DateTime(2025, 9, 1, 7, 37, 41, 194, DateTimeKind.Utc).AddTicks(9860) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 1, 7, 37, 41, 195, DateTimeKind.Utc).AddTicks(79), new DateTime(2025, 9, 1, 7, 37, 41, 195, DateTimeKind.Utc).AddTicks(80) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 1, 7, 37, 41, 195, DateTimeKind.Utc).AddTicks(91), new DateTime(2025, 9, 1, 7, 37, 41, 195, DateTimeKind.Utc).AddTicks(91) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 1, 7, 37, 41, 195, DateTimeKind.Utc).AddTicks(95), new DateTime(2025, 9, 1, 7, 37, 41, 195, DateTimeKind.Utc).AddTicks(95) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 1, 7, 37, 41, 195, DateTimeKind.Utc).AddTicks(98), new DateTime(2025, 9, 1, 7, 37, 41, 195, DateTimeKind.Utc).AddTicks(99) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 1, 7, 37, 41, 195, DateTimeKind.Utc).AddTicks(102), new DateTime(2025, 9, 1, 7, 37, 41, 195, DateTimeKind.Utc).AddTicks(103) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 1, 7, 37, 41, 195, DateTimeKind.Utc).AddTicks(105), new DateTime(2025, 9, 1, 7, 37, 41, 195, DateTimeKind.Utc).AddTicks(106) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 1, 7, 37, 41, 195, DateTimeKind.Utc).AddTicks(109), new DateTime(2025, 9, 1, 7, 37, 41, 195, DateTimeKind.Utc).AddTicks(109) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 1, 7, 37, 41, 195, DateTimeKind.Utc).AddTicks(112), new DateTime(2025, 9, 1, 7, 37, 41, 195, DateTimeKind.Utc).AddTicks(113) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 1, 7, 37, 41, 195, DateTimeKind.Utc).AddTicks(115), new DateTime(2025, 9, 1, 7, 37, 41, 195, DateTimeKind.Utc).AddTicks(116) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 1, 7, 37, 41, 195, DateTimeKind.Utc).AddTicks(119), new DateTime(2025, 9, 1, 7, 37, 41, 195, DateTimeKind.Utc).AddTicks(119) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 1, 7, 37, 41, 195, DateTimeKind.Utc).AddTicks(122), new DateTime(2025, 9, 1, 7, 37, 41, 195, DateTimeKind.Utc).AddTicks(122) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 1,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 1, 7, 37, 41, 194, DateTimeKind.Utc).AddTicks(9919));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 2,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 1, 7, 37, 41, 194, DateTimeKind.Utc).AddTicks(9924));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 3,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 1, 7, 37, 41, 194, DateTimeKind.Utc).AddTicks(9926));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 4,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 1, 7, 37, 41, 194, DateTimeKind.Utc).AddTicks(9928));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 5,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 1, 7, 37, 41, 194, DateTimeKind.Utc).AddTicks(9930));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 6,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 1, 7, 37, 41, 194, DateTimeKind.Utc).AddTicks(9932));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 7,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 1, 7, 37, 41, 194, DateTimeKind.Utc).AddTicks(9935));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 8,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 1, 7, 37, 41, 194, DateTimeKind.Utc).AddTicks(9937));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 9,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 1, 7, 37, 41, 194, DateTimeKind.Utc).AddTicks(9939));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 1, 7, 37, 41, 194, DateTimeKind.Utc).AddTicks(9513), new DateTime(2025, 9, 1, 7, 37, 41, 194, DateTimeKind.Utc).AddTicks(9516) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 1, 7, 37, 41, 194, DateTimeKind.Utc).AddTicks(9525), new DateTime(2025, 9, 1, 7, 37, 41, 194, DateTimeKind.Utc).AddTicks(9525) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 1, 7, 37, 41, 194, DateTimeKind.Utc).AddTicks(9529), new DateTime(2025, 9, 1, 7, 37, 41, 194, DateTimeKind.Utc).AddTicks(9529) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 1, 7, 37, 41, 194, DateTimeKind.Utc).AddTicks(9532), new DateTime(2025, 9, 1, 7, 37, 41, 194, DateTimeKind.Utc).AddTicks(9533) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 1, 7, 37, 41, 194, DateTimeKind.Utc).AddTicks(9536), new DateTime(2025, 9, 1, 7, 37, 41, 194, DateTimeKind.Utc).AddTicks(9536) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 1, 7, 37, 41, 194, DateTimeKind.Utc).AddTicks(9539), new DateTime(2025, 9, 1, 7, 37, 41, 194, DateTimeKind.Utc).AddTicks(9540) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 1, 7, 37, 41, 194, DateTimeKind.Utc).AddTicks(9543), new DateTime(2025, 9, 1, 7, 37, 41, 194, DateTimeKind.Utc).AddTicks(9543) });
        }
    }
}
