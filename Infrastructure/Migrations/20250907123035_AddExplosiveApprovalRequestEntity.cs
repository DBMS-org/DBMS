using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddExplosiveApprovalRequestEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpectedUsageDate",
                table: "ProjectSites");

            migrationBuilder.DropColumn(
                name: "ExplosiveApprovalComments",
                table: "ProjectSites");

            migrationBuilder.DropColumn(
                name: "IsExplosiveApprovalRequested",
                table: "ProjectSites");

            migrationBuilder.CreateTable(
                name: "ExplosiveApprovalRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectSiteId = table.Column<int>(type: "int", nullable: false),
                    RequestedByUserId = table.Column<int>(type: "int", nullable: false),
                    ExpectedUsageDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    ApprovalType = table.Column<int>(type: "int", nullable: false),
                    ProcessedByUserId = table.Column<int>(type: "int", nullable: true),
                    ProcessedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RejectionReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    AdditionalData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EstimatedDurationHours = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    SafetyChecklistCompleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    EnvironmentalAssessmentCompleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExplosiveApprovalRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExplosiveApprovalRequests_ProjectSites_ProjectSiteId",
                        column: x => x.ProjectSiteId,
                        principalTable: "ProjectSites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExplosiveApprovalRequests_Users_ProcessedByUserId",
                        column: x => x.ProcessedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExplosiveApprovalRequests_Users_RequestedByUserId",
                        column: x => x.RequestedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(9066), new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(9067) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(9071), new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(9072) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(9074), new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(9075) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(9077), new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(9078) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(9080), new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(9080) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(9082), new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(9082) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(9084), new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(9085) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(9087), new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(9087) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(9212), new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(9213) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(9224), new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(9225) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(9227), new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(9228) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(9230), new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(9230) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(9232), new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(9233) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(9235), new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(9235) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(9237), new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(9238) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(9239), new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(9240) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(9242), new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(9242) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(9244), new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(9244) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(9246), new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(9247) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 1,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(9136));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 2,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(9142));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 3,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(9144));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 4,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(9145));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 5,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(9147));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 6,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(9148));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 7,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(9149));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 8,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(9151));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 9,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(9152));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(8770), new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(8774) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(8786), new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(8786) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(8789), new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(8789) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(8792), new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(8792) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(8794), new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(8795) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(8797), new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(8797) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(8799), new DateTime(2025, 9, 7, 12, 30, 34, 862, DateTimeKind.Utc).AddTicks(8799) });

            migrationBuilder.CreateIndex(
                name: "IX_ExplosiveApprovalRequests_ExpectedUsageDate",
                table: "ExplosiveApprovalRequests",
                column: "ExpectedUsageDate");

            migrationBuilder.CreateIndex(
                name: "IX_ExplosiveApprovalRequests_ProcessedByUserId",
                table: "ExplosiveApprovalRequests",
                column: "ProcessedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ExplosiveApprovalRequests_ProjectSite_Status",
                table: "ExplosiveApprovalRequests",
                columns: new[] { "ProjectSiteId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_ExplosiveApprovalRequests_ProjectSiteId",
                table: "ExplosiveApprovalRequests",
                column: "ProjectSiteId");

            migrationBuilder.CreateIndex(
                name: "IX_ExplosiveApprovalRequests_RequestedByUserId",
                table: "ExplosiveApprovalRequests",
                column: "RequestedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ExplosiveApprovalRequests_Status",
                table: "ExplosiveApprovalRequests",
                column: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExplosiveApprovalRequests");

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpectedUsageDate",
                table: "ProjectSites",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExplosiveApprovalComments",
                table: "ProjectSites",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsExplosiveApprovalRequested",
                table: "ProjectSites",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 8, 55, 12, 289, DateTimeKind.Utc).AddTicks(2574), new DateTime(2025, 9, 7, 8, 55, 12, 289, DateTimeKind.Utc).AddTicks(2575) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 8, 55, 12, 289, DateTimeKind.Utc).AddTicks(2584), new DateTime(2025, 9, 7, 8, 55, 12, 289, DateTimeKind.Utc).AddTicks(2585) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 8, 55, 12, 289, DateTimeKind.Utc).AddTicks(2588), new DateTime(2025, 9, 7, 8, 55, 12, 289, DateTimeKind.Utc).AddTicks(2588) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 8, 55, 12, 289, DateTimeKind.Utc).AddTicks(2590), new DateTime(2025, 9, 7, 8, 55, 12, 289, DateTimeKind.Utc).AddTicks(2591) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 8, 55, 12, 289, DateTimeKind.Utc).AddTicks(2592), new DateTime(2025, 9, 7, 8, 55, 12, 289, DateTimeKind.Utc).AddTicks(2593) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 8, 55, 12, 289, DateTimeKind.Utc).AddTicks(2595), new DateTime(2025, 9, 7, 8, 55, 12, 289, DateTimeKind.Utc).AddTicks(2595) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 8, 55, 12, 289, DateTimeKind.Utc).AddTicks(2597), new DateTime(2025, 9, 7, 8, 55, 12, 289, DateTimeKind.Utc).AddTicks(2597) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 8, 55, 12, 289, DateTimeKind.Utc).AddTicks(2599), new DateTime(2025, 9, 7, 8, 55, 12, 289, DateTimeKind.Utc).AddTicks(2600) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 8, 55, 12, 289, DateTimeKind.Utc).AddTicks(2705), new DateTime(2025, 9, 7, 8, 55, 12, 289, DateTimeKind.Utc).AddTicks(2706) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 8, 55, 12, 289, DateTimeKind.Utc).AddTicks(2714), new DateTime(2025, 9, 7, 8, 55, 12, 289, DateTimeKind.Utc).AddTicks(2714) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 8, 55, 12, 289, DateTimeKind.Utc).AddTicks(2768), new DateTime(2025, 9, 7, 8, 55, 12, 289, DateTimeKind.Utc).AddTicks(2768) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 8, 55, 12, 289, DateTimeKind.Utc).AddTicks(2771), new DateTime(2025, 9, 7, 8, 55, 12, 289, DateTimeKind.Utc).AddTicks(2771) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 8, 55, 12, 289, DateTimeKind.Utc).AddTicks(2773), new DateTime(2025, 9, 7, 8, 55, 12, 289, DateTimeKind.Utc).AddTicks(2773) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 8, 55, 12, 289, DateTimeKind.Utc).AddTicks(2775), new DateTime(2025, 9, 7, 8, 55, 12, 289, DateTimeKind.Utc).AddTicks(2776) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 8, 55, 12, 289, DateTimeKind.Utc).AddTicks(2778), new DateTime(2025, 9, 7, 8, 55, 12, 289, DateTimeKind.Utc).AddTicks(2778) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 8, 55, 12, 289, DateTimeKind.Utc).AddTicks(2780), new DateTime(2025, 9, 7, 8, 55, 12, 289, DateTimeKind.Utc).AddTicks(2780) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 8, 55, 12, 289, DateTimeKind.Utc).AddTicks(2782), new DateTime(2025, 9, 7, 8, 55, 12, 289, DateTimeKind.Utc).AddTicks(2783) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 8, 55, 12, 289, DateTimeKind.Utc).AddTicks(2785), new DateTime(2025, 9, 7, 8, 55, 12, 289, DateTimeKind.Utc).AddTicks(2785) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 8, 55, 12, 289, DateTimeKind.Utc).AddTicks(2787), new DateTime(2025, 9, 7, 8, 55, 12, 289, DateTimeKind.Utc).AddTicks(2787) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 1,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 7, 8, 55, 12, 289, DateTimeKind.Utc).AddTicks(2638));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 2,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 7, 8, 55, 12, 289, DateTimeKind.Utc).AddTicks(2644));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 3,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 7, 8, 55, 12, 289, DateTimeKind.Utc).AddTicks(2646));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 4,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 7, 8, 55, 12, 289, DateTimeKind.Utc).AddTicks(2648));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 5,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 7, 8, 55, 12, 289, DateTimeKind.Utc).AddTicks(2649));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 6,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 7, 8, 55, 12, 289, DateTimeKind.Utc).AddTicks(2651));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 7,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 7, 8, 55, 12, 289, DateTimeKind.Utc).AddTicks(2652));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 8,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 7, 8, 55, 12, 289, DateTimeKind.Utc).AddTicks(2654));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 9,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 7, 8, 55, 12, 289, DateTimeKind.Utc).AddTicks(2655));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 8, 55, 12, 289, DateTimeKind.Utc).AddTicks(2329), new DateTime(2025, 9, 7, 8, 55, 12, 289, DateTimeKind.Utc).AddTicks(2332) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 8, 55, 12, 289, DateTimeKind.Utc).AddTicks(2339), new DateTime(2025, 9, 7, 8, 55, 12, 289, DateTimeKind.Utc).AddTicks(2339) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 8, 55, 12, 289, DateTimeKind.Utc).AddTicks(2342), new DateTime(2025, 9, 7, 8, 55, 12, 289, DateTimeKind.Utc).AddTicks(2342) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 8, 55, 12, 289, DateTimeKind.Utc).AddTicks(2345), new DateTime(2025, 9, 7, 8, 55, 12, 289, DateTimeKind.Utc).AddTicks(2345) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 8, 55, 12, 289, DateTimeKind.Utc).AddTicks(2347), new DateTime(2025, 9, 7, 8, 55, 12, 289, DateTimeKind.Utc).AddTicks(2348) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 8, 55, 12, 289, DateTimeKind.Utc).AddTicks(2350), new DateTime(2025, 9, 7, 8, 55, 12, 289, DateTimeKind.Utc).AddTicks(2350) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 8, 55, 12, 289, DateTimeKind.Utc).AddTicks(2352), new DateTime(2025, 9, 7, 8, 55, 12, 289, DateTimeKind.Utc).AddTicks(2352) });
        }
    }
}
