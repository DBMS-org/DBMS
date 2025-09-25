using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixDrillHoleIdPrimaryKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "SerialNumber",
                table: "DrillHoles",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "SerialNumber",
                table: "DrillHoles",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

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
    }
}
