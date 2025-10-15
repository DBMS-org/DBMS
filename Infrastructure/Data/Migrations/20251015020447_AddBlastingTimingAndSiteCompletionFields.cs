using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddBlastingTimingAndSiteCompletionFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CompletedAt",
                table: "ProjectSites",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CompletedByUserId",
                table: "ProjectSites",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "ProjectSites",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "BlastTiming",
                table: "ExplosiveApprovalRequests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "BlastingDate",
                table: "ExplosiveApprovalRequests",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(4011), new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(4012) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(4018), new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(4018) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(4021), new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(4021) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(4023), new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(4024) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(4025), new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(4026) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(4028), new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(4028) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(4030), new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(4030) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(4032), new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(4039) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(4186), new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(4187) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(4194), new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(4194) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(4197), new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(4197) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(4199), new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(4199) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(4201), new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(4202) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(4204), new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(4204) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(4206), new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(4207) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(4209), new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(4209) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(4211), new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(4211) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(4213), new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(4213) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(4217), new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(4217) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 1,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(4093));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 2,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(4097));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 3,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(4099));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 4,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(4100));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 5,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(4101));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 6,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(4103));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 7,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(4104));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 8,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(4105));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 9,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(4107));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(3733), new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(3736) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(3742), new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(3743) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(3745), new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(3746) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(3748), new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(3749) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(3751), new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(3751) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(3753), new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(3754) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(3755), new DateTime(2025, 10, 15, 2, 4, 45, 996, DateTimeKind.Utc).AddTicks(3756) });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectSites_CompletedByUserId",
                table: "ProjectSites",
                column: "CompletedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectSites_Users_CompletedByUserId",
                table: "ProjectSites",
                column: "CompletedByUserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectSites_Users_CompletedByUserId",
                table: "ProjectSites");

            migrationBuilder.DropIndex(
                name: "IX_ProjectSites_CompletedByUserId",
                table: "ProjectSites");

            migrationBuilder.DropColumn(
                name: "CompletedAt",
                table: "ProjectSites");

            migrationBuilder.DropColumn(
                name: "CompletedByUserId",
                table: "ProjectSites");

            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "ProjectSites");

            migrationBuilder.DropColumn(
                name: "BlastTiming",
                table: "ExplosiveApprovalRequests");

            migrationBuilder.DropColumn(
                name: "BlastingDate",
                table: "ExplosiveApprovalRequests");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(7161), new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(7162) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(7169), new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(7169) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(7174), new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(7175) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(7177), new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(7178) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(7181), new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(7181) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(7184), new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(7185) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(7187), new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(7188) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(7190), new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(7206) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(7421), new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(7422) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(7431), new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(7432) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(7435), new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(7436) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(7439), new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(7439) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(7442), new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(7443) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(7446), new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(7446) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(7451), new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(7451) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(7454), new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(7455) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(7457), new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(7458) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(7461), new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(7461) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(7464), new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(7465) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 1,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(7287));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 2,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(7292));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 3,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(7294));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 4,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(7296));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 5,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(7298));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 6,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(7300));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 7,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(7302));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 8,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(7304));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 9,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(7306));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(6755), new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(6759) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(6768), new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(6769) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(6773), new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(6773) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(6777), new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(6777) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(6780), new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(6780) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(6783), new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(6784) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(6786), new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(6787) });
        }
    }
}
