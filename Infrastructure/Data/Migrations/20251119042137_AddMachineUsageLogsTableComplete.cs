using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddMachineUsageLogsTableComplete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MachineUsageLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MachineId = table.Column<int>(type: "int", nullable: false),
                    OperatorId = table.Column<int>(type: "int", nullable: true),
                    SiteEngineer = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LogDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EngineHourStart = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EngineHourEnd = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EngineHoursDelta = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DrifterHourStart = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DrifterHourEnd = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DrifterHoursDelta = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IdleHours = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    WorkingHours = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FuelConsumed = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    HasDowntime = table.Column<bool>(type: "bit", nullable: false),
                    DowntimeHours = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    BreakdownDescription = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MachineUsageLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MachineUsageLogs_Machines_MachineId",
                        column: x => x.MachineId,
                        principalTable: "Machines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MachineUsageLogs_Users_OperatorId",
                        column: x => x.OperatorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2733), new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2734) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2738), new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2739) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2741), new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2742) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2744), new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2744) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2746), new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2747) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2749), new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2749) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2751), new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2751) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2753), new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2760) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2890), new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2891) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2897), new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2898) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2901), new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2901) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2903), new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2904) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2964), new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2965) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2967), new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2967) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2969), new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2970) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2972), new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2972) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2974), new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2975) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2977), new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2977) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2979), new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2979) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 1,
                column: "GrantedAt",
                value: new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2820));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 2,
                column: "GrantedAt",
                value: new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2824));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 3,
                column: "GrantedAt",
                value: new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2826));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 4,
                column: "GrantedAt",
                value: new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2827));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 5,
                column: "GrantedAt",
                value: new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2829));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 6,
                column: "GrantedAt",
                value: new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2830));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 7,
                column: "GrantedAt",
                value: new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2831));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 8,
                column: "GrantedAt",
                value: new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2833));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 9,
                column: "GrantedAt",
                value: new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2834));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2497), new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2501) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2507), new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2508) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2511), new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2511) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2513), new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2514) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2516), new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2516) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2518), new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2518) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2520), new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2521) });

            migrationBuilder.CreateIndex(
                name: "IX_MachineUsageLogs_LogDate",
                table: "MachineUsageLogs",
                column: "LogDate");

            migrationBuilder.CreateIndex(
                name: "IX_MachineUsageLogs_MachineId",
                table: "MachineUsageLogs",
                column: "MachineId");

            migrationBuilder.CreateIndex(
                name: "IX_MachineUsageLogs_MachineId_LogDate",
                table: "MachineUsageLogs",
                columns: new[] { "MachineId", "LogDate" });

            migrationBuilder.CreateIndex(
                name: "IX_MachineUsageLogs_OperatorId",
                table: "MachineUsageLogs",
                column: "OperatorId");

            migrationBuilder.CreateIndex(
                name: "IX_MachineUsageLogs_Status",
                table: "MachineUsageLogs",
                column: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MachineUsageLogs");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(5039), new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(5039) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(5045), new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(5046) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(5049), new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(5049) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(5051), new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(5052) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(5053), new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(5054) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(5056), new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(5056) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(5058), new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(5058) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(5060), new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(5068) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(5200), new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(5200) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(5208), new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(5208) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(5211), new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(5211) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(5214), new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(5214) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(5216), new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(5217) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(5219), new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(5219) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(5221), new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(5221) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(5224), new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(5224) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(5226), new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(5226) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(5229), new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(5229) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(5231), new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(5231) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 1,
                column: "GrantedAt",
                value: new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(5125));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 2,
                column: "GrantedAt",
                value: new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(5131));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 3,
                column: "GrantedAt",
                value: new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(5133));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 4,
                column: "GrantedAt",
                value: new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(5134));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 5,
                column: "GrantedAt",
                value: new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(5136));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 6,
                column: "GrantedAt",
                value: new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(5138));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 7,
                column: "GrantedAt",
                value: new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(5139));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 8,
                column: "GrantedAt",
                value: new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(5141));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 9,
                column: "GrantedAt",
                value: new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(5142));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(4692), new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(4696) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(4704), new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(4704) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(4707), new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(4708) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(4710), new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(4710) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(4712), new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(4713) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(4822), new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(4822) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(4825), new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(4826) });
        }
    }
}
