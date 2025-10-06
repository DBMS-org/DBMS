using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddExplosiveInventoryManagement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CentralWarehouseInventories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BatchId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ExplosiveType = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    AllocatedQuantity = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false, defaultValue: 0m),
                    Unit = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ManufacturingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Supplier = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ManufacturerBatchNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    StorageLocation = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CentralWarehouseStoreId = table.Column<int>(type: "int", nullable: false),
                    ANFOTechnicalPropertiesId = table.Column<int>(type: "int", nullable: true),
                    EmulsionTechnicalPropertiesId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CentralWarehouseInventories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CentralWarehouseInventories_Stores_CentralWarehouseStoreId",
                        column: x => x.CentralWarehouseStoreId,
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ANFOTechnicalProperties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CentralWarehouseInventoryId = table.Column<int>(type: "int", nullable: false),
                    Density = table.Column<decimal>(type: "decimal(4,2)", precision: 4, scale: 2, nullable: false),
                    FuelOilContent = table.Column<decimal>(type: "decimal(4,2)", precision: 4, scale: 2, nullable: false),
                    MoistureContent = table.Column<decimal>(type: "decimal(4,2)", precision: 4, scale: 2, nullable: true),
                    PrillSize = table.Column<decimal>(type: "decimal(4,2)", precision: 4, scale: 2, nullable: true),
                    DetonationVelocity = table.Column<int>(type: "int", nullable: true),
                    Grade = table.Column<int>(type: "int", nullable: false),
                    StorageTemperature = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    StorageHumidity = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    FumeClass = table.Column<int>(type: "int", nullable: false),
                    QualityCheckDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    QualityStatus = table.Column<int>(type: "int", nullable: false),
                    WaterResistance = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "None"),
                    Notes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ANFOTechnicalProperties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ANFOTechnicalProperties_CentralWarehouseInventories_CentralWarehouseInventoryId",
                        column: x => x.CentralWarehouseInventoryId,
                        principalTable: "CentralWarehouseInventories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmulsionTechnicalProperties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CentralWarehouseInventoryId = table.Column<int>(type: "int", nullable: false),
                    DensityUnsensitized = table.Column<decimal>(type: "decimal(4,2)", precision: 4, scale: 2, nullable: false),
                    DensitySensitized = table.Column<decimal>(type: "decimal(4,2)", precision: 4, scale: 2, nullable: false),
                    Viscosity = table.Column<int>(type: "int", nullable: false),
                    WaterContent = table.Column<decimal>(type: "decimal(4,2)", precision: 4, scale: 2, nullable: false),
                    pH = table.Column<decimal>(type: "decimal(3,1)", precision: 3, scale: 1, nullable: false),
                    DetonationVelocity = table.Column<int>(type: "int", nullable: true),
                    BubbleSize = table.Column<int>(type: "int", nullable: true),
                    StorageTemperature = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    ApplicationTemperature = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    Grade = table.Column<int>(type: "int", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SensitizationType = table.Column<int>(type: "int", nullable: false),
                    SensitizerContent = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    FumeClass = table.Column<int>(type: "int", nullable: false),
                    QualityCheckDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    QualityStatus = table.Column<int>(type: "int", nullable: false),
                    PhaseSeparation = table.Column<bool>(type: "bit", nullable: true),
                    Crystallization = table.Column<bool>(type: "bit", nullable: true),
                    ColorConsistency = table.Column<bool>(type: "bit", nullable: true),
                    WaterResistance = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Excellent"),
                    Notes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmulsionTechnicalProperties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmulsionTechnicalProperties_CentralWarehouseInventories_CentralWarehouseInventoryId",
                        column: x => x.CentralWarehouseInventoryId,
                        principalTable: "CentralWarehouseInventories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InventoryTransferRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CentralWarehouseInventoryId = table.Column<int>(type: "int", nullable: false),
                    DestinationStoreId = table.Column<int>(type: "int", nullable: false),
                    ProjectSiteId = table.Column<int>(type: "int", nullable: true),
                    RequestedQuantity = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    ApprovedQuantity = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: true),
                    Unit = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ApprovedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RequiredByDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RequestedByUserId = table.Column<int>(type: "int", nullable: false),
                    ApprovedByUserId = table.Column<int>(type: "int", nullable: true),
                    ProcessedByUserId = table.Column<int>(type: "int", nullable: true),
                    RequestNotes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ApprovalNotes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    RejectionReason = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CompletedTransactionId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryTransferRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryTransferRequests_CentralWarehouseInventories_CentralWarehouseInventoryId",
                        column: x => x.CentralWarehouseInventoryId,
                        principalTable: "CentralWarehouseInventories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InventoryTransferRequests_ProjectSites_ProjectSiteId",
                        column: x => x.ProjectSiteId,
                        principalTable: "ProjectSites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InventoryTransferRequests_StoreTransactions_CompletedTransactionId",
                        column: x => x.CompletedTransactionId,
                        principalTable: "StoreTransactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InventoryTransferRequests_Stores_DestinationStoreId",
                        column: x => x.DestinationStoreId,
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InventoryTransferRequests_Users_ApprovedByUserId",
                        column: x => x.ApprovedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InventoryTransferRequests_Users_ProcessedByUserId",
                        column: x => x.ProcessedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InventoryTransferRequests_Users_RequestedByUserId",
                        column: x => x.RequestedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QualityCheckRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CentralWarehouseInventoryId = table.Column<int>(type: "int", nullable: false),
                    CheckDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CheckedByUserId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CheckType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Findings = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    ActionTaken = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    RequiresFollowUp = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    FollowUpDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QualityCheckRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QualityCheckRecords_CentralWarehouseInventories_CentralWarehouseInventoryId",
                        column: x => x.CentralWarehouseInventoryId,
                        principalTable: "CentralWarehouseInventories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QualityCheckRecords_Users_CheckedByUserId",
                        column: x => x.CheckedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(9036), new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(9036) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(9042), new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(9042) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(9045), new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(9046) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(9048), new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(9048) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(9050), new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(9050) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(9052), new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(9053) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(9055), new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(9055) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(9057), new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(9067) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(9232), new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(9232) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(9239), new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(9240) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(9242), new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(9243) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(9245), new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(9246) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(9248), new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(9248) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(9251), new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(9251) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(9253), new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(9254) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(9256), new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(9256) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(9258), new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(9258) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(9260), new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(9261) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(9263), new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(9263) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 1,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(9118));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 2,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(9122));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 3,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(9124));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 4,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(9126));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 5,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(9127));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 6,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(9129));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 7,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(9130));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 8,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(9131));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 9,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(9133));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(8653), new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(8656) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(8663), new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(8664) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(8667), new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(8667) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(8670), new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(8670) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(8672), new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(8673) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(8674), new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(8675) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(8677), new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(8677) });

            migrationBuilder.CreateIndex(
                name: "IX_ANFOTechnicalProperties_CentralWarehouseInventoryId",
                table: "ANFOTechnicalProperties",
                column: "CentralWarehouseInventoryId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CentralWarehouseInventories_BatchId",
                table: "CentralWarehouseInventories",
                column: "BatchId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CentralWarehouseInventories_CentralWarehouseStoreId",
                table: "CentralWarehouseInventories",
                column: "CentralWarehouseStoreId");

            migrationBuilder.CreateIndex(
                name: "IX_CentralWarehouseInventories_CreatedAt",
                table: "CentralWarehouseInventories",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_CentralWarehouseInventories_ExpiryDate",
                table: "CentralWarehouseInventories",
                column: "ExpiryDate");

            migrationBuilder.CreateIndex(
                name: "IX_CentralWarehouseInventories_ExplosiveType",
                table: "CentralWarehouseInventories",
                column: "ExplosiveType");

            migrationBuilder.CreateIndex(
                name: "IX_CentralWarehouseInventories_Status",
                table: "CentralWarehouseInventories",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_CentralWarehouseInventories_Supplier",
                table: "CentralWarehouseInventories",
                column: "Supplier");

            migrationBuilder.CreateIndex(
                name: "IX_EmulsionTechnicalProperties_CentralWarehouseInventoryId",
                table: "EmulsionTechnicalProperties",
                column: "CentralWarehouseInventoryId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransferRequests_ApprovedByUserId",
                table: "InventoryTransferRequests",
                column: "ApprovedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransferRequests_CentralWarehouseInventoryId",
                table: "InventoryTransferRequests",
                column: "CentralWarehouseInventoryId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransferRequests_CompletedTransactionId",
                table: "InventoryTransferRequests",
                column: "CompletedTransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransferRequests_DestinationStoreId",
                table: "InventoryTransferRequests",
                column: "DestinationStoreId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransferRequests_ProcessedByUserId",
                table: "InventoryTransferRequests",
                column: "ProcessedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransferRequests_ProjectSiteId",
                table: "InventoryTransferRequests",
                column: "ProjectSiteId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransferRequests_RequestDate",
                table: "InventoryTransferRequests",
                column: "RequestDate");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransferRequests_RequestedByUserId",
                table: "InventoryTransferRequests",
                column: "RequestedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransferRequests_RequestNumber",
                table: "InventoryTransferRequests",
                column: "RequestNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransferRequests_RequiredByDate",
                table: "InventoryTransferRequests",
                column: "RequiredByDate");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransferRequests_Status",
                table: "InventoryTransferRequests",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_QualityCheckRecords_CentralWarehouseInventoryId",
                table: "QualityCheckRecords",
                column: "CentralWarehouseInventoryId");

            migrationBuilder.CreateIndex(
                name: "IX_QualityCheckRecords_CheckDate",
                table: "QualityCheckRecords",
                column: "CheckDate");

            migrationBuilder.CreateIndex(
                name: "IX_QualityCheckRecords_CheckedByUserId",
                table: "QualityCheckRecords",
                column: "CheckedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_QualityCheckRecords_CheckType",
                table: "QualityCheckRecords",
                column: "CheckType");

            migrationBuilder.CreateIndex(
                name: "IX_QualityCheckRecords_RequiresFollowUp",
                table: "QualityCheckRecords",
                column: "RequiresFollowUp");

            migrationBuilder.CreateIndex(
                name: "IX_QualityCheckRecords_Status",
                table: "QualityCheckRecords",
                column: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ANFOTechnicalProperties");

            migrationBuilder.DropTable(
                name: "EmulsionTechnicalProperties");

            migrationBuilder.DropTable(
                name: "InventoryTransferRequests");

            migrationBuilder.DropTable(
                name: "QualityCheckRecords");

            migrationBuilder.DropTable(
                name: "CentralWarehouseInventories");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 5, 7, 49, 229, DateTimeKind.Utc).AddTicks(7721), new DateTime(2025, 10, 3, 5, 7, 49, 229, DateTimeKind.Utc).AddTicks(7721) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 5, 7, 49, 229, DateTimeKind.Utc).AddTicks(7726), new DateTime(2025, 10, 3, 5, 7, 49, 229, DateTimeKind.Utc).AddTicks(7727) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 5, 7, 49, 229, DateTimeKind.Utc).AddTicks(7729), new DateTime(2025, 10, 3, 5, 7, 49, 229, DateTimeKind.Utc).AddTicks(7730) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 5, 7, 49, 229, DateTimeKind.Utc).AddTicks(7732), new DateTime(2025, 10, 3, 5, 7, 49, 229, DateTimeKind.Utc).AddTicks(7732) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 5, 7, 49, 229, DateTimeKind.Utc).AddTicks(7734), new DateTime(2025, 10, 3, 5, 7, 49, 229, DateTimeKind.Utc).AddTicks(7735) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 5, 7, 49, 229, DateTimeKind.Utc).AddTicks(7736), new DateTime(2025, 10, 3, 5, 7, 49, 229, DateTimeKind.Utc).AddTicks(7737) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 5, 7, 49, 229, DateTimeKind.Utc).AddTicks(7739), new DateTime(2025, 10, 3, 5, 7, 49, 229, DateTimeKind.Utc).AddTicks(7739) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 5, 7, 49, 229, DateTimeKind.Utc).AddTicks(7741), new DateTime(2025, 10, 3, 5, 7, 49, 230, DateTimeKind.Utc).AddTicks(174) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 5, 7, 49, 230, DateTimeKind.Utc).AddTicks(477), new DateTime(2025, 10, 3, 5, 7, 49, 230, DateTimeKind.Utc).AddTicks(477) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 5, 7, 49, 230, DateTimeKind.Utc).AddTicks(486), new DateTime(2025, 10, 3, 5, 7, 49, 230, DateTimeKind.Utc).AddTicks(486) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 5, 7, 49, 230, DateTimeKind.Utc).AddTicks(489), new DateTime(2025, 10, 3, 5, 7, 49, 230, DateTimeKind.Utc).AddTicks(489) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 5, 7, 49, 230, DateTimeKind.Utc).AddTicks(492), new DateTime(2025, 10, 3, 5, 7, 49, 230, DateTimeKind.Utc).AddTicks(492) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 5, 7, 49, 230, DateTimeKind.Utc).AddTicks(494), new DateTime(2025, 10, 3, 5, 7, 49, 230, DateTimeKind.Utc).AddTicks(495) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 5, 7, 49, 230, DateTimeKind.Utc).AddTicks(524), new DateTime(2025, 10, 3, 5, 7, 49, 230, DateTimeKind.Utc).AddTicks(524) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 5, 7, 49, 230, DateTimeKind.Utc).AddTicks(527), new DateTime(2025, 10, 3, 5, 7, 49, 230, DateTimeKind.Utc).AddTicks(527) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 5, 7, 49, 230, DateTimeKind.Utc).AddTicks(530), new DateTime(2025, 10, 3, 5, 7, 49, 230, DateTimeKind.Utc).AddTicks(530) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 5, 7, 49, 230, DateTimeKind.Utc).AddTicks(532), new DateTime(2025, 10, 3, 5, 7, 49, 230, DateTimeKind.Utc).AddTicks(533) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 5, 7, 49, 230, DateTimeKind.Utc).AddTicks(535), new DateTime(2025, 10, 3, 5, 7, 49, 230, DateTimeKind.Utc).AddTicks(535) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 5, 7, 49, 230, DateTimeKind.Utc).AddTicks(538), new DateTime(2025, 10, 3, 5, 7, 49, 230, DateTimeKind.Utc).AddTicks(538) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 1,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 3, 5, 7, 49, 230, DateTimeKind.Utc).AddTicks(399));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 2,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 3, 5, 7, 49, 230, DateTimeKind.Utc).AddTicks(403));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 3,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 3, 5, 7, 49, 230, DateTimeKind.Utc).AddTicks(405));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 4,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 3, 5, 7, 49, 230, DateTimeKind.Utc).AddTicks(406));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 5,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 3, 5, 7, 49, 230, DateTimeKind.Utc).AddTicks(408));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 6,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 3, 5, 7, 49, 230, DateTimeKind.Utc).AddTicks(409));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 7,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 3, 5, 7, 49, 230, DateTimeKind.Utc).AddTicks(411));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 8,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 3, 5, 7, 49, 230, DateTimeKind.Utc).AddTicks(412));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 9,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 3, 5, 7, 49, 230, DateTimeKind.Utc).AddTicks(414));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 5, 7, 49, 229, DateTimeKind.Utc).AddTicks(7343), new DateTime(2025, 10, 3, 5, 7, 49, 229, DateTimeKind.Utc).AddTicks(7347) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 5, 7, 49, 229, DateTimeKind.Utc).AddTicks(7354), new DateTime(2025, 10, 3, 5, 7, 49, 229, DateTimeKind.Utc).AddTicks(7367) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 5, 7, 49, 229, DateTimeKind.Utc).AddTicks(7369), new DateTime(2025, 10, 3, 5, 7, 49, 229, DateTimeKind.Utc).AddTicks(7370) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 5, 7, 49, 229, DateTimeKind.Utc).AddTicks(7372), new DateTime(2025, 10, 3, 5, 7, 49, 229, DateTimeKind.Utc).AddTicks(7372) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 5, 7, 49, 229, DateTimeKind.Utc).AddTicks(7436), new DateTime(2025, 10, 3, 5, 7, 49, 229, DateTimeKind.Utc).AddTicks(7437) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 5, 7, 49, 229, DateTimeKind.Utc).AddTicks(7439), new DateTime(2025, 10, 3, 5, 7, 49, 229, DateTimeKind.Utc).AddTicks(7439) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 5, 7, 49, 229, DateTimeKind.Utc).AddTicks(7441), new DateTime(2025, 10, 3, 5, 7, 49, 229, DateTimeKind.Utc).AddTicks(7442) });
        }
    }
}
