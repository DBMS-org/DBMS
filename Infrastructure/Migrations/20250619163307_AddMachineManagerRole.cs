using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMachineManagerRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 19, 16, 33, 6, 947, DateTimeKind.Utc).AddTicks(1116), new DateTime(2025, 6, 19, 16, 33, 6, 947, DateTimeKind.Utc).AddTicks(1117) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 19, 16, 33, 6, 947, DateTimeKind.Utc).AddTicks(1122), new DateTime(2025, 6, 19, 16, 33, 6, 947, DateTimeKind.Utc).AddTicks(1123) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 19, 16, 33, 6, 947, DateTimeKind.Utc).AddTicks(1126), new DateTime(2025, 6, 19, 16, 33, 6, 947, DateTimeKind.Utc).AddTicks(1127) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 19, 16, 33, 6, 947, DateTimeKind.Utc).AddTicks(1129), new DateTime(2025, 6, 19, 16, 33, 6, 947, DateTimeKind.Utc).AddTicks(1130) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 19, 16, 33, 6, 947, DateTimeKind.Utc).AddTicks(1132), new DateTime(2025, 6, 19, 16, 33, 6, 947, DateTimeKind.Utc).AddTicks(1133) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 19, 16, 33, 6, 947, DateTimeKind.Utc).AddTicks(1135), new DateTime(2025, 6, 19, 16, 33, 6, 947, DateTimeKind.Utc).AddTicks(1136) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 19, 16, 33, 6, 947, DateTimeKind.Utc).AddTicks(1138), new DateTime(2025, 6, 19, 16, 33, 6, 947, DateTimeKind.Utc).AddTicks(1139) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 19, 16, 33, 6, 947, DateTimeKind.Utc).AddTicks(1141), new DateTime(2025, 6, 19, 16, 33, 6, 947, DateTimeKind.Utc).AddTicks(1142) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 19, 16, 33, 6, 947, DateTimeKind.Utc).AddTicks(1144), new DateTime(2025, 6, 19, 16, 33, 6, 947, DateTimeKind.Utc).AddTicks(1144) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 19, 16, 33, 6, 947, DateTimeKind.Utc).AddTicks(1147), new DateTime(2025, 6, 19, 16, 33, 6, 947, DateTimeKind.Utc).AddTicks(1147) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 19, 16, 33, 6, 947, DateTimeKind.Utc).AddTicks(1150), new DateTime(2025, 6, 19, 16, 33, 6, 947, DateTimeKind.Utc).AddTicks(1150) });

            migrationBuilder.UpdateData(
                table: "ProjectSites",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 19, 16, 33, 6, 947, DateTimeKind.Utc).AddTicks(1482), new DateTime(2025, 6, 19, 16, 33, 6, 947, DateTimeKind.Utc).AddTicks(1483) });

            migrationBuilder.UpdateData(
                table: "ProjectSites",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 19, 16, 33, 6, 947, DateTimeKind.Utc).AddTicks(1489), new DateTime(2025, 6, 19, 16, 33, 6, 947, DateTimeKind.Utc).AddTicks(1490) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 19, 16, 33, 6, 947, DateTimeKind.Utc).AddTicks(1276), new DateTime(2025, 6, 19, 16, 33, 6, 947, DateTimeKind.Utc).AddTicks(1277) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 19, 16, 33, 6, 947, DateTimeKind.Utc).AddTicks(1407), new DateTime(2025, 6, 19, 16, 33, 6, 947, DateTimeKind.Utc).AddTicks(1408) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 12, 19, 16, 33, 6, 947, DateTimeKind.Utc).AddTicks(1416), new DateTime(2025, 5, 19, 16, 33, 6, 947, DateTimeKind.Utc).AddTicks(1429) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 19, 16, 33, 6, 947, DateTimeKind.Utc).AddTicks(771), new DateTime(2025, 6, 19, 16, 33, 6, 947, DateTimeKind.Utc).AddTicks(776) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 19, 16, 33, 6, 947, DateTimeKind.Utc).AddTicks(787), new DateTime(2025, 6, 19, 16, 33, 6, 947, DateTimeKind.Utc).AddTicks(788) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 19, 16, 33, 6, 947, DateTimeKind.Utc).AddTicks(791), new DateTime(2025, 6, 19, 16, 33, 6, 947, DateTimeKind.Utc).AddTicks(792) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 19, 16, 33, 6, 947, DateTimeKind.Utc).AddTicks(794), new DateTime(2025, 6, 19, 16, 33, 6, 947, DateTimeKind.Utc).AddTicks(795) });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedAt", "Description", "IsActive", "Name", "NormalizedName", "UpdatedAt" },
                values: new object[] { 5, new DateTime(2025, 6, 19, 16, 33, 6, 947, DateTimeKind.Utc).AddTicks(797), "Machine Manager with machinery operational access", true, "MachineManager", "MACHINEMANAGER", new DateTime(2025, 6, 19, 16, 33, 6, 947, DateTimeKind.Utc).AddTicks(798) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 19, 16, 33, 6, 947, DateTimeKind.Utc).AddTicks(1209), new DateTime(2025, 6, 19, 16, 33, 6, 947, DateTimeKind.Utc).AddTicks(1210) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 18, 4, 38, 40, 460, DateTimeKind.Utc).AddTicks(4663), new DateTime(2025, 6, 18, 4, 38, 40, 460, DateTimeKind.Utc).AddTicks(4663) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 18, 4, 38, 40, 460, DateTimeKind.Utc).AddTicks(4669), new DateTime(2025, 6, 18, 4, 38, 40, 460, DateTimeKind.Utc).AddTicks(4669) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 18, 4, 38, 40, 460, DateTimeKind.Utc).AddTicks(4672), new DateTime(2025, 6, 18, 4, 38, 40, 460, DateTimeKind.Utc).AddTicks(4673) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 18, 4, 38, 40, 460, DateTimeKind.Utc).AddTicks(4675), new DateTime(2025, 6, 18, 4, 38, 40, 460, DateTimeKind.Utc).AddTicks(4675) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 18, 4, 38, 40, 460, DateTimeKind.Utc).AddTicks(4678), new DateTime(2025, 6, 18, 4, 38, 40, 460, DateTimeKind.Utc).AddTicks(4678) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 18, 4, 38, 40, 460, DateTimeKind.Utc).AddTicks(4681), new DateTime(2025, 6, 18, 4, 38, 40, 460, DateTimeKind.Utc).AddTicks(4681) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 18, 4, 38, 40, 460, DateTimeKind.Utc).AddTicks(4684), new DateTime(2025, 6, 18, 4, 38, 40, 460, DateTimeKind.Utc).AddTicks(4684) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 18, 4, 38, 40, 460, DateTimeKind.Utc).AddTicks(4687), new DateTime(2025, 6, 18, 4, 38, 40, 460, DateTimeKind.Utc).AddTicks(4687) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 18, 4, 38, 40, 460, DateTimeKind.Utc).AddTicks(4689), new DateTime(2025, 6, 18, 4, 38, 40, 460, DateTimeKind.Utc).AddTicks(4690) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 18, 4, 38, 40, 460, DateTimeKind.Utc).AddTicks(4692), new DateTime(2025, 6, 18, 4, 38, 40, 460, DateTimeKind.Utc).AddTicks(4692) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 18, 4, 38, 40, 460, DateTimeKind.Utc).AddTicks(4695), new DateTime(2025, 6, 18, 4, 38, 40, 460, DateTimeKind.Utc).AddTicks(4695) });

            migrationBuilder.UpdateData(
                table: "ProjectSites",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 18, 4, 38, 40, 460, DateTimeKind.Utc).AddTicks(4934), new DateTime(2025, 6, 18, 4, 38, 40, 460, DateTimeKind.Utc).AddTicks(4935) });

            migrationBuilder.UpdateData(
                table: "ProjectSites",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 18, 4, 38, 40, 460, DateTimeKind.Utc).AddTicks(4940), new DateTime(2025, 6, 18, 4, 38, 40, 460, DateTimeKind.Utc).AddTicks(4940) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 18, 4, 38, 40, 460, DateTimeKind.Utc).AddTicks(4809), new DateTime(2025, 6, 18, 4, 38, 40, 460, DateTimeKind.Utc).AddTicks(4810) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 18, 4, 38, 40, 460, DateTimeKind.Utc).AddTicks(4816), new DateTime(2025, 6, 18, 4, 38, 40, 460, DateTimeKind.Utc).AddTicks(4817) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 12, 18, 4, 38, 40, 460, DateTimeKind.Utc).AddTicks(4823), new DateTime(2025, 5, 18, 4, 38, 40, 460, DateTimeKind.Utc).AddTicks(4835) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 18, 4, 38, 40, 460, DateTimeKind.Utc).AddTicks(4409), new DateTime(2025, 6, 18, 4, 38, 40, 460, DateTimeKind.Utc).AddTicks(4412) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 18, 4, 38, 40, 460, DateTimeKind.Utc).AddTicks(4419), new DateTime(2025, 6, 18, 4, 38, 40, 460, DateTimeKind.Utc).AddTicks(4419) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 18, 4, 38, 40, 460, DateTimeKind.Utc).AddTicks(4422), new DateTime(2025, 6, 18, 4, 38, 40, 460, DateTimeKind.Utc).AddTicks(4423) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 18, 4, 38, 40, 460, DateTimeKind.Utc).AddTicks(4425), new DateTime(2025, 6, 18, 4, 38, 40, 460, DateTimeKind.Utc).AddTicks(4426) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 18, 4, 38, 40, 460, DateTimeKind.Utc).AddTicks(4745), new DateTime(2025, 6, 18, 4, 38, 40, 460, DateTimeKind.Utc).AddTicks(4745) });
        }
    }
}
