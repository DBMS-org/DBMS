using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveDuplicateManagerFieldsFromStore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stores_Projects_ProjectId",
                table: "Stores");

            migrationBuilder.DropForeignKey(
                name: "FK_Stores_Regions_RegionId",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "StoreManagerContact",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "StoreManagerEmail",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "StoreManagerName",
                table: "Stores");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(558), new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(558) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(564), new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(564) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(567), new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(567) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(570), new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(570) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(572), new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(573) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(575), new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(575) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(577), new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(578) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(580), new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(580) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(692), new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(693) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(700), new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(701) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(703), new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(704) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(706), new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(706) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(709), new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(709) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(712), new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(712) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(714), new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(715) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(717), new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(717) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(719), new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(719) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(721), new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(722) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(724), new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(724) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 1,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(626));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 2,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(631));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 3,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(632));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 4,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(634));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 5,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(636));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 6,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(637));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 7,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(638));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 8,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(639));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 9,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(641));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(283), new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(287) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(294), new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(294) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(297), new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(297) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(300), new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(300) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(302), new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(302) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(305), new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(305) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(307), new DateTime(2025, 10, 2, 9, 55, 34, 532, DateTimeKind.Utc).AddTicks(307) });

            migrationBuilder.AddForeignKey(
                name: "FK_Stores_Projects_ProjectId",
                table: "Stores",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Stores_Regions_RegionId",
                table: "Stores",
                column: "RegionId",
                principalTable: "Regions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stores_Projects_ProjectId",
                table: "Stores");

            migrationBuilder.DropForeignKey(
                name: "FK_Stores_Regions_RegionId",
                table: "Stores");

            migrationBuilder.AddColumn<string>(
                name: "StoreManagerContact",
                table: "Stores",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StoreManagerEmail",
                table: "Stores",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StoreManagerName",
                table: "Stores",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(5893), new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(5894) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(5898), new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(5899) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(5902), new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(5902) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(5904), new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(5905) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(5907), new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(5907) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(5910), new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(5910) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(5912), new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(5913) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(5915), new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(5915) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(6024), new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(6024) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(6032), new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(6032) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(6035), new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(6035) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(6038), new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(6038) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(6040), new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(6040) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(6042), new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(6043) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(6045), new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(6045) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(6047), new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(6048) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(6050), new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(6050) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(6052), new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(6053) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(6055), new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(6055) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 1,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(5962));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 2,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(5966));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 3,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(5968));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 4,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(5969));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 5,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(5971));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 6,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(5972));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 7,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(5973));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 8,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(5975));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 9,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(5976));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(5671), new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(5675) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(5681), new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(5682) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(5684), new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(5685) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(5687), new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(5687) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(5689), new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(5690) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(5692), new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(5692) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(5694), new DateTime(2025, 10, 1, 6, 20, 12, 968, DateTimeKind.Utc).AddTicks(5695) });

            migrationBuilder.AddForeignKey(
                name: "FK_Stores_Projects_ProjectId",
                table: "Stores",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Stores_Regions_RegionId",
                table: "Stores",
                column: "RegionId",
                principalTable: "Regions",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
