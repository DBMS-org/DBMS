using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMachineEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Machines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Model = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Manufacturer = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SerialNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RigNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PlateNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ChassisDetails = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ManufacturingYear = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CurrentLocation = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    AssignedToProject = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AssignedToOperator = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LastMaintenanceDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NextMaintenanceDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SpecificationsJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: true),
                    OperatorId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Machines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Machines_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Machines_Users_OperatorId",
                        column: x => x.OperatorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4371), new DateTime(2025, 6, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4371) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4376), new DateTime(2025, 6, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4376) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4380), new DateTime(2025, 6, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4380) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4383), new DateTime(2025, 6, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4383) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4386), new DateTime(2025, 6, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4386) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4388), new DateTime(2025, 6, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4389) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4391), new DateTime(2025, 6, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4391) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4394), new DateTime(2025, 6, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4394) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4396), new DateTime(2025, 6, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4397) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4399), new DateTime(2025, 6, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4399) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4401), new DateTime(2025, 6, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4402) });

            migrationBuilder.UpdateData(
                table: "ProjectSites",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4626), new DateTime(2025, 6, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4627) });

            migrationBuilder.UpdateData(
                table: "ProjectSites",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4632), new DateTime(2025, 6, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4633) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4562), new DateTime(2025, 6, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4563) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4570), new DateTime(2025, 6, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4570) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 12, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4576), new DateTime(2025, 5, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4588) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4117), new DateTime(2025, 6, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4120) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4127), new DateTime(2025, 6, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4128) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4131), new DateTime(2025, 6, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4131) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4134), new DateTime(2025, 6, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4134) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4136), new DateTime(2025, 6, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4137) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4139), new DateTime(2025, 6, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4139) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4141), new DateTime(2025, 6, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4142) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4504), new DateTime(2025, 6, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4504) });

            migrationBuilder.CreateIndex(
                name: "IX_Machines_Name",
                table: "Machines",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Machines_OperatorId",
                table: "Machines",
                column: "OperatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Machines_ProjectId",
                table: "Machines",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Machines_SerialNumber",
                table: "Machines",
                column: "SerialNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Machines_Status",
                table: "Machines",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Machines_Type",
                table: "Machines",
                column: "Type");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Machines");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2450), new DateTime(2025, 6, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2450) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2457), new DateTime(2025, 6, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2457) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2460), new DateTime(2025, 6, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2461) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2463), new DateTime(2025, 6, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2464) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2466), new DateTime(2025, 6, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2466) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2469), new DateTime(2025, 6, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2469) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2471), new DateTime(2025, 6, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2472) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2474), new DateTime(2025, 6, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2474) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2477), new DateTime(2025, 6, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2477) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2479), new DateTime(2025, 6, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2480) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2482), new DateTime(2025, 6, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2482) });

            migrationBuilder.UpdateData(
                table: "ProjectSites",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2649), new DateTime(2025, 6, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2650) });

            migrationBuilder.UpdateData(
                table: "ProjectSites",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2655), new DateTime(2025, 6, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2655) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2585), new DateTime(2025, 6, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2586) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2592), new DateTime(2025, 6, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2593) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 12, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2598), new DateTime(2025, 5, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2609) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2217), new DateTime(2025, 6, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2221) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2229), new DateTime(2025, 6, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2229) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2233), new DateTime(2025, 6, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2233) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2236), new DateTime(2025, 6, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2236) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2238), new DateTime(2025, 6, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2239) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2241), new DateTime(2025, 6, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2241) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2243), new DateTime(2025, 6, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2244) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2531), new DateTime(2025, 6, 22, 8, 8, 4, 546, DateTimeKind.Utc).AddTicks(2532) });
        }
    }
}
