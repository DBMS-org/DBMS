using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveMachineOperatorRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 8);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 20, 10, 26, 12, 244, DateTimeKind.Utc).AddTicks(1831), new DateTime(2025, 6, 20, 10, 26, 12, 244, DateTimeKind.Utc).AddTicks(1831) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 20, 10, 26, 12, 244, DateTimeKind.Utc).AddTicks(1837), new DateTime(2025, 6, 20, 10, 26, 12, 244, DateTimeKind.Utc).AddTicks(1837) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 20, 10, 26, 12, 244, DateTimeKind.Utc).AddTicks(1840), new DateTime(2025, 6, 20, 10, 26, 12, 244, DateTimeKind.Utc).AddTicks(1841) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 20, 10, 26, 12, 244, DateTimeKind.Utc).AddTicks(1843), new DateTime(2025, 6, 20, 10, 26, 12, 244, DateTimeKind.Utc).AddTicks(1844) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 20, 10, 26, 12, 244, DateTimeKind.Utc).AddTicks(1846), new DateTime(2025, 6, 20, 10, 26, 12, 244, DateTimeKind.Utc).AddTicks(1847) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 20, 10, 26, 12, 244, DateTimeKind.Utc).AddTicks(1849), new DateTime(2025, 6, 20, 10, 26, 12, 244, DateTimeKind.Utc).AddTicks(1850) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 20, 10, 26, 12, 244, DateTimeKind.Utc).AddTicks(1852), new DateTime(2025, 6, 20, 10, 26, 12, 244, DateTimeKind.Utc).AddTicks(1852) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 20, 10, 26, 12, 244, DateTimeKind.Utc).AddTicks(1855), new DateTime(2025, 6, 20, 10, 26, 12, 244, DateTimeKind.Utc).AddTicks(1855) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 20, 10, 26, 12, 244, DateTimeKind.Utc).AddTicks(1857), new DateTime(2025, 6, 20, 10, 26, 12, 244, DateTimeKind.Utc).AddTicks(1858) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 20, 10, 26, 12, 244, DateTimeKind.Utc).AddTicks(1860), new DateTime(2025, 6, 20, 10, 26, 12, 244, DateTimeKind.Utc).AddTicks(1860) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 20, 10, 26, 12, 244, DateTimeKind.Utc).AddTicks(1862), new DateTime(2025, 6, 20, 10, 26, 12, 244, DateTimeKind.Utc).AddTicks(1863) });

            migrationBuilder.UpdateData(
                table: "ProjectSites",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 20, 10, 26, 12, 244, DateTimeKind.Utc).AddTicks(2037), new DateTime(2025, 6, 20, 10, 26, 12, 244, DateTimeKind.Utc).AddTicks(2038) });

            migrationBuilder.UpdateData(
                table: "ProjectSites",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 20, 10, 26, 12, 244, DateTimeKind.Utc).AddTicks(2043), new DateTime(2025, 6, 20, 10, 26, 12, 244, DateTimeKind.Utc).AddTicks(2044) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 20, 10, 26, 12, 244, DateTimeKind.Utc).AddTicks(1972), new DateTime(2025, 6, 20, 10, 26, 12, 244, DateTimeKind.Utc).AddTicks(1973) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 20, 10, 26, 12, 244, DateTimeKind.Utc).AddTicks(1979), new DateTime(2025, 6, 20, 10, 26, 12, 244, DateTimeKind.Utc).AddTicks(1979) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 12, 20, 10, 26, 12, 244, DateTimeKind.Utc).AddTicks(1985), new DateTime(2025, 5, 20, 10, 26, 12, 244, DateTimeKind.Utc).AddTicks(1996) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 20, 10, 26, 12, 244, DateTimeKind.Utc).AddTicks(1531), new DateTime(2025, 6, 20, 10, 26, 12, 244, DateTimeKind.Utc).AddTicks(1536) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 20, 10, 26, 12, 244, DateTimeKind.Utc).AddTicks(1546), new DateTime(2025, 6, 20, 10, 26, 12, 244, DateTimeKind.Utc).AddTicks(1547) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 20, 10, 26, 12, 244, DateTimeKind.Utc).AddTicks(1550), new DateTime(2025, 6, 20, 10, 26, 12, 244, DateTimeKind.Utc).AddTicks(1550) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 20, 10, 26, 12, 244, DateTimeKind.Utc).AddTicks(1553), new DateTime(2025, 6, 20, 10, 26, 12, 244, DateTimeKind.Utc).AddTicks(1553) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 20, 10, 26, 12, 244, DateTimeKind.Utc).AddTicks(1555), new DateTime(2025, 6, 20, 10, 26, 12, 244, DateTimeKind.Utc).AddTicks(1555) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 20, 10, 26, 12, 244, DateTimeKind.Utc).AddTicks(1558), new DateTime(2025, 6, 20, 10, 26, 12, 244, DateTimeKind.Utc).AddTicks(1558) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 20, 10, 26, 12, 244, DateTimeKind.Utc).AddTicks(1560), new DateTime(2025, 6, 20, 10, 26, 12, 244, DateTimeKind.Utc).AddTicks(1561) });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedAt", "Description", "IsActive", "Name", "NormalizedName", "UpdatedAt" },
                values: new object[] { 8, new DateTime(2025, 6, 20, 10, 26, 12, 244, DateTimeKind.Utc).AddTicks(1563), "Machine Operator with machine operation access", true, "MachineOperator", "MACHINEOPERATOR", new DateTime(2025, 6, 20, 10, 26, 12, 244, DateTimeKind.Utc).AddTicks(1563) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 20, 10, 26, 12, 244, DateTimeKind.Utc).AddTicks(1911), new DateTime(2025, 6, 20, 10, 26, 12, 244, DateTimeKind.Utc).AddTicks(1911) });
        }
    }
}
