using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAllowedExplosiveTypesToStore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AllowedExplosiveTypes",
                table: "Stores",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 5, 7, 49, 229, DateTimeKind.Utc).AddTicks(7721), new DateTime(2025, 10, 3, 5, 7, 49, 229, DateTimeKind.Utc).AddTicks(7721) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 5, 7, 49, 229, DateTimeKind.Utc).AddTicks(7726), new DateTime(2025, 10, 3, 5, 7, 49, 229, DateTimeKind.Utc).AddTicks(7727) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 5, 7, 49, 229, DateTimeKind.Utc).AddTicks(7729), new DateTime(2025, 10, 3, 5, 7, 49, 229, DateTimeKind.Utc).AddTicks(7730) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 5, 7, 49, 229, DateTimeKind.Utc).AddTicks(7732), new DateTime(2025, 10, 3, 5, 7, 49, 229, DateTimeKind.Utc).AddTicks(7732) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 5, 7, 49, 229, DateTimeKind.Utc).AddTicks(7734), new DateTime(2025, 10, 3, 5, 7, 49, 229, DateTimeKind.Utc).AddTicks(7735) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 5, 7, 49, 229, DateTimeKind.Utc).AddTicks(7736), new DateTime(2025, 10, 3, 5, 7, 49, 229, DateTimeKind.Utc).AddTicks(7737) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 5, 7, 49, 229, DateTimeKind.Utc).AddTicks(7739), new DateTime(2025, 10, 3, 5, 7, 49, 229, DateTimeKind.Utc).AddTicks(7739) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 5, 7, 49, 229, DateTimeKind.Utc).AddTicks(7741), new DateTime(2025, 10, 3, 5, 7, 49, 230, DateTimeKind.Utc).AddTicks(174) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 5, 7, 49, 230, DateTimeKind.Utc).AddTicks(477), new DateTime(2025, 10, 3, 5, 7, 49, 230, DateTimeKind.Utc).AddTicks(477) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 5, 7, 49, 230, DateTimeKind.Utc).AddTicks(486), new DateTime(2025, 10, 3, 5, 7, 49, 230, DateTimeKind.Utc).AddTicks(486) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 5, 7, 49, 230, DateTimeKind.Utc).AddTicks(489), new DateTime(2025, 10, 3, 5, 7, 49, 230, DateTimeKind.Utc).AddTicks(489) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 5, 7, 49, 230, DateTimeKind.Utc).AddTicks(492), new DateTime(2025, 10, 3, 5, 7, 49, 230, DateTimeKind.Utc).AddTicks(492) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 5, 7, 49, 230, DateTimeKind.Utc).AddTicks(494), new DateTime(2025, 10, 3, 5, 7, 49, 230, DateTimeKind.Utc).AddTicks(495) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 5, 7, 49, 230, DateTimeKind.Utc).AddTicks(524), new DateTime(2025, 10, 3, 5, 7, 49, 230, DateTimeKind.Utc).AddTicks(524) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 5, 7, 49, 230, DateTimeKind.Utc).AddTicks(527), new DateTime(2025, 10, 3, 5, 7, 49, 230, DateTimeKind.Utc).AddTicks(527) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 5, 7, 49, 230, DateTimeKind.Utc).AddTicks(530), new DateTime(2025, 10, 3, 5, 7, 49, 230, DateTimeKind.Utc).AddTicks(530) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 5, 7, 49, 230, DateTimeKind.Utc).AddTicks(532), new DateTime(2025, 10, 3, 5, 7, 49, 230, DateTimeKind.Utc).AddTicks(533) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 5, 7, 49, 230, DateTimeKind.Utc).AddTicks(535), new DateTime(2025, 10, 3, 5, 7, 49, 230, DateTimeKind.Utc).AddTicks(535) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 5, 7, 49, 230, DateTimeKind.Utc).AddTicks(538), new DateTime(2025, 10, 3, 5, 7, 49, 230, DateTimeKind.Utc).AddTicks(538) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 1,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 3, 5, 7, 49, 230, DateTimeKind.Utc).AddTicks(399));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 2,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 3, 5, 7, 49, 230, DateTimeKind.Utc).AddTicks(403));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 3,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 3, 5, 7, 49, 230, DateTimeKind.Utc).AddTicks(405));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 4,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 3, 5, 7, 49, 230, DateTimeKind.Utc).AddTicks(406));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 5,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 3, 5, 7, 49, 230, DateTimeKind.Utc).AddTicks(408));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 6,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 3, 5, 7, 49, 230, DateTimeKind.Utc).AddTicks(409));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 7,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 3, 5, 7, 49, 230, DateTimeKind.Utc).AddTicks(411));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 8,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 3, 5, 7, 49, 230, DateTimeKind.Utc).AddTicks(412));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 9,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 3, 5, 7, 49, 230, DateTimeKind.Utc).AddTicks(414));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 5, 7, 49, 229, DateTimeKind.Utc).AddTicks(7343), new DateTime(2025, 10, 3, 5, 7, 49, 229, DateTimeKind.Utc).AddTicks(7347) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 5, 7, 49, 229, DateTimeKind.Utc).AddTicks(7354), new DateTime(2025, 10, 3, 5, 7, 49, 229, DateTimeKind.Utc).AddTicks(7367) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 5, 7, 49, 229, DateTimeKind.Utc).AddTicks(7369), new DateTime(2025, 10, 3, 5, 7, 49, 229, DateTimeKind.Utc).AddTicks(7370) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 5, 7, 49, 229, DateTimeKind.Utc).AddTicks(7372), new DateTime(2025, 10, 3, 5, 7, 49, 229, DateTimeKind.Utc).AddTicks(7372) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 5, 7, 49, 229, DateTimeKind.Utc).AddTicks(7436), new DateTime(2025, 10, 3, 5, 7, 49, 229, DateTimeKind.Utc).AddTicks(7437) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 5, 7, 49, 229, DateTimeKind.Utc).AddTicks(7439), new DateTime(2025, 10, 3, 5, 7, 49, 229, DateTimeKind.Utc).AddTicks(7439) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 5, 7, 49, 229, DateTimeKind.Utc).AddTicks(7441), new DateTime(2025, 10, 3, 5, 7, 49, 229, DateTimeKind.Utc).AddTicks(7442) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AllowedExplosiveTypes",
                table: "Stores");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(6723), new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(6724) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(6729), new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(6730) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(6733), new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(6733) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(6735), new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(6736) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(6738), new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(6738) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(6741), new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(6741) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(6743), new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(6744) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(6746), new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(7661) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(7805), new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(7805) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(7813), new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(7814) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(7816), new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(7817) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(7819), new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(7819) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(7822), new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(7822) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(7824), new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(7824) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(7826), new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(7827) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(7829), new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(7829) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(7831), new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(7832) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(7834), new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(7834) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(7836), new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(7836) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 1,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(7728));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 2,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(7732));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 3,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(7734));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 4,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(7736));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 5,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(7737));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 6,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(7739));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 7,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(7740));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 8,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(7742));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 9,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(7743));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(6390), new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(6393) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(6401), new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(6402) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(6405), new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(6405) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(6457), new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(6458) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(6460), new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(6460) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(6462), new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(6463) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(6465), new DateTime(2025, 10, 3, 3, 14, 57, 247, DateTimeKind.Utc).AddTicks(6465) });
        }
    }
}
