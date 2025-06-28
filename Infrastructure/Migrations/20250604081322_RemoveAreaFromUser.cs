using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveAreaFromUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Area",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 4, 8, 13, 21, 672, DateTimeKind.Utc).AddTicks(1725), new DateTime(2025, 6, 4, 8, 13, 21, 672, DateTimeKind.Utc).AddTicks(1726) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 4, 8, 13, 21, 672, DateTimeKind.Utc).AddTicks(1777), new DateTime(2025, 6, 4, 8, 13, 21, 672, DateTimeKind.Utc).AddTicks(1778) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 4, 8, 13, 21, 672, DateTimeKind.Utc).AddTicks(1780), new DateTime(2025, 6, 4, 8, 13, 21, 672, DateTimeKind.Utc).AddTicks(1780) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 4, 8, 13, 21, 672, DateTimeKind.Utc).AddTicks(1782), new DateTime(2025, 6, 4, 8, 13, 21, 672, DateTimeKind.Utc).AddTicks(1782) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 4, 8, 13, 21, 672, DateTimeKind.Utc).AddTicks(1783), new DateTime(2025, 6, 4, 8, 13, 21, 672, DateTimeKind.Utc).AddTicks(1784) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 4, 8, 13, 21, 672, DateTimeKind.Utc).AddTicks(1785), new DateTime(2025, 6, 4, 8, 13, 21, 672, DateTimeKind.Utc).AddTicks(1785) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 4, 8, 13, 21, 672, DateTimeKind.Utc).AddTicks(1786), new DateTime(2025, 6, 4, 8, 13, 21, 672, DateTimeKind.Utc).AddTicks(1787) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 4, 8, 13, 21, 672, DateTimeKind.Utc).AddTicks(1590), new DateTime(2025, 6, 4, 8, 13, 21, 672, DateTimeKind.Utc).AddTicks(1591) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 4, 8, 13, 21, 672, DateTimeKind.Utc).AddTicks(1596), new DateTime(2025, 6, 4, 8, 13, 21, 672, DateTimeKind.Utc).AddTicks(1596) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 4, 8, 13, 21, 672, DateTimeKind.Utc).AddTicks(1598), new DateTime(2025, 6, 4, 8, 13, 21, 672, DateTimeKind.Utc).AddTicks(1598) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 4, 8, 13, 21, 672, DateTimeKind.Utc).AddTicks(1817), new DateTime(2025, 6, 4, 8, 13, 21, 672, DateTimeKind.Utc).AddTicks(1818) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Area",
                table: "Users",
                type: "nvarchar(100)",
                maxLength: 100,
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
                columns: new[] { "Area", "CreatedAt", "UpdatedAt" },
                values: new object[] { "Central", new DateTime(2025, 6, 2, 7, 3, 21, 730, DateTimeKind.Utc).AddTicks(6900), new DateTime(2025, 6, 2, 7, 3, 21, 730, DateTimeKind.Utc).AddTicks(6901) });
        }
    }
}
