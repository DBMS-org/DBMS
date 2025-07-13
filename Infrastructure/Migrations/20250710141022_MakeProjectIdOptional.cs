using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MakeProjectIdOptional : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Machines_Projects_ProjectId",
                table: "Machines");

            migrationBuilder.AlterColumn<int>(
                name: "ProjectId",
                table: "Machines",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 10, 14, 10, 21, 95, DateTimeKind.Utc).AddTicks(9845), new DateTime(2025, 7, 10, 14, 10, 21, 95, DateTimeKind.Utc).AddTicks(9845) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 10, 14, 10, 21, 95, DateTimeKind.Utc).AddTicks(9850), new DateTime(2025, 7, 10, 14, 10, 21, 95, DateTimeKind.Utc).AddTicks(9850) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 10, 14, 10, 21, 95, DateTimeKind.Utc).AddTicks(9853), new DateTime(2025, 7, 10, 14, 10, 21, 95, DateTimeKind.Utc).AddTicks(9853) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 10, 14, 10, 21, 95, DateTimeKind.Utc).AddTicks(9855), new DateTime(2025, 7, 10, 14, 10, 21, 95, DateTimeKind.Utc).AddTicks(9855) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 10, 14, 10, 21, 95, DateTimeKind.Utc).AddTicks(9857), new DateTime(2025, 7, 10, 14, 10, 21, 95, DateTimeKind.Utc).AddTicks(9858) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 10, 14, 10, 21, 95, DateTimeKind.Utc).AddTicks(9860), new DateTime(2025, 7, 10, 14, 10, 21, 95, DateTimeKind.Utc).AddTicks(9860) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 10, 14, 10, 21, 95, DateTimeKind.Utc).AddTicks(9862), new DateTime(2025, 7, 10, 14, 10, 21, 95, DateTimeKind.Utc).AddTicks(9862) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 10, 14, 10, 21, 95, DateTimeKind.Utc).AddTicks(9864), new DateTime(2025, 7, 10, 14, 10, 21, 95, DateTimeKind.Utc).AddTicks(9865) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 10, 14, 10, 21, 96, DateTimeKind.Utc).AddTicks(4), new DateTime(2025, 7, 10, 14, 10, 21, 96, DateTimeKind.Utc).AddTicks(4) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 10, 14, 10, 21, 96, DateTimeKind.Utc).AddTicks(14), new DateTime(2025, 7, 10, 14, 10, 21, 96, DateTimeKind.Utc).AddTicks(14) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 10, 14, 10, 21, 96, DateTimeKind.Utc).AddTicks(17), new DateTime(2025, 7, 10, 14, 10, 21, 96, DateTimeKind.Utc).AddTicks(17) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 10, 14, 10, 21, 96, DateTimeKind.Utc).AddTicks(19), new DateTime(2025, 7, 10, 14, 10, 21, 96, DateTimeKind.Utc).AddTicks(20) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 10, 14, 10, 21, 96, DateTimeKind.Utc).AddTicks(68), new DateTime(2025, 7, 10, 14, 10, 21, 96, DateTimeKind.Utc).AddTicks(69) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 10, 14, 10, 21, 96, DateTimeKind.Utc).AddTicks(71), new DateTime(2025, 7, 10, 14, 10, 21, 96, DateTimeKind.Utc).AddTicks(72) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 10, 14, 10, 21, 96, DateTimeKind.Utc).AddTicks(74), new DateTime(2025, 7, 10, 14, 10, 21, 96, DateTimeKind.Utc).AddTicks(74) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 10, 14, 10, 21, 96, DateTimeKind.Utc).AddTicks(76), new DateTime(2025, 7, 10, 14, 10, 21, 96, DateTimeKind.Utc).AddTicks(77) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 10, 14, 10, 21, 96, DateTimeKind.Utc).AddTicks(79), new DateTime(2025, 7, 10, 14, 10, 21, 96, DateTimeKind.Utc).AddTicks(79) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 10, 14, 10, 21, 96, DateTimeKind.Utc).AddTicks(81), new DateTime(2025, 7, 10, 14, 10, 21, 96, DateTimeKind.Utc).AddTicks(82) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 10, 14, 10, 21, 96, DateTimeKind.Utc).AddTicks(84), new DateTime(2025, 7, 10, 14, 10, 21, 96, DateTimeKind.Utc).AddTicks(84) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 1,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 10, 14, 10, 21, 95, DateTimeKind.Utc).AddTicks(9918));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 2,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 10, 14, 10, 21, 95, DateTimeKind.Utc).AddTicks(9921));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 3,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 10, 14, 10, 21, 95, DateTimeKind.Utc).AddTicks(9923));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 4,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 10, 14, 10, 21, 95, DateTimeKind.Utc).AddTicks(9925));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 5,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 10, 14, 10, 21, 95, DateTimeKind.Utc).AddTicks(9928));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 6,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 10, 14, 10, 21, 95, DateTimeKind.Utc).AddTicks(9931));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 7,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 10, 14, 10, 21, 95, DateTimeKind.Utc).AddTicks(9933));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 8,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 10, 14, 10, 21, 95, DateTimeKind.Utc).AddTicks(9937));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 9,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 10, 14, 10, 21, 95, DateTimeKind.Utc).AddTicks(9939));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 10, 14, 10, 21, 95, DateTimeKind.Utc).AddTicks(9591), new DateTime(2025, 7, 10, 14, 10, 21, 95, DateTimeKind.Utc).AddTicks(9594) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 10, 14, 10, 21, 95, DateTimeKind.Utc).AddTicks(9601), new DateTime(2025, 7, 10, 14, 10, 21, 95, DateTimeKind.Utc).AddTicks(9601) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 10, 14, 10, 21, 95, DateTimeKind.Utc).AddTicks(9604), new DateTime(2025, 7, 10, 14, 10, 21, 95, DateTimeKind.Utc).AddTicks(9604) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 10, 14, 10, 21, 95, DateTimeKind.Utc).AddTicks(9607), new DateTime(2025, 7, 10, 14, 10, 21, 95, DateTimeKind.Utc).AddTicks(9607) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 10, 14, 10, 21, 95, DateTimeKind.Utc).AddTicks(9609), new DateTime(2025, 7, 10, 14, 10, 21, 95, DateTimeKind.Utc).AddTicks(9610) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 10, 14, 10, 21, 95, DateTimeKind.Utc).AddTicks(9612), new DateTime(2025, 7, 10, 14, 10, 21, 95, DateTimeKind.Utc).AddTicks(9612) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 10, 14, 10, 21, 95, DateTimeKind.Utc).AddTicks(9614), new DateTime(2025, 7, 10, 14, 10, 21, 95, DateTimeKind.Utc).AddTicks(9614) });

            migrationBuilder.AddForeignKey(
                name: "FK_Machines_Projects_ProjectId",
                table: "Machines",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Machines_Projects_ProjectId",
                table: "Machines");

            migrationBuilder.AlterColumn<int>(
                name: "ProjectId",
                table: "Machines",
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
                values: new object[] { new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7388), new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7389) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7398), new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7398) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7401), new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7401) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7403), new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7404) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7405), new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7406) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7408), new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7408) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7410), new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7411) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7412), new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7413) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7572), new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7573) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7584), new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7585) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7587), new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7588) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7590), new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7590) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7592), new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7593) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7595), new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7595) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7597), new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7598) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7600), new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7600) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7602), new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7602) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7604), new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7605) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7607), new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7607) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 1,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7473));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 2,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7481));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 3,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7483));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 4,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7484));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 5,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7486));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 6,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7487));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 7,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7488));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 8,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7490));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 9,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7491));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(6999), new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7006) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7016), new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7017) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7020), new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7020) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7022), new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7023) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7025), new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7025) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7027), new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7027) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7029), new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7030) });

            migrationBuilder.AddForeignKey(
                name: "FK_Machines_Projects_ProjectId",
                table: "Machines",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
