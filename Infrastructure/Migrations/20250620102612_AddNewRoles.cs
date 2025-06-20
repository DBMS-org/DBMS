using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNewRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedAt", "Description", "IsActive", "Name", "NormalizedName", "UpdatedAt" },
                values: new object[,]
                {
                    { 6, new DateTime(2025, 6, 20, 10, 26, 12, 244, DateTimeKind.Utc).AddTicks(1558), "Explosive Manager with explosives handling and safety access", true, "ExplosiveManager", "EXPLOSIVEMANAGER", new DateTime(2025, 6, 20, 10, 26, 12, 244, DateTimeKind.Utc).AddTicks(1558) },
                    { 7, new DateTime(2025, 6, 20, 10, 26, 12, 244, DateTimeKind.Utc).AddTicks(1560), "Store Manager with inventory and supply chain access", true, "StoreManager", "STOREMANAGER", new DateTime(2025, 6, 20, 10, 26, 12, 244, DateTimeKind.Utc).AddTicks(1561) },
                    { 8, new DateTime(2025, 6, 20, 10, 26, 12, 244, DateTimeKind.Utc).AddTicks(1563), "Machine Operator with machine operation access", true, "MachineOperator", "MACHINEOPERATOR", new DateTime(2025, 6, 20, 10, 26, 12, 244, DateTimeKind.Utc).AddTicks(1563) }
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 20, 10, 26, 12, 244, DateTimeKind.Utc).AddTicks(1911), new DateTime(2025, 6, 20, 10, 26, 12, 244, DateTimeKind.Utc).AddTicks(1911) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 20, 5, 47, 48, 629, DateTimeKind.Utc).AddTicks(8874), new DateTime(2025, 6, 20, 5, 47, 48, 629, DateTimeKind.Utc).AddTicks(8875) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 20, 5, 47, 48, 629, DateTimeKind.Utc).AddTicks(8892), new DateTime(2025, 6, 20, 5, 47, 48, 629, DateTimeKind.Utc).AddTicks(8892) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 20, 5, 47, 48, 629, DateTimeKind.Utc).AddTicks(8896), new DateTime(2025, 6, 20, 5, 47, 48, 629, DateTimeKind.Utc).AddTicks(8896) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 20, 5, 47, 48, 629, DateTimeKind.Utc).AddTicks(8899), new DateTime(2025, 6, 20, 5, 47, 48, 629, DateTimeKind.Utc).AddTicks(8899) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 20, 5, 47, 48, 629, DateTimeKind.Utc).AddTicks(8902), new DateTime(2025, 6, 20, 5, 47, 48, 629, DateTimeKind.Utc).AddTicks(8902) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 20, 5, 47, 48, 629, DateTimeKind.Utc).AddTicks(8905), new DateTime(2025, 6, 20, 5, 47, 48, 629, DateTimeKind.Utc).AddTicks(8905) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 20, 5, 47, 48, 629, DateTimeKind.Utc).AddTicks(8907), new DateTime(2025, 6, 20, 5, 47, 48, 629, DateTimeKind.Utc).AddTicks(8908) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 20, 5, 47, 48, 629, DateTimeKind.Utc).AddTicks(8910), new DateTime(2025, 6, 20, 5, 47, 48, 629, DateTimeKind.Utc).AddTicks(8910) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 20, 5, 47, 48, 629, DateTimeKind.Utc).AddTicks(8913), new DateTime(2025, 6, 20, 5, 47, 48, 629, DateTimeKind.Utc).AddTicks(8913) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 20, 5, 47, 48, 629, DateTimeKind.Utc).AddTicks(8915), new DateTime(2025, 6, 20, 5, 47, 48, 629, DateTimeKind.Utc).AddTicks(8916) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 20, 5, 47, 48, 629, DateTimeKind.Utc).AddTicks(8918), new DateTime(2025, 6, 20, 5, 47, 48, 629, DateTimeKind.Utc).AddTicks(8918) });

            migrationBuilder.UpdateData(
                table: "ProjectSites",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 20, 5, 47, 48, 629, DateTimeKind.Utc).AddTicks(9343), new DateTime(2025, 6, 20, 5, 47, 48, 629, DateTimeKind.Utc).AddTicks(9344) });

            migrationBuilder.UpdateData(
                table: "ProjectSites",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 20, 5, 47, 48, 629, DateTimeKind.Utc).AddTicks(9354), new DateTime(2025, 6, 20, 5, 47, 48, 629, DateTimeKind.Utc).AddTicks(9354) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 20, 5, 47, 48, 629, DateTimeKind.Utc).AddTicks(9232), new DateTime(2025, 6, 20, 5, 47, 48, 629, DateTimeKind.Utc).AddTicks(9232) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 20, 5, 47, 48, 629, DateTimeKind.Utc).AddTicks(9246), new DateTime(2025, 6, 20, 5, 47, 48, 629, DateTimeKind.Utc).AddTicks(9247) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 12, 20, 5, 47, 48, 629, DateTimeKind.Utc).AddTicks(9256), new DateTime(2025, 5, 20, 5, 47, 48, 629, DateTimeKind.Utc).AddTicks(9271) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 20, 5, 47, 48, 629, DateTimeKind.Utc).AddTicks(8323), new DateTime(2025, 6, 20, 5, 47, 48, 629, DateTimeKind.Utc).AddTicks(8332) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 20, 5, 47, 48, 629, DateTimeKind.Utc).AddTicks(8349), new DateTime(2025, 6, 20, 5, 47, 48, 629, DateTimeKind.Utc).AddTicks(8349) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 20, 5, 47, 48, 629, DateTimeKind.Utc).AddTicks(8352), new DateTime(2025, 6, 20, 5, 47, 48, 629, DateTimeKind.Utc).AddTicks(8353) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 20, 5, 47, 48, 629, DateTimeKind.Utc).AddTicks(8355), new DateTime(2025, 6, 20, 5, 47, 48, 629, DateTimeKind.Utc).AddTicks(8356) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 20, 5, 47, 48, 629, DateTimeKind.Utc).AddTicks(8358), new DateTime(2025, 6, 20, 5, 47, 48, 629, DateTimeKind.Utc).AddTicks(8359) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 20, 5, 47, 48, 629, DateTimeKind.Utc).AddTicks(9126), new DateTime(2025, 6, 20, 5, 47, 48, 629, DateTimeKind.Utc).AddTicks(9126) });
        }
    }
}
