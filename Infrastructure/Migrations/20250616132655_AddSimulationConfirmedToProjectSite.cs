using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSimulationConfirmedToProjectSite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSimulationConfirmed",
                table: "ProjectSites",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 13, 26, 55, 41, DateTimeKind.Utc).AddTicks(7885), new DateTime(2025, 6, 16, 13, 26, 55, 41, DateTimeKind.Utc).AddTicks(7886) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 13, 26, 55, 41, DateTimeKind.Utc).AddTicks(7891), new DateTime(2025, 6, 16, 13, 26, 55, 41, DateTimeKind.Utc).AddTicks(7892) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 13, 26, 55, 41, DateTimeKind.Utc).AddTicks(7895), new DateTime(2025, 6, 16, 13, 26, 55, 41, DateTimeKind.Utc).AddTicks(7895) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 13, 26, 55, 41, DateTimeKind.Utc).AddTicks(7897), new DateTime(2025, 6, 16, 13, 26, 55, 41, DateTimeKind.Utc).AddTicks(7898) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 13, 26, 55, 41, DateTimeKind.Utc).AddTicks(7901), new DateTime(2025, 6, 16, 13, 26, 55, 41, DateTimeKind.Utc).AddTicks(7901) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 13, 26, 55, 41, DateTimeKind.Utc).AddTicks(7903), new DateTime(2025, 6, 16, 13, 26, 55, 41, DateTimeKind.Utc).AddTicks(7904) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 13, 26, 55, 41, DateTimeKind.Utc).AddTicks(7906), new DateTime(2025, 6, 16, 13, 26, 55, 41, DateTimeKind.Utc).AddTicks(7907) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 13, 26, 55, 41, DateTimeKind.Utc).AddTicks(7909), new DateTime(2025, 6, 16, 13, 26, 55, 41, DateTimeKind.Utc).AddTicks(7909) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 13, 26, 55, 41, DateTimeKind.Utc).AddTicks(7911), new DateTime(2025, 6, 16, 13, 26, 55, 41, DateTimeKind.Utc).AddTicks(7912) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 13, 26, 55, 41, DateTimeKind.Utc).AddTicks(7914), new DateTime(2025, 6, 16, 13, 26, 55, 41, DateTimeKind.Utc).AddTicks(7914) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 13, 26, 55, 41, DateTimeKind.Utc).AddTicks(7916), new DateTime(2025, 6, 16, 13, 26, 55, 41, DateTimeKind.Utc).AddTicks(7917) });

            migrationBuilder.UpdateData(
                table: "ProjectSites",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "IsSimulationConfirmed", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 13, 26, 55, 41, DateTimeKind.Utc).AddTicks(8086), false, new DateTime(2025, 6, 16, 13, 26, 55, 41, DateTimeKind.Utc).AddTicks(8087) });

            migrationBuilder.UpdateData(
                table: "ProjectSites",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "IsSimulationConfirmed", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 13, 26, 55, 41, DateTimeKind.Utc).AddTicks(8092), false, new DateTime(2025, 6, 16, 13, 26, 55, 41, DateTimeKind.Utc).AddTicks(8093) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 13, 26, 55, 41, DateTimeKind.Utc).AddTicks(8022), new DateTime(2025, 6, 16, 13, 26, 55, 41, DateTimeKind.Utc).AddTicks(8022) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 13, 26, 55, 41, DateTimeKind.Utc).AddTicks(8029), new DateTime(2025, 6, 16, 13, 26, 55, 41, DateTimeKind.Utc).AddTicks(8030) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 12, 16, 13, 26, 55, 41, DateTimeKind.Utc).AddTicks(8036), new DateTime(2025, 5, 16, 13, 26, 55, 41, DateTimeKind.Utc).AddTicks(8047) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 13, 26, 55, 41, DateTimeKind.Utc).AddTicks(7567), new DateTime(2025, 6, 16, 13, 26, 55, 41, DateTimeKind.Utc).AddTicks(7574) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 13, 26, 55, 41, DateTimeKind.Utc).AddTicks(7581), new DateTime(2025, 6, 16, 13, 26, 55, 41, DateTimeKind.Utc).AddTicks(7581) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 13, 26, 55, 41, DateTimeKind.Utc).AddTicks(7584), new DateTime(2025, 6, 16, 13, 26, 55, 41, DateTimeKind.Utc).AddTicks(7585) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 13, 26, 55, 41, DateTimeKind.Utc).AddTicks(7587), new DateTime(2025, 6, 16, 13, 26, 55, 41, DateTimeKind.Utc).AddTicks(7588) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 13, 26, 55, 41, DateTimeKind.Utc).AddTicks(7965), new DateTime(2025, 6, 16, 13, 26, 55, 41, DateTimeKind.Utc).AddTicks(7966) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSimulationConfirmed",
                table: "ProjectSites");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 7, 43, 28, 366, DateTimeKind.Utc).AddTicks(3345), new DateTime(2025, 6, 16, 7, 43, 28, 366, DateTimeKind.Utc).AddTicks(3345) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 7, 43, 28, 366, DateTimeKind.Utc).AddTicks(3350), new DateTime(2025, 6, 16, 7, 43, 28, 366, DateTimeKind.Utc).AddTicks(3351) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 7, 43, 28, 366, DateTimeKind.Utc).AddTicks(3354), new DateTime(2025, 6, 16, 7, 43, 28, 366, DateTimeKind.Utc).AddTicks(3354) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 7, 43, 28, 366, DateTimeKind.Utc).AddTicks(3357), new DateTime(2025, 6, 16, 7, 43, 28, 366, DateTimeKind.Utc).AddTicks(3357) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 7, 43, 28, 366, DateTimeKind.Utc).AddTicks(3360), new DateTime(2025, 6, 16, 7, 43, 28, 366, DateTimeKind.Utc).AddTicks(3360) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 7, 43, 28, 366, DateTimeKind.Utc).AddTicks(3362), new DateTime(2025, 6, 16, 7, 43, 28, 366, DateTimeKind.Utc).AddTicks(3363) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 7, 43, 28, 366, DateTimeKind.Utc).AddTicks(3365), new DateTime(2025, 6, 16, 7, 43, 28, 366, DateTimeKind.Utc).AddTicks(3365) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 7, 43, 28, 366, DateTimeKind.Utc).AddTicks(3367), new DateTime(2025, 6, 16, 7, 43, 28, 366, DateTimeKind.Utc).AddTicks(3368) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 7, 43, 28, 366, DateTimeKind.Utc).AddTicks(3370), new DateTime(2025, 6, 16, 7, 43, 28, 366, DateTimeKind.Utc).AddTicks(3370) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 7, 43, 28, 366, DateTimeKind.Utc).AddTicks(3372), new DateTime(2025, 6, 16, 7, 43, 28, 366, DateTimeKind.Utc).AddTicks(3373) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 7, 43, 28, 366, DateTimeKind.Utc).AddTicks(3375), new DateTime(2025, 6, 16, 7, 43, 28, 366, DateTimeKind.Utc).AddTicks(3375) });

            migrationBuilder.UpdateData(
                table: "ProjectSites",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 7, 43, 28, 366, DateTimeKind.Utc).AddTicks(3535), new DateTime(2025, 6, 16, 7, 43, 28, 366, DateTimeKind.Utc).AddTicks(3536) });

            migrationBuilder.UpdateData(
                table: "ProjectSites",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 7, 43, 28, 366, DateTimeKind.Utc).AddTicks(3541), new DateTime(2025, 6, 16, 7, 43, 28, 366, DateTimeKind.Utc).AddTicks(3542) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 7, 43, 28, 366, DateTimeKind.Utc).AddTicks(3477), new DateTime(2025, 6, 16, 7, 43, 28, 366, DateTimeKind.Utc).AddTicks(3478) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 7, 43, 28, 366, DateTimeKind.Utc).AddTicks(3484), new DateTime(2025, 6, 16, 7, 43, 28, 366, DateTimeKind.Utc).AddTicks(3485) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 12, 16, 7, 43, 28, 366, DateTimeKind.Utc).AddTicks(3491), new DateTime(2025, 5, 16, 7, 43, 28, 366, DateTimeKind.Utc).AddTicks(3500) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 7, 43, 28, 366, DateTimeKind.Utc).AddTicks(3092), new DateTime(2025, 6, 16, 7, 43, 28, 366, DateTimeKind.Utc).AddTicks(3094) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 7, 43, 28, 366, DateTimeKind.Utc).AddTicks(3101), new DateTime(2025, 6, 16, 7, 43, 28, 366, DateTimeKind.Utc).AddTicks(3101) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 7, 43, 28, 366, DateTimeKind.Utc).AddTicks(3104), new DateTime(2025, 6, 16, 7, 43, 28, 366, DateTimeKind.Utc).AddTicks(3104) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 7, 43, 28, 366, DateTimeKind.Utc).AddTicks(3107), new DateTime(2025, 6, 16, 7, 43, 28, 366, DateTimeKind.Utc).AddTicks(3107) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 7, 43, 28, 366, DateTimeKind.Utc).AddTicks(3423), new DateTime(2025, 6, 16, 7, 43, 28, 366, DateTimeKind.Utc).AddTicks(3424) });
        }
    }
}
