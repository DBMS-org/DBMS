using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddExplosiveApprovalToProjectSite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(6766), new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(6767) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(6776), new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(6776) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(6782), new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(6783) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(6787), new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(6787) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(6791), new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(6792) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(6796), new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(6796) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(6800), new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(6800) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(6806), new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(6823) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(7030), new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(7031) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(7044), new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(7045) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(7050), new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(7050) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(7054), new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(7054) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(7058), new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(7058) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(7062), new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(7063) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(7066), new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(7067) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(7273), new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(7274) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(7279), new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(7280) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(7283), new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(7284) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(7287), new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(7288) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 1,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(6909));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 2,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(6916));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 3,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(6919));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 4,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(6922));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 5,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(6925));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 6,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(6927));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 7,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(6929));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 8,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(6932));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 9,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(6935));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(6319), new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(6325) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(6342), new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(6343) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(6349), new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(6350) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(6354), new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(6354) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(6359), new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(6360) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(6363), new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(6364) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(6368), new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(6369) });
        }
    }
}
