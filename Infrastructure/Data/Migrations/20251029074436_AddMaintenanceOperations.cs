using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddMaintenanceOperations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MaintenanceReports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TicketId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    OperatorId = table.Column<int>(type: "int", nullable: false),
                    MachineId = table.Column<int>(type: "int", nullable: false),
                    MachineName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    MachineModel = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SerialNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Location = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    AffectedPart = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ProblemCategory = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CustomDescription = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Symptoms = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ErrorCodes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RecentMaintenanceHistory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Severity = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ReportedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AcknowledgedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    InProgressAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ResolvedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ClosedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MechanicalEngineerId = table.Column<int>(type: "int", nullable: true),
                    MechanicalEngineerName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ResolutionNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EstimatedResponseTime = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaintenanceReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaintenanceReports_Machines_MachineId",
                        column: x => x.MachineId,
                        principalTable: "Machines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MaintenanceReports_Users_MechanicalEngineerId",
                        column: x => x.MechanicalEngineerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_MaintenanceReports_Users_OperatorId",
                        column: x => x.OperatorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MaintenanceJobs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MachineId = table.Column<int>(type: "int", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: true),
                    MaintenanceReportId = table.Column<int>(type: "int", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ScheduledDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    InProgressAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EstimatedHours = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    ActualHours = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    Reason = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Observations = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PartsReplaced = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaintenanceJobs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaintenanceJobs_Machines_MachineId",
                        column: x => x.MachineId,
                        principalTable: "Machines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MaintenanceJobs_MaintenanceReports_MaintenanceReportId",
                        column: x => x.MaintenanceReportId,
                        principalTable: "MaintenanceReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_MaintenanceJobs_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_MaintenanceJobs_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MaintenanceJobAssignments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaintenanceJobId = table.Column<int>(type: "int", nullable: false),
                    MechanicalEngineerId = table.Column<int>(type: "int", nullable: false),
                    AssignedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaintenanceJobAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaintenanceJobAssignments_MaintenanceJobs_MaintenanceJobId",
                        column: x => x.MaintenanceJobId,
                        principalTable: "MaintenanceJobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MaintenanceJobAssignments_Users_MechanicalEngineerId",
                        column: x => x.MechanicalEngineerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 29, 7, 44, 34, 327, DateTimeKind.Utc).AddTicks(8268), new DateTime(2025, 10, 29, 7, 44, 34, 327, DateTimeKind.Utc).AddTicks(8269) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 29, 7, 44, 34, 327, DateTimeKind.Utc).AddTicks(8277), new DateTime(2025, 10, 29, 7, 44, 34, 327, DateTimeKind.Utc).AddTicks(8278) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 29, 7, 44, 34, 327, DateTimeKind.Utc).AddTicks(8283), new DateTime(2025, 10, 29, 7, 44, 34, 327, DateTimeKind.Utc).AddTicks(8283) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 29, 7, 44, 34, 327, DateTimeKind.Utc).AddTicks(8286), new DateTime(2025, 10, 29, 7, 44, 34, 327, DateTimeKind.Utc).AddTicks(8287) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 29, 7, 44, 34, 327, DateTimeKind.Utc).AddTicks(8290), new DateTime(2025, 10, 29, 7, 44, 34, 327, DateTimeKind.Utc).AddTicks(8291) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 29, 7, 44, 34, 327, DateTimeKind.Utc).AddTicks(8294), new DateTime(2025, 10, 29, 7, 44, 34, 327, DateTimeKind.Utc).AddTicks(8295) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 29, 7, 44, 34, 327, DateTimeKind.Utc).AddTicks(8298), new DateTime(2025, 10, 29, 7, 44, 34, 327, DateTimeKind.Utc).AddTicks(8298) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 29, 7, 44, 34, 327, DateTimeKind.Utc).AddTicks(8302), new DateTime(2025, 10, 29, 7, 44, 34, 327, DateTimeKind.Utc).AddTicks(8316) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 29, 7, 44, 34, 327, DateTimeKind.Utc).AddTicks(8604), new DateTime(2025, 10, 29, 7, 44, 34, 327, DateTimeKind.Utc).AddTicks(8604) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 29, 7, 44, 34, 327, DateTimeKind.Utc).AddTicks(8634), new DateTime(2025, 10, 29, 7, 44, 34, 327, DateTimeKind.Utc).AddTicks(8634) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 29, 7, 44, 34, 327, DateTimeKind.Utc).AddTicks(8638), new DateTime(2025, 10, 29, 7, 44, 34, 327, DateTimeKind.Utc).AddTicks(8639) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 29, 7, 44, 34, 327, DateTimeKind.Utc).AddTicks(8642), new DateTime(2025, 10, 29, 7, 44, 34, 327, DateTimeKind.Utc).AddTicks(8643) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 29, 7, 44, 34, 327, DateTimeKind.Utc).AddTicks(8646), new DateTime(2025, 10, 29, 7, 44, 34, 327, DateTimeKind.Utc).AddTicks(8647) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 29, 7, 44, 34, 327, DateTimeKind.Utc).AddTicks(8651), new DateTime(2025, 10, 29, 7, 44, 34, 327, DateTimeKind.Utc).AddTicks(8651) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 29, 7, 44, 34, 327, DateTimeKind.Utc).AddTicks(8655), new DateTime(2025, 10, 29, 7, 44, 34, 327, DateTimeKind.Utc).AddTicks(8656) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 29, 7, 44, 34, 327, DateTimeKind.Utc).AddTicks(8660), new DateTime(2025, 10, 29, 7, 44, 34, 327, DateTimeKind.Utc).AddTicks(8660) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 29, 7, 44, 34, 327, DateTimeKind.Utc).AddTicks(8664), new DateTime(2025, 10, 29, 7, 44, 34, 327, DateTimeKind.Utc).AddTicks(8664) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 29, 7, 44, 34, 327, DateTimeKind.Utc).AddTicks(8668), new DateTime(2025, 10, 29, 7, 44, 34, 327, DateTimeKind.Utc).AddTicks(8668) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 29, 7, 44, 34, 327, DateTimeKind.Utc).AddTicks(8672), new DateTime(2025, 10, 29, 7, 44, 34, 327, DateTimeKind.Utc).AddTicks(8672) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 1,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 29, 7, 44, 34, 327, DateTimeKind.Utc).AddTicks(8411));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 2,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 29, 7, 44, 34, 327, DateTimeKind.Utc).AddTicks(8416));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 3,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 29, 7, 44, 34, 327, DateTimeKind.Utc).AddTicks(8419));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 4,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 29, 7, 44, 34, 327, DateTimeKind.Utc).AddTicks(8421));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 5,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 29, 7, 44, 34, 327, DateTimeKind.Utc).AddTicks(8424));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 6,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 29, 7, 44, 34, 327, DateTimeKind.Utc).AddTicks(8426));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 7,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 29, 7, 44, 34, 327, DateTimeKind.Utc).AddTicks(8428));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 8,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 29, 7, 44, 34, 327, DateTimeKind.Utc).AddTicks(8430));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 9,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 29, 7, 44, 34, 327, DateTimeKind.Utc).AddTicks(8432));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 29, 7, 44, 34, 327, DateTimeKind.Utc).AddTicks(7749), new DateTime(2025, 10, 29, 7, 44, 34, 327, DateTimeKind.Utc).AddTicks(7754) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 29, 7, 44, 34, 327, DateTimeKind.Utc).AddTicks(7813), new DateTime(2025, 10, 29, 7, 44, 34, 327, DateTimeKind.Utc).AddTicks(7814) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 29, 7, 44, 34, 327, DateTimeKind.Utc).AddTicks(7819), new DateTime(2025, 10, 29, 7, 44, 34, 327, DateTimeKind.Utc).AddTicks(7819) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 29, 7, 44, 34, 327, DateTimeKind.Utc).AddTicks(7823), new DateTime(2025, 10, 29, 7, 44, 34, 327, DateTimeKind.Utc).AddTicks(7823) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 29, 7, 44, 34, 327, DateTimeKind.Utc).AddTicks(7826), new DateTime(2025, 10, 29, 7, 44, 34, 327, DateTimeKind.Utc).AddTicks(7827) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 29, 7, 44, 34, 327, DateTimeKind.Utc).AddTicks(7830), new DateTime(2025, 10, 29, 7, 44, 34, 327, DateTimeKind.Utc).AddTicks(7831) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 29, 7, 44, 34, 327, DateTimeKind.Utc).AddTicks(7834), new DateTime(2025, 10, 29, 7, 44, 34, 327, DateTimeKind.Utc).AddTicks(7835) });

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceJobAssignments_MaintenanceJobId",
                table: "MaintenanceJobAssignments",
                column: "MaintenanceJobId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceJobAssignments_MaintenanceJobId_MechanicalEngineerId",
                table: "MaintenanceJobAssignments",
                columns: new[] { "MaintenanceJobId", "MechanicalEngineerId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceJobAssignments_MechanicalEngineerId",
                table: "MaintenanceJobAssignments",
                column: "MechanicalEngineerId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceJobs_CreatedBy",
                table: "MaintenanceJobs",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceJobs_MachineId",
                table: "MaintenanceJobs",
                column: "MachineId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceJobs_MaintenanceReportId",
                table: "MaintenanceJobs",
                column: "MaintenanceReportId",
                unique: true,
                filter: "[MaintenanceReportId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceJobs_ProjectId",
                table: "MaintenanceJobs",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceJobs_ScheduledDate",
                table: "MaintenanceJobs",
                column: "ScheduledDate");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceJobs_Status",
                table: "MaintenanceJobs",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceJobs_Type",
                table: "MaintenanceJobs",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceReports_MachineId",
                table: "MaintenanceReports",
                column: "MachineId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceReports_MechanicalEngineerId",
                table: "MaintenanceReports",
                column: "MechanicalEngineerId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceReports_OperatorId",
                table: "MaintenanceReports",
                column: "OperatorId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceReports_ReportedAt",
                table: "MaintenanceReports",
                column: "ReportedAt");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceReports_Severity",
                table: "MaintenanceReports",
                column: "Severity");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceReports_Status",
                table: "MaintenanceReports",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceReports_TicketId",
                table: "MaintenanceReports",
                column: "TicketId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MaintenanceJobAssignments");

            migrationBuilder.DropTable(
                name: "MaintenanceJobs");

            migrationBuilder.DropTable(
                name: "MaintenanceReports");

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
        }
    }
}
