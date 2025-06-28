using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UniqueOperatorPerProject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Projects_AssignedUserId",
                table: "Projects");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 6, 42, 31, 682, DateTimeKind.Utc).AddTicks(7787), new DateTime(2025, 6, 16, 6, 42, 31, 682, DateTimeKind.Utc).AddTicks(7787) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 6, 42, 31, 682, DateTimeKind.Utc).AddTicks(7793), new DateTime(2025, 6, 16, 6, 42, 31, 682, DateTimeKind.Utc).AddTicks(7793) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 6, 42, 31, 682, DateTimeKind.Utc).AddTicks(7797), new DateTime(2025, 6, 16, 6, 42, 31, 682, DateTimeKind.Utc).AddTicks(7797) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 6, 42, 31, 682, DateTimeKind.Utc).AddTicks(7799), new DateTime(2025, 6, 16, 6, 42, 31, 682, DateTimeKind.Utc).AddTicks(7800) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 6, 42, 31, 682, DateTimeKind.Utc).AddTicks(7802), new DateTime(2025, 6, 16, 6, 42, 31, 682, DateTimeKind.Utc).AddTicks(7802) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 6, 42, 31, 682, DateTimeKind.Utc).AddTicks(7805), new DateTime(2025, 6, 16, 6, 42, 31, 682, DateTimeKind.Utc).AddTicks(7805) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 6, 42, 31, 682, DateTimeKind.Utc).AddTicks(7807), new DateTime(2025, 6, 16, 6, 42, 31, 682, DateTimeKind.Utc).AddTicks(7808) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 6, 42, 31, 682, DateTimeKind.Utc).AddTicks(7810), new DateTime(2025, 6, 16, 6, 42, 31, 682, DateTimeKind.Utc).AddTicks(7810) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 6, 42, 31, 682, DateTimeKind.Utc).AddTicks(7812), new DateTime(2025, 6, 16, 6, 42, 31, 682, DateTimeKind.Utc).AddTicks(7813) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 6, 42, 31, 682, DateTimeKind.Utc).AddTicks(7815), new DateTime(2025, 6, 16, 6, 42, 31, 682, DateTimeKind.Utc).AddTicks(7815) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 6, 42, 31, 682, DateTimeKind.Utc).AddTicks(7817), new DateTime(2025, 6, 16, 6, 42, 31, 682, DateTimeKind.Utc).AddTicks(7818) });

            migrationBuilder.UpdateData(
                table: "ProjectSites",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 6, 42, 31, 682, DateTimeKind.Utc).AddTicks(7972), new DateTime(2025, 6, 16, 6, 42, 31, 682, DateTimeKind.Utc).AddTicks(7973) });

            migrationBuilder.UpdateData(
                table: "ProjectSites",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 6, 42, 31, 682, DateTimeKind.Utc).AddTicks(7978), new DateTime(2025, 6, 16, 6, 42, 31, 682, DateTimeKind.Utc).AddTicks(7979) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 6, 42, 31, 682, DateTimeKind.Utc).AddTicks(7914), new DateTime(2025, 6, 16, 6, 42, 31, 682, DateTimeKind.Utc).AddTicks(7915) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 6, 42, 31, 682, DateTimeKind.Utc).AddTicks(7921), new DateTime(2025, 6, 16, 6, 42, 31, 682, DateTimeKind.Utc).AddTicks(7922) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 12, 16, 6, 42, 31, 682, DateTimeKind.Utc).AddTicks(7927), new DateTime(2025, 5, 16, 6, 42, 31, 682, DateTimeKind.Utc).AddTicks(7935) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 6, 42, 31, 682, DateTimeKind.Utc).AddTicks(7592), new DateTime(2025, 6, 16, 6, 42, 31, 682, DateTimeKind.Utc).AddTicks(7594) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 6, 42, 31, 682, DateTimeKind.Utc).AddTicks(7601), new DateTime(2025, 6, 16, 6, 42, 31, 682, DateTimeKind.Utc).AddTicks(7601) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 6, 42, 31, 682, DateTimeKind.Utc).AddTicks(7604), new DateTime(2025, 6, 16, 6, 42, 31, 682, DateTimeKind.Utc).AddTicks(7605) });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedAt", "Description", "IsActive", "Name", "NormalizedName", "UpdatedAt" },
                values: new object[] { 4, new DateTime(2025, 6, 16, 6, 42, 31, 682, DateTimeKind.Utc).AddTicks(7607), "Operator with operational access", true, "Operator", "OPERATOR", new DateTime(2025, 6, 16, 6, 42, 31, 682, DateTimeKind.Utc).AddTicks(7607) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 6, 42, 31, 682, DateTimeKind.Utc).AddTicks(7862), new DateTime(2025, 6, 16, 6, 42, 31, 682, DateTimeKind.Utc).AddTicks(7863) });

            // Ensure no duplicate operator assignments before creating unique index
            migrationBuilder.Sql(@"
                ;WITH cte AS (
                    SELECT Id, ROW_NUMBER() OVER (PARTITION BY AssignedUserId ORDER BY Id) AS rn
                    FROM Projects
                    WHERE AssignedUserId IS NOT NULL
                )
                UPDATE p
                SET AssignedUserId = NULL
                FROM Projects p
                JOIN cte ON p.Id = cte.Id
                WHERE cte.rn > 1;
            ");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_AssignedUserId",
                table: "Projects",
                column: "AssignedUserId",
                unique: true,
                filter: "[AssignedUserId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Projects_AssignedUserId",
                table: "Projects");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 11, 8, 0, 34, 148, DateTimeKind.Utc).AddTicks(4146), new DateTime(2025, 6, 11, 8, 0, 34, 148, DateTimeKind.Utc).AddTicks(4146) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 11, 8, 0, 34, 148, DateTimeKind.Utc).AddTicks(4157), new DateTime(2025, 6, 11, 8, 0, 34, 148, DateTimeKind.Utc).AddTicks(4158) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 11, 8, 0, 34, 148, DateTimeKind.Utc).AddTicks(4161), new DateTime(2025, 6, 11, 8, 0, 34, 148, DateTimeKind.Utc).AddTicks(4161) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 11, 8, 0, 34, 148, DateTimeKind.Utc).AddTicks(4163), new DateTime(2025, 6, 11, 8, 0, 34, 148, DateTimeKind.Utc).AddTicks(4164) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 11, 8, 0, 34, 148, DateTimeKind.Utc).AddTicks(4166), new DateTime(2025, 6, 11, 8, 0, 34, 148, DateTimeKind.Utc).AddTicks(4166) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 11, 8, 0, 34, 148, DateTimeKind.Utc).AddTicks(4169), new DateTime(2025, 6, 11, 8, 0, 34, 148, DateTimeKind.Utc).AddTicks(4169) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 11, 8, 0, 34, 148, DateTimeKind.Utc).AddTicks(4171), new DateTime(2025, 6, 11, 8, 0, 34, 148, DateTimeKind.Utc).AddTicks(4172) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 11, 8, 0, 34, 148, DateTimeKind.Utc).AddTicks(4174), new DateTime(2025, 6, 11, 8, 0, 34, 148, DateTimeKind.Utc).AddTicks(4174) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 11, 8, 0, 34, 148, DateTimeKind.Utc).AddTicks(4176), new DateTime(2025, 6, 11, 8, 0, 34, 148, DateTimeKind.Utc).AddTicks(4177) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 11, 8, 0, 34, 148, DateTimeKind.Utc).AddTicks(4179), new DateTime(2025, 6, 11, 8, 0, 34, 148, DateTimeKind.Utc).AddTicks(4179) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 11, 8, 0, 34, 148, DateTimeKind.Utc).AddTicks(4181), new DateTime(2025, 6, 11, 8, 0, 34, 148, DateTimeKind.Utc).AddTicks(4182) });

            migrationBuilder.UpdateData(
                table: "ProjectSites",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 11, 8, 0, 34, 148, DateTimeKind.Utc).AddTicks(4514), new DateTime(2025, 6, 11, 8, 0, 34, 148, DateTimeKind.Utc).AddTicks(4515) });

            migrationBuilder.UpdateData(
                table: "ProjectSites",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 11, 8, 0, 34, 148, DateTimeKind.Utc).AddTicks(4520), new DateTime(2025, 6, 11, 8, 0, 34, 148, DateTimeKind.Utc).AddTicks(4521) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 11, 8, 0, 34, 148, DateTimeKind.Utc).AddTicks(4436), new DateTime(2025, 6, 11, 8, 0, 34, 148, DateTimeKind.Utc).AddTicks(4437) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 11, 8, 0, 34, 148, DateTimeKind.Utc).AddTicks(4444), new DateTime(2025, 6, 11, 8, 0, 34, 148, DateTimeKind.Utc).AddTicks(4444) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 12, 11, 8, 0, 34, 148, DateTimeKind.Utc).AddTicks(4450), new DateTime(2025, 5, 11, 8, 0, 34, 148, DateTimeKind.Utc).AddTicks(4462) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 11, 8, 0, 34, 148, DateTimeKind.Utc).AddTicks(3863), new DateTime(2025, 6, 11, 8, 0, 34, 148, DateTimeKind.Utc).AddTicks(3870) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 11, 8, 0, 34, 148, DateTimeKind.Utc).AddTicks(3883), new DateTime(2025, 6, 11, 8, 0, 34, 148, DateTimeKind.Utc).AddTicks(3884) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 11, 8, 0, 34, 148, DateTimeKind.Utc).AddTicks(3887), new DateTime(2025, 6, 11, 8, 0, 34, 148, DateTimeKind.Utc).AddTicks(3887) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 11, 8, 0, 34, 148, DateTimeKind.Utc).AddTicks(4241), new DateTime(2025, 6, 11, 8, 0, 34, 148, DateTimeKind.Utc).AddTicks(4242) });

            migrationBuilder.CreateIndex(
                name: "IX_Projects_AssignedUserId",
                table: "Projects",
                column: "AssignedUserId");
        }
    }
}
