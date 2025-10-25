using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAccessoriesInventory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accessories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PartNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MinStockLevel = table.Column<int>(type: "int", nullable: false),
                    Supplier = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Location = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accessories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccessoryStockAdjustments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccessoryId = table.Column<int>(type: "int", nullable: false),
                    AdjustmentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuantityChanged = table.Column<int>(type: "int", nullable: false),
                    PreviousQuantity = table.Column<int>(type: "int", nullable: false),
                    NewQuantity = table.Column<int>(type: "int", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    AdjustedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AdjustedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessoryStockAdjustments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccessoryStockAdjustments_Accessories_AccessoryId",
                        column: x => x.AccessoryId,
                        principalTable: "Accessories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 25, 3, 24, 23, 450, DateTimeKind.Utc).AddTicks(8431), new DateTime(2025, 10, 25, 3, 24, 23, 450, DateTimeKind.Utc).AddTicks(8432) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 25, 3, 24, 23, 450, DateTimeKind.Utc).AddTicks(8438), new DateTime(2025, 10, 25, 3, 24, 23, 450, DateTimeKind.Utc).AddTicks(8439) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 25, 3, 24, 23, 450, DateTimeKind.Utc).AddTicks(8441), new DateTime(2025, 10, 25, 3, 24, 23, 450, DateTimeKind.Utc).AddTicks(8442) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 25, 3, 24, 23, 450, DateTimeKind.Utc).AddTicks(8444), new DateTime(2025, 10, 25, 3, 24, 23, 450, DateTimeKind.Utc).AddTicks(8444) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 25, 3, 24, 23, 450, DateTimeKind.Utc).AddTicks(8446), new DateTime(2025, 10, 25, 3, 24, 23, 450, DateTimeKind.Utc).AddTicks(8447) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 25, 3, 24, 23, 450, DateTimeKind.Utc).AddTicks(8449), new DateTime(2025, 10, 25, 3, 24, 23, 450, DateTimeKind.Utc).AddTicks(8449) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 25, 3, 24, 23, 450, DateTimeKind.Utc).AddTicks(8451), new DateTime(2025, 10, 25, 3, 24, 23, 450, DateTimeKind.Utc).AddTicks(8452) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 25, 3, 24, 23, 450, DateTimeKind.Utc).AddTicks(8454), new DateTime(2025, 10, 25, 3, 24, 23, 450, DateTimeKind.Utc).AddTicks(8463) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 25, 3, 24, 23, 450, DateTimeKind.Utc).AddTicks(8691), new DateTime(2025, 10, 25, 3, 24, 23, 450, DateTimeKind.Utc).AddTicks(8692) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 25, 3, 24, 23, 450, DateTimeKind.Utc).AddTicks(8699), new DateTime(2025, 10, 25, 3, 24, 23, 450, DateTimeKind.Utc).AddTicks(8700) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 25, 3, 24, 23, 450, DateTimeKind.Utc).AddTicks(8703), new DateTime(2025, 10, 25, 3, 24, 23, 450, DateTimeKind.Utc).AddTicks(8703) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 25, 3, 24, 23, 450, DateTimeKind.Utc).AddTicks(8706), new DateTime(2025, 10, 25, 3, 24, 23, 450, DateTimeKind.Utc).AddTicks(8706) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 25, 3, 24, 23, 450, DateTimeKind.Utc).AddTicks(8708), new DateTime(2025, 10, 25, 3, 24, 23, 450, DateTimeKind.Utc).AddTicks(8709) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 25, 3, 24, 23, 450, DateTimeKind.Utc).AddTicks(8711), new DateTime(2025, 10, 25, 3, 24, 23, 450, DateTimeKind.Utc).AddTicks(8711) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 25, 3, 24, 23, 450, DateTimeKind.Utc).AddTicks(8713), new DateTime(2025, 10, 25, 3, 24, 23, 450, DateTimeKind.Utc).AddTicks(8714) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 25, 3, 24, 23, 450, DateTimeKind.Utc).AddTicks(8716), new DateTime(2025, 10, 25, 3, 24, 23, 450, DateTimeKind.Utc).AddTicks(8716) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 25, 3, 24, 23, 450, DateTimeKind.Utc).AddTicks(8719), new DateTime(2025, 10, 25, 3, 24, 23, 450, DateTimeKind.Utc).AddTicks(8719) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 25, 3, 24, 23, 450, DateTimeKind.Utc).AddTicks(8721), new DateTime(2025, 10, 25, 3, 24, 23, 450, DateTimeKind.Utc).AddTicks(8721) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 25, 3, 24, 23, 450, DateTimeKind.Utc).AddTicks(8724), new DateTime(2025, 10, 25, 3, 24, 23, 450, DateTimeKind.Utc).AddTicks(8724) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 1,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 25, 3, 24, 23, 450, DateTimeKind.Utc).AddTicks(8517));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 2,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 25, 3, 24, 23, 450, DateTimeKind.Utc).AddTicks(8521));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 3,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 25, 3, 24, 23, 450, DateTimeKind.Utc).AddTicks(8523));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 4,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 25, 3, 24, 23, 450, DateTimeKind.Utc).AddTicks(8524));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 5,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 25, 3, 24, 23, 450, DateTimeKind.Utc).AddTicks(8526));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 6,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 25, 3, 24, 23, 450, DateTimeKind.Utc).AddTicks(8527));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 7,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 25, 3, 24, 23, 450, DateTimeKind.Utc).AddTicks(8529));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 8,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 25, 3, 24, 23, 450, DateTimeKind.Utc).AddTicks(8637));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 9,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 25, 3, 24, 23, 450, DateTimeKind.Utc).AddTicks(8639));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 25, 3, 24, 23, 450, DateTimeKind.Utc).AddTicks(8152), new DateTime(2025, 10, 25, 3, 24, 23, 450, DateTimeKind.Utc).AddTicks(8158) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 25, 3, 24, 23, 450, DateTimeKind.Utc).AddTicks(8168), new DateTime(2025, 10, 25, 3, 24, 23, 450, DateTimeKind.Utc).AddTicks(8168) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 25, 3, 24, 23, 450, DateTimeKind.Utc).AddTicks(8171), new DateTime(2025, 10, 25, 3, 24, 23, 450, DateTimeKind.Utc).AddTicks(8171) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 25, 3, 24, 23, 450, DateTimeKind.Utc).AddTicks(8174), new DateTime(2025, 10, 25, 3, 24, 23, 450, DateTimeKind.Utc).AddTicks(8174) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 25, 3, 24, 23, 450, DateTimeKind.Utc).AddTicks(8176), new DateTime(2025, 10, 25, 3, 24, 23, 450, DateTimeKind.Utc).AddTicks(8177) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 25, 3, 24, 23, 450, DateTimeKind.Utc).AddTicks(8179), new DateTime(2025, 10, 25, 3, 24, 23, 450, DateTimeKind.Utc).AddTicks(8179) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 25, 3, 24, 23, 450, DateTimeKind.Utc).AddTicks(8181), new DateTime(2025, 10, 25, 3, 24, 23, 450, DateTimeKind.Utc).AddTicks(8182) });

            migrationBuilder.CreateIndex(
                name: "IX_Accessories_PartNumber",
                table: "Accessories",
                column: "PartNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccessoryStockAdjustments_AccessoryId",
                table: "AccessoryStockAdjustments",
                column: "AccessoryId");

            migrationBuilder.CreateIndex(
                name: "IX_AccessoryStockAdjustments_AdjustedDate",
                table: "AccessoryStockAdjustments",
                column: "AdjustedDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccessoryStockAdjustments");

            migrationBuilder.DropTable(
                name: "Accessories");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 24, 3, 35, 49, 592, DateTimeKind.Utc).AddTicks(2204), new DateTime(2025, 10, 24, 3, 35, 49, 592, DateTimeKind.Utc).AddTicks(2204) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 24, 3, 35, 49, 592, DateTimeKind.Utc).AddTicks(2209), new DateTime(2025, 10, 24, 3, 35, 49, 592, DateTimeKind.Utc).AddTicks(2209) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 24, 3, 35, 49, 592, DateTimeKind.Utc).AddTicks(2213), new DateTime(2025, 10, 24, 3, 35, 49, 592, DateTimeKind.Utc).AddTicks(2214) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 24, 3, 35, 49, 592, DateTimeKind.Utc).AddTicks(2217), new DateTime(2025, 10, 24, 3, 35, 49, 592, DateTimeKind.Utc).AddTicks(2217) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 24, 3, 35, 49, 592, DateTimeKind.Utc).AddTicks(2219), new DateTime(2025, 10, 24, 3, 35, 49, 592, DateTimeKind.Utc).AddTicks(2220) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 24, 3, 35, 49, 592, DateTimeKind.Utc).AddTicks(2221), new DateTime(2025, 10, 24, 3, 35, 49, 592, DateTimeKind.Utc).AddTicks(2222) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 24, 3, 35, 49, 592, DateTimeKind.Utc).AddTicks(2224), new DateTime(2025, 10, 24, 3, 35, 49, 592, DateTimeKind.Utc).AddTicks(2224) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 24, 3, 35, 49, 592, DateTimeKind.Utc).AddTicks(2226), new DateTime(2025, 10, 24, 3, 35, 49, 592, DateTimeKind.Utc).AddTicks(2233) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 24, 3, 35, 49, 592, DateTimeKind.Utc).AddTicks(2365), new DateTime(2025, 10, 24, 3, 35, 49, 592, DateTimeKind.Utc).AddTicks(2366) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 24, 3, 35, 49, 592, DateTimeKind.Utc).AddTicks(2373), new DateTime(2025, 10, 24, 3, 35, 49, 592, DateTimeKind.Utc).AddTicks(2374) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 24, 3, 35, 49, 592, DateTimeKind.Utc).AddTicks(2376), new DateTime(2025, 10, 24, 3, 35, 49, 592, DateTimeKind.Utc).AddTicks(2377) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 24, 3, 35, 49, 592, DateTimeKind.Utc).AddTicks(2379), new DateTime(2025, 10, 24, 3, 35, 49, 592, DateTimeKind.Utc).AddTicks(2379) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 24, 3, 35, 49, 592, DateTimeKind.Utc).AddTicks(2382), new DateTime(2025, 10, 24, 3, 35, 49, 592, DateTimeKind.Utc).AddTicks(2382) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 24, 3, 35, 49, 592, DateTimeKind.Utc).AddTicks(2385), new DateTime(2025, 10, 24, 3, 35, 49, 592, DateTimeKind.Utc).AddTicks(2386) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 24, 3, 35, 49, 592, DateTimeKind.Utc).AddTicks(2388), new DateTime(2025, 10, 24, 3, 35, 49, 592, DateTimeKind.Utc).AddTicks(2388) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 24, 3, 35, 49, 592, DateTimeKind.Utc).AddTicks(2390), new DateTime(2025, 10, 24, 3, 35, 49, 592, DateTimeKind.Utc).AddTicks(2390) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 24, 3, 35, 49, 592, DateTimeKind.Utc).AddTicks(2393), new DateTime(2025, 10, 24, 3, 35, 49, 592, DateTimeKind.Utc).AddTicks(2394) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 24, 3, 35, 49, 592, DateTimeKind.Utc).AddTicks(2395), new DateTime(2025, 10, 24, 3, 35, 49, 592, DateTimeKind.Utc).AddTicks(2396) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 24, 3, 35, 49, 592, DateTimeKind.Utc).AddTicks(2398), new DateTime(2025, 10, 24, 3, 35, 49, 592, DateTimeKind.Utc).AddTicks(2398) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 1,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 24, 3, 35, 49, 592, DateTimeKind.Utc).AddTicks(2291));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 2,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 24, 3, 35, 49, 592, DateTimeKind.Utc).AddTicks(2296));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 3,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 24, 3, 35, 49, 592, DateTimeKind.Utc).AddTicks(2298));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 4,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 24, 3, 35, 49, 592, DateTimeKind.Utc).AddTicks(2299));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 5,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 24, 3, 35, 49, 592, DateTimeKind.Utc).AddTicks(2301));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 6,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 24, 3, 35, 49, 592, DateTimeKind.Utc).AddTicks(2302));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 7,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 24, 3, 35, 49, 592, DateTimeKind.Utc).AddTicks(2304));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 8,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 24, 3, 35, 49, 592, DateTimeKind.Utc).AddTicks(2306));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 9,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 24, 3, 35, 49, 592, DateTimeKind.Utc).AddTicks(2307));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 24, 3, 35, 49, 592, DateTimeKind.Utc).AddTicks(1837), new DateTime(2025, 10, 24, 3, 35, 49, 592, DateTimeKind.Utc).AddTicks(1840) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 24, 3, 35, 49, 592, DateTimeKind.Utc).AddTicks(1847), new DateTime(2025, 10, 24, 3, 35, 49, 592, DateTimeKind.Utc).AddTicks(1847) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 24, 3, 35, 49, 592, DateTimeKind.Utc).AddTicks(1850), new DateTime(2025, 10, 24, 3, 35, 49, 592, DateTimeKind.Utc).AddTicks(1851) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 24, 3, 35, 49, 592, DateTimeKind.Utc).AddTicks(1852), new DateTime(2025, 10, 24, 3, 35, 49, 592, DateTimeKind.Utc).AddTicks(1853) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 24, 3, 35, 49, 592, DateTimeKind.Utc).AddTicks(1855), new DateTime(2025, 10, 24, 3, 35, 49, 592, DateTimeKind.Utc).AddTicks(1855) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 24, 3, 35, 49, 592, DateTimeKind.Utc).AddTicks(1915), new DateTime(2025, 10, 24, 3, 35, 49, 592, DateTimeKind.Utc).AddTicks(1916) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 24, 3, 35, 49, 592, DateTimeKind.Utc).AddTicks(1918), new DateTime(2025, 10, 24, 3, 35, 49, 592, DateTimeKind.Utc).AddTicks(1918) });
        }
    }
}
