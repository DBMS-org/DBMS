using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PatternApprovalFlag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPatternApproved",
                table: "ProjectSites",
                type: "bit",
                nullable: false,
                defaultValue: false);

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
                columns: new[] { "CreatedAt", "IsPatternApproved", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 7, 43, 28, 366, DateTimeKind.Utc).AddTicks(3535), false, new DateTime(2025, 6, 16, 7, 43, 28, 366, DateTimeKind.Utc).AddTicks(3536) });

            migrationBuilder.UpdateData(
                table: "ProjectSites",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "IsPatternApproved", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 7, 43, 28, 366, DateTimeKind.Utc).AddTicks(3541), false, new DateTime(2025, 6, 16, 7, 43, 28, 366, DateTimeKind.Utc).AddTicks(3542) });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPatternApproved",
                table: "ProjectSites");

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
    }
}
