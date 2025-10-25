using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddMachineAssignmentEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MachineAssignmentRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    MachineType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    RequestedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RequestedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Urgency = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DetailsOrExplanation = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ApprovedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ApprovedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AssignedMachinesJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Comments = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ExpectedUsageDuration = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ExpectedReturnDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MachineAssignmentRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MachineAssignmentRequests_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MachineAssignments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MachineId = table.Column<int>(type: "int", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    OperatorId = table.Column<int>(type: "int", nullable: false),
                    AssignedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AssignedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpectedReturnDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ActualReturnDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Location = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MachineAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MachineAssignments_Machines_MachineId",
                        column: x => x.MachineId,
                        principalTable: "Machines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MachineAssignments_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MachineAssignments_Users_OperatorId",
                        column: x => x.OperatorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_MachineAssignmentRequests_ProjectId",
                table: "MachineAssignmentRequests",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_MachineAssignmentRequests_RequestedDate",
                table: "MachineAssignmentRequests",
                column: "RequestedDate");

            migrationBuilder.CreateIndex(
                name: "IX_MachineAssignmentRequests_Status",
                table: "MachineAssignmentRequests",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_MachineAssignmentRequests_Urgency",
                table: "MachineAssignmentRequests",
                column: "Urgency");

            migrationBuilder.CreateIndex(
                name: "IX_MachineAssignments_AssignedDate",
                table: "MachineAssignments",
                column: "AssignedDate");

            migrationBuilder.CreateIndex(
                name: "IX_MachineAssignments_MachineId",
                table: "MachineAssignments",
                column: "MachineId");

            migrationBuilder.CreateIndex(
                name: "IX_MachineAssignments_OperatorId",
                table: "MachineAssignments",
                column: "OperatorId");

            migrationBuilder.CreateIndex(
                name: "IX_MachineAssignments_ProjectId",
                table: "MachineAssignments",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_MachineAssignments_Status",
                table: "MachineAssignments",
                column: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MachineAssignmentRequests");

            migrationBuilder.DropTable(
                name: "MachineAssignments");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 16, 5, 29, 44, 694, DateTimeKind.Utc).AddTicks(3), new DateTime(2025, 10, 16, 5, 29, 44, 694, DateTimeKind.Utc).AddTicks(3) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 16, 5, 29, 44, 694, DateTimeKind.Utc).AddTicks(9), new DateTime(2025, 10, 16, 5, 29, 44, 694, DateTimeKind.Utc).AddTicks(9) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 16, 5, 29, 44, 694, DateTimeKind.Utc).AddTicks(12), new DateTime(2025, 10, 16, 5, 29, 44, 694, DateTimeKind.Utc).AddTicks(12) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 16, 5, 29, 44, 694, DateTimeKind.Utc).AddTicks(15), new DateTime(2025, 10, 16, 5, 29, 44, 694, DateTimeKind.Utc).AddTicks(15) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 16, 5, 29, 44, 694, DateTimeKind.Utc).AddTicks(17), new DateTime(2025, 10, 16, 5, 29, 44, 694, DateTimeKind.Utc).AddTicks(17) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 16, 5, 29, 44, 694, DateTimeKind.Utc).AddTicks(19), new DateTime(2025, 10, 16, 5, 29, 44, 694, DateTimeKind.Utc).AddTicks(20) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 16, 5, 29, 44, 694, DateTimeKind.Utc).AddTicks(21), new DateTime(2025, 10, 16, 5, 29, 44, 694, DateTimeKind.Utc).AddTicks(22) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 16, 5, 29, 44, 694, DateTimeKind.Utc).AddTicks(24), new DateTime(2025, 10, 16, 5, 29, 44, 694, DateTimeKind.Utc).AddTicks(24) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 16, 5, 29, 44, 694, DateTimeKind.Utc).AddTicks(128), new DateTime(2025, 10, 16, 5, 29, 44, 694, DateTimeKind.Utc).AddTicks(128) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 16, 5, 29, 44, 694, DateTimeKind.Utc).AddTicks(138), new DateTime(2025, 10, 16, 5, 29, 44, 694, DateTimeKind.Utc).AddTicks(138) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 16, 5, 29, 44, 694, DateTimeKind.Utc).AddTicks(141), new DateTime(2025, 10, 16, 5, 29, 44, 694, DateTimeKind.Utc).AddTicks(141) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 16, 5, 29, 44, 694, DateTimeKind.Utc).AddTicks(144), new DateTime(2025, 10, 16, 5, 29, 44, 694, DateTimeKind.Utc).AddTicks(144) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 16, 5, 29, 44, 694, DateTimeKind.Utc).AddTicks(147), new DateTime(2025, 10, 16, 5, 29, 44, 694, DateTimeKind.Utc).AddTicks(147) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 16, 5, 29, 44, 694, DateTimeKind.Utc).AddTicks(150), new DateTime(2025, 10, 16, 5, 29, 44, 694, DateTimeKind.Utc).AddTicks(150) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 16, 5, 29, 44, 694, DateTimeKind.Utc).AddTicks(152), new DateTime(2025, 10, 16, 5, 29, 44, 694, DateTimeKind.Utc).AddTicks(153) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 16, 5, 29, 44, 694, DateTimeKind.Utc).AddTicks(155), new DateTime(2025, 10, 16, 5, 29, 44, 694, DateTimeKind.Utc).AddTicks(155) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 16, 5, 29, 44, 694, DateTimeKind.Utc).AddTicks(158), new DateTime(2025, 10, 16, 5, 29, 44, 694, DateTimeKind.Utc).AddTicks(158) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 16, 5, 29, 44, 694, DateTimeKind.Utc).AddTicks(160), new DateTime(2025, 10, 16, 5, 29, 44, 694, DateTimeKind.Utc).AddTicks(160) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 16, 5, 29, 44, 694, DateTimeKind.Utc).AddTicks(163), new DateTime(2025, 10, 16, 5, 29, 44, 694, DateTimeKind.Utc).AddTicks(163) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 1,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 16, 5, 29, 44, 694, DateTimeKind.Utc).AddTicks(68));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 2,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 16, 5, 29, 44, 694, DateTimeKind.Utc).AddTicks(72));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 3,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 16, 5, 29, 44, 694, DateTimeKind.Utc).AddTicks(73));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 4,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 16, 5, 29, 44, 694, DateTimeKind.Utc).AddTicks(75));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 5,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 16, 5, 29, 44, 694, DateTimeKind.Utc).AddTicks(76));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 6,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 16, 5, 29, 44, 694, DateTimeKind.Utc).AddTicks(78));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 7,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 16, 5, 29, 44, 694, DateTimeKind.Utc).AddTicks(79));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 8,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 16, 5, 29, 44, 694, DateTimeKind.Utc).AddTicks(81));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 9,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 16, 5, 29, 44, 694, DateTimeKind.Utc).AddTicks(82));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 16, 5, 29, 44, 693, DateTimeKind.Utc).AddTicks(9704), new DateTime(2025, 10, 16, 5, 29, 44, 693, DateTimeKind.Utc).AddTicks(9706) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 16, 5, 29, 44, 693, DateTimeKind.Utc).AddTicks(9711), new DateTime(2025, 10, 16, 5, 29, 44, 693, DateTimeKind.Utc).AddTicks(9712) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 16, 5, 29, 44, 693, DateTimeKind.Utc).AddTicks(9769), new DateTime(2025, 10, 16, 5, 29, 44, 693, DateTimeKind.Utc).AddTicks(9769) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 16, 5, 29, 44, 693, DateTimeKind.Utc).AddTicks(9771), new DateTime(2025, 10, 16, 5, 29, 44, 693, DateTimeKind.Utc).AddTicks(9772) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 16, 5, 29, 44, 693, DateTimeKind.Utc).AddTicks(9774), new DateTime(2025, 10, 16, 5, 29, 44, 693, DateTimeKind.Utc).AddTicks(9774) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 16, 5, 29, 44, 693, DateTimeKind.Utc).AddTicks(9776), new DateTime(2025, 10, 16, 5, 29, 44, 693, DateTimeKind.Utc).AddTicks(9776) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 16, 5, 29, 44, 693, DateTimeKind.Utc).AddTicks(9778), new DateTime(2025, 10, 16, 5, 29, 44, 693, DateTimeKind.Utc).AddTicks(9779) });
        }
    }
}
