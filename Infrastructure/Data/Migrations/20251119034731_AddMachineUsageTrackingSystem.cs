using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddMachineUsageTrackingSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DrillBitType",
                table: "MaintenanceJobs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DrillBitsUsed",
                table: "MaintenanceJobs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DrillRodType",
                table: "MaintenanceJobs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DrillRodsUsed",
                table: "MaintenanceJobs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsServiceCompleted",
                table: "MaintenanceJobs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ShankType",
                table: "MaintenanceJobs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ShanksUsed",
                table: "MaintenanceJobs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CurrentDrifterServiceHours",
                table: "Machines",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CurrentEngineServiceHours",
                table: "Machines",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "DrifterServiceInterval",
                table: "Machines",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "EngineServiceInterval",
                table: "Machines",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastDrifterServiceDate",
                table: "Machines",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastEngineServiceDate",
                table: "Machines",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(5039), new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(5039) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(5045), new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(5046) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(5049), new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(5049) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(5051), new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(5052) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(5053), new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(5054) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(5056), new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(5056) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(5058), new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(5058) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(5060), new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(5068) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(5200), new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(5200) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(5208), new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(5208) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(5211), new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(5211) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(5214), new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(5214) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(5216), new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(5217) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(5219), new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(5219) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(5221), new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(5221) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(5224), new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(5224) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(5226), new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(5226) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(5229), new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(5229) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(5231), new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(5231) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 1,
                column: "GrantedAt",
                value: new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(5125));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 2,
                column: "GrantedAt",
                value: new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(5131));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 3,
                column: "GrantedAt",
                value: new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(5133));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 4,
                column: "GrantedAt",
                value: new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(5134));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 5,
                column: "GrantedAt",
                value: new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(5136));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 6,
                column: "GrantedAt",
                value: new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(5138));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 7,
                column: "GrantedAt",
                value: new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(5139));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 8,
                column: "GrantedAt",
                value: new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(5141));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 9,
                column: "GrantedAt",
                value: new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(5142));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(4692), new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(4696) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(4704), new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(4704) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(4707), new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(4708) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(4710), new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(4710) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(4712), new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(4713) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(4822), new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(4822) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(4825), new DateTime(2025, 11, 19, 3, 47, 30, 35, DateTimeKind.Utc).AddTicks(4826) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DrillBitType",
                table: "MaintenanceJobs");

            migrationBuilder.DropColumn(
                name: "DrillBitsUsed",
                table: "MaintenanceJobs");

            migrationBuilder.DropColumn(
                name: "DrillRodType",
                table: "MaintenanceJobs");

            migrationBuilder.DropColumn(
                name: "DrillRodsUsed",
                table: "MaintenanceJobs");

            migrationBuilder.DropColumn(
                name: "IsServiceCompleted",
                table: "MaintenanceJobs");

            migrationBuilder.DropColumn(
                name: "ShankType",
                table: "MaintenanceJobs");

            migrationBuilder.DropColumn(
                name: "ShanksUsed",
                table: "MaintenanceJobs");

            migrationBuilder.DropColumn(
                name: "CurrentDrifterServiceHours",
                table: "Machines");

            migrationBuilder.DropColumn(
                name: "CurrentEngineServiceHours",
                table: "Machines");

            migrationBuilder.DropColumn(
                name: "DrifterServiceInterval",
                table: "Machines");

            migrationBuilder.DropColumn(
                name: "EngineServiceInterval",
                table: "Machines");

            migrationBuilder.DropColumn(
                name: "LastDrifterServiceDate",
                table: "Machines");

            migrationBuilder.DropColumn(
                name: "LastEngineServiceDate",
                table: "Machines");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 13, 15, 32, 6, 72, DateTimeKind.Utc).AddTicks(2848), new DateTime(2025, 11, 13, 15, 32, 6, 72, DateTimeKind.Utc).AddTicks(2849) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 13, 15, 32, 6, 72, DateTimeKind.Utc).AddTicks(2856), new DateTime(2025, 11, 13, 15, 32, 6, 72, DateTimeKind.Utc).AddTicks(2856) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 13, 15, 32, 6, 72, DateTimeKind.Utc).AddTicks(2860), new DateTime(2025, 11, 13, 15, 32, 6, 72, DateTimeKind.Utc).AddTicks(2861) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 13, 15, 32, 6, 72, DateTimeKind.Utc).AddTicks(2863), new DateTime(2025, 11, 13, 15, 32, 6, 72, DateTimeKind.Utc).AddTicks(2864) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 13, 15, 32, 6, 72, DateTimeKind.Utc).AddTicks(2866), new DateTime(2025, 11, 13, 15, 32, 6, 72, DateTimeKind.Utc).AddTicks(2867) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 13, 15, 32, 6, 72, DateTimeKind.Utc).AddTicks(2869), new DateTime(2025, 11, 13, 15, 32, 6, 72, DateTimeKind.Utc).AddTicks(2870) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 13, 15, 32, 6, 72, DateTimeKind.Utc).AddTicks(2872), new DateTime(2025, 11, 13, 15, 32, 6, 72, DateTimeKind.Utc).AddTicks(2873) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 13, 15, 32, 6, 72, DateTimeKind.Utc).AddTicks(2875), new DateTime(2025, 11, 13, 15, 32, 6, 72, DateTimeKind.Utc).AddTicks(2876) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 13, 15, 32, 6, 72, DateTimeKind.Utc).AddTicks(3194), new DateTime(2025, 11, 13, 15, 32, 6, 72, DateTimeKind.Utc).AddTicks(3196) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 13, 15, 32, 6, 72, DateTimeKind.Utc).AddTicks(3219), new DateTime(2025, 11, 13, 15, 32, 6, 72, DateTimeKind.Utc).AddTicks(3220) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 13, 15, 32, 6, 72, DateTimeKind.Utc).AddTicks(3223), new DateTime(2025, 11, 13, 15, 32, 6, 72, DateTimeKind.Utc).AddTicks(3223) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 13, 15, 32, 6, 72, DateTimeKind.Utc).AddTicks(3226), new DateTime(2025, 11, 13, 15, 32, 6, 72, DateTimeKind.Utc).AddTicks(3227) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 13, 15, 32, 6, 72, DateTimeKind.Utc).AddTicks(3230), new DateTime(2025, 11, 13, 15, 32, 6, 72, DateTimeKind.Utc).AddTicks(3231) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 13, 15, 32, 6, 72, DateTimeKind.Utc).AddTicks(3234), new DateTime(2025, 11, 13, 15, 32, 6, 72, DateTimeKind.Utc).AddTicks(3234) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 13, 15, 32, 6, 72, DateTimeKind.Utc).AddTicks(3237), new DateTime(2025, 11, 13, 15, 32, 6, 72, DateTimeKind.Utc).AddTicks(3238) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 13, 15, 32, 6, 72, DateTimeKind.Utc).AddTicks(3241), new DateTime(2025, 11, 13, 15, 32, 6, 72, DateTimeKind.Utc).AddTicks(3241) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 13, 15, 32, 6, 72, DateTimeKind.Utc).AddTicks(3244), new DateTime(2025, 11, 13, 15, 32, 6, 72, DateTimeKind.Utc).AddTicks(3245) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 13, 15, 32, 6, 72, DateTimeKind.Utc).AddTicks(3247), new DateTime(2025, 11, 13, 15, 32, 6, 72, DateTimeKind.Utc).AddTicks(3248) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 13, 15, 32, 6, 72, DateTimeKind.Utc).AddTicks(3251), new DateTime(2025, 11, 13, 15, 32, 6, 72, DateTimeKind.Utc).AddTicks(3251) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 1,
                column: "GrantedAt",
                value: new DateTime(2025, 11, 13, 15, 32, 6, 72, DateTimeKind.Utc).AddTicks(3001));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 2,
                column: "GrantedAt",
                value: new DateTime(2025, 11, 13, 15, 32, 6, 72, DateTimeKind.Utc).AddTicks(3003));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 3,
                column: "GrantedAt",
                value: new DateTime(2025, 11, 13, 15, 32, 6, 72, DateTimeKind.Utc).AddTicks(3005));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 4,
                column: "GrantedAt",
                value: new DateTime(2025, 11, 13, 15, 32, 6, 72, DateTimeKind.Utc).AddTicks(3007));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 5,
                column: "GrantedAt",
                value: new DateTime(2025, 11, 13, 15, 32, 6, 72, DateTimeKind.Utc).AddTicks(3008));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 6,
                column: "GrantedAt",
                value: new DateTime(2025, 11, 13, 15, 32, 6, 72, DateTimeKind.Utc).AddTicks(3010));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 7,
                column: "GrantedAt",
                value: new DateTime(2025, 11, 13, 15, 32, 6, 72, DateTimeKind.Utc).AddTicks(3012));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 8,
                column: "GrantedAt",
                value: new DateTime(2025, 11, 13, 15, 32, 6, 72, DateTimeKind.Utc).AddTicks(3013));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 9,
                column: "GrantedAt",
                value: new DateTime(2025, 11, 13, 15, 32, 6, 72, DateTimeKind.Utc).AddTicks(3015));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 13, 15, 32, 6, 72, DateTimeKind.Utc).AddTicks(2306), new DateTime(2025, 11, 13, 15, 32, 6, 72, DateTimeKind.Utc).AddTicks(2311) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 13, 15, 32, 6, 72, DateTimeKind.Utc).AddTicks(2385), new DateTime(2025, 11, 13, 15, 32, 6, 72, DateTimeKind.Utc).AddTicks(2386) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 13, 15, 32, 6, 72, DateTimeKind.Utc).AddTicks(2389), new DateTime(2025, 11, 13, 15, 32, 6, 72, DateTimeKind.Utc).AddTicks(2390) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 13, 15, 32, 6, 72, DateTimeKind.Utc).AddTicks(2393), new DateTime(2025, 11, 13, 15, 32, 6, 72, DateTimeKind.Utc).AddTicks(2393) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 13, 15, 32, 6, 72, DateTimeKind.Utc).AddTicks(2396), new DateTime(2025, 11, 13, 15, 32, 6, 72, DateTimeKind.Utc).AddTicks(2396) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 13, 15, 32, 6, 72, DateTimeKind.Utc).AddTicks(2399), new DateTime(2025, 11, 13, 15, 32, 6, 72, DateTimeKind.Utc).AddTicks(2399) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 13, 15, 32, 6, 72, DateTimeKind.Utc).AddTicks(2402), new DateTime(2025, 11, 13, 15, 32, 6, 72, DateTimeKind.Utc).AddTicks(2402) });
        }
    }
}
