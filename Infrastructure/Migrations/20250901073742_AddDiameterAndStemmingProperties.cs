using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDiameterAndStemmingProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Spacing",
                table: "PatternSettings",
                type: "float(18)",
                precision: 18,
                scale: 6,
                nullable: false,
                defaultValue: 3.0,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "Burden",
                table: "PatternSettings",
                type: "float(18)",
                precision: 18,
                scale: 6,
                nullable: false,
                defaultValue: 2.5,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddColumn<double>(
                name: "Diameter",
                table: "PatternSettings",
                type: "float(18)",
                precision: 18,
                scale: 6,
                nullable: false,
                defaultValue: 0.14999999999999999);

            migrationBuilder.AddColumn<double>(
                name: "Stemming",
                table: "PatternSettings",
                type: "float(18)",
                precision: 18,
                scale: 6,
                nullable: false,
                defaultValue: 2.0);

            migrationBuilder.AddColumn<double>(
                name: "Diameter",
                table: "DrillPoints",
                type: "float(18)",
                precision: 18,
                scale: 6,
                nullable: false,
                defaultValue: 0.14999999999999999);

            migrationBuilder.AddColumn<double>(
                name: "Stemming",
                table: "DrillPoints",
                type: "float(18)",
                precision: 18,
                scale: 6,
                nullable: false,
                defaultValue: 2.0);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Diameter",
                table: "PatternSettings");

            migrationBuilder.DropColumn(
                name: "Stemming",
                table: "PatternSettings");

            migrationBuilder.DropColumn(
                name: "Diameter",
                table: "DrillPoints");

            migrationBuilder.DropColumn(
                name: "Stemming",
                table: "DrillPoints");

            migrationBuilder.AlterColumn<double>(
                name: "Spacing",
                table: "PatternSettings",
                type: "float",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float(18)",
                oldPrecision: 18,
                oldScale: 6,
                oldDefaultValue: 3.0);

            migrationBuilder.AlterColumn<double>(
                name: "Burden",
                table: "PatternSettings",
                type: "float",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float(18)",
                oldPrecision: 18,
                oldScale: 6,
                oldDefaultValue: 2.5);

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 10, 14, 10, 21, 95, DateTimeKind.Utc).AddTicks(9845), new DateTime(2025, 7, 10, 14, 10, 21, 95, DateTimeKind.Utc).AddTicks(9845) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 10, 14, 10, 21, 95, DateTimeKind.Utc).AddTicks(9850), new DateTime(2025, 7, 10, 14, 10, 21, 95, DateTimeKind.Utc).AddTicks(9850) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 10, 14, 10, 21, 95, DateTimeKind.Utc).AddTicks(9853), new DateTime(2025, 7, 10, 14, 10, 21, 95, DateTimeKind.Utc).AddTicks(9853) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 10, 14, 10, 21, 95, DateTimeKind.Utc).AddTicks(9855), new DateTime(2025, 7, 10, 14, 10, 21, 95, DateTimeKind.Utc).AddTicks(9855) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 10, 14, 10, 21, 95, DateTimeKind.Utc).AddTicks(9857), new DateTime(2025, 7, 10, 14, 10, 21, 95, DateTimeKind.Utc).AddTicks(9858) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 10, 14, 10, 21, 95, DateTimeKind.Utc).AddTicks(9860), new DateTime(2025, 7, 10, 14, 10, 21, 95, DateTimeKind.Utc).AddTicks(9860) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 10, 14, 10, 21, 95, DateTimeKind.Utc).AddTicks(9862), new DateTime(2025, 7, 10, 14, 10, 21, 95, DateTimeKind.Utc).AddTicks(9862) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 10, 14, 10, 21, 95, DateTimeKind.Utc).AddTicks(9864), new DateTime(2025, 7, 10, 14, 10, 21, 95, DateTimeKind.Utc).AddTicks(9865) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 10, 14, 10, 21, 96, DateTimeKind.Utc).AddTicks(4), new DateTime(2025, 7, 10, 14, 10, 21, 96, DateTimeKind.Utc).AddTicks(4) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 10, 14, 10, 21, 96, DateTimeKind.Utc).AddTicks(14), new DateTime(2025, 7, 10, 14, 10, 21, 96, DateTimeKind.Utc).AddTicks(14) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 10, 14, 10, 21, 96, DateTimeKind.Utc).AddTicks(17), new DateTime(2025, 7, 10, 14, 10, 21, 96, DateTimeKind.Utc).AddTicks(17) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 10, 14, 10, 21, 96, DateTimeKind.Utc).AddTicks(19), new DateTime(2025, 7, 10, 14, 10, 21, 96, DateTimeKind.Utc).AddTicks(20) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 10, 14, 10, 21, 96, DateTimeKind.Utc).AddTicks(68), new DateTime(2025, 7, 10, 14, 10, 21, 96, DateTimeKind.Utc).AddTicks(69) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 10, 14, 10, 21, 96, DateTimeKind.Utc).AddTicks(71), new DateTime(2025, 7, 10, 14, 10, 21, 96, DateTimeKind.Utc).AddTicks(72) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 10, 14, 10, 21, 96, DateTimeKind.Utc).AddTicks(74), new DateTime(2025, 7, 10, 14, 10, 21, 96, DateTimeKind.Utc).AddTicks(74) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 10, 14, 10, 21, 96, DateTimeKind.Utc).AddTicks(76), new DateTime(2025, 7, 10, 14, 10, 21, 96, DateTimeKind.Utc).AddTicks(77) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 10, 14, 10, 21, 96, DateTimeKind.Utc).AddTicks(79), new DateTime(2025, 7, 10, 14, 10, 21, 96, DateTimeKind.Utc).AddTicks(79) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 10, 14, 10, 21, 96, DateTimeKind.Utc).AddTicks(81), new DateTime(2025, 7, 10, 14, 10, 21, 96, DateTimeKind.Utc).AddTicks(82) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 10, 14, 10, 21, 96, DateTimeKind.Utc).AddTicks(84), new DateTime(2025, 7, 10, 14, 10, 21, 96, DateTimeKind.Utc).AddTicks(84) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 1,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 10, 14, 10, 21, 95, DateTimeKind.Utc).AddTicks(9918));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 2,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 10, 14, 10, 21, 95, DateTimeKind.Utc).AddTicks(9921));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 3,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 10, 14, 10, 21, 95, DateTimeKind.Utc).AddTicks(9923));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 4,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 10, 14, 10, 21, 95, DateTimeKind.Utc).AddTicks(9925));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 5,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 10, 14, 10, 21, 95, DateTimeKind.Utc).AddTicks(9928));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 6,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 10, 14, 10, 21, 95, DateTimeKind.Utc).AddTicks(9931));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 7,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 10, 14, 10, 21, 95, DateTimeKind.Utc).AddTicks(9933));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 8,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 10, 14, 10, 21, 95, DateTimeKind.Utc).AddTicks(9937));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 9,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 10, 14, 10, 21, 95, DateTimeKind.Utc).AddTicks(9939));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 10, 14, 10, 21, 95, DateTimeKind.Utc).AddTicks(9591), new DateTime(2025, 7, 10, 14, 10, 21, 95, DateTimeKind.Utc).AddTicks(9594) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 10, 14, 10, 21, 95, DateTimeKind.Utc).AddTicks(9601), new DateTime(2025, 7, 10, 14, 10, 21, 95, DateTimeKind.Utc).AddTicks(9601) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 10, 14, 10, 21, 95, DateTimeKind.Utc).AddTicks(9604), new DateTime(2025, 7, 10, 14, 10, 21, 95, DateTimeKind.Utc).AddTicks(9604) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 10, 14, 10, 21, 95, DateTimeKind.Utc).AddTicks(9607), new DateTime(2025, 7, 10, 14, 10, 21, 95, DateTimeKind.Utc).AddTicks(9607) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 10, 14, 10, 21, 95, DateTimeKind.Utc).AddTicks(9609), new DateTime(2025, 7, 10, 14, 10, 21, 95, DateTimeKind.Utc).AddTicks(9610) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 10, 14, 10, 21, 95, DateTimeKind.Utc).AddTicks(9612), new DateTime(2025, 7, 10, 14, 10, 21, 95, DateTimeKind.Utc).AddTicks(9612) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 10, 14, 10, 21, 95, DateTimeKind.Utc).AddTicks(9614), new DateTime(2025, 7, 10, 14, 10, 21, 95, DateTimeKind.Utc).AddTicks(9614) });
        }
    }
}
