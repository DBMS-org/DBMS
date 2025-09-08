using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddExplosiveCalculationEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExplosiveCalculations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    SiteId = table.Column<int>(type: "int", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    CreatedByName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumberOfHoles = table.Column<int>(type: "int", nullable: false),
                    NumberOfColumns = table.Column<int>(type: "int", nullable: false),
                    HoleDiameter = table.Column<double>(type: "float", nullable: false),
                    Stemming = table.Column<double>(type: "float", nullable: false),
                    SubdrillLength = table.Column<double>(type: "float", nullable: false),
                    BenchHeight = table.Column<double>(type: "float", nullable: false),
                    Spacing = table.Column<double>(type: "float", nullable: false),
                    Burden = table.Column<double>(type: "float", nullable: false),
                    EmulsionDensity = table.Column<double>(type: "float", nullable: false),
                    AnfoDensity = table.Column<double>(type: "float", nullable: false),
                    EmulsionPerHole = table.Column<double>(type: "float", nullable: false),
                    DepthsJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalDepth = table.Column<double>(type: "float", nullable: false),
                    AverageDepth = table.Column<double>(type: "float", nullable: false),
                    NumberOfFilledHoles = table.Column<int>(type: "int", nullable: false),
                    EmulsionPerMeter = table.Column<double>(type: "float", nullable: false),
                    AnfoPerMeter = table.Column<double>(type: "float", nullable: false),
                    EmulsionCoveringSpace = table.Column<double>(type: "float", nullable: false),
                    RemainingSpace = table.Column<double>(type: "float", nullable: false),
                    AnfoCoveringSpace = table.Column<double>(type: "float", nullable: false),
                    TotalAnfo = table.Column<double>(type: "float", nullable: false),
                    TotalEmulsion = table.Column<double>(type: "float", nullable: false),
                    TotalExplosive = table.Column<double>(type: "float", nullable: false),
                    PowderFactor = table.Column<double>(type: "float", nullable: false),
                    TotalRock = table.Column<double>(type: "float", nullable: false),
                    TotalVolume = table.Column<double>(type: "float", nullable: false),
                    DrillDataSource = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CalculatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExplosiveCalculations");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(8153), new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(8154) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(8160), new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(8160) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(8163), new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(8163) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(8165), new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(8166) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(8168), new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(8168) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(8170), new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(8170) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(8172), new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(8173) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(8174), new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(8175) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(8292), new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(8293) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(8302), new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(8302) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(8305), new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(8305) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(8307), new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(8308) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(8310), new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(8310) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(8313), new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(8313) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(8315), new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(8316) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(8318), new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(8318) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(8320), new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(8320) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(8322), new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(8323) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(8324), new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(8325) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 1,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(8216));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 2,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(8221));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 3,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(8223));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 4,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(8224));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 5,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(8226));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 6,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(8228));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 7,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(8229));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 8,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(8231));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 9,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(8232));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(7915), new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(7920) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(7927), new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(7927) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(7930), new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(7930) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(7932), new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(7933) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(7935), new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(7935) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(7937), new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(7938) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(7940), new DateTime(2025, 9, 1, 7, 50, 34, 710, DateTimeKind.Utc).AddTicks(7940) });
        }
    }
}
