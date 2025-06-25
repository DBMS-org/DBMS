using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_Username",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LastLoginAt",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 2, 6, 46, 10, 30, DateTimeKind.Utc).AddTicks(219), new DateTime(2025, 6, 2, 6, 46, 10, 30, DateTimeKind.Utc).AddTicks(219) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 2, 6, 46, 10, 30, DateTimeKind.Utc).AddTicks(222), new DateTime(2025, 6, 2, 6, 46, 10, 30, DateTimeKind.Utc).AddTicks(222) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 2, 6, 46, 10, 30, DateTimeKind.Utc).AddTicks(224), new DateTime(2025, 6, 2, 6, 46, 10, 30, DateTimeKind.Utc).AddTicks(224) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 2, 6, 46, 10, 30, DateTimeKind.Utc).AddTicks(226), new DateTime(2025, 6, 2, 6, 46, 10, 30, DateTimeKind.Utc).AddTicks(226) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 2, 6, 46, 10, 30, DateTimeKind.Utc).AddTicks(227), new DateTime(2025, 6, 2, 6, 46, 10, 30, DateTimeKind.Utc).AddTicks(228) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 2, 6, 46, 10, 30, DateTimeKind.Utc).AddTicks(229), new DateTime(2025, 6, 2, 6, 46, 10, 30, DateTimeKind.Utc).AddTicks(229) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 2, 6, 46, 10, 30, DateTimeKind.Utc).AddTicks(231), new DateTime(2025, 6, 2, 6, 46, 10, 30, DateTimeKind.Utc).AddTicks(231) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 2, 6, 46, 10, 30, DateTimeKind.Utc).AddTicks(103), new DateTime(2025, 6, 2, 6, 46, 10, 30, DateTimeKind.Utc).AddTicks(106) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 2, 6, 46, 10, 30, DateTimeKind.Utc).AddTicks(110), new DateTime(2025, 6, 2, 6, 46, 10, 30, DateTimeKind.Utc).AddTicks(110) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 2, 6, 46, 10, 30, DateTimeKind.Utc).AddTicks(112), new DateTime(2025, 6, 2, 6, 46, 10, 30, DateTimeKind.Utc).AddTicks(112) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 2, 6, 46, 10, 30, DateTimeKind.Utc).AddTicks(254), new DateTime(2025, 6, 2, 6, 46, 10, 30, DateTimeKind.Utc).AddTicks(254) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastLoginAt",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Users",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 2, 5, 13, 52, 346, DateTimeKind.Utc).AddTicks(5352), new DateTime(2025, 6, 2, 5, 13, 52, 346, DateTimeKind.Utc).AddTicks(5352) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 2, 5, 13, 52, 346, DateTimeKind.Utc).AddTicks(5355), new DateTime(2025, 6, 2, 5, 13, 52, 346, DateTimeKind.Utc).AddTicks(5355) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 2, 5, 13, 52, 346, DateTimeKind.Utc).AddTicks(5357), new DateTime(2025, 6, 2, 5, 13, 52, 346, DateTimeKind.Utc).AddTicks(5357) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 2, 5, 13, 52, 346, DateTimeKind.Utc).AddTicks(5359), new DateTime(2025, 6, 2, 5, 13, 52, 346, DateTimeKind.Utc).AddTicks(5359) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 2, 5, 13, 52, 346, DateTimeKind.Utc).AddTicks(5360), new DateTime(2025, 6, 2, 5, 13, 52, 346, DateTimeKind.Utc).AddTicks(5360) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 2, 5, 13, 52, 346, DateTimeKind.Utc).AddTicks(5361), new DateTime(2025, 6, 2, 5, 13, 52, 346, DateTimeKind.Utc).AddTicks(5362) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 2, 5, 13, 52, 346, DateTimeKind.Utc).AddTicks(5363), new DateTime(2025, 6, 2, 5, 13, 52, 346, DateTimeKind.Utc).AddTicks(5363) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 2, 5, 13, 52, 346, DateTimeKind.Utc).AddTicks(5221), new DateTime(2025, 6, 2, 5, 13, 52, 346, DateTimeKind.Utc).AddTicks(5223) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 2, 5, 13, 52, 346, DateTimeKind.Utc).AddTicks(5228), new DateTime(2025, 6, 2, 5, 13, 52, 346, DateTimeKind.Utc).AddTicks(5229) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 2, 5, 13, 52, 346, DateTimeKind.Utc).AddTicks(5230), new DateTime(2025, 6, 2, 5, 13, 52, 346, DateTimeKind.Utc).AddTicks(5230) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastLoginAt", "PasswordHash", "UpdatedAt", "Username" },
                values: new object[] { new DateTime(2025, 6, 2, 5, 13, 52, 346, DateTimeKind.Utc).AddTicks(5386), null, "$2a$11$K7XfBJ2OwQX8.mKlM.cK6u5VDJ8N8LjqfKfwJV.6NZGR8X.mJKjOq", new DateTime(2025, 6, 2, 5, 13, 52, 346, DateTimeKind.Utc).AddTicks(5386), "admin" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);
        }
    }
}
