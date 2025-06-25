using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRegionEntityAndUpdateProjectMachineFixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Region",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "CurrentLocation",
                table: "Machines");

            migrationBuilder.AddColumn<int>(
                name: "RegionId",
                table: "Projects",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "RegionId",
                table: "Machines",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Regions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regions", x => x.Id);
                });

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

            migrationBuilder.InsertData(
                table: "Regions",
                columns: new[] { "Id", "CreatedAt", "Description", "IsActive", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 6, 23, 13, 22, 45, 389, DateTimeKind.Utc).AddTicks(9173), "Muscat Governorate", true, "Muscat", new DateTime(2025, 6, 23, 13, 22, 45, 389, DateTimeKind.Utc).AddTicks(9176) },
                    { 2, new DateTime(2025, 6, 23, 13, 22, 45, 389, DateTimeKind.Utc).AddTicks(9180), "Dhofar Governorate", true, "Dhofar", new DateTime(2025, 6, 23, 13, 22, 45, 389, DateTimeKind.Utc).AddTicks(9181) },
                    { 3, new DateTime(2025, 6, 23, 13, 22, 45, 389, DateTimeKind.Utc).AddTicks(9184), "Musandam Governorate", true, "Musandam", new DateTime(2025, 6, 23, 13, 22, 45, 389, DateTimeKind.Utc).AddTicks(9184) },
                    { 4, new DateTime(2025, 6, 23, 13, 22, 45, 389, DateTimeKind.Utc).AddTicks(9187), "Al Buraimi Governorate", true, "Al Buraimi", new DateTime(2025, 6, 23, 13, 22, 45, 389, DateTimeKind.Utc).AddTicks(9188) },
                    { 5, new DateTime(2025, 6, 23, 13, 22, 45, 389, DateTimeKind.Utc).AddTicks(9191), "Al Dakhiliyah Governorate", true, "Al Dakhiliyah", new DateTime(2025, 6, 23, 13, 22, 45, 389, DateTimeKind.Utc).AddTicks(9191) },
                    { 6, new DateTime(2025, 6, 23, 13, 22, 45, 389, DateTimeKind.Utc).AddTicks(9194), "Al Dhahirah Governorate", true, "Al Dhahirah", new DateTime(2025, 6, 23, 13, 22, 45, 389, DateTimeKind.Utc).AddTicks(9195) },
                    { 7, new DateTime(2025, 6, 23, 13, 22, 45, 389, DateTimeKind.Utc).AddTicks(9198), "Al Wusta Governorate", true, "Al Wusta", new DateTime(2025, 6, 23, 13, 22, 45, 389, DateTimeKind.Utc).AddTicks(9198) },
                    { 8, new DateTime(2025, 6, 23, 13, 22, 45, 389, DateTimeKind.Utc).AddTicks(9201), "Al Batinah North Governorate", true, "Al Batinah North", new DateTime(2025, 6, 23, 13, 22, 45, 389, DateTimeKind.Utc).AddTicks(9202) },
                    { 9, new DateTime(2025, 6, 23, 13, 22, 45, 389, DateTimeKind.Utc).AddTicks(9204), "Al Batinah South Governorate", true, "Al Batinah South", new DateTime(2025, 6, 23, 13, 22, 45, 389, DateTimeKind.Utc).AddTicks(9205) },
                    { 10, new DateTime(2025, 6, 23, 13, 22, 45, 389, DateTimeKind.Utc).AddTicks(9208), "Ash Sharqiyah North Governorate", true, "Ash Sharqiyah North", new DateTime(2025, 6, 23, 13, 22, 45, 389, DateTimeKind.Utc).AddTicks(9208) },
                    { 11, new DateTime(2025, 6, 23, 13, 22, 45, 389, DateTimeKind.Utc).AddTicks(9211), "Ash Sharqiyah South Governorate", true, "Ash Sharqiyah South", new DateTime(2025, 6, 23, 13, 22, 45, 389, DateTimeKind.Utc).AddTicks(9212) }
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_Projects_RegionId",
                table: "Projects",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_Machines_RegionId",
                table: "Machines",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_Regions_IsActive",
                table: "Regions",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Regions_Name",
                table: "Regions",
                column: "Name",
                unique: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Machines_Regions_RegionId",
                table: "Machines");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Regions_RegionId",
                table: "Projects");

            migrationBuilder.DropTable(
                name: "Regions");

            migrationBuilder.DropIndex(
                name: "IX_Projects_RegionId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Machines_RegionId",
                table: "Machines");

            migrationBuilder.DropColumn(
                name: "RegionId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "RegionId",
                table: "Machines");

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
                values: new object[] { new DateTime(2025, 6, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9337), new DateTime(2025, 6, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9338) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9343), new DateTime(2025, 6, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9343) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9346), new DateTime(2025, 6, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9346) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9349), new DateTime(2025, 6, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9349) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9352), new DateTime(2025, 6, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9352) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9354), new DateTime(2025, 6, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9355) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9357), new DateTime(2025, 6, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9357) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9360), new DateTime(2025, 6, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9360) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9362), new DateTime(2025, 6, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9363) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9365), new DateTime(2025, 6, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9365) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9367), new DateTime(2025, 6, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9368) });

            migrationBuilder.UpdateData(
                table: "ProjectSites",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9546), new DateTime(2025, 6, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9547) });

            migrationBuilder.UpdateData(
                table: "ProjectSites",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9552), new DateTime(2025, 6, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9553) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Region", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9478), "Muscat", new DateTime(2025, 6, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9479) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "Region", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9485), "Dhofar", new DateTime(2025, 6, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9486) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "Region", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 12, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9491), "Al Batinah North", new DateTime(2025, 5, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9503) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9005), new DateTime(2025, 6, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9010) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9017), new DateTime(2025, 6, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9017) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9021), new DateTime(2025, 6, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9021) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9024), new DateTime(2025, 6, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9024) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9026), new DateTime(2025, 6, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9027) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9029), new DateTime(2025, 6, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9030) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9032), new DateTime(2025, 6, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9032) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9418), new DateTime(2025, 6, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9418) });
        }
    }
}
