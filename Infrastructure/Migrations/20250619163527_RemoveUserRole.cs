using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUserRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 19, 16, 35, 27, 136, DateTimeKind.Utc).AddTicks(9729), new DateTime(2025, 6, 19, 16, 35, 27, 136, DateTimeKind.Utc).AddTicks(9730) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 19, 16, 35, 27, 136, DateTimeKind.Utc).AddTicks(9735), new DateTime(2025, 6, 19, 16, 35, 27, 136, DateTimeKind.Utc).AddTicks(9736) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 19, 16, 35, 27, 136, DateTimeKind.Utc).AddTicks(9739), new DateTime(2025, 6, 19, 16, 35, 27, 136, DateTimeKind.Utc).AddTicks(9739) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 19, 16, 35, 27, 136, DateTimeKind.Utc).AddTicks(9742), new DateTime(2025, 6, 19, 16, 35, 27, 136, DateTimeKind.Utc).AddTicks(9742) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 19, 16, 35, 27, 136, DateTimeKind.Utc).AddTicks(9745), new DateTime(2025, 6, 19, 16, 35, 27, 136, DateTimeKind.Utc).AddTicks(9745) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 19, 16, 35, 27, 136, DateTimeKind.Utc).AddTicks(9748), new DateTime(2025, 6, 19, 16, 35, 27, 136, DateTimeKind.Utc).AddTicks(9748) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 19, 16, 35, 27, 136, DateTimeKind.Utc).AddTicks(9751), new DateTime(2025, 6, 19, 16, 35, 27, 136, DateTimeKind.Utc).AddTicks(9751) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 19, 16, 35, 27, 136, DateTimeKind.Utc).AddTicks(9753), new DateTime(2025, 6, 19, 16, 35, 27, 136, DateTimeKind.Utc).AddTicks(9754) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 19, 16, 35, 27, 136, DateTimeKind.Utc).AddTicks(9756), new DateTime(2025, 6, 19, 16, 35, 27, 136, DateTimeKind.Utc).AddTicks(9757) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 19, 16, 35, 27, 136, DateTimeKind.Utc).AddTicks(9759), new DateTime(2025, 6, 19, 16, 35, 27, 136, DateTimeKind.Utc).AddTicks(9760) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 19, 16, 35, 27, 136, DateTimeKind.Utc).AddTicks(9762), new DateTime(2025, 6, 19, 16, 35, 27, 136, DateTimeKind.Utc).AddTicks(9762) });

            migrationBuilder.UpdateData(
                table: "ProjectSites",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 19, 16, 35, 27, 136, DateTimeKind.Utc).AddTicks(9948), new DateTime(2025, 6, 19, 16, 35, 27, 136, DateTimeKind.Utc).AddTicks(9949) });

            migrationBuilder.UpdateData(
                table: "ProjectSites",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 19, 16, 35, 27, 136, DateTimeKind.Utc).AddTicks(9954), new DateTime(2025, 6, 19, 16, 35, 27, 136, DateTimeKind.Utc).AddTicks(9955) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 19, 16, 35, 27, 136, DateTimeKind.Utc).AddTicks(9877), new DateTime(2025, 6, 19, 16, 35, 27, 136, DateTimeKind.Utc).AddTicks(9878) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 19, 16, 35, 27, 136, DateTimeKind.Utc).AddTicks(9885), new DateTime(2025, 6, 19, 16, 35, 27, 136, DateTimeKind.Utc).AddTicks(9886) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 12, 19, 16, 35, 27, 136, DateTimeKind.Utc).AddTicks(9892), new DateTime(2025, 5, 19, 16, 35, 27, 136, DateTimeKind.Utc).AddTicks(9904) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 19, 16, 35, 27, 136, DateTimeKind.Utc).AddTicks(9445), new DateTime(2025, 6, 19, 16, 35, 27, 136, DateTimeKind.Utc).AddTicks(9451) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 19, 16, 35, 27, 136, DateTimeKind.Utc).AddTicks(9461), new DateTime(2025, 6, 19, 16, 35, 27, 136, DateTimeKind.Utc).AddTicks(9462) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 19, 16, 35, 27, 136, DateTimeKind.Utc).AddTicks(9466), new DateTime(2025, 6, 19, 16, 35, 27, 136, DateTimeKind.Utc).AddTicks(9466) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 19, 16, 35, 27, 136, DateTimeKind.Utc).AddTicks(9469), new DateTime(2025, 6, 19, 16, 35, 27, 136, DateTimeKind.Utc).AddTicks(9469) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 19, 16, 35, 27, 136, DateTimeKind.Utc).AddTicks(9818), new DateTime(2025, 6, 19, 16, 35, 27, 136, DateTimeKind.Utc).AddTicks(9818) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 19, 16, 33, 6, 947, DateTimeKind.Utc).AddTicks(1116), new DateTime(2025, 6, 19, 16, 33, 6, 947, DateTimeKind.Utc).AddTicks(1117) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 19, 16, 33, 6, 947, DateTimeKind.Utc).AddTicks(1122), new DateTime(2025, 6, 19, 16, 33, 6, 947, DateTimeKind.Utc).AddTicks(1123) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 19, 16, 33, 6, 947, DateTimeKind.Utc).AddTicks(1126), new DateTime(2025, 6, 19, 16, 33, 6, 947, DateTimeKind.Utc).AddTicks(1127) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 19, 16, 33, 6, 947, DateTimeKind.Utc).AddTicks(1129), new DateTime(2025, 6, 19, 16, 33, 6, 947, DateTimeKind.Utc).AddTicks(1130) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 19, 16, 33, 6, 947, DateTimeKind.Utc).AddTicks(1132), new DateTime(2025, 6, 19, 16, 33, 6, 947, DateTimeKind.Utc).AddTicks(1133) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 19, 16, 33, 6, 947, DateTimeKind.Utc).AddTicks(1135), new DateTime(2025, 6, 19, 16, 33, 6, 947, DateTimeKind.Utc).AddTicks(1136) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 19, 16, 33, 6, 947, DateTimeKind.Utc).AddTicks(1138), new DateTime(2025, 6, 19, 16, 33, 6, 947, DateTimeKind.Utc).AddTicks(1139) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 19, 16, 33, 6, 947, DateTimeKind.Utc).AddTicks(1141), new DateTime(2025, 6, 19, 16, 33, 6, 947, DateTimeKind.Utc).AddTicks(1142) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 19, 16, 33, 6, 947, DateTimeKind.Utc).AddTicks(1144), new DateTime(2025, 6, 19, 16, 33, 6, 947, DateTimeKind.Utc).AddTicks(1144) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 19, 16, 33, 6, 947, DateTimeKind.Utc).AddTicks(1147), new DateTime(2025, 6, 19, 16, 33, 6, 947, DateTimeKind.Utc).AddTicks(1147) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 19, 16, 33, 6, 947, DateTimeKind.Utc).AddTicks(1150), new DateTime(2025, 6, 19, 16, 33, 6, 947, DateTimeKind.Utc).AddTicks(1150) });

            migrationBuilder.UpdateData(
                table: "ProjectSites",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 19, 16, 33, 6, 947, DateTimeKind.Utc).AddTicks(1482), new DateTime(2025, 6, 19, 16, 33, 6, 947, DateTimeKind.Utc).AddTicks(1483) });

            migrationBuilder.UpdateData(
                table: "ProjectSites",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 19, 16, 33, 6, 947, DateTimeKind.Utc).AddTicks(1489), new DateTime(2025, 6, 19, 16, 33, 6, 947, DateTimeKind.Utc).AddTicks(1490) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 19, 16, 33, 6, 947, DateTimeKind.Utc).AddTicks(1276), new DateTime(2025, 6, 19, 16, 33, 6, 947, DateTimeKind.Utc).AddTicks(1277) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 19, 16, 33, 6, 947, DateTimeKind.Utc).AddTicks(1407), new DateTime(2025, 6, 19, 16, 33, 6, 947, DateTimeKind.Utc).AddTicks(1408) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 12, 19, 16, 33, 6, 947, DateTimeKind.Utc).AddTicks(1416), new DateTime(2025, 5, 19, 16, 33, 6, 947, DateTimeKind.Utc).AddTicks(1429) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 19, 16, 33, 6, 947, DateTimeKind.Utc).AddTicks(771), new DateTime(2025, 6, 19, 16, 33, 6, 947, DateTimeKind.Utc).AddTicks(776) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 19, 16, 33, 6, 947, DateTimeKind.Utc).AddTicks(787), new DateTime(2025, 6, 19, 16, 33, 6, 947, DateTimeKind.Utc).AddTicks(788) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 19, 16, 33, 6, 947, DateTimeKind.Utc).AddTicks(794), new DateTime(2025, 6, 19, 16, 33, 6, 947, DateTimeKind.Utc).AddTicks(795) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 19, 16, 33, 6, 947, DateTimeKind.Utc).AddTicks(797), new DateTime(2025, 6, 19, 16, 33, 6, 947, DateTimeKind.Utc).AddTicks(798) });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedAt", "Description", "IsActive", "Name", "NormalizedName", "UpdatedAt" },
                values: new object[] { 3, new DateTime(2025, 6, 19, 16, 33, 6, 947, DateTimeKind.Utc).AddTicks(791), "Regular user with limited access", true, "User", "USER", new DateTime(2025, 6, 19, 16, 33, 6, 947, DateTimeKind.Utc).AddTicks(792) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 19, 16, 33, 6, 947, DateTimeKind.Utc).AddTicks(1209), new DateTime(2025, 6, 19, 16, 33, 6, 947, DateTimeKind.Utc).AddTicks(1210) });
        }
    }
}
