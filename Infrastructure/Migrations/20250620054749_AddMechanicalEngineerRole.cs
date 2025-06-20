using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMechanicalEngineerRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 20, 5, 47, 48, 629, DateTimeKind.Utc).AddTicks(8874), new DateTime(2025, 6, 20, 5, 47, 48, 629, DateTimeKind.Utc).AddTicks(8875) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 20, 5, 47, 48, 629, DateTimeKind.Utc).AddTicks(8892), new DateTime(2025, 6, 20, 5, 47, 48, 629, DateTimeKind.Utc).AddTicks(8892) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 20, 5, 47, 48, 629, DateTimeKind.Utc).AddTicks(8896), new DateTime(2025, 6, 20, 5, 47, 48, 629, DateTimeKind.Utc).AddTicks(8896) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 20, 5, 47, 48, 629, DateTimeKind.Utc).AddTicks(8899), new DateTime(2025, 6, 20, 5, 47, 48, 629, DateTimeKind.Utc).AddTicks(8899) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 20, 5, 47, 48, 629, DateTimeKind.Utc).AddTicks(8902), new DateTime(2025, 6, 20, 5, 47, 48, 629, DateTimeKind.Utc).AddTicks(8902) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 20, 5, 47, 48, 629, DateTimeKind.Utc).AddTicks(8905), new DateTime(2025, 6, 20, 5, 47, 48, 629, DateTimeKind.Utc).AddTicks(8905) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 20, 5, 47, 48, 629, DateTimeKind.Utc).AddTicks(8907), new DateTime(2025, 6, 20, 5, 47, 48, 629, DateTimeKind.Utc).AddTicks(8908) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 20, 5, 47, 48, 629, DateTimeKind.Utc).AddTicks(8910), new DateTime(2025, 6, 20, 5, 47, 48, 629, DateTimeKind.Utc).AddTicks(8910) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 20, 5, 47, 48, 629, DateTimeKind.Utc).AddTicks(8913), new DateTime(2025, 6, 20, 5, 47, 48, 629, DateTimeKind.Utc).AddTicks(8913) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 20, 5, 47, 48, 629, DateTimeKind.Utc).AddTicks(8915), new DateTime(2025, 6, 20, 5, 47, 48, 629, DateTimeKind.Utc).AddTicks(8916) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 20, 5, 47, 48, 629, DateTimeKind.Utc).AddTicks(8918), new DateTime(2025, 6, 20, 5, 47, 48, 629, DateTimeKind.Utc).AddTicks(8918) });

            migrationBuilder.UpdateData(
                table: "ProjectSites",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 20, 5, 47, 48, 629, DateTimeKind.Utc).AddTicks(9343), new DateTime(2025, 6, 20, 5, 47, 48, 629, DateTimeKind.Utc).AddTicks(9344) });

            migrationBuilder.UpdateData(
                table: "ProjectSites",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 20, 5, 47, 48, 629, DateTimeKind.Utc).AddTicks(9354), new DateTime(2025, 6, 20, 5, 47, 48, 629, DateTimeKind.Utc).AddTicks(9354) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 20, 5, 47, 48, 629, DateTimeKind.Utc).AddTicks(9232), new DateTime(2025, 6, 20, 5, 47, 48, 629, DateTimeKind.Utc).AddTicks(9232) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 20, 5, 47, 48, 629, DateTimeKind.Utc).AddTicks(9246), new DateTime(2025, 6, 20, 5, 47, 48, 629, DateTimeKind.Utc).AddTicks(9247) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 12, 20, 5, 47, 48, 629, DateTimeKind.Utc).AddTicks(9256), new DateTime(2025, 5, 20, 5, 47, 48, 629, DateTimeKind.Utc).AddTicks(9271) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 20, 5, 47, 48, 629, DateTimeKind.Utc).AddTicks(8323), new DateTime(2025, 6, 20, 5, 47, 48, 629, DateTimeKind.Utc).AddTicks(8332) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 20, 5, 47, 48, 629, DateTimeKind.Utc).AddTicks(8349), new DateTime(2025, 6, 20, 5, 47, 48, 629, DateTimeKind.Utc).AddTicks(8349) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 20, 5, 47, 48, 629, DateTimeKind.Utc).AddTicks(8355), new DateTime(2025, 6, 20, 5, 47, 48, 629, DateTimeKind.Utc).AddTicks(8356) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 20, 5, 47, 48, 629, DateTimeKind.Utc).AddTicks(8358), new DateTime(2025, 6, 20, 5, 47, 48, 629, DateTimeKind.Utc).AddTicks(8359) });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedAt", "Description", "IsActive", "Name", "NormalizedName", "UpdatedAt" },
                values: new object[] { 3, new DateTime(2025, 6, 20, 5, 47, 48, 629, DateTimeKind.Utc).AddTicks(8352), "Mechanical Engineer with analysis and design access", true, "MechanicalEngineer", "MECHANICALENGINEER", new DateTime(2025, 6, 20, 5, 47, 48, 629, DateTimeKind.Utc).AddTicks(8353) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 20, 5, 47, 48, 629, DateTimeKind.Utc).AddTicks(9126), new DateTime(2025, 6, 20, 5, 47, 48, 629, DateTimeKind.Utc).AddTicks(9126) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
    }
}
