using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSiteBlastingWorkflow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DrillPatterns",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    SiteId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Spacing = table.Column<double>(type: "float", nullable: false),
                    Burden = table.Column<double>(type: "float", nullable: false),
                    Depth = table.Column<double>(type: "float", nullable: false),
                    DrillPointsJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrillPatterns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DrillPatterns_ProjectSites_SiteId",
                        column: x => x.SiteId,
                        principalTable: "ProjectSites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DrillPatterns_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DrillPatterns_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SiteBlastingData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    SiteId = table.Column<int>(type: "int", nullable: false),
                    DataType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    JsonData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteBlastingData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SiteBlastingData_ProjectSites_SiteId",
                        column: x => x.SiteId,
                        principalTable: "ProjectSites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SiteBlastingData_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SiteBlastingData_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BlastSequences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    SiteId = table.Column<int>(type: "int", nullable: false),
                    DrillPatternId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ConnectionsJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SimulationSettingsJson = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "{}"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlastSequences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlastSequences_DrillPatterns_DrillPatternId",
                        column: x => x.DrillPatternId,
                        principalTable: "DrillPatterns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BlastSequences_ProjectSites_SiteId",
                        column: x => x.SiteId,
                        principalTable: "ProjectSites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BlastSequences_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BlastSequences_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 10, 7, 35, 24, 444, DateTimeKind.Utc).AddTicks(7547), new DateTime(2025, 6, 10, 7, 35, 24, 444, DateTimeKind.Utc).AddTicks(7547) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 10, 7, 35, 24, 444, DateTimeKind.Utc).AddTicks(7554), new DateTime(2025, 6, 10, 7, 35, 24, 444, DateTimeKind.Utc).AddTicks(7555) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 10, 7, 35, 24, 444, DateTimeKind.Utc).AddTicks(7558), new DateTime(2025, 6, 10, 7, 35, 24, 444, DateTimeKind.Utc).AddTicks(7558) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 10, 7, 35, 24, 444, DateTimeKind.Utc).AddTicks(7560), new DateTime(2025, 6, 10, 7, 35, 24, 444, DateTimeKind.Utc).AddTicks(7561) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 10, 7, 35, 24, 444, DateTimeKind.Utc).AddTicks(7563), new DateTime(2025, 6, 10, 7, 35, 24, 444, DateTimeKind.Utc).AddTicks(7563) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 10, 7, 35, 24, 444, DateTimeKind.Utc).AddTicks(7565), new DateTime(2025, 6, 10, 7, 35, 24, 444, DateTimeKind.Utc).AddTicks(7566) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 10, 7, 35, 24, 444, DateTimeKind.Utc).AddTicks(7568), new DateTime(2025, 6, 10, 7, 35, 24, 444, DateTimeKind.Utc).AddTicks(7568) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 10, 7, 35, 24, 444, DateTimeKind.Utc).AddTicks(7571), new DateTime(2025, 6, 10, 7, 35, 24, 444, DateTimeKind.Utc).AddTicks(7571) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 10, 7, 35, 24, 444, DateTimeKind.Utc).AddTicks(7573), new DateTime(2025, 6, 10, 7, 35, 24, 444, DateTimeKind.Utc).AddTicks(7573) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 10, 7, 35, 24, 444, DateTimeKind.Utc).AddTicks(7575), new DateTime(2025, 6, 10, 7, 35, 24, 444, DateTimeKind.Utc).AddTicks(7576) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 10, 7, 35, 24, 444, DateTimeKind.Utc).AddTicks(7578), new DateTime(2025, 6, 10, 7, 35, 24, 444, DateTimeKind.Utc).AddTicks(7578) });

            migrationBuilder.UpdateData(
                table: "ProjectSites",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 10, 7, 35, 24, 444, DateTimeKind.Utc).AddTicks(7750), new DateTime(2025, 6, 10, 7, 35, 24, 444, DateTimeKind.Utc).AddTicks(7751) });

            migrationBuilder.UpdateData(
                table: "ProjectSites",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 10, 7, 35, 24, 444, DateTimeKind.Utc).AddTicks(7755), new DateTime(2025, 6, 10, 7, 35, 24, 444, DateTimeKind.Utc).AddTicks(7756) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 10, 7, 35, 24, 444, DateTimeKind.Utc).AddTicks(7690), new DateTime(2025, 6, 10, 7, 35, 24, 444, DateTimeKind.Utc).AddTicks(7690) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 10, 7, 35, 24, 444, DateTimeKind.Utc).AddTicks(7696), new DateTime(2025, 6, 10, 7, 35, 24, 444, DateTimeKind.Utc).AddTicks(7697) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 12, 10, 7, 35, 24, 444, DateTimeKind.Utc).AddTicks(7703), new DateTime(2025, 5, 10, 7, 35, 24, 444, DateTimeKind.Utc).AddTicks(7714) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 10, 7, 35, 24, 444, DateTimeKind.Utc).AddTicks(7262), new DateTime(2025, 6, 10, 7, 35, 24, 444, DateTimeKind.Utc).AddTicks(7268) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 10, 7, 35, 24, 444, DateTimeKind.Utc).AddTicks(7275), new DateTime(2025, 6, 10, 7, 35, 24, 444, DateTimeKind.Utc).AddTicks(7275) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 10, 7, 35, 24, 444, DateTimeKind.Utc).AddTicks(7278), new DateTime(2025, 6, 10, 7, 35, 24, 444, DateTimeKind.Utc).AddTicks(7278) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 10, 7, 35, 24, 444, DateTimeKind.Utc).AddTicks(7633), new DateTime(2025, 6, 10, 7, 35, 24, 444, DateTimeKind.Utc).AddTicks(7634) });

            migrationBuilder.CreateIndex(
                name: "IX_BlastSequences_CreatedByUserId",
                table: "BlastSequences",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BlastSequences_DrillPatternId",
                table: "BlastSequences",
                column: "DrillPatternId");

            migrationBuilder.CreateIndex(
                name: "IX_BlastSequences_IsActive",
                table: "BlastSequences",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_BlastSequences_ProjectId",
                table: "BlastSequences",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_BlastSequences_ProjectId_SiteId",
                table: "BlastSequences",
                columns: new[] { "ProjectId", "SiteId" });

            migrationBuilder.CreateIndex(
                name: "IX_BlastSequences_SiteId",
                table: "BlastSequences",
                column: "SiteId");

            migrationBuilder.CreateIndex(
                name: "IX_DrillPatterns_CreatedByUserId",
                table: "DrillPatterns",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_DrillPatterns_IsActive",
                table: "DrillPatterns",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_DrillPatterns_ProjectId",
                table: "DrillPatterns",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_DrillPatterns_ProjectId_SiteId",
                table: "DrillPatterns",
                columns: new[] { "ProjectId", "SiteId" });

            migrationBuilder.CreateIndex(
                name: "IX_DrillPatterns_SiteId",
                table: "DrillPatterns",
                column: "SiteId");

            migrationBuilder.CreateIndex(
                name: "IX_SiteBlastingData_CreatedByUserId",
                table: "SiteBlastingData",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SiteBlastingData_ProjectId",
                table: "SiteBlastingData",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_SiteBlastingData_ProjectId_SiteId_DataType",
                table: "SiteBlastingData",
                columns: new[] { "ProjectId", "SiteId", "DataType" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SiteBlastingData_SiteId",
                table: "SiteBlastingData",
                column: "SiteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlastSequences");

            migrationBuilder.DropTable(
                name: "SiteBlastingData");

            migrationBuilder.DropTable(
                name: "DrillPatterns");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 5, 4, 19, 29, 302, DateTimeKind.Utc).AddTicks(2113), new DateTime(2025, 6, 5, 4, 19, 29, 302, DateTimeKind.Utc).AddTicks(2113) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 5, 4, 19, 29, 302, DateTimeKind.Utc).AddTicks(2116), new DateTime(2025, 6, 5, 4, 19, 29, 302, DateTimeKind.Utc).AddTicks(2117) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 5, 4, 19, 29, 302, DateTimeKind.Utc).AddTicks(2118), new DateTime(2025, 6, 5, 4, 19, 29, 302, DateTimeKind.Utc).AddTicks(2119) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 5, 4, 19, 29, 302, DateTimeKind.Utc).AddTicks(2120), new DateTime(2025, 6, 5, 4, 19, 29, 302, DateTimeKind.Utc).AddTicks(2120) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 5, 4, 19, 29, 302, DateTimeKind.Utc).AddTicks(2122), new DateTime(2025, 6, 5, 4, 19, 29, 302, DateTimeKind.Utc).AddTicks(2122) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 5, 4, 19, 29, 302, DateTimeKind.Utc).AddTicks(2123), new DateTime(2025, 6, 5, 4, 19, 29, 302, DateTimeKind.Utc).AddTicks(2123) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 5, 4, 19, 29, 302, DateTimeKind.Utc).AddTicks(2124), new DateTime(2025, 6, 5, 4, 19, 29, 302, DateTimeKind.Utc).AddTicks(2125) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 5, 4, 19, 29, 302, DateTimeKind.Utc).AddTicks(2126), new DateTime(2025, 6, 5, 4, 19, 29, 302, DateTimeKind.Utc).AddTicks(2126) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 5, 4, 19, 29, 302, DateTimeKind.Utc).AddTicks(2127), new DateTime(2025, 6, 5, 4, 19, 29, 302, DateTimeKind.Utc).AddTicks(2127) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 5, 4, 19, 29, 302, DateTimeKind.Utc).AddTicks(2129), new DateTime(2025, 6, 5, 4, 19, 29, 302, DateTimeKind.Utc).AddTicks(2129) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 5, 4, 19, 29, 302, DateTimeKind.Utc).AddTicks(2130), new DateTime(2025, 6, 5, 4, 19, 29, 302, DateTimeKind.Utc).AddTicks(2130) });

            migrationBuilder.UpdateData(
                table: "ProjectSites",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 5, 4, 19, 29, 302, DateTimeKind.Utc).AddTicks(2226), new DateTime(2025, 6, 5, 4, 19, 29, 302, DateTimeKind.Utc).AddTicks(2226) });

            migrationBuilder.UpdateData(
                table: "ProjectSites",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 5, 4, 19, 29, 302, DateTimeKind.Utc).AddTicks(2229), new DateTime(2025, 6, 5, 4, 19, 29, 302, DateTimeKind.Utc).AddTicks(2229) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 5, 4, 19, 29, 302, DateTimeKind.Utc).AddTicks(2188), new DateTime(2025, 6, 5, 4, 19, 29, 302, DateTimeKind.Utc).AddTicks(2188) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 5, 4, 19, 29, 302, DateTimeKind.Utc).AddTicks(2192), new DateTime(2025, 6, 5, 4, 19, 29, 302, DateTimeKind.Utc).AddTicks(2192) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 12, 5, 4, 19, 29, 302, DateTimeKind.Utc).AddTicks(2197), new DateTime(2025, 5, 5, 4, 19, 29, 302, DateTimeKind.Utc).AddTicks(2206) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 5, 4, 19, 29, 302, DateTimeKind.Utc).AddTicks(1961), new DateTime(2025, 6, 5, 4, 19, 29, 302, DateTimeKind.Utc).AddTicks(1964) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 5, 4, 19, 29, 302, DateTimeKind.Utc).AddTicks(1969), new DateTime(2025, 6, 5, 4, 19, 29, 302, DateTimeKind.Utc).AddTicks(1969) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 5, 4, 19, 29, 302, DateTimeKind.Utc).AddTicks(1971), new DateTime(2025, 6, 5, 4, 19, 29, 302, DateTimeKind.Utc).AddTicks(1971) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 5, 4, 19, 29, 302, DateTimeKind.Utc).AddTicks(2155), new DateTime(2025, 6, 5, 4, 19, 29, 302, DateTimeKind.Utc).AddTicks(2156) });
        }
    }
}
