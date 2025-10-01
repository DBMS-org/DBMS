using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSchema_20251001095215 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5611), new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5611) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5616), new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5616) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5619), new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5619) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5622), new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5622) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5624), new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5625) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5627), new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5627) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5629), new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5629) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5709), new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5711) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5823), new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5823) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5833), new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5833) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5836), new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5836) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5838), new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5839) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5841), new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5841) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5843), new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5843) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5845), new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5846) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5848), new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5848) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5850), new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5850) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5852), new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5853) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5854), new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5855) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 1,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5758));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 2,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5762));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 3,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5764));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 4,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5765));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 5,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5766));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 6,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5768));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 7,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5769));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 8,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5770));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 9,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5771));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5326), new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5328) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5334), new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5335) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5337), new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5338) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5340), new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5340) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5343), new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5343) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5345), new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5345) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5348), new DateTime(2025, 10, 1, 4, 52, 22, 191, DateTimeKind.Utc).AddTicks(5348) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 25, 4, 52, 13, 766, DateTimeKind.Utc).AddTicks(4321), new DateTime(2025, 9, 25, 4, 52, 13, 766, DateTimeKind.Utc).AddTicks(4322) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 25, 4, 52, 13, 766, DateTimeKind.Utc).AddTicks(4328), new DateTime(2025, 9, 25, 4, 52, 13, 766, DateTimeKind.Utc).AddTicks(4329) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 25, 4, 52, 13, 766, DateTimeKind.Utc).AddTicks(4332), new DateTime(2025, 9, 25, 4, 52, 13, 766, DateTimeKind.Utc).AddTicks(4332) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 25, 4, 52, 13, 766, DateTimeKind.Utc).AddTicks(4334), new DateTime(2025, 9, 25, 4, 52, 13, 766, DateTimeKind.Utc).AddTicks(4335) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 25, 4, 52, 13, 766, DateTimeKind.Utc).AddTicks(4336), new DateTime(2025, 9, 25, 4, 52, 13, 766, DateTimeKind.Utc).AddTicks(4337) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 25, 4, 52, 13, 766, DateTimeKind.Utc).AddTicks(4339), new DateTime(2025, 9, 25, 4, 52, 13, 766, DateTimeKind.Utc).AddTicks(4339) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 25, 4, 52, 13, 766, DateTimeKind.Utc).AddTicks(4341), new DateTime(2025, 9, 25, 4, 52, 13, 766, DateTimeKind.Utc).AddTicks(4341) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 25, 4, 52, 13, 766, DateTimeKind.Utc).AddTicks(4343), new DateTime(2025, 9, 25, 4, 52, 13, 766, DateTimeKind.Utc).AddTicks(4350) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 25, 4, 52, 13, 766, DateTimeKind.Utc).AddTicks(4486), new DateTime(2025, 9, 25, 4, 52, 13, 766, DateTimeKind.Utc).AddTicks(4486) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 25, 4, 52, 13, 766, DateTimeKind.Utc).AddTicks(4495), new DateTime(2025, 9, 25, 4, 52, 13, 766, DateTimeKind.Utc).AddTicks(4496) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 25, 4, 52, 13, 766, DateTimeKind.Utc).AddTicks(4498), new DateTime(2025, 9, 25, 4, 52, 13, 766, DateTimeKind.Utc).AddTicks(4498) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 25, 4, 52, 13, 766, DateTimeKind.Utc).AddTicks(4500), new DateTime(2025, 9, 25, 4, 52, 13, 766, DateTimeKind.Utc).AddTicks(4501) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 25, 4, 52, 13, 766, DateTimeKind.Utc).AddTicks(4504), new DateTime(2025, 9, 25, 4, 52, 13, 766, DateTimeKind.Utc).AddTicks(4504) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 25, 4, 52, 13, 766, DateTimeKind.Utc).AddTicks(4506), new DateTime(2025, 9, 25, 4, 52, 13, 766, DateTimeKind.Utc).AddTicks(4507) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 25, 4, 52, 13, 766, DateTimeKind.Utc).AddTicks(4509), new DateTime(2025, 9, 25, 4, 52, 13, 766, DateTimeKind.Utc).AddTicks(4509) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 25, 4, 52, 13, 766, DateTimeKind.Utc).AddTicks(4511), new DateTime(2025, 9, 25, 4, 52, 13, 766, DateTimeKind.Utc).AddTicks(4511) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 25, 4, 52, 13, 766, DateTimeKind.Utc).AddTicks(4513), new DateTime(2025, 9, 25, 4, 52, 13, 766, DateTimeKind.Utc).AddTicks(4514) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 25, 4, 52, 13, 766, DateTimeKind.Utc).AddTicks(4515), new DateTime(2025, 9, 25, 4, 52, 13, 766, DateTimeKind.Utc).AddTicks(4516) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 25, 4, 52, 13, 766, DateTimeKind.Utc).AddTicks(4518), new DateTime(2025, 9, 25, 4, 52, 13, 766, DateTimeKind.Utc).AddTicks(4518) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 1,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 25, 4, 52, 13, 766, DateTimeKind.Utc).AddTicks(4410));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 2,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 25, 4, 52, 13, 766, DateTimeKind.Utc).AddTicks(4415));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 3,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 25, 4, 52, 13, 766, DateTimeKind.Utc).AddTicks(4417));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 4,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 25, 4, 52, 13, 766, DateTimeKind.Utc).AddTicks(4418));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 5,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 25, 4, 52, 13, 766, DateTimeKind.Utc).AddTicks(4419));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 6,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 25, 4, 52, 13, 766, DateTimeKind.Utc).AddTicks(4421));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 7,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 25, 4, 52, 13, 766, DateTimeKind.Utc).AddTicks(4422));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 8,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 25, 4, 52, 13, 766, DateTimeKind.Utc).AddTicks(4423));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 9,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 25, 4, 52, 13, 766, DateTimeKind.Utc).AddTicks(4425));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 25, 4, 52, 13, 766, DateTimeKind.Utc).AddTicks(3903), new DateTime(2025, 9, 25, 4, 52, 13, 766, DateTimeKind.Utc).AddTicks(3908) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 25, 4, 52, 13, 766, DateTimeKind.Utc).AddTicks(3916), new DateTime(2025, 9, 25, 4, 52, 13, 766, DateTimeKind.Utc).AddTicks(3916) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 25, 4, 52, 13, 766, DateTimeKind.Utc).AddTicks(3919), new DateTime(2025, 9, 25, 4, 52, 13, 766, DateTimeKind.Utc).AddTicks(3920) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 25, 4, 52, 13, 766, DateTimeKind.Utc).AddTicks(3922), new DateTime(2025, 9, 25, 4, 52, 13, 766, DateTimeKind.Utc).AddTicks(3922) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 25, 4, 52, 13, 766, DateTimeKind.Utc).AddTicks(3924), new DateTime(2025, 9, 25, 4, 52, 13, 766, DateTimeKind.Utc).AddTicks(3925) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 25, 4, 52, 13, 766, DateTimeKind.Utc).AddTicks(3927), new DateTime(2025, 9, 25, 4, 52, 13, 766, DateTimeKind.Utc).AddTicks(3927) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 25, 4, 52, 13, 766, DateTimeKind.Utc).AddTicks(3929), new DateTime(2025, 9, 25, 4, 52, 13, 766, DateTimeKind.Utc).AddTicks(3929) });
        }
    }
}
