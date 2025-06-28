using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAuthenticationSupport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 2, 7, 3, 21, 730, DateTimeKind.Utc).AddTicks(6853), new DateTime(2025, 6, 2, 7, 3, 21, 730, DateTimeKind.Utc).AddTicks(6854) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 2, 7, 3, 21, 730, DateTimeKind.Utc).AddTicks(6858), new DateTime(2025, 6, 2, 7, 3, 21, 730, DateTimeKind.Utc).AddTicks(6858) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 2, 7, 3, 21, 730, DateTimeKind.Utc).AddTicks(6861), new DateTime(2025, 6, 2, 7, 3, 21, 730, DateTimeKind.Utc).AddTicks(6861) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 2, 7, 3, 21, 730, DateTimeKind.Utc).AddTicks(6862), new DateTime(2025, 6, 2, 7, 3, 21, 730, DateTimeKind.Utc).AddTicks(6863) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 2, 7, 3, 21, 730, DateTimeKind.Utc).AddTicks(6864), new DateTime(2025, 6, 2, 7, 3, 21, 730, DateTimeKind.Utc).AddTicks(6865) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 2, 7, 3, 21, 730, DateTimeKind.Utc).AddTicks(6866), new DateTime(2025, 6, 2, 7, 3, 21, 730, DateTimeKind.Utc).AddTicks(6867) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 2, 7, 3, 21, 730, DateTimeKind.Utc).AddTicks(6868), new DateTime(2025, 6, 2, 7, 3, 21, 730, DateTimeKind.Utc).AddTicks(6868) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 2, 7, 3, 21, 730, DateTimeKind.Utc).AddTicks(6618), new DateTime(2025, 6, 2, 7, 3, 21, 730, DateTimeKind.Utc).AddTicks(6621) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 2, 7, 3, 21, 730, DateTimeKind.Utc).AddTicks(6626), new DateTime(2025, 6, 2, 7, 3, 21, 730, DateTimeKind.Utc).AddTicks(6626) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 2, 7, 3, 21, 730, DateTimeKind.Utc).AddTicks(6629), new DateTime(2025, 6, 2, 7, 3, 21, 730, DateTimeKind.Utc).AddTicks(6629) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastLoginAt", "PasswordHash", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 2, 7, 3, 21, 730, DateTimeKind.Utc).AddTicks(6900), null, "$2a$11$K8QQfR6Z5j6XgkHjWo9xXeNqO7QDj9qQVvjBZjR8g1jzQzKL9Yd3W", new DateTime(2025, 6, 2, 7, 3, 21, 730, DateTimeKind.Utc).AddTicks(6901) });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Name",
                table: "Users",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_Name",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LastLoginAt",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
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
    }
}
