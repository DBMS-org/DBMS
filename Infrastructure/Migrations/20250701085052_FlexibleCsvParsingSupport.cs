using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FlexibleCsvParsingSupport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Dip",
                table: "DrillHoles",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "Azimuth",
                table: "DrillHoles",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(1793), new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(1793) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(1798), new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(1799) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(1802), new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(1802) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(1805), new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(1805) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(1808), new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(1808) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(1810), new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(1811) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(1813), new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(1813) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(1816), new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(1816) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(1818), new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(1818) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(1821), new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(1821) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(1823), new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(1823) });

            migrationBuilder.UpdateData(
                table: "ProjectSites",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(2006), new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(2007) });

            migrationBuilder.UpdateData(
                table: "ProjectSites",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(2013), new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(2014) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(1940), new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(1941) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(1947), new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(1948) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 1, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(1954), new DateTime(2025, 6, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(1963) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(2051), new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(2052) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(2056), new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(2057) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(2060), new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(2061) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(2064), new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(2064) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(2067), new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(2068) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(2071), new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(2071) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(2074), new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(2075) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(2077), new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(2078) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(2081), new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(2081) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(2084), new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(2085) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(2087), new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(2088) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(1487), new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(1490) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(1497), new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(1497) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(1501), new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(1501) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(1503), new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(1504) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(1509), new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(1509) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(1540), new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(1541) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(1544), new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(1545) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(1875), new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(1875) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Dip",
                table: "DrillHoles",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Azimuth",
                table: "DrillHoles",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6075), new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6075) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6082), new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6083) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6086), new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6086) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6089), new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6089) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6091), new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6092) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6094), new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6094) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6097), new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6097) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6099), new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6099) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6102), new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6102) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6104), new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6104) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6107), new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6107) });

            migrationBuilder.UpdateData(
                table: "ProjectSites",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6291), new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6292) });

            migrationBuilder.UpdateData(
                table: "ProjectSites",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6297), new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6297) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6222), new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6223) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6234), new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6235) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 12, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6241), new DateTime(2025, 5, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6251) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6394), new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6395) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6399), new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6400) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6403), new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6404) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6406), new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6407) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6410), new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6411) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6414), new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6414) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6417), new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6418) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6421), new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6421) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6424), new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6425) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6427), new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6428) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6431), new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6431) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(5771), new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(5774) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(5782), new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(5783) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(5788), new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(5788) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(5793), new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(5794) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(5797), new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(5798) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(5801), new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(5802) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(5804), new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(5804) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6161), new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6162) });
        }
    }
}
