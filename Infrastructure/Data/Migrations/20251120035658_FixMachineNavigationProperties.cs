using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixMachineNavigationProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Machines_Projects_ProjectId",
                table: "Machines");

            migrationBuilder.DropForeignKey(
                name: "FK_Machines_Users_OperatorId",
                table: "Machines");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Machines",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            // Convert string Status values to int enum values before changing column type
            // MachineStatus enum: 0=Available, 1=Assigned, 2=InMaintenance, 3=OutOfService, 4=UnderRepair, 5=Retired
            migrationBuilder.Sql(@"
                UPDATE Machines SET Status = '0' WHERE Status = 'Available';
                UPDATE Machines SET Status = '1' WHERE Status = 'Assigned' OR Status = 'ASSIGNED';
                UPDATE Machines SET Status = '2' WHERE Status IN ('Under Maintenance', 'UNDER_MAINTENANCE', 'InMaintenance', 'IN_MAINTENANCE');
                UPDATE Machines SET Status = '3' WHERE Status IN ('Out of Service', 'OUT_OF_SERVICE', 'OutOfService');
                UPDATE Machines SET Status = '4' WHERE Status IN ('Under Repair', 'UNDER_REPAIR', 'UnderRepair');
                UPDATE Machines SET Status = '5' WHERE Status IN ('Retired', 'RETIRED', 'Decommissioned', 'DECOMMISSIONED');
            ");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Machines",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "SerialNumber",
                table: "Machines",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "RigNo",
                table: "Machines",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PlateNo",
                table: "Machines",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Machines",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Model",
                table: "Machines",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Manufacturer",
                table: "Machines",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "Machines",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CurrentLocation",
                table: "Machines",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ChassisDetails",
                table: "Machines",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AssignedToProject",
                table: "Machines",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AssignedToOperator",
                table: "Machines",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 3, 56, 55, 551, DateTimeKind.Utc).AddTicks(9630), new DateTime(2025, 11, 20, 3, 56, 55, 551, DateTimeKind.Utc).AddTicks(9630) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 3, 56, 55, 551, DateTimeKind.Utc).AddTicks(9636), new DateTime(2025, 11, 20, 3, 56, 55, 551, DateTimeKind.Utc).AddTicks(9636) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 3, 56, 55, 551, DateTimeKind.Utc).AddTicks(9640), new DateTime(2025, 11, 20, 3, 56, 55, 551, DateTimeKind.Utc).AddTicks(9640) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 3, 56, 55, 551, DateTimeKind.Utc).AddTicks(9643), new DateTime(2025, 11, 20, 3, 56, 55, 551, DateTimeKind.Utc).AddTicks(9643) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 3, 56, 55, 551, DateTimeKind.Utc).AddTicks(9646), new DateTime(2025, 11, 20, 3, 56, 55, 551, DateTimeKind.Utc).AddTicks(9647) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 3, 56, 55, 551, DateTimeKind.Utc).AddTicks(9649), new DateTime(2025, 11, 20, 3, 56, 55, 551, DateTimeKind.Utc).AddTicks(9650) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 3, 56, 55, 551, DateTimeKind.Utc).AddTicks(9652), new DateTime(2025, 11, 20, 3, 56, 55, 551, DateTimeKind.Utc).AddTicks(9653) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 3, 56, 55, 551, DateTimeKind.Utc).AddTicks(9655), new DateTime(2025, 11, 20, 3, 56, 55, 551, DateTimeKind.Utc).AddTicks(9656) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 3, 56, 55, 552, DateTimeKind.Utc).AddTicks(104), new DateTime(2025, 11, 20, 3, 56, 55, 552, DateTimeKind.Utc).AddTicks(105) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 3, 56, 55, 552, DateTimeKind.Utc).AddTicks(125), new DateTime(2025, 11, 20, 3, 56, 55, 552, DateTimeKind.Utc).AddTicks(125) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 3, 56, 55, 552, DateTimeKind.Utc).AddTicks(129), new DateTime(2025, 11, 20, 3, 56, 55, 552, DateTimeKind.Utc).AddTicks(129) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 3, 56, 55, 552, DateTimeKind.Utc).AddTicks(132), new DateTime(2025, 11, 20, 3, 56, 55, 552, DateTimeKind.Utc).AddTicks(133) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 3, 56, 55, 552, DateTimeKind.Utc).AddTicks(136), new DateTime(2025, 11, 20, 3, 56, 55, 552, DateTimeKind.Utc).AddTicks(136) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 3, 56, 55, 552, DateTimeKind.Utc).AddTicks(139), new DateTime(2025, 11, 20, 3, 56, 55, 552, DateTimeKind.Utc).AddTicks(140) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 3, 56, 55, 552, DateTimeKind.Utc).AddTicks(143), new DateTime(2025, 11, 20, 3, 56, 55, 552, DateTimeKind.Utc).AddTicks(143) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 3, 56, 55, 552, DateTimeKind.Utc).AddTicks(146), new DateTime(2025, 11, 20, 3, 56, 55, 552, DateTimeKind.Utc).AddTicks(147) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 3, 56, 55, 552, DateTimeKind.Utc).AddTicks(149), new DateTime(2025, 11, 20, 3, 56, 55, 552, DateTimeKind.Utc).AddTicks(150) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 3, 56, 55, 552, DateTimeKind.Utc).AddTicks(153), new DateTime(2025, 11, 20, 3, 56, 55, 552, DateTimeKind.Utc).AddTicks(153) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 3, 56, 55, 552, DateTimeKind.Utc).AddTicks(156), new DateTime(2025, 11, 20, 3, 56, 55, 552, DateTimeKind.Utc).AddTicks(157) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 1,
                column: "GrantedAt",
                value: new DateTime(2025, 11, 20, 3, 56, 55, 551, DateTimeKind.Utc).AddTicks(9776));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 2,
                column: "GrantedAt",
                value: new DateTime(2025, 11, 20, 3, 56, 55, 551, DateTimeKind.Utc).AddTicks(9779));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 3,
                column: "GrantedAt",
                value: new DateTime(2025, 11, 20, 3, 56, 55, 551, DateTimeKind.Utc).AddTicks(9781));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 4,
                column: "GrantedAt",
                value: new DateTime(2025, 11, 20, 3, 56, 55, 551, DateTimeKind.Utc).AddTicks(9782));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 5,
                column: "GrantedAt",
                value: new DateTime(2025, 11, 20, 3, 56, 55, 551, DateTimeKind.Utc).AddTicks(9784));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 6,
                column: "GrantedAt",
                value: new DateTime(2025, 11, 20, 3, 56, 55, 551, DateTimeKind.Utc).AddTicks(9785));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 7,
                column: "GrantedAt",
                value: new DateTime(2025, 11, 20, 3, 56, 55, 551, DateTimeKind.Utc).AddTicks(9787));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 8,
                column: "GrantedAt",
                value: new DateTime(2025, 11, 20, 3, 56, 55, 551, DateTimeKind.Utc).AddTicks(9789));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 9,
                column: "GrantedAt",
                value: new DateTime(2025, 11, 20, 3, 56, 55, 551, DateTimeKind.Utc).AddTicks(9914));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 3, 56, 55, 551, DateTimeKind.Utc).AddTicks(9102), new DateTime(2025, 11, 20, 3, 56, 55, 551, DateTimeKind.Utc).AddTicks(9107) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 3, 56, 55, 551, DateTimeKind.Utc).AddTicks(9171), new DateTime(2025, 11, 20, 3, 56, 55, 551, DateTimeKind.Utc).AddTicks(9172) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 3, 56, 55, 551, DateTimeKind.Utc).AddTicks(9175), new DateTime(2025, 11, 20, 3, 56, 55, 551, DateTimeKind.Utc).AddTicks(9176) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 3, 56, 55, 551, DateTimeKind.Utc).AddTicks(9178), new DateTime(2025, 11, 20, 3, 56, 55, 551, DateTimeKind.Utc).AddTicks(9179) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 3, 56, 55, 551, DateTimeKind.Utc).AddTicks(9181), new DateTime(2025, 11, 20, 3, 56, 55, 551, DateTimeKind.Utc).AddTicks(9182) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 3, 56, 55, 551, DateTimeKind.Utc).AddTicks(9184), new DateTime(2025, 11, 20, 3, 56, 55, 551, DateTimeKind.Utc).AddTicks(9185) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 3, 56, 55, 551, DateTimeKind.Utc).AddTicks(9187), new DateTime(2025, 11, 20, 3, 56, 55, 551, DateTimeKind.Utc).AddTicks(9188) });

            migrationBuilder.AddForeignKey(
                name: "FK_Machines_Projects_ProjectId",
                table: "Machines",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Machines_Users_OperatorId",
                table: "Machines",
                column: "OperatorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Machines_Projects_ProjectId",
                table: "Machines");

            migrationBuilder.DropForeignKey(
                name: "FK_Machines_Users_OperatorId",
                table: "Machines");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Machines",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Machines",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "SerialNumber",
                table: "Machines",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "RigNo",
                table: "Machines",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PlateNo",
                table: "Machines",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Machines",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Model",
                table: "Machines",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Manufacturer",
                table: "Machines",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "Machines",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CurrentLocation",
                table: "Machines",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ChassisDetails",
                table: "Machines",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AssignedToProject",
                table: "Machines",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AssignedToOperator",
                table: "Machines",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2733), new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2734) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2738), new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2739) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2741), new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2742) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2744), new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2744) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2746), new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2747) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2749), new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2749) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2751), new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2751) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2753), new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2760) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2890), new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2891) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2897), new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2898) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2901), new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2901) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2903), new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2904) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2964), new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2965) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2967), new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2967) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2969), new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2970) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2972), new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2972) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2974), new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2975) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2977), new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2977) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2979), new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2979) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 1,
                column: "GrantedAt",
                value: new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2820));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 2,
                column: "GrantedAt",
                value: new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2824));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 3,
                column: "GrantedAt",
                value: new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2826));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 4,
                column: "GrantedAt",
                value: new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2827));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 5,
                column: "GrantedAt",
                value: new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2829));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 6,
                column: "GrantedAt",
                value: new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2830));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 7,
                column: "GrantedAt",
                value: new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2831));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 8,
                column: "GrantedAt",
                value: new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2833));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 9,
                column: "GrantedAt",
                value: new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2834));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2497), new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2501) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2507), new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2508) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2511), new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2511) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2513), new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2514) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2516), new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2516) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2518), new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2518) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2520), new DateTime(2025, 11, 19, 4, 21, 36, 292, DateTimeKind.Utc).AddTicks(2521) });

            migrationBuilder.AddForeignKey(
                name: "FK_Machines_Projects_ProjectId",
                table: "Machines",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Machines_Users_OperatorId",
                table: "Machines",
                column: "OperatorId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
