using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveBudgetColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Budget",
                table: "Projects");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 5, 4, 19, 29, 302, DateTimeKind.Utc).AddTicks(2113), new DateTime(2025, 6, 5, 4, 19, 29, 302, DateTimeKind.Utc).AddTicks(2113) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 5, 4, 19, 29, 302, DateTimeKind.Utc).AddTicks(2116), new DateTime(2025, 6, 5, 4, 19, 29, 302, DateTimeKind.Utc).AddTicks(2117) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 5, 4, 19, 29, 302, DateTimeKind.Utc).AddTicks(2118), new DateTime(2025, 6, 5, 4, 19, 29, 302, DateTimeKind.Utc).AddTicks(2119) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 5, 4, 19, 29, 302, DateTimeKind.Utc).AddTicks(2120), new DateTime(2025, 6, 5, 4, 19, 29, 302, DateTimeKind.Utc).AddTicks(2120) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 5, 4, 19, 29, 302, DateTimeKind.Utc).AddTicks(2122), new DateTime(2025, 6, 5, 4, 19, 29, 302, DateTimeKind.Utc).AddTicks(2122) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 5, 4, 19, 29, 302, DateTimeKind.Utc).AddTicks(2123), new DateTime(2025, 6, 5, 4, 19, 29, 302, DateTimeKind.Utc).AddTicks(2123) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 5, 4, 19, 29, 302, DateTimeKind.Utc).AddTicks(2124), new DateTime(2025, 6, 5, 4, 19, 29, 302, DateTimeKind.Utc).AddTicks(2125) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 5, 4, 19, 29, 302, DateTimeKind.Utc).AddTicks(2126), new DateTime(2025, 6, 5, 4, 19, 29, 302, DateTimeKind.Utc).AddTicks(2126) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 5, 4, 19, 29, 302, DateTimeKind.Utc).AddTicks(2127), new DateTime(2025, 6, 5, 4, 19, 29, 302, DateTimeKind.Utc).AddTicks(2127) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 5, 4, 19, 29, 302, DateTimeKind.Utc).AddTicks(2129), new DateTime(2025, 6, 5, 4, 19, 29, 302, DateTimeKind.Utc).AddTicks(2129) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 5, 4, 19, 29, 302, DateTimeKind.Utc).AddTicks(2130), new DateTime(2025, 6, 5, 4, 19, 29, 302, DateTimeKind.Utc).AddTicks(2130) });

            migrationBuilder.UpdateData(
                table: "ProjectSites",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 5, 4, 19, 29, 302, DateTimeKind.Utc).AddTicks(2226), new DateTime(2025, 6, 5, 4, 19, 29, 302, DateTimeKind.Utc).AddTicks(2226) });

            migrationBuilder.UpdateData(
                table: "ProjectSites",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 5, 4, 19, 29, 302, DateTimeKind.Utc).AddTicks(2229), new DateTime(2025, 6, 5, 4, 19, 29, 302, DateTimeKind.Utc).AddTicks(2229) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 5, 4, 19, 29, 302, DateTimeKind.Utc).AddTicks(2188), new DateTime(2025, 6, 5, 4, 19, 29, 302, DateTimeKind.Utc).AddTicks(2188) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 5, 4, 19, 29, 302, DateTimeKind.Utc).AddTicks(2192), new DateTime(2025, 6, 5, 4, 19, 29, 302, DateTimeKind.Utc).AddTicks(2192) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 12, 5, 4, 19, 29, 302, DateTimeKind.Utc).AddTicks(2197), new DateTime(2025, 5, 5, 4, 19, 29, 302, DateTimeKind.Utc).AddTicks(2206) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 5, 4, 19, 29, 302, DateTimeKind.Utc).AddTicks(1961), new DateTime(2025, 6, 5, 4, 19, 29, 302, DateTimeKind.Utc).AddTicks(1964) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 5, 4, 19, 29, 302, DateTimeKind.Utc).AddTicks(1969), new DateTime(2025, 6, 5, 4, 19, 29, 302, DateTimeKind.Utc).AddTicks(1969) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 5, 4, 19, 29, 302, DateTimeKind.Utc).AddTicks(1971), new DateTime(2025, 6, 5, 4, 19, 29, 302, DateTimeKind.Utc).AddTicks(1971) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 5, 4, 19, 29, 302, DateTimeKind.Utc).AddTicks(2155), new DateTime(2025, 6, 5, 4, 19, 29, 302, DateTimeKind.Utc).AddTicks(2156) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Budget",
                table: "Projects",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 5, 4, 7, 25, 702, DateTimeKind.Utc).AddTicks(444), new DateTime(2025, 6, 5, 4, 7, 25, 702, DateTimeKind.Utc).AddTicks(445) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 5, 4, 7, 25, 702, DateTimeKind.Utc).AddTicks(448), new DateTime(2025, 6, 5, 4, 7, 25, 702, DateTimeKind.Utc).AddTicks(448) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 5, 4, 7, 25, 702, DateTimeKind.Utc).AddTicks(479), new DateTime(2025, 6, 5, 4, 7, 25, 702, DateTimeKind.Utc).AddTicks(479) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 5, 4, 7, 25, 702, DateTimeKind.Utc).AddTicks(481), new DateTime(2025, 6, 5, 4, 7, 25, 702, DateTimeKind.Utc).AddTicks(481) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 5, 4, 7, 25, 702, DateTimeKind.Utc).AddTicks(482), new DateTime(2025, 6, 5, 4, 7, 25, 702, DateTimeKind.Utc).AddTicks(482) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 5, 4, 7, 25, 702, DateTimeKind.Utc).AddTicks(484), new DateTime(2025, 6, 5, 4, 7, 25, 702, DateTimeKind.Utc).AddTicks(484) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 5, 4, 7, 25, 702, DateTimeKind.Utc).AddTicks(485), new DateTime(2025, 6, 5, 4, 7, 25, 702, DateTimeKind.Utc).AddTicks(485) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 5, 4, 7, 25, 702, DateTimeKind.Utc).AddTicks(487), new DateTime(2025, 6, 5, 4, 7, 25, 702, DateTimeKind.Utc).AddTicks(487) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 5, 4, 7, 25, 702, DateTimeKind.Utc).AddTicks(488), new DateTime(2025, 6, 5, 4, 7, 25, 702, DateTimeKind.Utc).AddTicks(488) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 5, 4, 7, 25, 702, DateTimeKind.Utc).AddTicks(489), new DateTime(2025, 6, 5, 4, 7, 25, 702, DateTimeKind.Utc).AddTicks(490) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 5, 4, 7, 25, 702, DateTimeKind.Utc).AddTicks(491), new DateTime(2025, 6, 5, 4, 7, 25, 702, DateTimeKind.Utc).AddTicks(491) });

            migrationBuilder.UpdateData(
                table: "ProjectSites",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 5, 4, 7, 25, 702, DateTimeKind.Utc).AddTicks(592), new DateTime(2025, 6, 5, 4, 7, 25, 702, DateTimeKind.Utc).AddTicks(593) });

            migrationBuilder.UpdateData(
                table: "ProjectSites",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 5, 4, 7, 25, 702, DateTimeKind.Utc).AddTicks(595), new DateTime(2025, 6, 5, 4, 7, 25, 702, DateTimeKind.Utc).AddTicks(596) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Budget", "CreatedAt", "UpdatedAt" },
                values: new object[] { 5500000.00m, new DateTime(2025, 6, 5, 4, 7, 25, 702, DateTimeKind.Utc).AddTicks(557), new DateTime(2025, 6, 5, 4, 7, 25, 702, DateTimeKind.Utc).AddTicks(557) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Budget", "CreatedAt", "UpdatedAt" },
                values: new object[] { 8200000.00m, new DateTime(2025, 6, 5, 4, 7, 25, 702, DateTimeKind.Utc).AddTicks(561), new DateTime(2025, 6, 5, 4, 7, 25, 702, DateTimeKind.Utc).AddTicks(562) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Budget", "CreatedAt", "UpdatedAt" },
                values: new object[] { 3200000.00m, new DateTime(2024, 12, 5, 4, 7, 25, 702, DateTimeKind.Utc).AddTicks(566), new DateTime(2025, 5, 5, 4, 7, 25, 702, DateTimeKind.Utc).AddTicks(573) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 5, 4, 7, 25, 702, DateTimeKind.Utc).AddTicks(306), new DateTime(2025, 6, 5, 4, 7, 25, 702, DateTimeKind.Utc).AddTicks(308) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 5, 4, 7, 25, 702, DateTimeKind.Utc).AddTicks(313), new DateTime(2025, 6, 5, 4, 7, 25, 702, DateTimeKind.Utc).AddTicks(313) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 5, 4, 7, 25, 702, DateTimeKind.Utc).AddTicks(315), new DateTime(2025, 6, 5, 4, 7, 25, 702, DateTimeKind.Utc).AddTicks(316) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 5, 4, 7, 25, 702, DateTimeKind.Utc).AddTicks(518), new DateTime(2025, 6, 5, 4, 7, 25, 702, DateTimeKind.Utc).AddTicks(518) });
        }
    }
}
