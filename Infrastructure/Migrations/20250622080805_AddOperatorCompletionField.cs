using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddOperatorCompletionField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsOperatorCompleted",
                table: "ProjectSites",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2450), new DateTime(2025, 6, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2450) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2457), new DateTime(2025, 6, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2457) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2460), new DateTime(2025, 6, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2461) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2463), new DateTime(2025, 6, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2464) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2466), new DateTime(2025, 6, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2466) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2469), new DateTime(2025, 6, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2469) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2471), new DateTime(2025, 6, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2472) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2474), new DateTime(2025, 6, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2474) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2477), new DateTime(2025, 6, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2477) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2479), new DateTime(2025, 6, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2480) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2482), new DateTime(2025, 6, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2482) });

            migrationBuilder.UpdateData(
                table: "ProjectSites",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "IsOperatorCompleted", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2649), false, new DateTime(2025, 6, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2650) });

            migrationBuilder.UpdateData(
                table: "ProjectSites",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "IsOperatorCompleted", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2655), false, new DateTime(2025, 6, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2655) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2585), new DateTime(2025, 6, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2586) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2592), new DateTime(2025, 6, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2593) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 12, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2598), new DateTime(2025, 5, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2609) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2217), new DateTime(2025, 6, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2221) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2229), new DateTime(2025, 6, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2229) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2233), new DateTime(2025, 6, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2233) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2236), new DateTime(2025, 6, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2236) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2238), new DateTime(2025, 6, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2239) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2241), new DateTime(2025, 6, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2241) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2243), new DateTime(2025, 6, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2244) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2531), new DateTime(2025, 6, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2532) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsOperatorCompleted",
                table: "ProjectSites");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 20, 10, 40, 56, 185, DateTimeKind.Utc).AddTicks(6577), new DateTime(2025, 6, 20, 10, 40, 56, 185, DateTimeKind.Utc).AddTicks(6577) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 20, 10, 40, 56, 185, DateTimeKind.Utc).AddTicks(6584), new DateTime(2025, 6, 20, 10, 40, 56, 185, DateTimeKind.Utc).AddTicks(6584) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 20, 10, 40, 56, 185, DateTimeKind.Utc).AddTicks(6587), new DateTime(2025, 6, 20, 10, 40, 56, 185, DateTimeKind.Utc).AddTicks(6588) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 20, 10, 40, 56, 185, DateTimeKind.Utc).AddTicks(6590), new DateTime(2025, 6, 20, 10, 40, 56, 185, DateTimeKind.Utc).AddTicks(6591) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 20, 10, 40, 56, 185, DateTimeKind.Utc).AddTicks(6593), new DateTime(2025, 6, 20, 10, 40, 56, 185, DateTimeKind.Utc).AddTicks(6593) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 20, 10, 40, 56, 185, DateTimeKind.Utc).AddTicks(6596), new DateTime(2025, 6, 20, 10, 40, 56, 185, DateTimeKind.Utc).AddTicks(6596) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 20, 10, 40, 56, 185, DateTimeKind.Utc).AddTicks(6599), new DateTime(2025, 6, 20, 10, 40, 56, 185, DateTimeKind.Utc).AddTicks(6599) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 20, 10, 40, 56, 185, DateTimeKind.Utc).AddTicks(6601), new DateTime(2025, 6, 20, 10, 40, 56, 185, DateTimeKind.Utc).AddTicks(6602) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 20, 10, 40, 56, 185, DateTimeKind.Utc).AddTicks(6604), new DateTime(2025, 6, 20, 10, 40, 56, 185, DateTimeKind.Utc).AddTicks(6604) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 20, 10, 40, 56, 185, DateTimeKind.Utc).AddTicks(6606), new DateTime(2025, 6, 20, 10, 40, 56, 185, DateTimeKind.Utc).AddTicks(6607) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 20, 10, 40, 56, 185, DateTimeKind.Utc).AddTicks(6609), new DateTime(2025, 6, 20, 10, 40, 56, 185, DateTimeKind.Utc).AddTicks(6609) });

            migrationBuilder.UpdateData(
                table: "ProjectSites",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 20, 10, 40, 56, 185, DateTimeKind.Utc).AddTicks(6784), new DateTime(2025, 6, 20, 10, 40, 56, 185, DateTimeKind.Utc).AddTicks(6785) });

            migrationBuilder.UpdateData(
                table: "ProjectSites",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 20, 10, 40, 56, 185, DateTimeKind.Utc).AddTicks(6789), new DateTime(2025, 6, 20, 10, 40, 56, 185, DateTimeKind.Utc).AddTicks(6790) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 20, 10, 40, 56, 185, DateTimeKind.Utc).AddTicks(6717), new DateTime(2025, 6, 20, 10, 40, 56, 185, DateTimeKind.Utc).AddTicks(6718) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 20, 10, 40, 56, 185, DateTimeKind.Utc).AddTicks(6725), new DateTime(2025, 6, 20, 10, 40, 56, 185, DateTimeKind.Utc).AddTicks(6726) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 12, 20, 10, 40, 56, 185, DateTimeKind.Utc).AddTicks(6732), new DateTime(2025, 5, 20, 10, 40, 56, 185, DateTimeKind.Utc).AddTicks(6744) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 20, 10, 40, 56, 185, DateTimeKind.Utc).AddTicks(6324), new DateTime(2025, 6, 20, 10, 40, 56, 185, DateTimeKind.Utc).AddTicks(6327) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 20, 10, 40, 56, 185, DateTimeKind.Utc).AddTicks(6334), new DateTime(2025, 6, 20, 10, 40, 56, 185, DateTimeKind.Utc).AddTicks(6334) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 20, 10, 40, 56, 185, DateTimeKind.Utc).AddTicks(6337), new DateTime(2025, 6, 20, 10, 40, 56, 185, DateTimeKind.Utc).AddTicks(6338) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 20, 10, 40, 56, 185, DateTimeKind.Utc).AddTicks(6340), new DateTime(2025, 6, 20, 10, 40, 56, 185, DateTimeKind.Utc).AddTicks(6340) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 20, 10, 40, 56, 185, DateTimeKind.Utc).AddTicks(6343), new DateTime(2025, 6, 20, 10, 40, 56, 185, DateTimeKind.Utc).AddTicks(6343) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 20, 10, 40, 56, 185, DateTimeKind.Utc).AddTicks(6345), new DateTime(2025, 6, 20, 10, 40, 56, 185, DateTimeKind.Utc).AddTicks(6346) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 20, 10, 40, 56, 185, DateTimeKind.Utc).AddTicks(6348), new DateTime(2025, 6, 20, 10, 40, 56, 185, DateTimeKind.Utc).AddTicks(6348) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 20, 10, 40, 56, 185, DateTimeKind.Utc).AddTicks(6661), new DateTime(2025, 6, 20, 10, 40, 56, 185, DateTimeKind.Utc).AddTicks(6662) });
        }
    }
}
