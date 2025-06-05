using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveProjectTypeColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProjectType",
                table: "Projects");

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
                columns: new[] { "CreatedAt", "Name", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 5, 4, 7, 25, 702, DateTimeKind.Utc).AddTicks(557), "Muscat Infrastructure Development - Highway Construction", new DateTime(2025, 6, 5, 4, 7, 25, 702, DateTimeKind.Utc).AddTicks(557) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "Name", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 5, 4, 7, 25, 702, DateTimeKind.Utc).AddTicks(561), "Dhofar Mining Operations - Site Development", new DateTime(2025, 6, 5, 4, 7, 25, 702, DateTimeKind.Utc).AddTicks(562) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "Name", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 12, 5, 4, 7, 25, 702, DateTimeKind.Utc).AddTicks(566), "Sohar Industrial Zone - Development", new DateTime(2025, 5, 5, 4, 7, 25, 702, DateTimeKind.Utc).AddTicks(573) });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProjectType",
                table: "Projects",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 4, 16, 57, 1, 77, DateTimeKind.Utc).AddTicks(7331), new DateTime(2025, 6, 4, 16, 57, 1, 77, DateTimeKind.Utc).AddTicks(7332) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 4, 16, 57, 1, 77, DateTimeKind.Utc).AddTicks(7336), new DateTime(2025, 6, 4, 16, 57, 1, 77, DateTimeKind.Utc).AddTicks(7336) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 4, 16, 57, 1, 77, DateTimeKind.Utc).AddTicks(7339), new DateTime(2025, 6, 4, 16, 57, 1, 77, DateTimeKind.Utc).AddTicks(7340) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 4, 16, 57, 1, 77, DateTimeKind.Utc).AddTicks(7342), new DateTime(2025, 6, 4, 16, 57, 1, 77, DateTimeKind.Utc).AddTicks(7342) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 4, 16, 57, 1, 77, DateTimeKind.Utc).AddTicks(7345), new DateTime(2025, 6, 4, 16, 57, 1, 77, DateTimeKind.Utc).AddTicks(7345) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 4, 16, 57, 1, 77, DateTimeKind.Utc).AddTicks(7347), new DateTime(2025, 6, 4, 16, 57, 1, 77, DateTimeKind.Utc).AddTicks(7347) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 4, 16, 57, 1, 77, DateTimeKind.Utc).AddTicks(7349), new DateTime(2025, 6, 4, 16, 57, 1, 77, DateTimeKind.Utc).AddTicks(7350) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 4, 16, 57, 1, 77, DateTimeKind.Utc).AddTicks(7352), new DateTime(2025, 6, 4, 16, 57, 1, 77, DateTimeKind.Utc).AddTicks(7352) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 4, 16, 57, 1, 77, DateTimeKind.Utc).AddTicks(7354), new DateTime(2025, 6, 4, 16, 57, 1, 77, DateTimeKind.Utc).AddTicks(7355) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 4, 16, 57, 1, 77, DateTimeKind.Utc).AddTicks(7357), new DateTime(2025, 6, 4, 16, 57, 1, 77, DateTimeKind.Utc).AddTicks(7357) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 4, 16, 57, 1, 77, DateTimeKind.Utc).AddTicks(7359), new DateTime(2025, 6, 4, 16, 57, 1, 77, DateTimeKind.Utc).AddTicks(7359) });

            migrationBuilder.UpdateData(
                table: "ProjectSites",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 4, 16, 57, 1, 77, DateTimeKind.Utc).AddTicks(7500), new DateTime(2025, 6, 4, 16, 57, 1, 77, DateTimeKind.Utc).AddTicks(7500) });

            migrationBuilder.UpdateData(
                table: "ProjectSites",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 4, 16, 57, 1, 77, DateTimeKind.Utc).AddTicks(7504), new DateTime(2025, 6, 4, 16, 57, 1, 77, DateTimeKind.Utc).AddTicks(7505) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Name", "ProjectType", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 4, 16, 57, 1, 77, DateTimeKind.Utc).AddTicks(7448), "Muscat Infrastructure Development", "Highway Construction", new DateTime(2025, 6, 4, 16, 57, 1, 77, DateTimeKind.Utc).AddTicks(7449) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "Name", "ProjectType", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 4, 16, 57, 1, 77, DateTimeKind.Utc).AddTicks(7456), "Dhofar Mining Operations", "Mining Site Development", new DateTime(2025, 6, 4, 16, 57, 1, 77, DateTimeKind.Utc).AddTicks(7457) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "Name", "ProjectType", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 12, 4, 16, 57, 1, 77, DateTimeKind.Utc).AddTicks(7462), "Sohar Industrial Zone", "Industrial Development", new DateTime(2025, 5, 4, 16, 57, 1, 77, DateTimeKind.Utc).AddTicks(7470) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 4, 16, 57, 1, 77, DateTimeKind.Utc).AddTicks(7168), new DateTime(2025, 6, 4, 16, 57, 1, 77, DateTimeKind.Utc).AddTicks(7171) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 4, 16, 57, 1, 77, DateTimeKind.Utc).AddTicks(7178), new DateTime(2025, 6, 4, 16, 57, 1, 77, DateTimeKind.Utc).AddTicks(7178) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 4, 16, 57, 1, 77, DateTimeKind.Utc).AddTicks(7181), new DateTime(2025, 6, 4, 16, 57, 1, 77, DateTimeKind.Utc).AddTicks(7181) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 4, 16, 57, 1, 77, DateTimeKind.Utc).AddTicks(7397), new DateTime(2025, 6, 4, 16, 57, 1, 77, DateTimeKind.Utc).AddTicks(7397) });
        }
    }
}
