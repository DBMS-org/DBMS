using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddProjectSiteContextToDrillHoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "DrillHoles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SiteId",
                table: "DrillHoles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 11, 8, 0, 34, 148, DateTimeKind.Utc).AddTicks(4146), new DateTime(2025, 6, 11, 8, 0, 34, 148, DateTimeKind.Utc).AddTicks(4146) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 11, 8, 0, 34, 148, DateTimeKind.Utc).AddTicks(4157), new DateTime(2025, 6, 11, 8, 0, 34, 148, DateTimeKind.Utc).AddTicks(4158) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 11, 8, 0, 34, 148, DateTimeKind.Utc).AddTicks(4161), new DateTime(2025, 6, 11, 8, 0, 34, 148, DateTimeKind.Utc).AddTicks(4161) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 11, 8, 0, 34, 148, DateTimeKind.Utc).AddTicks(4163), new DateTime(2025, 6, 11, 8, 0, 34, 148, DateTimeKind.Utc).AddTicks(4164) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 11, 8, 0, 34, 148, DateTimeKind.Utc).AddTicks(4166), new DateTime(2025, 6, 11, 8, 0, 34, 148, DateTimeKind.Utc).AddTicks(4166) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 11, 8, 0, 34, 148, DateTimeKind.Utc).AddTicks(4169), new DateTime(2025, 6, 11, 8, 0, 34, 148, DateTimeKind.Utc).AddTicks(4169) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 11, 8, 0, 34, 148, DateTimeKind.Utc).AddTicks(4171), new DateTime(2025, 6, 11, 8, 0, 34, 148, DateTimeKind.Utc).AddTicks(4172) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 11, 8, 0, 34, 148, DateTimeKind.Utc).AddTicks(4174), new DateTime(2025, 6, 11, 8, 0, 34, 148, DateTimeKind.Utc).AddTicks(4174) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 11, 8, 0, 34, 148, DateTimeKind.Utc).AddTicks(4176), new DateTime(2025, 6, 11, 8, 0, 34, 148, DateTimeKind.Utc).AddTicks(4177) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 11, 8, 0, 34, 148, DateTimeKind.Utc).AddTicks(4179), new DateTime(2025, 6, 11, 8, 0, 34, 148, DateTimeKind.Utc).AddTicks(4179) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 11, 8, 0, 34, 148, DateTimeKind.Utc).AddTicks(4181), new DateTime(2025, 6, 11, 8, 0, 34, 148, DateTimeKind.Utc).AddTicks(4182) });

            migrationBuilder.UpdateData(
                table: "ProjectSites",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 11, 8, 0, 34, 148, DateTimeKind.Utc).AddTicks(4514), new DateTime(2025, 6, 11, 8, 0, 34, 148, DateTimeKind.Utc).AddTicks(4515) });

            migrationBuilder.UpdateData(
                table: "ProjectSites",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 11, 8, 0, 34, 148, DateTimeKind.Utc).AddTicks(4520), new DateTime(2025, 6, 11, 8, 0, 34, 148, DateTimeKind.Utc).AddTicks(4521) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 11, 8, 0, 34, 148, DateTimeKind.Utc).AddTicks(4436), new DateTime(2025, 6, 11, 8, 0, 34, 148, DateTimeKind.Utc).AddTicks(4437) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 11, 8, 0, 34, 148, DateTimeKind.Utc).AddTicks(4444), new DateTime(2025, 6, 11, 8, 0, 34, 148, DateTimeKind.Utc).AddTicks(4444) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 12, 11, 8, 0, 34, 148, DateTimeKind.Utc).AddTicks(4450), new DateTime(2025, 5, 11, 8, 0, 34, 148, DateTimeKind.Utc).AddTicks(4462) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 11, 8, 0, 34, 148, DateTimeKind.Utc).AddTicks(3863), new DateTime(2025, 6, 11, 8, 0, 34, 148, DateTimeKind.Utc).AddTicks(3870) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 11, 8, 0, 34, 148, DateTimeKind.Utc).AddTicks(3883), new DateTime(2025, 6, 11, 8, 0, 34, 148, DateTimeKind.Utc).AddTicks(3884) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 11, 8, 0, 34, 148, DateTimeKind.Utc).AddTicks(3887), new DateTime(2025, 6, 11, 8, 0, 34, 148, DateTimeKind.Utc).AddTicks(3887) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 11, 8, 0, 34, 148, DateTimeKind.Utc).AddTicks(4241), new DateTime(2025, 6, 11, 8, 0, 34, 148, DateTimeKind.Utc).AddTicks(4242) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "DrillHoles");

            migrationBuilder.DropColumn(
                name: "SiteId",
                table: "DrillHoles");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 10, 7, 35, 24, 444, DateTimeKind.Utc).AddTicks(7547), new DateTime(2025, 6, 10, 7, 35, 24, 444, DateTimeKind.Utc).AddTicks(7547) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 10, 7, 35, 24, 444, DateTimeKind.Utc).AddTicks(7554), new DateTime(2025, 6, 10, 7, 35, 24, 444, DateTimeKind.Utc).AddTicks(7555) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 10, 7, 35, 24, 444, DateTimeKind.Utc).AddTicks(7558), new DateTime(2025, 6, 10, 7, 35, 24, 444, DateTimeKind.Utc).AddTicks(7558) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 10, 7, 35, 24, 444, DateTimeKind.Utc).AddTicks(7560), new DateTime(2025, 6, 10, 7, 35, 24, 444, DateTimeKind.Utc).AddTicks(7561) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 10, 7, 35, 24, 444, DateTimeKind.Utc).AddTicks(7563), new DateTime(2025, 6, 10, 7, 35, 24, 444, DateTimeKind.Utc).AddTicks(7563) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 10, 7, 35, 24, 444, DateTimeKind.Utc).AddTicks(7565), new DateTime(2025, 6, 10, 7, 35, 24, 444, DateTimeKind.Utc).AddTicks(7566) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 10, 7, 35, 24, 444, DateTimeKind.Utc).AddTicks(7568), new DateTime(2025, 6, 10, 7, 35, 24, 444, DateTimeKind.Utc).AddTicks(7568) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 10, 7, 35, 24, 444, DateTimeKind.Utc).AddTicks(7571), new DateTime(2025, 6, 10, 7, 35, 24, 444, DateTimeKind.Utc).AddTicks(7571) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 10, 7, 35, 24, 444, DateTimeKind.Utc).AddTicks(7573), new DateTime(2025, 6, 10, 7, 35, 24, 444, DateTimeKind.Utc).AddTicks(7573) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 10, 7, 35, 24, 444, DateTimeKind.Utc).AddTicks(7575), new DateTime(2025, 6, 10, 7, 35, 24, 444, DateTimeKind.Utc).AddTicks(7576) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 10, 7, 35, 24, 444, DateTimeKind.Utc).AddTicks(7578), new DateTime(2025, 6, 10, 7, 35, 24, 444, DateTimeKind.Utc).AddTicks(7578) });

            migrationBuilder.UpdateData(
                table: "ProjectSites",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 10, 7, 35, 24, 444, DateTimeKind.Utc).AddTicks(7750), new DateTime(2025, 6, 10, 7, 35, 24, 444, DateTimeKind.Utc).AddTicks(7751) });

            migrationBuilder.UpdateData(
                table: "ProjectSites",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 10, 7, 35, 24, 444, DateTimeKind.Utc).AddTicks(7755), new DateTime(2025, 6, 10, 7, 35, 24, 444, DateTimeKind.Utc).AddTicks(7756) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 10, 7, 35, 24, 444, DateTimeKind.Utc).AddTicks(7690), new DateTime(2025, 6, 10, 7, 35, 24, 444, DateTimeKind.Utc).AddTicks(7690) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 10, 7, 35, 24, 444, DateTimeKind.Utc).AddTicks(7696), new DateTime(2025, 6, 10, 7, 35, 24, 444, DateTimeKind.Utc).AddTicks(7697) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 12, 10, 7, 35, 24, 444, DateTimeKind.Utc).AddTicks(7703), new DateTime(2025, 5, 10, 7, 35, 24, 444, DateTimeKind.Utc).AddTicks(7714) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 10, 7, 35, 24, 444, DateTimeKind.Utc).AddTicks(7262), new DateTime(2025, 6, 10, 7, 35, 24, 444, DateTimeKind.Utc).AddTicks(7268) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 10, 7, 35, 24, 444, DateTimeKind.Utc).AddTicks(7275), new DateTime(2025, 6, 10, 7, 35, 24, 444, DateTimeKind.Utc).AddTicks(7275) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 10, 7, 35, 24, 444, DateTimeKind.Utc).AddTicks(7278), new DateTime(2025, 6, 10, 7, 35, 24, 444, DateTimeKind.Utc).AddTicks(7278) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 10, 7, 35, 24, 444, DateTimeKind.Utc).AddTicks(7633), new DateTime(2025, 6, 10, 7, 35, 24, 444, DateTimeKind.Utc).AddTicks(7634) });
        }
    }
}
