using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UniqueOperatorPerProjects : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 6, 44, 5, 871, DateTimeKind.Utc).AddTicks(3440), new DateTime(2025, 6, 16, 6, 44, 5, 871, DateTimeKind.Utc).AddTicks(3441) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 6, 44, 5, 871, DateTimeKind.Utc).AddTicks(3446), new DateTime(2025, 6, 16, 6, 44, 5, 871, DateTimeKind.Utc).AddTicks(3446) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 6, 44, 5, 871, DateTimeKind.Utc).AddTicks(3449), new DateTime(2025, 6, 16, 6, 44, 5, 871, DateTimeKind.Utc).AddTicks(3450) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 6, 44, 5, 871, DateTimeKind.Utc).AddTicks(3452), new DateTime(2025, 6, 16, 6, 44, 5, 871, DateTimeKind.Utc).AddTicks(3453) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 6, 44, 5, 871, DateTimeKind.Utc).AddTicks(3455), new DateTime(2025, 6, 16, 6, 44, 5, 871, DateTimeKind.Utc).AddTicks(3456) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 6, 44, 5, 871, DateTimeKind.Utc).AddTicks(3458), new DateTime(2025, 6, 16, 6, 44, 5, 871, DateTimeKind.Utc).AddTicks(3458) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 6, 44, 5, 871, DateTimeKind.Utc).AddTicks(3460), new DateTime(2025, 6, 16, 6, 44, 5, 871, DateTimeKind.Utc).AddTicks(3461) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 6, 44, 5, 871, DateTimeKind.Utc).AddTicks(3463), new DateTime(2025, 6, 16, 6, 44, 5, 871, DateTimeKind.Utc).AddTicks(3463) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 6, 44, 5, 871, DateTimeKind.Utc).AddTicks(3465), new DateTime(2025, 6, 16, 6, 44, 5, 871, DateTimeKind.Utc).AddTicks(3466) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 6, 44, 5, 871, DateTimeKind.Utc).AddTicks(3468), new DateTime(2025, 6, 16, 6, 44, 5, 871, DateTimeKind.Utc).AddTicks(3468) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 6, 44, 5, 871, DateTimeKind.Utc).AddTicks(3470), new DateTime(2025, 6, 16, 6, 44, 5, 871, DateTimeKind.Utc).AddTicks(3471) });

            migrationBuilder.UpdateData(
                table: "ProjectSites",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 6, 44, 5, 871, DateTimeKind.Utc).AddTicks(3631), new DateTime(2025, 6, 16, 6, 44, 5, 871, DateTimeKind.Utc).AddTicks(3632) });

            migrationBuilder.UpdateData(
                table: "ProjectSites",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 6, 44, 5, 871, DateTimeKind.Utc).AddTicks(3636), new DateTime(2025, 6, 16, 6, 44, 5, 871, DateTimeKind.Utc).AddTicks(3637) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 6, 44, 5, 871, DateTimeKind.Utc).AddTicks(3573), new DateTime(2025, 6, 16, 6, 44, 5, 871, DateTimeKind.Utc).AddTicks(3574) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 6, 44, 5, 871, DateTimeKind.Utc).AddTicks(3580), new DateTime(2025, 6, 16, 6, 44, 5, 871, DateTimeKind.Utc).AddTicks(3581) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 12, 16, 6, 44, 5, 871, DateTimeKind.Utc).AddTicks(3586), new DateTime(2025, 5, 16, 6, 44, 5, 871, DateTimeKind.Utc).AddTicks(3594) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 6, 44, 5, 871, DateTimeKind.Utc).AddTicks(3176), new DateTime(2025, 6, 16, 6, 44, 5, 871, DateTimeKind.Utc).AddTicks(3179) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 6, 44, 5, 871, DateTimeKind.Utc).AddTicks(3186), new DateTime(2025, 6, 16, 6, 44, 5, 871, DateTimeKind.Utc).AddTicks(3187) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 6, 44, 5, 871, DateTimeKind.Utc).AddTicks(3190), new DateTime(2025, 6, 16, 6, 44, 5, 871, DateTimeKind.Utc).AddTicks(3190) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 6, 44, 5, 871, DateTimeKind.Utc).AddTicks(3193), new DateTime(2025, 6, 16, 6, 44, 5, 871, DateTimeKind.Utc).AddTicks(3193) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 6, 44, 5, 871, DateTimeKind.Utc).AddTicks(3518), new DateTime(2025, 6, 16, 6, 44, 5, 871, DateTimeKind.Utc).AddTicks(3519) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 6, 42, 31, 682, DateTimeKind.Utc).AddTicks(7787), new DateTime(2025, 6, 16, 6, 42, 31, 682, DateTimeKind.Utc).AddTicks(7787) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 6, 42, 31, 682, DateTimeKind.Utc).AddTicks(7793), new DateTime(2025, 6, 16, 6, 42, 31, 682, DateTimeKind.Utc).AddTicks(7793) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 6, 42, 31, 682, DateTimeKind.Utc).AddTicks(7797), new DateTime(2025, 6, 16, 6, 42, 31, 682, DateTimeKind.Utc).AddTicks(7797) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 6, 42, 31, 682, DateTimeKind.Utc).AddTicks(7799), new DateTime(2025, 6, 16, 6, 42, 31, 682, DateTimeKind.Utc).AddTicks(7800) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 6, 42, 31, 682, DateTimeKind.Utc).AddTicks(7802), new DateTime(2025, 6, 16, 6, 42, 31, 682, DateTimeKind.Utc).AddTicks(7802) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 6, 42, 31, 682, DateTimeKind.Utc).AddTicks(7805), new DateTime(2025, 6, 16, 6, 42, 31, 682, DateTimeKind.Utc).AddTicks(7805) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 6, 42, 31, 682, DateTimeKind.Utc).AddTicks(7807), new DateTime(2025, 6, 16, 6, 42, 31, 682, DateTimeKind.Utc).AddTicks(7808) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 6, 42, 31, 682, DateTimeKind.Utc).AddTicks(7810), new DateTime(2025, 6, 16, 6, 42, 31, 682, DateTimeKind.Utc).AddTicks(7810) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 6, 42, 31, 682, DateTimeKind.Utc).AddTicks(7812), new DateTime(2025, 6, 16, 6, 42, 31, 682, DateTimeKind.Utc).AddTicks(7813) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 6, 42, 31, 682, DateTimeKind.Utc).AddTicks(7815), new DateTime(2025, 6, 16, 6, 42, 31, 682, DateTimeKind.Utc).AddTicks(7815) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 6, 42, 31, 682, DateTimeKind.Utc).AddTicks(7817), new DateTime(2025, 6, 16, 6, 42, 31, 682, DateTimeKind.Utc).AddTicks(7818) });

            migrationBuilder.UpdateData(
                table: "ProjectSites",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 6, 42, 31, 682, DateTimeKind.Utc).AddTicks(7972), new DateTime(2025, 6, 16, 6, 42, 31, 682, DateTimeKind.Utc).AddTicks(7973) });

            migrationBuilder.UpdateData(
                table: "ProjectSites",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 6, 42, 31, 682, DateTimeKind.Utc).AddTicks(7978), new DateTime(2025, 6, 16, 6, 42, 31, 682, DateTimeKind.Utc).AddTicks(7979) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 6, 42, 31, 682, DateTimeKind.Utc).AddTicks(7914), new DateTime(2025, 6, 16, 6, 42, 31, 682, DateTimeKind.Utc).AddTicks(7915) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 6, 42, 31, 682, DateTimeKind.Utc).AddTicks(7921), new DateTime(2025, 6, 16, 6, 42, 31, 682, DateTimeKind.Utc).AddTicks(7922) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 12, 16, 6, 42, 31, 682, DateTimeKind.Utc).AddTicks(7927), new DateTime(2025, 5, 16, 6, 42, 31, 682, DateTimeKind.Utc).AddTicks(7935) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 6, 42, 31, 682, DateTimeKind.Utc).AddTicks(7592), new DateTime(2025, 6, 16, 6, 42, 31, 682, DateTimeKind.Utc).AddTicks(7594) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 6, 42, 31, 682, DateTimeKind.Utc).AddTicks(7601), new DateTime(2025, 6, 16, 6, 42, 31, 682, DateTimeKind.Utc).AddTicks(7601) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 6, 42, 31, 682, DateTimeKind.Utc).AddTicks(7604), new DateTime(2025, 6, 16, 6, 42, 31, 682, DateTimeKind.Utc).AddTicks(7605) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 6, 42, 31, 682, DateTimeKind.Utc).AddTicks(7607), new DateTime(2025, 6, 16, 6, 42, 31, 682, DateTimeKind.Utc).AddTicks(7607) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 6, 42, 31, 682, DateTimeKind.Utc).AddTicks(7862), new DateTime(2025, 6, 16, 6, 42, 31, 682, DateTimeKind.Utc).AddTicks(7863) });
        }
    }
}
