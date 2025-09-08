using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveExplosiveCalculationResultDrillPointsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExplosiveCalculationResultDrillPoints");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7497), new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7497) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7504), new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7505) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7507), new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7508) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7510), new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7511) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7513), new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7513) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7515), new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7516) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7518), new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7518) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7572), new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7572) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7688), new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7688) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7697), new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7698) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7700), new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7701) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7703), new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7703) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7705), new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7705) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7707), new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7708) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7710), new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7711) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7713), new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7713) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7715), new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7715) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7717), new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7718) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7720), new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7720) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 1,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7616));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 2,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7620));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 3,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7622));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 4,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7624));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 5,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7625));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 6,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7627));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 7,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7628));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 8,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7630));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 9,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7631));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7214), new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7217) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7223), new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7223) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7226), new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7227) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7229), new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7229) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7232), new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7232) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7234), new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7234) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7236), new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7237) });

            migrationBuilder.CreateIndex(
                name: "IX_ExplosiveCalculationResults_OwningUserId",
                table: "ExplosiveCalculationResults",
                column: "OwningUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExplosiveCalculationResults_Users_OwningUserId",
                table: "ExplosiveCalculationResults",
                column: "OwningUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExplosiveCalculationResults_Users_OwningUserId",
                table: "ExplosiveCalculationResults");

            migrationBuilder.DropIndex(
                name: "IX_ExplosiveCalculationResults_OwningUserId",
                table: "ExplosiveCalculationResults");

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
        }
    }
}
