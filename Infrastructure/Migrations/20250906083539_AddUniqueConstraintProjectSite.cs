using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueConstraintProjectSite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3768), new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3769) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3773), new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3773) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3776), new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3776) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3780), new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3781) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3782), new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3783) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3785), new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3785) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3790), new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3791) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3793), new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3793) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3906), new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3906) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3914), new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3915) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3918), new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3918) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3920), new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3920) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3922), new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3923) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3924), new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3925) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3927), new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3927) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3929), new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3930) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3931), new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3932) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3934), new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3934) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3936), new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3936) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 1,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3836));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 2,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3841));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 3,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3843));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 4,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3844));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 5,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3846));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 6,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3847));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 7,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3849));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 8,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3850));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 9,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3852));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3463), new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3467) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3473), new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3474) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3477), new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3477) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3479), new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3480) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3482), new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3482) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3484), new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3484) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3486), new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3487) });

            migrationBuilder.CreateIndex(
                name: "IX_ExplosiveCalculationResults_ProjectSite_Unique",
                table: "ExplosiveCalculationResults",
                columns: new[] { "ProjectId", "SiteId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ExplosiveCalculationResults_ProjectSite_Unique",
                table: "ExplosiveCalculationResults");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7497), new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7497) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7504), new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7505) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7507), new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7508) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7510), new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7511) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7513), new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7513) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7515), new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7516) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7518), new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7518) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7572), new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7572) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7688), new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7688) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7697), new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7698) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7700), new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7701) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7703), new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7703) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7705), new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7705) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7707), new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7708) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7710), new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7711) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7713), new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7713) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7715), new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7715) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7717), new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7718) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7720), new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7720) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 1,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7616));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 2,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7620));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 3,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7622));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 4,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7624));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 5,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7625));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 6,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7627));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 7,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7628));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 8,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7630));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 9,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7631));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7214), new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7217) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7223), new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7223) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7226), new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7227) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7229), new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7229) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7232), new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7232) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7234), new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7234) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7236), new DateTime(2025, 9, 6, 5, 19, 39, 71, DateTimeKind.Utc).AddTicks(7237) });
        }
    }
}
