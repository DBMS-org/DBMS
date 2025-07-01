using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPasswordResetFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PasswordResetCode",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PasswordResetCodeExpiry",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6075), new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6075) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6082), new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6083) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6086), new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6086) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6089), new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6089) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6091), new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6092) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6094), new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6094) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6097), new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6097) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6099), new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6099) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6102), new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6102) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6104), new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6104) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6107), new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6107) });

            migrationBuilder.UpdateData(
                table: "ProjectSites",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6291), new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6292) });

            migrationBuilder.UpdateData(
                table: "ProjectSites",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6297), new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6297) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6222), new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6223) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6234), new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6235) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 12, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6241), new DateTime(2025, 5, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6251) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6394), new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6395) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6399), new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6400) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6403), new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6404) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6406), new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6407) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6410), new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6411) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6414), new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6414) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6417), new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6418) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6421), new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6421) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6424), new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6425) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6427), new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6428) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6431), new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6431) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(5771), new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(5774) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(5782), new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(5783) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(5788), new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(5788) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(5793), new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(5794) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(5797), new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(5798) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(5801), new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(5802) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(5804), new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(5804) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordResetCode", "PasswordResetCodeExpiry", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6161), null, null, new DateTime(2025, 6, 30, 11, 24, 58, 302, DateTimeKind.Utc).AddTicks(6162) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordResetCode",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PasswordResetCodeExpiry",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3279), new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3279) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3289), new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3290) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3293), new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3293) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3296), new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3296) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3298), new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3299) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3301), new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3301) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3303), new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3304) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3306), new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3306) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3308), new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3309) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3311), new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3311) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3313), new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3314) });

            migrationBuilder.UpdateData(
                table: "ProjectSites",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3593), new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3594) });

            migrationBuilder.UpdateData(
                table: "ProjectSites",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3598), new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3599) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3442), new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3443) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3451), new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3451) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 12, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3458), new DateTime(2025, 5, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3475) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3643), new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3644) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3648), new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3649) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3652), new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3653) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3656), new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3656) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3659), new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3660) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3663), new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3663) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3666), new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3667) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3670), new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3670) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3673), new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3674) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3676), new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3677) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3680), new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3680) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(2974), new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(2977) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(2986), new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(2986) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(2990), new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(2990) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(2992), new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(2993) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(2995), new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(2995) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(2997), new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(2998) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3000), new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3001) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3368), new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3369) });
        }
    }
}
