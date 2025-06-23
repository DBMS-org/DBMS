using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AlignSchemaWithCodeModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Machines_Projects_ProjectId",
                table: "Machines");

            migrationBuilder.DropForeignKey(
                name: "FK_Machines_Regions_RegionId",
                table: "Machines");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Regions_RegionId",
                table: "Projects");

            // Update any machines with NULL ProjectId to reference the first available project (safety measure)
            migrationBuilder.Sql(@"
                UPDATE Machines 
                SET ProjectId = (SELECT TOP 1 Id FROM Projects WHERE Projects.Id IS NOT NULL ORDER BY Id)
                WHERE ProjectId IS NULL AND EXISTS (SELECT 1 FROM Projects)");

            migrationBuilder.AlterColumn<int>(
                name: "ProjectId",
                table: "Machines",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3279), new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3279) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3289), new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3290) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3293), new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3293) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3296), new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3296) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3298), new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3299) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3301), new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3301) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3303), new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3304) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3306), new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3306) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3308), new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3309) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3311), new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3311) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3313), new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3314) });

            migrationBuilder.UpdateData(
                table: "ProjectSites",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3593), new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3594) });

            migrationBuilder.UpdateData(
                table: "ProjectSites",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3598), new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3599) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3442), new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3443) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3451), new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3451) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 12, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3458), new DateTime(2025, 5, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3475) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3643), new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3644) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3648), new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3649) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3652), new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3653) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3656), new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3656) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3659), new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3660) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3663), new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3663) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3666), new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3667) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3670), new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3670) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3673), new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3674) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3676), new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3677) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3680), new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3680) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(2974), new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(2977) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(2986), new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(2986) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(2990), new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(2990) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(2992), new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(2993) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(2995), new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(2995) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(2997), new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(2998) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3000), new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3001) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3368), new DateTime(2025, 6, 23, 15, 57, 52, 627, DateTimeKind.Utc).AddTicks(3369) });

            migrationBuilder.AddForeignKey(
                name: "FK_Machines_Projects_ProjectId",
                table: "Machines",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Machines_Regions_RegionId",
                table: "Machines",
                column: "RegionId",
                principalTable: "Regions",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Regions_RegionId",
                table: "Projects",
                column: "RegionId",
                principalTable: "Regions",
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
                name: "FK_Machines_Regions_RegionId",
                table: "Machines");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Regions_RegionId",
                table: "Projects");

            migrationBuilder.AlterColumn<int>(
                name: "ProjectId",
                table: "Machines",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(5832), new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(5832) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(5837), new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(5838) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(5934), new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(5935) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(5938), new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(5938) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(5941), new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(5941) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(5944), new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(5944) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(5946), new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(5947) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(5949), new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(5949) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(5952), new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(5952) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(5954), new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(5954) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(5957), new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(5957) });

            migrationBuilder.UpdateData(
                table: "ProjectSites",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(6146), new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(6147) });

            migrationBuilder.UpdateData(
                table: "ProjectSites",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(6152), new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(6153) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(6078), new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(6078) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(6085), new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(6086) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 12, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(6091), new DateTime(2025, 5, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(6102) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(6189), new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(6190) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(6194), new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(6194) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(6198), new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(6198) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(6201), new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(6202) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(6205), new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(6205) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(6208), new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(6209) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(6212), new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(6212) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(6215), new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(6216) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(6219), new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(6219) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(6222), new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(6223) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(6226), new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(6226) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(5529), new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(5535) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(5542), new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(5542) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(5545), new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(5546) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(5548), new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(5549) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(5551), new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(5551) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(5554), new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(5554) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(5556), new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(5557) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(6008), new DateTime(2025, 6, 23, 14, 46, 16, 470, DateTimeKind.Utc).AddTicks(6008) });

            migrationBuilder.AddForeignKey(
                name: "FK_Machines_Projects_ProjectId",
                table: "Machines",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Machines_Regions_RegionId",
                table: "Machines",
                column: "RegionId",
                principalTable: "Regions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Regions_RegionId",
                table: "Projects",
                column: "RegionId",
                principalTable: "Regions",
                principalColumn: "Id");
        }
    }
}
