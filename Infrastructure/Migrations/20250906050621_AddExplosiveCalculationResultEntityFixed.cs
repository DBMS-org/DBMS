using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddExplosiveCalculationResultEntityFixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExplosiveCalculations");

            migrationBuilder.CreateTable(
                name: "ExplosiveCalculationResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CalculationId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    PatternSettingsId = table.Column<int>(type: "int", nullable: true),
                    EmulsionDensity = table.Column<double>(type: "float(18)", precision: 18, scale: 6, nullable: false),
                    AnfoDensity = table.Column<double>(type: "float(18)", precision: 18, scale: 6, nullable: false),
                    EmulsionPerHole = table.Column<double>(type: "float(18)", precision: 18, scale: 6, nullable: false),
                    TotalDepth = table.Column<double>(type: "float(18)", precision: 18, scale: 6, nullable: false),
                    AverageDepth = table.Column<double>(type: "float(18)", precision: 18, scale: 6, nullable: false),
                    NumberOfFilledHoles = table.Column<int>(type: "int", nullable: false),
                    EmulsionPerMeter = table.Column<double>(type: "float(18)", precision: 18, scale: 6, nullable: false),
                    AnfoPerMeter = table.Column<double>(type: "float(18)", precision: 18, scale: 6, nullable: false),
                    EmulsionCoveringSpace = table.Column<double>(type: "float(18)", precision: 18, scale: 6, nullable: false),
                    RemainingSpace = table.Column<double>(type: "float(18)", precision: 18, scale: 6, nullable: false),
                    AnfoCoveringSpace = table.Column<double>(type: "float(18)", precision: 18, scale: 6, nullable: false),
                    TotalAnfo = table.Column<double>(type: "float(18)", precision: 18, scale: 6, nullable: false),
                    TotalEmulsion = table.Column<double>(type: "float(18)", precision: 18, scale: 6, nullable: false),
                    TotalVolume = table.Column<double>(type: "float(18)", precision: 18, scale: 6, nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    SiteId = table.Column<int>(type: "int", nullable: false),
                    OwningUserId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExplosiveCalculationResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExplosiveCalculationResults_PatternSettings_PatternSettingsId",
                        column: x => x.PatternSettingsId,
                        principalTable: "PatternSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ExplosiveCalculationResults_ProjectSites_SiteId",
                        column: x => x.SiteId,
                        principalTable: "ProjectSites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExplosiveCalculationResults_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExplosiveCalculationResultDrillPoints",
                columns: table => new
                {
                    ExplosiveCalculationResultId = table.Column<int>(type: "int", nullable: false),
                    DrillPointId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExplosiveCalculationResultDrillPoints", x => new { x.ExplosiveCalculationResultId, x.DrillPointId });
                    table.ForeignKey(
                        name: "FK_ExplosiveCalculationResultDrillPoints_DrillPoints_DrillPointId",
                        column: x => x.DrillPointId,
                        principalTable: "DrillPoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExplosiveCalculationResultDrillPoints_ExplosiveCalculationResults_ExplosiveCalculationResultId",
                        column: x => x.ExplosiveCalculationResultId,
                        principalTable: "ExplosiveCalculationResults",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 5, 6, 20, 376, DateTimeKind.Utc).AddTicks(2785), new DateTime(2025, 9, 6, 5, 6, 20, 376, DateTimeKind.Utc).AddTicks(2785) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 5, 6, 20, 376, DateTimeKind.Utc).AddTicks(2789), new DateTime(2025, 9, 6, 5, 6, 20, 376, DateTimeKind.Utc).AddTicks(2790) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 5, 6, 20, 376, DateTimeKind.Utc).AddTicks(2793), new DateTime(2025, 9, 6, 5, 6, 20, 376, DateTimeKind.Utc).AddTicks(2793) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 5, 6, 20, 376, DateTimeKind.Utc).AddTicks(2795), new DateTime(2025, 9, 6, 5, 6, 20, 376, DateTimeKind.Utc).AddTicks(2796) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 5, 6, 20, 376, DateTimeKind.Utc).AddTicks(2798), new DateTime(2025, 9, 6, 5, 6, 20, 376, DateTimeKind.Utc).AddTicks(2798) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 5, 6, 20, 376, DateTimeKind.Utc).AddTicks(2800), new DateTime(2025, 9, 6, 5, 6, 20, 376, DateTimeKind.Utc).AddTicks(2801) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 5, 6, 20, 376, DateTimeKind.Utc).AddTicks(2803), new DateTime(2025, 9, 6, 5, 6, 20, 376, DateTimeKind.Utc).AddTicks(2803) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 5, 6, 20, 376, DateTimeKind.Utc).AddTicks(2805), new DateTime(2025, 9, 6, 5, 6, 20, 376, DateTimeKind.Utc).AddTicks(2805) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 5, 6, 20, 376, DateTimeKind.Utc).AddTicks(2909), new DateTime(2025, 9, 6, 5, 6, 20, 376, DateTimeKind.Utc).AddTicks(2909) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 5, 6, 20, 376, DateTimeKind.Utc).AddTicks(2918), new DateTime(2025, 9, 6, 5, 6, 20, 376, DateTimeKind.Utc).AddTicks(2918) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 5, 6, 20, 376, DateTimeKind.Utc).AddTicks(2921), new DateTime(2025, 9, 6, 5, 6, 20, 376, DateTimeKind.Utc).AddTicks(2921) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 5, 6, 20, 376, DateTimeKind.Utc).AddTicks(2923), new DateTime(2025, 9, 6, 5, 6, 20, 376, DateTimeKind.Utc).AddTicks(2924) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 5, 6, 20, 376, DateTimeKind.Utc).AddTicks(2926), new DateTime(2025, 9, 6, 5, 6, 20, 376, DateTimeKind.Utc).AddTicks(2926) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 5, 6, 20, 376, DateTimeKind.Utc).AddTicks(2928), new DateTime(2025, 9, 6, 5, 6, 20, 376, DateTimeKind.Utc).AddTicks(2929) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 5, 6, 20, 376, DateTimeKind.Utc).AddTicks(2931), new DateTime(2025, 9, 6, 5, 6, 20, 376, DateTimeKind.Utc).AddTicks(2931) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 5, 6, 20, 376, DateTimeKind.Utc).AddTicks(2933), new DateTime(2025, 9, 6, 5, 6, 20, 376, DateTimeKind.Utc).AddTicks(2934) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 5, 6, 20, 376, DateTimeKind.Utc).AddTicks(2936), new DateTime(2025, 9, 6, 5, 6, 20, 376, DateTimeKind.Utc).AddTicks(2936) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 5, 6, 20, 376, DateTimeKind.Utc).AddTicks(2938), new DateTime(2025, 9, 6, 5, 6, 20, 376, DateTimeKind.Utc).AddTicks(2938) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 5, 6, 20, 376, DateTimeKind.Utc).AddTicks(2940), new DateTime(2025, 9, 6, 5, 6, 20, 376, DateTimeKind.Utc).AddTicks(2941) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 1,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 6, 5, 6, 20, 376, DateTimeKind.Utc).AddTicks(2845));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 2,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 6, 5, 6, 20, 376, DateTimeKind.Utc).AddTicks(2850));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 3,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 6, 5, 6, 20, 376, DateTimeKind.Utc).AddTicks(2851));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 4,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 6, 5, 6, 20, 376, DateTimeKind.Utc).AddTicks(2853));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 5,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 6, 5, 6, 20, 376, DateTimeKind.Utc).AddTicks(2854));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 6,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 6, 5, 6, 20, 376, DateTimeKind.Utc).AddTicks(2856));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 7,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 6, 5, 6, 20, 376, DateTimeKind.Utc).AddTicks(2857));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 8,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 6, 5, 6, 20, 376, DateTimeKind.Utc).AddTicks(2859));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 9,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 6, 5, 6, 20, 376, DateTimeKind.Utc).AddTicks(2860));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 5, 6, 20, 376, DateTimeKind.Utc).AddTicks(2513), new DateTime(2025, 9, 6, 5, 6, 20, 376, DateTimeKind.Utc).AddTicks(2515) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 5, 6, 20, 376, DateTimeKind.Utc).AddTicks(2521), new DateTime(2025, 9, 6, 5, 6, 20, 376, DateTimeKind.Utc).AddTicks(2522) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 5, 6, 20, 376, DateTimeKind.Utc).AddTicks(2524), new DateTime(2025, 9, 6, 5, 6, 20, 376, DateTimeKind.Utc).AddTicks(2525) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 5, 6, 20, 376, DateTimeKind.Utc).AddTicks(2562), new DateTime(2025, 9, 6, 5, 6, 20, 376, DateTimeKind.Utc).AddTicks(2563) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 5, 6, 20, 376, DateTimeKind.Utc).AddTicks(2566), new DateTime(2025, 9, 6, 5, 6, 20, 376, DateTimeKind.Utc).AddTicks(2566) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 5, 6, 20, 376, DateTimeKind.Utc).AddTicks(2568), new DateTime(2025, 9, 6, 5, 6, 20, 376, DateTimeKind.Utc).AddTicks(2569) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 5, 6, 20, 376, DateTimeKind.Utc).AddTicks(2571), new DateTime(2025, 9, 6, 5, 6, 20, 376, DateTimeKind.Utc).AddTicks(2571) });

            migrationBuilder.CreateIndex(
                name: "IX_ExplosiveCalculationResultDrillPoints_DrillPointId",
                table: "ExplosiveCalculationResultDrillPoints",
                column: "DrillPointId");

            migrationBuilder.CreateIndex(
                name: "IX_ExplosiveCalculationResults_CalculationId",
                table: "ExplosiveCalculationResults",
                column: "CalculationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExplosiveCalculationResults_PatternSettingsId",
                table: "ExplosiveCalculationResults",
                column: "PatternSettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_ExplosiveCalculationResults_ProjectSite_CreatedAt",
                table: "ExplosiveCalculationResults",
                columns: new[] { "ProjectId", "SiteId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_ExplosiveCalculationResults_SiteId",
                table: "ExplosiveCalculationResults",
                column: "SiteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExplosiveCalculationResultDrillPoints");

            migrationBuilder.DropTable(
                name: "ExplosiveCalculationResults");

            migrationBuilder.CreateTable(
                name: "ExplosiveCalculations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    SiteId = table.Column<int>(type: "int", nullable: false),
                    AnfoCoveringSpace = table.Column<double>(type: "float", nullable: false),
                    AnfoDensity = table.Column<double>(type: "float", nullable: false),
                    AnfoPerMeter = table.Column<double>(type: "float", nullable: false),
                    AverageDepth = table.Column<double>(type: "float", nullable: false),
                    BenchHeight = table.Column<double>(type: "float", nullable: false),
                    Burden = table.Column<double>(type: "float", nullable: false),
                    CalculatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    DepthsJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DrillDataSource = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmulsionCoveringSpace = table.Column<double>(type: "float", nullable: false),
                    EmulsionDensity = table.Column<double>(type: "float", nullable: false),
                    EmulsionPerHole = table.Column<double>(type: "float", nullable: false),
                    EmulsionPerMeter = table.Column<double>(type: "float", nullable: false),
                    HoleDiameter = table.Column<double>(type: "float", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    NumberOfColumns = table.Column<int>(type: "int", nullable: false),
                    NumberOfFilledHoles = table.Column<int>(type: "int", nullable: false),
                    NumberOfHoles = table.Column<int>(type: "int", nullable: false),
                    PowderFactor = table.Column<double>(type: "float", nullable: false),
                    RemainingSpace = table.Column<double>(type: "float", nullable: false),
                    Spacing = table.Column<double>(type: "float", nullable: false),
                    Stemming = table.Column<double>(type: "float", nullable: false),
                    SubdrillLength = table.Column<double>(type: "float", nullable: false),
                    TotalAnfo = table.Column<double>(type: "float", nullable: false),
                    TotalDepth = table.Column<double>(type: "float", nullable: false),
                    TotalEmulsion = table.Column<double>(type: "float", nullable: false),
                    TotalExplosive = table.Column<double>(type: "float", nullable: false),
                    TotalRock = table.Column<double>(type: "float", nullable: false),
                    TotalVolume = table.Column<double>(type: "float", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExplosiveCalculations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExplosiveCalculations_ProjectSites_SiteId",
                        column: x => x.SiteId,
                        principalTable: "ProjectSites",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ExplosiveCalculations_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 4, 9, 5, 36, 555, DateTimeKind.Utc).AddTicks(9384), new DateTime(2025, 9, 4, 9, 5, 36, 555, DateTimeKind.Utc).AddTicks(9384) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 4, 9, 5, 36, 555, DateTimeKind.Utc).AddTicks(9389), new DateTime(2025, 9, 4, 9, 5, 36, 555, DateTimeKind.Utc).AddTicks(9389) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 4, 9, 5, 36, 555, DateTimeKind.Utc).AddTicks(9392), new DateTime(2025, 9, 4, 9, 5, 36, 555, DateTimeKind.Utc).AddTicks(9392) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 4, 9, 5, 36, 555, DateTimeKind.Utc).AddTicks(9395), new DateTime(2025, 9, 4, 9, 5, 36, 555, DateTimeKind.Utc).AddTicks(9395) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 4, 9, 5, 36, 555, DateTimeKind.Utc).AddTicks(9397), new DateTime(2025, 9, 4, 9, 5, 36, 555, DateTimeKind.Utc).AddTicks(9398) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 4, 9, 5, 36, 555, DateTimeKind.Utc).AddTicks(9400), new DateTime(2025, 9, 4, 9, 5, 36, 555, DateTimeKind.Utc).AddTicks(9400) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 4, 9, 5, 36, 555, DateTimeKind.Utc).AddTicks(9402), new DateTime(2025, 9, 4, 9, 5, 36, 555, DateTimeKind.Utc).AddTicks(9402) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 4, 9, 5, 36, 555, DateTimeKind.Utc).AddTicks(9404), new DateTime(2025, 9, 4, 9, 5, 36, 555, DateTimeKind.Utc).AddTicks(9405) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 4, 9, 5, 36, 555, DateTimeKind.Utc).AddTicks(9519), new DateTime(2025, 9, 4, 9, 5, 36, 555, DateTimeKind.Utc).AddTicks(9519) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 4, 9, 5, 36, 555, DateTimeKind.Utc).AddTicks(9527), new DateTime(2025, 9, 4, 9, 5, 36, 555, DateTimeKind.Utc).AddTicks(9528) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 4, 9, 5, 36, 555, DateTimeKind.Utc).AddTicks(9530), new DateTime(2025, 9, 4, 9, 5, 36, 555, DateTimeKind.Utc).AddTicks(9531) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 4, 9, 5, 36, 555, DateTimeKind.Utc).AddTicks(9533), new DateTime(2025, 9, 4, 9, 5, 36, 555, DateTimeKind.Utc).AddTicks(9533) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 4, 9, 5, 36, 555, DateTimeKind.Utc).AddTicks(9535), new DateTime(2025, 9, 4, 9, 5, 36, 555, DateTimeKind.Utc).AddTicks(9536) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 4, 9, 5, 36, 555, DateTimeKind.Utc).AddTicks(9537), new DateTime(2025, 9, 4, 9, 5, 36, 555, DateTimeKind.Utc).AddTicks(9538) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 4, 9, 5, 36, 555, DateTimeKind.Utc).AddTicks(9613), new DateTime(2025, 9, 4, 9, 5, 36, 555, DateTimeKind.Utc).AddTicks(9613) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 4, 9, 5, 36, 555, DateTimeKind.Utc).AddTicks(9615), new DateTime(2025, 9, 4, 9, 5, 36, 555, DateTimeKind.Utc).AddTicks(9616) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 4, 9, 5, 36, 555, DateTimeKind.Utc).AddTicks(9618), new DateTime(2025, 9, 4, 9, 5, 36, 555, DateTimeKind.Utc).AddTicks(9618) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 4, 9, 5, 36, 555, DateTimeKind.Utc).AddTicks(9620), new DateTime(2025, 9, 4, 9, 5, 36, 555, DateTimeKind.Utc).AddTicks(9621) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 4, 9, 5, 36, 555, DateTimeKind.Utc).AddTicks(9623), new DateTime(2025, 9, 4, 9, 5, 36, 555, DateTimeKind.Utc).AddTicks(9623) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 1,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 4, 9, 5, 36, 555, DateTimeKind.Utc).AddTicks(9454));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 2,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 4, 9, 5, 36, 555, DateTimeKind.Utc).AddTicks(9457));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 3,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 4, 9, 5, 36, 555, DateTimeKind.Utc).AddTicks(9459));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 4,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 4, 9, 5, 36, 555, DateTimeKind.Utc).AddTicks(9461));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 5,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 4, 9, 5, 36, 555, DateTimeKind.Utc).AddTicks(9462));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 6,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 4, 9, 5, 36, 555, DateTimeKind.Utc).AddTicks(9464));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 7,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 4, 9, 5, 36, 555, DateTimeKind.Utc).AddTicks(9465));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 8,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 4, 9, 5, 36, 555, DateTimeKind.Utc).AddTicks(9466));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 9,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 4, 9, 5, 36, 555, DateTimeKind.Utc).AddTicks(9468));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 4, 9, 5, 36, 555, DateTimeKind.Utc).AddTicks(9135), new DateTime(2025, 9, 4, 9, 5, 36, 555, DateTimeKind.Utc).AddTicks(9137) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 4, 9, 5, 36, 555, DateTimeKind.Utc).AddTicks(9144), new DateTime(2025, 9, 4, 9, 5, 36, 555, DateTimeKind.Utc).AddTicks(9145) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 4, 9, 5, 36, 555, DateTimeKind.Utc).AddTicks(9147), new DateTime(2025, 9, 4, 9, 5, 36, 555, DateTimeKind.Utc).AddTicks(9148) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 4, 9, 5, 36, 555, DateTimeKind.Utc).AddTicks(9150), new DateTime(2025, 9, 4, 9, 5, 36, 555, DateTimeKind.Utc).AddTicks(9150) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 4, 9, 5, 36, 555, DateTimeKind.Utc).AddTicks(9153), new DateTime(2025, 9, 4, 9, 5, 36, 555, DateTimeKind.Utc).AddTicks(9153) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 4, 9, 5, 36, 555, DateTimeKind.Utc).AddTicks(9155), new DateTime(2025, 9, 4, 9, 5, 36, 555, DateTimeKind.Utc).AddTicks(9155) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 4, 9, 5, 36, 555, DateTimeKind.Utc).AddTicks(9157), new DateTime(2025, 9, 4, 9, 5, 36, 555, DateTimeKind.Utc).AddTicks(9158) });

            migrationBuilder.CreateIndex(
                name: "IX_ExplosiveCalculations_ProjectId",
                table: "ExplosiveCalculations",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ExplosiveCalculations_SiteId",
                table: "ExplosiveCalculations",
                column: "SiteId");
        }
    }
}
