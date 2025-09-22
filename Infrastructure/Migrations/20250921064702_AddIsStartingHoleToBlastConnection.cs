using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIsStartingHoleToBlastConnection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsStartingHole",
                table: "BlastConnections",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 21, 6, 47, 1, 861, DateTimeKind.Utc).AddTicks(9583), new DateTime(2025, 9, 21, 6, 47, 1, 861, DateTimeKind.Utc).AddTicks(9583) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 21, 6, 47, 1, 861, DateTimeKind.Utc).AddTicks(9589), new DateTime(2025, 9, 21, 6, 47, 1, 861, DateTimeKind.Utc).AddTicks(9590) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 21, 6, 47, 1, 861, DateTimeKind.Utc).AddTicks(9593), new DateTime(2025, 9, 21, 6, 47, 1, 861, DateTimeKind.Utc).AddTicks(9594) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 21, 6, 47, 1, 861, DateTimeKind.Utc).AddTicks(9597), new DateTime(2025, 9, 21, 6, 47, 1, 861, DateTimeKind.Utc).AddTicks(9597) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 21, 6, 47, 1, 861, DateTimeKind.Utc).AddTicks(9600), new DateTime(2025, 9, 21, 6, 47, 1, 861, DateTimeKind.Utc).AddTicks(9600) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 21, 6, 47, 1, 861, DateTimeKind.Utc).AddTicks(9603), new DateTime(2025, 9, 21, 6, 47, 1, 861, DateTimeKind.Utc).AddTicks(9603) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 21, 6, 47, 1, 861, DateTimeKind.Utc).AddTicks(9606), new DateTime(2025, 9, 21, 6, 47, 1, 861, DateTimeKind.Utc).AddTicks(9606) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 21, 6, 47, 1, 861, DateTimeKind.Utc).AddTicks(9612), new DateTime(2025, 9, 21, 6, 47, 1, 861, DateTimeKind.Utc).AddTicks(9618) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 21, 6, 47, 1, 861, DateTimeKind.Utc).AddTicks(9938), new DateTime(2025, 9, 21, 6, 47, 1, 861, DateTimeKind.Utc).AddTicks(9939) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 21, 6, 47, 1, 861, DateTimeKind.Utc).AddTicks(9947), new DateTime(2025, 9, 21, 6, 47, 1, 861, DateTimeKind.Utc).AddTicks(9948) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 21, 6, 47, 1, 861, DateTimeKind.Utc).AddTicks(9951), new DateTime(2025, 9, 21, 6, 47, 1, 861, DateTimeKind.Utc).AddTicks(9952) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 21, 6, 47, 1, 861, DateTimeKind.Utc).AddTicks(9954), new DateTime(2025, 9, 21, 6, 47, 1, 861, DateTimeKind.Utc).AddTicks(9955) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 21, 6, 47, 1, 861, DateTimeKind.Utc).AddTicks(9957), new DateTime(2025, 9, 21, 6, 47, 1, 861, DateTimeKind.Utc).AddTicks(9958) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 21, 6, 47, 1, 861, DateTimeKind.Utc).AddTicks(9961), new DateTime(2025, 9, 21, 6, 47, 1, 861, DateTimeKind.Utc).AddTicks(9961) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 21, 6, 47, 1, 861, DateTimeKind.Utc).AddTicks(9964), new DateTime(2025, 9, 21, 6, 47, 1, 861, DateTimeKind.Utc).AddTicks(9964) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 21, 6, 47, 1, 861, DateTimeKind.Utc).AddTicks(9967), new DateTime(2025, 9, 21, 6, 47, 1, 861, DateTimeKind.Utc).AddTicks(9967) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 21, 6, 47, 1, 861, DateTimeKind.Utc).AddTicks(9970), new DateTime(2025, 9, 21, 6, 47, 1, 861, DateTimeKind.Utc).AddTicks(9970) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 21, 6, 47, 1, 861, DateTimeKind.Utc).AddTicks(9973), new DateTime(2025, 9, 21, 6, 47, 1, 861, DateTimeKind.Utc).AddTicks(9974) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 21, 6, 47, 1, 861, DateTimeKind.Utc).AddTicks(9977), new DateTime(2025, 9, 21, 6, 47, 1, 861, DateTimeKind.Utc).AddTicks(9977) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 1,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 21, 6, 47, 1, 861, DateTimeKind.Utc).AddTicks(9735));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 2,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 21, 6, 47, 1, 861, DateTimeKind.Utc).AddTicks(9739));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 3,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 21, 6, 47, 1, 861, DateTimeKind.Utc).AddTicks(9741));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 4,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 21, 6, 47, 1, 861, DateTimeKind.Utc).AddTicks(9744));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 5,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 21, 6, 47, 1, 861, DateTimeKind.Utc).AddTicks(9746));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 6,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 21, 6, 47, 1, 861, DateTimeKind.Utc).AddTicks(9748));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 7,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 21, 6, 47, 1, 861, DateTimeKind.Utc).AddTicks(9750));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 8,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 21, 6, 47, 1, 861, DateTimeKind.Utc).AddTicks(9752));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 9,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 21, 6, 47, 1, 861, DateTimeKind.Utc).AddTicks(9754));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 21, 6, 47, 1, 861, DateTimeKind.Utc).AddTicks(9108), new DateTime(2025, 9, 21, 6, 47, 1, 861, DateTimeKind.Utc).AddTicks(9113) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 21, 6, 47, 1, 861, DateTimeKind.Utc).AddTicks(9123), new DateTime(2025, 9, 21, 6, 47, 1, 861, DateTimeKind.Utc).AddTicks(9123) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 21, 6, 47, 1, 861, DateTimeKind.Utc).AddTicks(9127), new DateTime(2025, 9, 21, 6, 47, 1, 861, DateTimeKind.Utc).AddTicks(9127) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 21, 6, 47, 1, 861, DateTimeKind.Utc).AddTicks(9130), new DateTime(2025, 9, 21, 6, 47, 1, 861, DateTimeKind.Utc).AddTicks(9130) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 21, 6, 47, 1, 861, DateTimeKind.Utc).AddTicks(9133), new DateTime(2025, 9, 21, 6, 47, 1, 861, DateTimeKind.Utc).AddTicks(9133) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 21, 6, 47, 1, 861, DateTimeKind.Utc).AddTicks(9136), new DateTime(2025, 9, 21, 6, 47, 1, 861, DateTimeKind.Utc).AddTicks(9136) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 21, 6, 47, 1, 861, DateTimeKind.Utc).AddTicks(9139), new DateTime(2025, 9, 21, 6, 47, 1, 861, DateTimeKind.Utc).AddTicks(9139) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsStartingHole",
                table: "BlastConnections");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(9078), new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(9078) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(9084), new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(9085) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(9087), new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(9088) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(9090), new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(9090) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(9092), new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(9092) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(9094), new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(9095) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(9096), new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(9097) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(9099), new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(9100) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(9207), new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(9208) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(9215), new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(9215) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(9218), new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(9218) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(9220), new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(9222) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(9224), new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(9224) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(9226), new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(9226) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(9228), new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(9229) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(9231), new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(9231) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(9233), new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(9233) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(9235), new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(9235) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(9237), new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(9238) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 1,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(9142));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 2,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(9145));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 3,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(9147));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 4,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(9148));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 5,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(9150));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 6,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(9151));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 7,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(9153));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 8,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(9154));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 9,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(9156));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(8826), new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(8829) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(8836), new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(8836) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(8839), new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(8839) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(8841), new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(8842) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(8844), new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(8844) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(8846), new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(8847) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(8848), new DateTime(2025, 9, 19, 4, 55, 42, 84, DateTimeKind.Utc).AddTicks(8849) });
        }
    }
}
