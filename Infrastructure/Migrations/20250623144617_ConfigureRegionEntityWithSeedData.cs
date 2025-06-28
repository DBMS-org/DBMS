using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ConfigureRegionEntityWithSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Machines_Regions_RegionId",
                table: "Machines");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Regions_RegionId",
                table: "Projects");

            migrationBuilder.AlterColumn<int>(
                name: "RegionId",
                table: "Projects",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 1);

            migrationBuilder.AddColumn<string>(
                name: "Region",
                table: "Projects",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CurrentLocation",
                table: "Machines",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(5832), new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(5832) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(5837), new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(5838) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(5934), new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(5935) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(5938), new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(5938) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(5941), new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(5941) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(5944), new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(5944) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(5946), new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(5947) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(5949), new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(5949) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(5952), new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(5952) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(5954), new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(5954) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(5957), new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(5957) });

            migrationBuilder.UpdateData(
                table: "ProjectSites",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(6146), new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(6147) });

            migrationBuilder.UpdateData(
                table: "ProjectSites",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(6152), new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(6153) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Region", "RegionId", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(6078), "Muscat", null, new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(6078) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "Region", "RegionId", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(6085), "Dhofar", null, new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(6086) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "Region", "RegionId", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 12, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(6091), "Al Batinah North", null, new DateTime(2025, 5, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(6102) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Description", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(6189), "Capital Governorate", new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(6190) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "Description", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(6194), "Southern Governorate", new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(6194) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "Description", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(6198), "Northern Governorate", new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(6198) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "Description", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(6201), "Western Governorate", new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(6202) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "Description", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(6205), "Interior Governorate", new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(6205) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(6208), new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(6209) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "Description", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(6212), "Central Governorate", new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(6212) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "Description", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(6215), "Northern Batinah Governorate", new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(6216) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "Description", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(6219), "Southern Batinah Governorate", new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(6219) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "Description", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(6222), "Northern Sharqiyah Governorate", new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(6223) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "Description", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(6226), "Southern Sharqiyah Governorate", new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(6226) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(5529), new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(5535) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(5542), new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(5542) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(5545), new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(5546) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(5548), new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(5549) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(5551), new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(5551) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(5554), new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(5554) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(5556), new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(5557) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(6008), new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(6008) });

            migrationBuilder.AddForeignKey(
                name: "FK_Machines_Regions_RegionId",
                table: "Machines",
                column: "RegionId",
                principalTable: "Regions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Regions_RegionId",
                table: "Projects",
                column: "RegionId",
                principalTable: "Regions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Machines_Regions_RegionId",
                table: "Machines");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Regions_RegionId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Region",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "CurrentLocation",
                table: "Machines");

            migrationBuilder.AlterColumn<int>(
                name: "RegionId",
                table: "Projects",
                type: "int",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 13, 22, 45, 389, DateTimeKind.Utc).AddTicks(9033), new DateTime(2025, 6, 23, 13, 22, 45, 389, DateTimeKind.Utc).AddTicks(9034) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 13, 22, 45, 389, DateTimeKind.Utc).AddTicks(9042), new DateTime(2025, 6, 23, 13, 22, 45, 389, DateTimeKind.Utc).AddTicks(9042) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 13, 22, 45, 389, DateTimeKind.Utc).AddTicks(9045), new DateTime(2025, 6, 23, 13, 22, 45, 389, DateTimeKind.Utc).AddTicks(9045) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 13, 22, 45, 389, DateTimeKind.Utc).AddTicks(9048), new DateTime(2025, 6, 23, 13, 22, 45, 389, DateTimeKind.Utc).AddTicks(9048) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 13, 22, 45, 389, DateTimeKind.Utc).AddTicks(9051), new DateTime(2025, 6, 23, 13, 22, 45, 389, DateTimeKind.Utc).AddTicks(9051) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 13, 22, 45, 389, DateTimeKind.Utc).AddTicks(9054), new DateTime(2025, 6, 23, 13, 22, 45, 389, DateTimeKind.Utc).AddTicks(9054) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 13, 22, 45, 389, DateTimeKind.Utc).AddTicks(9056), new DateTime(2025, 6, 23, 13, 22, 45, 389, DateTimeKind.Utc).AddTicks(9057) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 13, 22, 45, 389, DateTimeKind.Utc).AddTicks(9059), new DateTime(2025, 6, 23, 13, 22, 45, 389, DateTimeKind.Utc).AddTicks(9059) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 13, 22, 45, 389, DateTimeKind.Utc).AddTicks(9061), new DateTime(2025, 6, 23, 13, 22, 45, 389, DateTimeKind.Utc).AddTicks(9062) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 13, 22, 45, 389, DateTimeKind.Utc).AddTicks(9064), new DateTime(2025, 6, 23, 13, 22, 45, 389, DateTimeKind.Utc).AddTicks(9064) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 13, 22, 45, 389, DateTimeKind.Utc).AddTicks(9066), new DateTime(2025, 6, 23, 13, 22, 45, 389, DateTimeKind.Utc).AddTicks(9067) });

            migrationBuilder.UpdateData(
                table: "ProjectSites",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 13, 22, 45, 389, DateTimeKind.Utc).AddTicks(9416), new DateTime(2025, 6, 23, 13, 22, 45, 389, DateTimeKind.Utc).AddTicks(9416) });

            migrationBuilder.UpdateData(
                table: "ProjectSites",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 13, 22, 45, 389, DateTimeKind.Utc).AddTicks(9421), new DateTime(2025, 6, 23, 13, 22, 45, 389, DateTimeKind.Utc).AddTicks(9422) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "RegionId", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 13, 22, 45, 389, DateTimeKind.Utc).AddTicks(9285), 1, new DateTime(2025, 6, 23, 13, 22, 45, 389, DateTimeKind.Utc).AddTicks(9286) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "RegionId", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 13, 22, 45, 389, DateTimeKind.Utc).AddTicks(9292), 2, new DateTime(2025, 6, 23, 13, 22, 45, 389, DateTimeKind.Utc).AddTicks(9293) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "RegionId", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 12, 23, 13, 22, 45, 389, DateTimeKind.Utc).AddTicks(9302), 8, new DateTime(2025, 5, 23, 13, 22, 45, 389, DateTimeKind.Utc).AddTicks(9314) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Description", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 13, 22, 45, 389, DateTimeKind.Utc).AddTicks(9173), "Muscat Governorate", new DateTime(2025, 6, 23, 13, 22, 45, 389, DateTimeKind.Utc).AddTicks(9176) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "Description", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 13, 22, 45, 389, DateTimeKind.Utc).AddTicks(9180), "Dhofar Governorate", new DateTime(2025, 6, 23, 13, 22, 45, 389, DateTimeKind.Utc).AddTicks(9181) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "Description", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 13, 22, 45, 389, DateTimeKind.Utc).AddTicks(9184), "Musandam Governorate", new DateTime(2025, 6, 23, 13, 22, 45, 389, DateTimeKind.Utc).AddTicks(9184) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "Description", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 13, 22, 45, 389, DateTimeKind.Utc).AddTicks(9187), "Al Buraimi Governorate", new DateTime(2025, 6, 23, 13, 22, 45, 389, DateTimeKind.Utc).AddTicks(9188) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "Description", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 13, 22, 45, 389, DateTimeKind.Utc).AddTicks(9191), "Al Dakhiliyah Governorate", new DateTime(2025, 6, 23, 13, 22, 45, 389, DateTimeKind.Utc).AddTicks(9191) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 13, 22, 45, 389, DateTimeKind.Utc).AddTicks(9194), new DateTime(2025, 6, 23, 13, 22, 45, 389, DateTimeKind.Utc).AddTicks(9195) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "Description", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 13, 22, 45, 389, DateTimeKind.Utc).AddTicks(9198), "Al Wusta Governorate", new DateTime(2025, 6, 23, 13, 22, 45, 389, DateTimeKind.Utc).AddTicks(9198) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "Description", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 13, 22, 45, 389, DateTimeKind.Utc).AddTicks(9201), "Al Batinah North Governorate", new DateTime(2025, 6, 23, 13, 22, 45, 389, DateTimeKind.Utc).AddTicks(9202) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "Description", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 13, 22, 45, 389, DateTimeKind.Utc).AddTicks(9204), "Al Batinah South Governorate", new DateTime(2025, 6, 23, 13, 22, 45, 389, DateTimeKind.Utc).AddTicks(9205) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "Description", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 13, 22, 45, 389, DateTimeKind.Utc).AddTicks(9208), "Ash Sharqiyah North Governorate", new DateTime(2025, 6, 23, 13, 22, 45, 389, DateTimeKind.Utc).AddTicks(9208) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "Description", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 13, 22, 45, 389, DateTimeKind.Utc).AddTicks(9211), "Ash Sharqiyah South Governorate", new DateTime(2025, 6, 23, 13, 22, 45, 389, DateTimeKind.Utc).AddTicks(9212) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 13, 22, 45, 389, DateTimeKind.Utc).AddTicks(8760), new DateTime(2025, 6, 23, 13, 22, 45, 389, DateTimeKind.Utc).AddTicks(8766) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 13, 22, 45, 389, DateTimeKind.Utc).AddTicks(8775), new DateTime(2025, 6, 23, 13, 22, 45, 389, DateTimeKind.Utc).AddTicks(8775) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 13, 22, 45, 389, DateTimeKind.Utc).AddTicks(8778), new DateTime(2025, 6, 23, 13, 22, 45, 389, DateTimeKind.Utc).AddTicks(8779) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 13, 22, 45, 389, DateTimeKind.Utc).AddTicks(8781), new DateTime(2025, 6, 23, 13, 22, 45, 389, DateTimeKind.Utc).AddTicks(8781) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 13, 22, 45, 389, DateTimeKind.Utc).AddTicks(8784), new DateTime(2025, 6, 23, 13, 22, 45, 389, DateTimeKind.Utc).AddTicks(8784) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 13, 22, 45, 389, DateTimeKind.Utc).AddTicks(8786), new DateTime(2025, 6, 23, 13, 22, 45, 389, DateTimeKind.Utc).AddTicks(8786) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 13, 22, 45, 389, DateTimeKind.Utc).AddTicks(8788), new DateTime(2025, 6, 23, 13, 22, 45, 389, DateTimeKind.Utc).AddTicks(8789) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 13, 22, 45, 389, DateTimeKind.Utc).AddTicks(9120), new DateTime(2025, 6, 23, 13, 22, 45, 389, DateTimeKind.Utc).AddTicks(9121) });

            migrationBuilder.AddForeignKey(
                name: "FK_Machines_Regions_RegionId",
                table: "Machines",
                column: "RegionId",
                principalTable: "Regions",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Regions_RegionId",
                table: "Projects",
                column: "RegionId",
                principalTable: "Regions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
