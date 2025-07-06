using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveConnectionsJsonFromBlastSequence : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HoleId",
                table: "DetonatorInfos");

            migrationBuilder.DropColumn(
                name: "ConnectionsJson",
                table: "BlastSequences");

            migrationBuilder.DropColumn(
                name: "EndPointJson",
                table: "BlastConnections");

            migrationBuilder.DropColumn(
                name: "FromHoleId",
                table: "BlastConnections");

            migrationBuilder.DropColumn(
                name: "StartPointJson",
                table: "BlastConnections");

            migrationBuilder.DropColumn(
                name: "ToHoleId",
                table: "BlastConnections");

            migrationBuilder.AddColumn<string>(
                name: "DrillPointId",
                table: "DetonatorInfos",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsHidden",
                table: "BlastConnections",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Point1DrillPointId",
                table: "BlastConnections",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Point2DrillPointId",
                table: "BlastConnections",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

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
                name: "IX_DetonatorInfos_DrillPointId",
                table: "DetonatorInfos",
                column: "DrillPointId");

            migrationBuilder.CreateIndex(
                name: "IX_BlastConnections_Point1DrillPointId",
                table: "BlastConnections",
                column: "Point1DrillPointId");

            migrationBuilder.CreateIndex(
                name: "IX_BlastConnections_Point2DrillPointId",
                table: "BlastConnections",
                column: "Point2DrillPointId");

            migrationBuilder.AddForeignKey(
                name: "FK_BlastConnections_DrillPoints_Point1DrillPointId",
                table: "BlastConnections",
                column: "Point1DrillPointId",
                principalTable: "DrillPoints",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BlastConnections_DrillPoints_Point2DrillPointId",
                table: "BlastConnections",
                column: "Point2DrillPointId",
                principalTable: "DrillPoints",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DetonatorInfos_DrillPoints_DrillPointId",
                table: "DetonatorInfos",
                column: "DrillPointId",
                principalTable: "DrillPoints",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlastConnections_DrillPoints_Point1DrillPointId",
                table: "BlastConnections");

            migrationBuilder.DropForeignKey(
                name: "FK_BlastConnections_DrillPoints_Point2DrillPointId",
                table: "BlastConnections");

            migrationBuilder.DropForeignKey(
                name: "FK_DetonatorInfos_DrillPoints_DrillPointId",
                table: "DetonatorInfos");

            migrationBuilder.DropIndex(
                name: "IX_DetonatorInfos_DrillPointId",
                table: "DetonatorInfos");

            migrationBuilder.DropIndex(
                name: "IX_BlastConnections_Point1DrillPointId",
                table: "BlastConnections");

            migrationBuilder.DropIndex(
                name: "IX_BlastConnections_Point2DrillPointId",
                table: "BlastConnections");

            migrationBuilder.DropColumn(
                name: "DrillPointId",
                table: "DetonatorInfos");

            migrationBuilder.DropColumn(
                name: "IsHidden",
                table: "BlastConnections");

            migrationBuilder.DropColumn(
                name: "Point1DrillPointId",
                table: "BlastConnections");

            migrationBuilder.DropColumn(
                name: "Point2DrillPointId",
                table: "BlastConnections");

            migrationBuilder.AddColumn<string>(
                name: "HoleId",
                table: "DetonatorInfos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ConnectionsJson",
                table: "BlastSequences",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EndPointJson",
                table: "BlastConnections",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FromHoleId",
                table: "BlastConnections",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StartPointJson",
                table: "BlastConnections",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ToHoleId",
                table: "BlastConnections",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6575), new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6576) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6581), new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6582) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6585), new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6585) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6587), new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6588) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6590), new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6590) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6592), new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6593) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6595), new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6595) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6597), new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6597) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6856), new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6857) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6879), new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6879) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6882), new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6883) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6885), new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6885) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6887), new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6888) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6890), new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6891) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6892), new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6893) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6895), new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6895) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6897), new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6897) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6899), new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6900) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6902), new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6902) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 1,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6684));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 2,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6698));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 3,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6700));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 4,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6702));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 5,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6703));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 6,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6705));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 7,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6706));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 8,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6708));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 9,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6709));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6153), new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6157) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6166), new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6167) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6169), new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6170) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6172), new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6173) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6175), new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6175) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6177), new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6178) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6180), new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6180) });
        }
    }
}
