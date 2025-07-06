using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBlastConnectionForeignKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DetonatorInfos_ProjectId",
                table: "DetonatorInfos");

            migrationBuilder.DropIndex(
                name: "IX_BlastConnections_Point1DrillPointId",
                table: "BlastConnections");

            migrationBuilder.DropIndex(
                name: "IX_BlastConnections_ProjectId",
                table: "BlastConnections");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 12, 13, 43, 760, DateTimeKind.Utc).AddTicks(7328), new DateTime(2025, 7, 6, 12, 13, 43, 760, DateTimeKind.Utc).AddTicks(7328) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 12, 13, 43, 760, DateTimeKind.Utc).AddTicks(7333), new DateTime(2025, 7, 6, 12, 13, 43, 760, DateTimeKind.Utc).AddTicks(7334) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 12, 13, 43, 760, DateTimeKind.Utc).AddTicks(7337), new DateTime(2025, 7, 6, 12, 13, 43, 760, DateTimeKind.Utc).AddTicks(7337) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 12, 13, 43, 760, DateTimeKind.Utc).AddTicks(7339), new DateTime(2025, 7, 6, 12, 13, 43, 760, DateTimeKind.Utc).AddTicks(7340) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 12, 13, 43, 760, DateTimeKind.Utc).AddTicks(7342), new DateTime(2025, 7, 6, 12, 13, 43, 760, DateTimeKind.Utc).AddTicks(7342) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 12, 13, 43, 760, DateTimeKind.Utc).AddTicks(7344), new DateTime(2025, 7, 6, 12, 13, 43, 760, DateTimeKind.Utc).AddTicks(7345) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 12, 13, 43, 760, DateTimeKind.Utc).AddTicks(7392), new DateTime(2025, 7, 6, 12, 13, 43, 760, DateTimeKind.Utc).AddTicks(7393) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 12, 13, 43, 760, DateTimeKind.Utc).AddTicks(7395), new DateTime(2025, 7, 6, 12, 13, 43, 760, DateTimeKind.Utc).AddTicks(7396) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 12, 13, 43, 760, DateTimeKind.Utc).AddTicks(7560), new DateTime(2025, 7, 6, 12, 13, 43, 760, DateTimeKind.Utc).AddTicks(7561) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 12, 13, 43, 760, DateTimeKind.Utc).AddTicks(7569), new DateTime(2025, 7, 6, 12, 13, 43, 760, DateTimeKind.Utc).AddTicks(7570) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 12, 13, 43, 760, DateTimeKind.Utc).AddTicks(7573), new DateTime(2025, 7, 6, 12, 13, 43, 760, DateTimeKind.Utc).AddTicks(7573) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 12, 13, 43, 760, DateTimeKind.Utc).AddTicks(7575), new DateTime(2025, 7, 6, 12, 13, 43, 760, DateTimeKind.Utc).AddTicks(7576) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 12, 13, 43, 760, DateTimeKind.Utc).AddTicks(7577), new DateTime(2025, 7, 6, 12, 13, 43, 760, DateTimeKind.Utc).AddTicks(7578) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 12, 13, 43, 760, DateTimeKind.Utc).AddTicks(7580), new DateTime(2025, 7, 6, 12, 13, 43, 760, DateTimeKind.Utc).AddTicks(7580) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 12, 13, 43, 760, DateTimeKind.Utc).AddTicks(7582), new DateTime(2025, 7, 6, 12, 13, 43, 760, DateTimeKind.Utc).AddTicks(7583) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 12, 13, 43, 760, DateTimeKind.Utc).AddTicks(7585), new DateTime(2025, 7, 6, 12, 13, 43, 760, DateTimeKind.Utc).AddTicks(7585) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 12, 13, 43, 760, DateTimeKind.Utc).AddTicks(7587), new DateTime(2025, 7, 6, 12, 13, 43, 760, DateTimeKind.Utc).AddTicks(7588) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 12, 13, 43, 760, DateTimeKind.Utc).AddTicks(7590), new DateTime(2025, 7, 6, 12, 13, 43, 760, DateTimeKind.Utc).AddTicks(7590) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 12, 13, 43, 760, DateTimeKind.Utc).AddTicks(7592), new DateTime(2025, 7, 6, 12, 13, 43, 760, DateTimeKind.Utc).AddTicks(7592) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 1,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 6, 12, 13, 43, 760, DateTimeKind.Utc).AddTicks(7460));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 2,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 6, 12, 13, 43, 760, DateTimeKind.Utc).AddTicks(7467));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 3,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 6, 12, 13, 43, 760, DateTimeKind.Utc).AddTicks(7469));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 4,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 6, 12, 13, 43, 760, DateTimeKind.Utc).AddTicks(7470));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 5,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 6, 12, 13, 43, 760, DateTimeKind.Utc).AddTicks(7472));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 6,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 6, 12, 13, 43, 760, DateTimeKind.Utc).AddTicks(7474));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 7,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 6, 12, 13, 43, 760, DateTimeKind.Utc).AddTicks(7475));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 8,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 6, 12, 13, 43, 760, DateTimeKind.Utc).AddTicks(7477));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 9,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 6, 12, 13, 43, 760, DateTimeKind.Utc).AddTicks(7478));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 12, 13, 43, 760, DateTimeKind.Utc).AddTicks(6975), new DateTime(2025, 7, 6, 12, 13, 43, 760, DateTimeKind.Utc).AddTicks(6980) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 12, 13, 43, 760, DateTimeKind.Utc).AddTicks(6987), new DateTime(2025, 7, 6, 12, 13, 43, 760, DateTimeKind.Utc).AddTicks(6987) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 12, 13, 43, 760, DateTimeKind.Utc).AddTicks(6990), new DateTime(2025, 7, 6, 12, 13, 43, 760, DateTimeKind.Utc).AddTicks(6991) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 12, 13, 43, 760, DateTimeKind.Utc).AddTicks(6993), new DateTime(2025, 7, 6, 12, 13, 43, 760, DateTimeKind.Utc).AddTicks(6993) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 12, 13, 43, 760, DateTimeKind.Utc).AddTicks(6995), new DateTime(2025, 7, 6, 12, 13, 43, 760, DateTimeKind.Utc).AddTicks(6996) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 12, 13, 43, 760, DateTimeKind.Utc).AddTicks(6998), new DateTime(2025, 7, 6, 12, 13, 43, 760, DateTimeKind.Utc).AddTicks(6998) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 12, 13, 43, 760, DateTimeKind.Utc).AddTicks(7000), new DateTime(2025, 7, 6, 12, 13, 43, 760, DateTimeKind.Utc).AddTicks(7000) });

            migrationBuilder.CreateIndex(
                name: "IX_DetonatorInfos_ProjectId_SiteId",
                table: "DetonatorInfos",
                columns: new[] { "ProjectId", "SiteId" });

            migrationBuilder.CreateIndex(
                name: "IX_BlastConnections_Point1DrillPointId_Point2DrillPointId",
                table: "BlastConnections",
                columns: new[] { "Point1DrillPointId", "Point2DrillPointId" });

            migrationBuilder.CreateIndex(
                name: "IX_BlastConnections_ProjectId_SiteId",
                table: "BlastConnections",
                columns: new[] { "ProjectId", "SiteId" });

            migrationBuilder.CreateIndex(
                name: "IX_BlastConnections_Sequence",
                table: "BlastConnections",
                column: "Sequence");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DetonatorInfos_ProjectId_SiteId",
                table: "DetonatorInfos");

            migrationBuilder.DropIndex(
                name: "IX_BlastConnections_Point1DrillPointId_Point2DrillPointId",
                table: "BlastConnections");

            migrationBuilder.DropIndex(
                name: "IX_BlastConnections_ProjectId_SiteId",
                table: "BlastConnections");

            migrationBuilder.DropIndex(
                name: "IX_BlastConnections_Sequence",
                table: "BlastConnections");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 10, 37, 45, 668, DateTimeKind.Utc).AddTicks(2548), new DateTime(2025, 7, 6, 10, 37, 45, 668, DateTimeKind.Utc).AddTicks(2548) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 10, 37, 45, 668, DateTimeKind.Utc).AddTicks(2557), new DateTime(2025, 7, 6, 10, 37, 45, 668, DateTimeKind.Utc).AddTicks(2557) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 10, 37, 45, 668, DateTimeKind.Utc).AddTicks(2560), new DateTime(2025, 7, 6, 10, 37, 45, 668, DateTimeKind.Utc).AddTicks(2561) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 10, 37, 45, 668, DateTimeKind.Utc).AddTicks(2563), new DateTime(2025, 7, 6, 10, 37, 45, 668, DateTimeKind.Utc).AddTicks(2563) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 10, 37, 45, 668, DateTimeKind.Utc).AddTicks(2566), new DateTime(2025, 7, 6, 10, 37, 45, 668, DateTimeKind.Utc).AddTicks(2566) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 10, 37, 45, 668, DateTimeKind.Utc).AddTicks(2568), new DateTime(2025, 7, 6, 10, 37, 45, 668, DateTimeKind.Utc).AddTicks(2568) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 10, 37, 45, 668, DateTimeKind.Utc).AddTicks(2571), new DateTime(2025, 7, 6, 10, 37, 45, 668, DateTimeKind.Utc).AddTicks(2571) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 10, 37, 45, 668, DateTimeKind.Utc).AddTicks(2573), new DateTime(2025, 7, 6, 10, 37, 45, 668, DateTimeKind.Utc).AddTicks(2573) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 10, 37, 45, 668, DateTimeKind.Utc).AddTicks(2726), new DateTime(2025, 7, 6, 10, 37, 45, 668, DateTimeKind.Utc).AddTicks(2727) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 10, 37, 45, 668, DateTimeKind.Utc).AddTicks(2738), new DateTime(2025, 7, 6, 10, 37, 45, 668, DateTimeKind.Utc).AddTicks(2739) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 10, 37, 45, 668, DateTimeKind.Utc).AddTicks(2741), new DateTime(2025, 7, 6, 10, 37, 45, 668, DateTimeKind.Utc).AddTicks(2742) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 10, 37, 45, 668, DateTimeKind.Utc).AddTicks(2744), new DateTime(2025, 7, 6, 10, 37, 45, 668, DateTimeKind.Utc).AddTicks(2745) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 10, 37, 45, 668, DateTimeKind.Utc).AddTicks(2747), new DateTime(2025, 7, 6, 10, 37, 45, 668, DateTimeKind.Utc).AddTicks(2747) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 10, 37, 45, 668, DateTimeKind.Utc).AddTicks(2749), new DateTime(2025, 7, 6, 10, 37, 45, 668, DateTimeKind.Utc).AddTicks(2750) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 10, 37, 45, 668, DateTimeKind.Utc).AddTicks(2752), new DateTime(2025, 7, 6, 10, 37, 45, 668, DateTimeKind.Utc).AddTicks(2752) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 10, 37, 45, 668, DateTimeKind.Utc).AddTicks(2754), new DateTime(2025, 7, 6, 10, 37, 45, 668, DateTimeKind.Utc).AddTicks(2755) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 10, 37, 45, 668, DateTimeKind.Utc).AddTicks(2757), new DateTime(2025, 7, 6, 10, 37, 45, 668, DateTimeKind.Utc).AddTicks(2757) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 10, 37, 45, 668, DateTimeKind.Utc).AddTicks(2759), new DateTime(2025, 7, 6, 10, 37, 45, 668, DateTimeKind.Utc).AddTicks(2759) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 10, 37, 45, 668, DateTimeKind.Utc).AddTicks(2761), new DateTime(2025, 7, 6, 10, 37, 45, 668, DateTimeKind.Utc).AddTicks(2761) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 1,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 6, 10, 37, 45, 668, DateTimeKind.Utc).AddTicks(2635));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 2,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 6, 10, 37, 45, 668, DateTimeKind.Utc).AddTicks(2640));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 3,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 6, 10, 37, 45, 668, DateTimeKind.Utc).AddTicks(2641));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 4,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 6, 10, 37, 45, 668, DateTimeKind.Utc).AddTicks(2643));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 5,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 6, 10, 37, 45, 668, DateTimeKind.Utc).AddTicks(2644));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 6,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 6, 10, 37, 45, 668, DateTimeKind.Utc).AddTicks(2646));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 7,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 6, 10, 37, 45, 668, DateTimeKind.Utc).AddTicks(2648));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 8,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 6, 10, 37, 45, 668, DateTimeKind.Utc).AddTicks(2649));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 9,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 6, 10, 37, 45, 668, DateTimeKind.Utc).AddTicks(2651));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 10, 37, 45, 668, DateTimeKind.Utc).AddTicks(2157), new DateTime(2025, 7, 6, 10, 37, 45, 668, DateTimeKind.Utc).AddTicks(2164) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 10, 37, 45, 668, DateTimeKind.Utc).AddTicks(2177), new DateTime(2025, 7, 6, 10, 37, 45, 668, DateTimeKind.Utc).AddTicks(2177) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 10, 37, 45, 668, DateTimeKind.Utc).AddTicks(2180), new DateTime(2025, 7, 6, 10, 37, 45, 668, DateTimeKind.Utc).AddTicks(2180) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 10, 37, 45, 668, DateTimeKind.Utc).AddTicks(2183), new DateTime(2025, 7, 6, 10, 37, 45, 668, DateTimeKind.Utc).AddTicks(2183) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 10, 37, 45, 668, DateTimeKind.Utc).AddTicks(2250), new DateTime(2025, 7, 6, 10, 37, 45, 668, DateTimeKind.Utc).AddTicks(2251) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 10, 37, 45, 668, DateTimeKind.Utc).AddTicks(2253), new DateTime(2025, 7, 6, 10, 37, 45, 668, DateTimeKind.Utc).AddTicks(2254) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 10, 37, 45, 668, DateTimeKind.Utc).AddTicks(2256), new DateTime(2025, 7, 6, 10, 37, 45, 668, DateTimeKind.Utc).AddTicks(2256) });

            migrationBuilder.CreateIndex(
                name: "IX_DetonatorInfos_ProjectId",
                table: "DetonatorInfos",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_BlastConnections_Point1DrillPointId",
                table: "BlastConnections",
                column: "Point1DrillPointId");

            migrationBuilder.CreateIndex(
                name: "IX_BlastConnections_ProjectId",
                table: "BlastConnections",
                column: "ProjectId");
        }
    }
}
