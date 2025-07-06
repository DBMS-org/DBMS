using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveIsHiddenFromBlastConnection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlastConnections_ProjectSites_SiteId",
                table: "BlastConnections");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BlastConnections",
                table: "BlastConnections");

            migrationBuilder.DropIndex(
                name: "IX_BlastConnections_Point1DrillPointId_Point2DrillPointId",
                table: "BlastConnections");

            migrationBuilder.DropColumn(
                name: "IsHidden",
                table: "BlastConnections");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BlastConnections",
                table: "BlastConnections",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(986), new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(987) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(1000), new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(1001) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(1007), new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(1008) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(1013), new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(1014) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(1020), new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(1021) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(1026), new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(1027) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(1032), new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(1033) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(1038), new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(1038) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(1296), new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(1297) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(1312), new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(1313) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(1319), new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(1320) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(1325), new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(1325) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(1330), new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(1331) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(1336), new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(1337) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(1342), new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(1343) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(1347), new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(1348) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(1353), new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(1353) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(1358), new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(1359) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(1364), new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(1364) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 1,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(1137));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 2,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(1144));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 3,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(1147));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 4,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(1150));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 5,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(1153));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 6,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(1156));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 7,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(1159));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 8,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(1163));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 9,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(1166));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(356), new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(364) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(378), new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(379) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(385), new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(386) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(392), new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(393) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(398), new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(399) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(404), new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(405) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(410), new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(411) });

            migrationBuilder.CreateIndex(
                name: "IX_BlastConnections_Point1DrillPointId",
                table: "BlastConnections",
                column: "Point1DrillPointId");

            migrationBuilder.AddForeignKey(
                name: "FK_BlastConnections_ProjectSites_SiteId",
                table: "BlastConnections",
                column: "SiteId",
                principalTable: "ProjectSites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlastConnections_ProjectSites_SiteId",
                table: "BlastConnections");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BlastConnections",
                table: "BlastConnections");

            migrationBuilder.DropIndex(
                name: "IX_BlastConnections_Point1DrillPointId",
                table: "BlastConnections");

            migrationBuilder.AddColumn<bool>(
                name: "IsHidden",
                table: "BlastConnections",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_BlastConnections",
                table: "BlastConnections",
                columns: new[] { "Id", "ProjectId", "SiteId" });

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
                name: "IX_BlastConnections_Point1DrillPointId_Point2DrillPointId",
                table: "BlastConnections",
                columns: new[] { "Point1DrillPointId", "Point2DrillPointId" });

            migrationBuilder.AddForeignKey(
                name: "FK_BlastConnections_ProjectSites_SiteId",
                table: "BlastConnections",
                column: "SiteId",
                principalTable: "ProjectSites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
