using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddStoreManagementTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Stores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StoreName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    StoreAddress = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    StoreManagerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StoreManagerContact = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    StoreManagerEmail = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    StorageCapacity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CurrentOccupancy = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    RegionId = table.Column<int>(type: "int", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: true),
                    ManagerUserId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stores_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Stores_Regions_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Regions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Stores_Users_ManagerUserId",
                        column: x => x.ManagerUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Stores_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "StoreInventories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StoreId = table.Column<int>(type: "int", nullable: false),
                    ExplosiveType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ReservedQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    Unit = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MinimumStockLevel = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaximumStockLevel = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LastRestockedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BatchNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Supplier = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreInventories", x => x.Id);
                    table.CheckConstraint("CK_StoreInventory_MinMaxStock", "[MinimumStockLevel] IS NULL OR [MaximumStockLevel] IS NULL OR [MinimumStockLevel] <= [MaximumStockLevel]");
                    table.CheckConstraint("CK_StoreInventory_Quantity_NonNegative", "[Quantity] >= 0");
                    table.CheckConstraint("CK_StoreInventory_ReservedQuantity_LessOrEqualQuantity", "[ReservedQuantity] <= [Quantity]");
                    table.CheckConstraint("CK_StoreInventory_ReservedQuantity_NonNegative", "[ReservedQuantity] >= 0");
                    table.ForeignKey(
                        name: "FK_StoreInventories_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StoreTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StoreId = table.Column<int>(type: "int", nullable: false),
                    StoreInventoryId = table.Column<int>(type: "int", nullable: true),
                    ExplosiveType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TransactionType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ReferenceNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    RelatedStoreId = table.Column<int>(type: "int", nullable: true),
                    ProcessedByUserId = table.Column<int>(type: "int", nullable: true),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreTransactions", x => x.Id);
                    table.CheckConstraint("CK_StoreTransaction_Quantity_Positive", "[Quantity] > 0");
                    table.ForeignKey(
                        name: "FK_StoreTransactions_StoreInventories_StoreInventoryId",
                        column: x => x.StoreInventoryId,
                        principalTable: "StoreInventories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StoreTransactions_Stores_RelatedStoreId",
                        column: x => x.RelatedStoreId,
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_StoreTransactions_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StoreTransactions_Users_ProcessedByUserId",
                        column: x => x.ProcessedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(5893), new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(5894) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(5898), new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(5899) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(5902), new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(5902) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(5904), new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(5905) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(5907), new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(5907) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(5910), new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(5910) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(5912), new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(5913) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(5915), new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(5915) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(6024), new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(6024) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(6032), new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(6032) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(6035), new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(6035) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(6038), new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(6038) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(6040), new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(6040) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(6042), new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(6043) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(6045), new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(6045) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(6047), new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(6048) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(6050), new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(6050) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(6052), new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(6053) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(6055), new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(6055) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 1,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(5962));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 2,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(5966));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 3,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(5968));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 4,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(5969));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 5,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(5971));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 6,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(5972));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 7,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(5973));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 8,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(5975));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 9,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(5976));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(5671), new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(5675) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(5681), new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(5682) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(5684), new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(5685) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(5687), new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(5687) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(5689), new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(5690) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(5692), new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(5692) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(5694), new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(5695) });

            migrationBuilder.CreateIndex(
                name: "IX_StoreInventories_BatchNumber",
                table: "StoreInventories",
                column: "BatchNumber");

            migrationBuilder.CreateIndex(
                name: "IX_StoreInventories_ExpiryDate",
                table: "StoreInventories",
                column: "ExpiryDate");

            migrationBuilder.CreateIndex(
                name: "IX_StoreInventories_ExplosiveType",
                table: "StoreInventories",
                column: "ExplosiveType");

            migrationBuilder.CreateIndex(
                name: "IX_StoreInventories_StoreId",
                table: "StoreInventories",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreInventories_StoreId_ExplosiveType",
                table: "StoreInventories",
                columns: new[] { "StoreId", "ExplosiveType" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Stores_ManagerUserId",
                table: "Stores",
                column: "ManagerUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Stores_ProjectId",
                table: "Stores",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Stores_RegionId",
                table: "Stores",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_Stores_Status",
                table: "Stores",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Stores_StoreName",
                table: "Stores",
                column: "StoreName");

            migrationBuilder.CreateIndex(
                name: "IX_Stores_UserId",
                table: "Stores",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreTransactions_ExplosiveType",
                table: "StoreTransactions",
                column: "ExplosiveType");

            migrationBuilder.CreateIndex(
                name: "IX_StoreTransactions_ProcessedByUserId",
                table: "StoreTransactions",
                column: "ProcessedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreTransactions_ReferenceNumber",
                table: "StoreTransactions",
                column: "ReferenceNumber");

            migrationBuilder.CreateIndex(
                name: "IX_StoreTransactions_RelatedStoreId",
                table: "StoreTransactions",
                column: "RelatedStoreId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreTransactions_StoreId",
                table: "StoreTransactions",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreTransactions_StoreInventoryId",
                table: "StoreTransactions",
                column: "StoreInventoryId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreTransactions_TransactionDate",
                table: "StoreTransactions",
                column: "TransactionDate");

            migrationBuilder.CreateIndex(
                name: "IX_StoreTransactions_TransactionType",
                table: "StoreTransactions",
                column: "TransactionType");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StoreTransactions");

            migrationBuilder.DropTable(
                name: "StoreInventories");

            migrationBuilder.DropTable(
                name: "Stores");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5611), new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5611) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5616), new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5616) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5619), new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5619) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5622), new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5622) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5624), new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5625) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5627), new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5627) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5629), new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5629) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5709), new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5711) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5823), new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5823) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5833), new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5833) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5836), new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5836) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5838), new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5839) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5841), new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5841) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5843), new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5843) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5845), new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5846) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5848), new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5848) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5850), new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5850) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5852), new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5853) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5854), new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5855) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 1,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5758));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 2,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5762));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 3,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5764));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 4,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5765));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 5,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5766));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 6,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5768));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 7,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5769));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 8,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5770));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 9,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5771));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5326), new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5328) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5334), new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5335) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5337), new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5338) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5340), new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5340) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5343), new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5343) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5345), new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5345) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5348), new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5348) });
        }
    }
}
