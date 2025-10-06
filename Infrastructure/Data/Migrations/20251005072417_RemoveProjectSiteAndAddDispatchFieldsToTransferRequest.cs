using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveProjectSiteAndAddDispatchFieldsToTransferRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryTransferRequests_ProjectSites_ProjectSiteId",
                table: "InventoryTransferRequests");

            migrationBuilder.RenameColumn(
                name: "ProjectSiteId",
                table: "InventoryTransferRequests",
                newName: "DispatchedByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_InventoryTransferRequests_ProjectSiteId",
                table: "InventoryTransferRequests",
                newName: "IX_InventoryTransferRequests_DispatchedByUserId");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeliveryConfirmedDate",
                table: "InventoryTransferRequests",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DispatchDate",
                table: "InventoryTransferRequests",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DispatchNotes",
                table: "InventoryTransferRequests",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DriverContactNumber",
                table: "InventoryTransferRequests",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DriverName",
                table: "InventoryTransferRequests",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TruckNumber",
                table: "InventoryTransferRequests",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(7161), new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(7162) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(7169), new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(7169) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(7174), new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(7175) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(7177), new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(7178) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(7181), new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(7181) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(7184), new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(7185) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(7187), new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(7188) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(7190), new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(7206) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(7421), new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(7422) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(7431), new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(7432) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(7435), new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(7436) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(7439), new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(7439) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(7442), new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(7443) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(7446), new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(7446) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(7451), new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(7451) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(7454), new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(7455) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(7457), new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(7458) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(7461), new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(7461) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(7464), new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(7465) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 1,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(7287));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 2,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(7292));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 3,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(7294));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 4,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(7296));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 5,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(7298));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 6,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(7300));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 7,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(7302));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 8,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(7304));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 9,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(7306));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(6755), new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(6759) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(6768), new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(6769) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(6773), new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(6773) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(6777), new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(6777) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(6780), new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(6780) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(6783), new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(6784) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(6786), new DateTime(2025, 10, 5, 7, 24, 16, 228, DateTimeKind.Utc).AddTicks(6787) });

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransferRequests_DispatchDate",
                table: "InventoryTransferRequests",
                column: "DispatchDate");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryTransferRequests_Users_DispatchedByUserId",
                table: "InventoryTransferRequests",
                column: "DispatchedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryTransferRequests_Users_DispatchedByUserId",
                table: "InventoryTransferRequests");

            migrationBuilder.DropIndex(
                name: "IX_InventoryTransferRequests_DispatchDate",
                table: "InventoryTransferRequests");

            migrationBuilder.DropColumn(
                name: "DeliveryConfirmedDate",
                table: "InventoryTransferRequests");

            migrationBuilder.DropColumn(
                name: "DispatchDate",
                table: "InventoryTransferRequests");

            migrationBuilder.DropColumn(
                name: "DispatchNotes",
                table: "InventoryTransferRequests");

            migrationBuilder.DropColumn(
                name: "DriverContactNumber",
                table: "InventoryTransferRequests");

            migrationBuilder.DropColumn(
                name: "DriverName",
                table: "InventoryTransferRequests");

            migrationBuilder.DropColumn(
                name: "TruckNumber",
                table: "InventoryTransferRequests");

            migrationBuilder.RenameColumn(
                name: "DispatchedByUserId",
                table: "InventoryTransferRequests",
                newName: "ProjectSiteId");

            migrationBuilder.RenameIndex(
                name: "IX_InventoryTransferRequests_DispatchedByUserId",
                table: "InventoryTransferRequests",
                newName: "IX_InventoryTransferRequests_ProjectSiteId");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(9036), new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(9036) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(9042), new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(9042) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(9045), new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(9046) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(9048), new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(9048) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(9050), new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(9050) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(9052), new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(9053) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(9055), new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(9055) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(9057), new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(9067) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(9232), new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(9232) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(9239), new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(9240) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(9242), new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(9243) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(9245), new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(9246) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(9248), new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(9248) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(9251), new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(9251) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(9253), new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(9254) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(9256), new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(9256) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(9258), new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(9258) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(9260), new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(9261) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(9263), new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(9263) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 1,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(9118));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 2,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(9122));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 3,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(9124));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 4,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(9126));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 5,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(9127));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 6,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(9129));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 7,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(9130));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 8,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(9131));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 9,
                column: "GrantedAt",
                value: new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(9133));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(8653), new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(8656) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(8663), new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(8664) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(8667), new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(8667) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(8670), new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(8670) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(8672), new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(8673) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(8674), new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(8675) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(8677), new DateTime(2025, 10, 4, 6, 23, 48, 246, DateTimeKind.Utc).AddTicks(8677) });

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryTransferRequests_ProjectSites_ProjectSiteId",
                table: "InventoryTransferRequests",
                column: "ProjectSiteId",
                principalTable: "ProjectSites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
