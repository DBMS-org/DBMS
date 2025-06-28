using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddProjectManagement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Region = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ProjectValue = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Budget = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AssignedUserId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Projects_Users_AssignedUserId",
                        column: x => x.AssignedUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "ProjectSites",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Location = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Coordinates = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectSites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectSites_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "Action", "CreatedAt", "Description", "IsActive", "Module", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { 8, "View", new DateTime(2025, 6, 4, 12, 50, 3, 406, DateTimeKind.Utc).AddTicks(3351), "Can view project list", true, "Projects", "View Projects", new DateTime(2025, 6, 4, 12, 50, 3, 406, DateTimeKind.Utc).AddTicks(3351) },
                    { 9, "Create", new DateTime(2025, 6, 4, 12, 50, 3, 406, DateTimeKind.Utc).AddTicks(3353), "Can create new projects", true, "Projects", "Create Project", new DateTime(2025, 6, 4, 12, 50, 3, 406, DateTimeKind.Utc).AddTicks(3353) },
                    { 10, "Edit", new DateTime(2025, 6, 4, 12, 50, 3, 406, DateTimeKind.Utc).AddTicks(3354), "Can edit project details", true, "Projects", "Edit Project", new DateTime(2025, 6, 4, 12, 50, 3, 406, DateTimeKind.Utc).AddTicks(3354) },
                    { 11, "Delete", new DateTime(2025, 6, 4, 12, 50, 3, 406, DateTimeKind.Utc).AddTicks(3356), "Can delete projects", true, "Projects", "Delete Project", new DateTime(2025, 6, 4, 12, 50, 3, 406, DateTimeKind.Utc).AddTicks(3356) }
                });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Id", "AssignedUserId", "Budget", "CreatedAt", "Description", "EndDate", "Name", "ProjectValue", "Region", "StartDate", "Status", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, 1, 5500000.00m, new DateTime(2025, 6, 4, 12, 50, 3, 406, DateTimeKind.Utc).AddTicks(3484), "Major highway development project in Muscat region", new DateTime(2024, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "Muscat Infrastructure Development", "Highway Construction", "Muscat", new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "In Progress", new DateTime(2025, 6, 4, 12, 50, 3, 406, DateTimeKind.Utc).AddTicks(3484) },
                    { 2, 1, 8200000.00m, new DateTime(2025, 6, 4, 12, 50, 3, 406, DateTimeKind.Utc).AddTicks(3489), "Limestone quarrying operations in Dhofar region", new DateTime(2025, 2, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "Dhofar Mining Operations", "Mining Operations", "Dhofar", new DateTime(2024, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Planning", new DateTime(2025, 6, 4, 12, 50, 3, 406, DateTimeKind.Utc).AddTicks(3490) }
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_Projects_AssignedUserId",
                table: "Projects",
                column: "AssignedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_Name",
                table: "Projects",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectSites_ProjectId",
                table: "ProjectSites",
                column: "ProjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectSites");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 11);

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
    }
}
