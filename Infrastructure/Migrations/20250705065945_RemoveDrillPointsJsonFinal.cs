using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveDrillPointsJsonFinal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DrillPointsJson",
                table: "DrillPatterns");

            migrationBuilder.AddColumn<int>(
                name: "DrillPatternId",
                table: "DrillPoints",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 5, 6, 59, 45, 75, DateTimeKind.Utc).AddTicks(5862), new DateTime(2025, 7, 5, 6, 59, 45, 75, DateTimeKind.Utc).AddTicks(5862) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 5, 6, 59, 45, 75, DateTimeKind.Utc).AddTicks(5867), new DateTime(2025, 7, 5, 6, 59, 45, 75, DateTimeKind.Utc).AddTicks(5867) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 5, 6, 59, 45, 75, DateTimeKind.Utc).AddTicks(5870), new DateTime(2025, 7, 5, 6, 59, 45, 75, DateTimeKind.Utc).AddTicks(5870) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 5, 6, 59, 45, 75, DateTimeKind.Utc).AddTicks(5873), new DateTime(2025, 7, 5, 6, 59, 45, 75, DateTimeKind.Utc).AddTicks(5873) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 5, 6, 59, 45, 75, DateTimeKind.Utc).AddTicks(5875), new DateTime(2025, 7, 5, 6, 59, 45, 75, DateTimeKind.Utc).AddTicks(5875) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 5, 6, 59, 45, 75, DateTimeKind.Utc).AddTicks(5877), new DateTime(2025, 7, 5, 6, 59, 45, 75, DateTimeKind.Utc).AddTicks(5878) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 5, 6, 59, 45, 75, DateTimeKind.Utc).AddTicks(5880), new DateTime(2025, 7, 5, 6, 59, 45, 75, DateTimeKind.Utc).AddTicks(5880) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 5, 6, 59, 45, 75, DateTimeKind.Utc).AddTicks(5882), new DateTime(2025, 7, 5, 6, 59, 45, 75, DateTimeKind.Utc).AddTicks(5882) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 5, 6, 59, 45, 75, DateTimeKind.Utc).AddTicks(6050), new DateTime(2025, 7, 5, 6, 59, 45, 75, DateTimeKind.Utc).AddTicks(6051) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 5, 6, 59, 45, 75, DateTimeKind.Utc).AddTicks(6058), new DateTime(2025, 7, 5, 6, 59, 45, 75, DateTimeKind.Utc).AddTicks(6058) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 5, 6, 59, 45, 75, DateTimeKind.Utc).AddTicks(6061), new DateTime(2025, 7, 5, 6, 59, 45, 75, DateTimeKind.Utc).AddTicks(6061) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 5, 6, 59, 45, 75, DateTimeKind.Utc).AddTicks(6063), new DateTime(2025, 7, 5, 6, 59, 45, 75, DateTimeKind.Utc).AddTicks(6064) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 5, 6, 59, 45, 75, DateTimeKind.Utc).AddTicks(6066), new DateTime(2025, 7, 5, 6, 59, 45, 75, DateTimeKind.Utc).AddTicks(6066) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 5, 6, 59, 45, 75, DateTimeKind.Utc).AddTicks(6068), new DateTime(2025, 7, 5, 6, 59, 45, 75, DateTimeKind.Utc).AddTicks(6069) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 5, 6, 59, 45, 75, DateTimeKind.Utc).AddTicks(6071), new DateTime(2025, 7, 5, 6, 59, 45, 75, DateTimeKind.Utc).AddTicks(6071) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 5, 6, 59, 45, 75, DateTimeKind.Utc).AddTicks(6073), new DateTime(2025, 7, 5, 6, 59, 45, 75, DateTimeKind.Utc).AddTicks(6073) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 5, 6, 59, 45, 75, DateTimeKind.Utc).AddTicks(6075), new DateTime(2025, 7, 5, 6, 59, 45, 75, DateTimeKind.Utc).AddTicks(6075) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 5, 6, 59, 45, 75, DateTimeKind.Utc).AddTicks(6077), new DateTime(2025, 7, 5, 6, 59, 45, 75, DateTimeKind.Utc).AddTicks(6078) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 5, 6, 59, 45, 75, DateTimeKind.Utc).AddTicks(6080), new DateTime(2025, 7, 5, 6, 59, 45, 75, DateTimeKind.Utc).AddTicks(6080) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 1,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 5, 6, 59, 45, 75, DateTimeKind.Utc).AddTicks(5977));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 2,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 5, 6, 59, 45, 75, DateTimeKind.Utc).AddTicks(5982));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 3,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 5, 6, 59, 45, 75, DateTimeKind.Utc).AddTicks(5983));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 4,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 5, 6, 59, 45, 75, DateTimeKind.Utc).AddTicks(5985));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 5,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 5, 6, 59, 45, 75, DateTimeKind.Utc).AddTicks(5986));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 6,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 5, 6, 59, 45, 75, DateTimeKind.Utc).AddTicks(5987));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 7,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 5, 6, 59, 45, 75, DateTimeKind.Utc).AddTicks(5989));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 8,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 5, 6, 59, 45, 75, DateTimeKind.Utc).AddTicks(5990));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 9,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 5, 6, 59, 45, 75, DateTimeKind.Utc).AddTicks(5991));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 5, 6, 59, 45, 75, DateTimeKind.Utc).AddTicks(5610), new DateTime(2025, 7, 5, 6, 59, 45, 75, DateTimeKind.Utc).AddTicks(5613) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 5, 6, 59, 45, 75, DateTimeKind.Utc).AddTicks(5619), new DateTime(2025, 7, 5, 6, 59, 45, 75, DateTimeKind.Utc).AddTicks(5620) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 5, 6, 59, 45, 75, DateTimeKind.Utc).AddTicks(5623), new DateTime(2025, 7, 5, 6, 59, 45, 75, DateTimeKind.Utc).AddTicks(5623) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 5, 6, 59, 45, 75, DateTimeKind.Utc).AddTicks(5625), new DateTime(2025, 7, 5, 6, 59, 45, 75, DateTimeKind.Utc).AddTicks(5626) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 5, 6, 59, 45, 75, DateTimeKind.Utc).AddTicks(5628), new DateTime(2025, 7, 5, 6, 59, 45, 75, DateTimeKind.Utc).AddTicks(5628) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 5, 6, 59, 45, 75, DateTimeKind.Utc).AddTicks(5630), new DateTime(2025, 7, 5, 6, 59, 45, 75, DateTimeKind.Utc).AddTicks(5630) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 5, 6, 59, 45, 75, DateTimeKind.Utc).AddTicks(5632), new DateTime(2025, 7, 5, 6, 59, 45, 75, DateTimeKind.Utc).AddTicks(5632) });

            migrationBuilder.CreateIndex(
                name: "IX_DrillPoints_DrillPatternId",
                table: "DrillPoints",
                column: "DrillPatternId");

            migrationBuilder.AddForeignKey(
                name: "FK_DrillPoints_DrillPatterns_DrillPatternId",
                table: "DrillPoints",
                column: "DrillPatternId",
                principalTable: "DrillPatterns",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DrillPoints_DrillPatterns_DrillPatternId",
                table: "DrillPoints");

            migrationBuilder.DropIndex(
                name: "IX_DrillPoints_DrillPatternId",
                table: "DrillPoints");

            migrationBuilder.DropColumn(
                name: "DrillPatternId",
                table: "DrillPoints");

            migrationBuilder.AddColumn<string>(
                name: "DrillPointsJson",
                table: "DrillPatterns",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6523), new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6525) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6538), new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6539) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6547), new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6548) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6554), new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6556) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6561), new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6562) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6567), new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6568) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6573), new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6574) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6579), new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6594) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6874), new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6876) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6891), new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6892) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6899), new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6900) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6906), new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6906) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6912), new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6913) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6918), new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6919) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6923), new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6924) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6929), new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6930) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6935), new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6935) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6940), new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6941) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6945), new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6946) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 1,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6711));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 2,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6719));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 3,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6723));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 4,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6727));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 5,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6730));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 6,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6733));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 7,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6737));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 8,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6740));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 9,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6743));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(5888), new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(5895) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(5911), new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(5911) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(5919), new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(5919) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(5926), new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(5927) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(5932), new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(5933) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(5939), new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(5940) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6016), new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6018) });
        }
    }
}
