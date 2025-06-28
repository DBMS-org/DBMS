using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProjectsPropertyNaming : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProjectValue",
                table: "Projects",
                newName: "ProjectType");

            migrationBuilder.AlterColumn<string>(
                name: "Coordinates",
                table: "ProjectSites",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 4, 16, 57, 1, 77, DateTimeKind.Utc).AddTicks(7331), new DateTime(2025, 6, 4, 16, 57, 1, 77, DateTimeKind.Utc).AddTicks(7332) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 4, 16, 57, 1, 77, DateTimeKind.Utc).AddTicks(7336), new DateTime(2025, 6, 4, 16, 57, 1, 77, DateTimeKind.Utc).AddTicks(7336) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 4, 16, 57, 1, 77, DateTimeKind.Utc).AddTicks(7339), new DateTime(2025, 6, 4, 16, 57, 1, 77, DateTimeKind.Utc).AddTicks(7340) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 4, 16, 57, 1, 77, DateTimeKind.Utc).AddTicks(7342), new DateTime(2025, 6, 4, 16, 57, 1, 77, DateTimeKind.Utc).AddTicks(7342) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 4, 16, 57, 1, 77, DateTimeKind.Utc).AddTicks(7345), new DateTime(2025, 6, 4, 16, 57, 1, 77, DateTimeKind.Utc).AddTicks(7345) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 4, 16, 57, 1, 77, DateTimeKind.Utc).AddTicks(7347), new DateTime(2025, 6, 4, 16, 57, 1, 77, DateTimeKind.Utc).AddTicks(7347) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 4, 16, 57, 1, 77, DateTimeKind.Utc).AddTicks(7349), new DateTime(2025, 6, 4, 16, 57, 1, 77, DateTimeKind.Utc).AddTicks(7350) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 4, 16, 57, 1, 77, DateTimeKind.Utc).AddTicks(7352), new DateTime(2025, 6, 4, 16, 57, 1, 77, DateTimeKind.Utc).AddTicks(7352) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 4, 16, 57, 1, 77, DateTimeKind.Utc).AddTicks(7354), new DateTime(2025, 6, 4, 16, 57, 1, 77, DateTimeKind.Utc).AddTicks(7355) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 4, 16, 57, 1, 77, DateTimeKind.Utc).AddTicks(7357), new DateTime(2025, 6, 4, 16, 57, 1, 77, DateTimeKind.Utc).AddTicks(7357) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 4, 16, 57, 1, 77, DateTimeKind.Utc).AddTicks(7359), new DateTime(2025, 6, 4, 16, 57, 1, 77, DateTimeKind.Utc).AddTicks(7359) });

            migrationBuilder.InsertData(
                table: "ProjectSites",
                columns: new[] { "Id", "Coordinates", "CreatedAt", "Description", "Location", "Name", "ProjectId", "Status", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "{\"Latitude\":23.5880,\"Longitude\":58.3829}", new DateTime(2025, 6, 4, 16, 57, 1, 77, DateTimeKind.Utc).AddTicks(7500), "Primary construction site for highway project", "Muscat Highway Junction", "Muscat Main Site", 1, "Active", new DateTime(2025, 6, 4, 16, 57, 1, 77, DateTimeKind.Utc).AddTicks(7500) },
                    { 2, "{\"Latitude\":17.0194,\"Longitude\":54.1085}", new DateTime(2025, 6, 4, 16, 57, 1, 77, DateTimeKind.Utc).AddTicks(7504), "Main mining pit for ore extraction", "Dhofar Mining Area", "Dhofar Mining Pit A", 2, "Active", new DateTime(2025, 6, 4, 16, 57, 1, 77, DateTimeKind.Utc).AddTicks(7505) }
                });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Status", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 4, 16, 57, 1, 77, DateTimeKind.Utc).AddTicks(7448), "Active", new DateTime(2025, 6, 4, 16, 57, 1, 77, DateTimeKind.Utc).AddTicks(7449) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "Description", "EndDate", "ProjectType", "StartDate", "Status", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 4, 16, 57, 1, 77, DateTimeKind.Utc).AddTicks(7456), "Mining site expansion project in Dhofar region", new DateTime(2024, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mining Site Development", new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Active", new DateTime(2025, 6, 4, 16, 57, 1, 77, DateTimeKind.Utc).AddTicks(7457) });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Id", "AssignedUserId", "Budget", "CreatedAt", "Description", "EndDate", "Name", "ProjectType", "Region", "StartDate", "Status", "UpdatedAt" },
                values: new object[] { 3, 1, 3200000.00m, new DateTime(2024, 12, 4, 16, 57, 1, 77, DateTimeKind.Utc).AddTicks(7462), "Industrial zone construction project in Sohar", new DateTime(2023, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sohar Industrial Zone", "Industrial Development", "Al Batinah North", new DateTime(2023, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Completed", new DateTime(2025, 5, 4, 16, 57, 1, 77, DateTimeKind.Utc).AddTicks(7470) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 4, 16, 57, 1, 77, DateTimeKind.Utc).AddTicks(7168), new DateTime(2025, 6, 4, 16, 57, 1, 77, DateTimeKind.Utc).AddTicks(7171) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 4, 16, 57, 1, 77, DateTimeKind.Utc).AddTicks(7178), new DateTime(2025, 6, 4, 16, 57, 1, 77, DateTimeKind.Utc).AddTicks(7178) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 4, 16, 57, 1, 77, DateTimeKind.Utc).AddTicks(7181), new DateTime(2025, 6, 4, 16, 57, 1, 77, DateTimeKind.Utc).AddTicks(7181) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 4, 16, 57, 1, 77, DateTimeKind.Utc).AddTicks(7397), new DateTime(2025, 6, 4, 16, 57, 1, 77, DateTimeKind.Utc).AddTicks(7397) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ProjectSites",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ProjectSites",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.RenameColumn(
                name: "ProjectType",
                table: "Projects",
                newName: "ProjectValue");

            migrationBuilder.AlterColumn<string>(
                name: "Coordinates",
                table: "ProjectSites",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 4, 12, 50, 3, 406, DateTimeKind.Utc).AddTicks(3337), new DateTime(2025, 6, 4, 12, 50, 3, 406, DateTimeKind.Utc).AddTicks(3337) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 4, 12, 50, 3, 406, DateTimeKind.Utc).AddTicks(3341), new DateTime(2025, 6, 4, 12, 50, 3, 406, DateTimeKind.Utc).AddTicks(3341) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 4, 12, 50, 3, 406, DateTimeKind.Utc).AddTicks(3343), new DateTime(2025, 6, 4, 12, 50, 3, 406, DateTimeKind.Utc).AddTicks(3343) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 4, 12, 50, 3, 406, DateTimeKind.Utc).AddTicks(3345), new DateTime(2025, 6, 4, 12, 50, 3, 406, DateTimeKind.Utc).AddTicks(3345) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 4, 12, 50, 3, 406, DateTimeKind.Utc).AddTicks(3346), new DateTime(2025, 6, 4, 12, 50, 3, 406, DateTimeKind.Utc).AddTicks(3346) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 4, 12, 50, 3, 406, DateTimeKind.Utc).AddTicks(3348), new DateTime(2025, 6, 4, 12, 50, 3, 406, DateTimeKind.Utc).AddTicks(3348) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 4, 12, 50, 3, 406, DateTimeKind.Utc).AddTicks(3349), new DateTime(2025, 6, 4, 12, 50, 3, 406, DateTimeKind.Utc).AddTicks(3350) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 4, 12, 50, 3, 406, DateTimeKind.Utc).AddTicks(3351), new DateTime(2025, 6, 4, 12, 50, 3, 406, DateTimeKind.Utc).AddTicks(3351) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 4, 12, 50, 3, 406, DateTimeKind.Utc).AddTicks(3353), new DateTime(2025, 6, 4, 12, 50, 3, 406, DateTimeKind.Utc).AddTicks(3353) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 4, 12, 50, 3, 406, DateTimeKind.Utc).AddTicks(3354), new DateTime(2025, 6, 4, 12, 50, 3, 406, DateTimeKind.Utc).AddTicks(3354) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 4, 12, 50, 3, 406, DateTimeKind.Utc).AddTicks(3356), new DateTime(2025, 6, 4, 12, 50, 3, 406, DateTimeKind.Utc).AddTicks(3356) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Status", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 4, 12, 50, 3, 406, DateTimeKind.Utc).AddTicks(3484), "In Progress", new DateTime(2025, 6, 4, 12, 50, 3, 406, DateTimeKind.Utc).AddTicks(3484) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "Description", "EndDate", "ProjectValue", "StartDate", "Status", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 4, 12, 50, 3, 406, DateTimeKind.Utc).AddTicks(3489), "Limestone quarrying operations in Dhofar region", new DateTime(2025, 2, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mining Operations", new DateTime(2024, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Planning", new DateTime(2025, 6, 4, 12, 50, 3, 406, DateTimeKind.Utc).AddTicks(3490) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 4, 12, 50, 3, 406, DateTimeKind.Utc).AddTicks(3226), new DateTime(2025, 6, 4, 12, 50, 3, 406, DateTimeKind.Utc).AddTicks(3228) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 4, 12, 50, 3, 406, DateTimeKind.Utc).AddTicks(3233), new DateTime(2025, 6, 4, 12, 50, 3, 406, DateTimeKind.Utc).AddTicks(3233) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 4, 12, 50, 3, 406, DateTimeKind.Utc).AddTicks(3235), new DateTime(2025, 6, 4, 12, 50, 3, 406, DateTimeKind.Utc).AddTicks(3235) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 4, 12, 50, 3, 406, DateTimeKind.Utc).AddTicks(3384), new DateTime(2025, 6, 4, 12, 50, 3, 406, DateTimeKind.Utc).AddTicks(3384) });
        }
    }
}
