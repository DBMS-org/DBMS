using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDrillPatternCreatedByForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DrillPatterns_Users_CreatedById",
                table: "DrillPatterns");

            migrationBuilder.DropIndex(
                name: "IX_DrillPatterns_CreatedById",
                table: "DrillPatterns");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "DrillPatterns");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7759), new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7759) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7763), new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7764) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7767), new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7767) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7769), new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7770) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7772), new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7773) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7775), new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7775) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7777), new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7778) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7780), new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7780) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7889), new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7889) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7898), new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7898) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7900), new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7901) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7903), new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7904) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7906), new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7906) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7908), new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7909) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7911), new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7911) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7913), new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7913) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7915), new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7915) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7917), new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7918) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7919), new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7920) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 1,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7823));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 2,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7827));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 3,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7829));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 4,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7831));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 5,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7832));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 6,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7834));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 7,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7835));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 8,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7837));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 9,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7838));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7441), new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7446) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7455), new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7455) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7458), new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7459) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7521), new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7521) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7523), new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7524) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7526), new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7526) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7528), new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7528) });

            migrationBuilder.CreateIndex(
                name: "IX_DrillPatterns_CreatedByUserId",
                table: "DrillPatterns",
                column: "CreatedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_DrillPatterns_Users_CreatedByUserId",
                table: "DrillPatterns",
                column: "CreatedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DrillPatterns_Users_CreatedByUserId",
                table: "DrillPatterns");

            migrationBuilder.DropIndex(
                name: "IX_DrillPatterns_CreatedByUserId",
                table: "DrillPatterns");

            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "DrillPatterns",
                type: "int",
                nullable: false,
                defaultValue: 0);

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
                name: "IX_DrillPatterns_CreatedById",
                table: "DrillPatterns",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_DrillPatterns_Users_CreatedById",
                table: "DrillPatterns",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
