using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FinalSchemaUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlastSequences_Users_CreatedByUserId",
                table: "BlastSequences");

            migrationBuilder.DropForeignKey(
                name: "FK_DrillPatterns_Users_CreatedByUserId",
                table: "DrillPatterns");

            migrationBuilder.DropIndex(
                name: "IX_UserRoles_UserId",
                table: "UserRoles");

            migrationBuilder.DropIndex(
                name: "IX_SiteBlastingData_ProjectId",
                table: "SiteBlastingData");

            migrationBuilder.DropIndex(
                name: "IX_RolePermissions_RoleId",
                table: "RolePermissions");

            migrationBuilder.DropIndex(
                name: "IX_DrillPatterns_CreatedByUserId",
                table: "DrillPatterns");

            migrationBuilder.DropIndex(
                name: "IX_DrillPatterns_ProjectId",
                table: "DrillPatterns");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DrillHoles",
                table: "DrillHoles");

            migrationBuilder.DropIndex(
                name: "IX_BlastSequences_CreatedByUserId",
                table: "BlastSequences");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "UserRoles",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "RolePermissions",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Regions",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "DrillPatterns",
                type: "int",
                nullable: false,
                defaultValue: 0);

            // Drop and recreate DrillHoles table to handle identity column issue
            migrationBuilder.DropTable("DrillHoles");
            
            migrationBuilder.CreateTable(
                name: "DrillHoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SerialNumber = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Easting = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Northing = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Elevation = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Length = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Depth = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Azimuth = table.Column<double>(type: "float", nullable: true),
                    Dip = table.Column<double>(type: "float", nullable: true),
                    ActualDepth = table.Column<double>(type: "float", nullable: false),
                    Stemming = table.Column<double>(type: "float", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    SiteId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrillHoles", x => x.Id);
                });

            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "BlastSequences",
                type: "int",
                nullable: false,
                defaultValue: 0);



            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6523), new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6525) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6538), new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6539) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6547), new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6548) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6554), new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6556) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6561), new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6562) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6567), new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6568) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6573), new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6574) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6579), new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6594) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6874), new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6876) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6891), new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6892) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6899), new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6900) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6906), new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6906) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6912), new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6913) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6918), new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6919) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6923), new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6924) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6929), new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6930) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6935), new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6935) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6940), new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6941) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6945), new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6946) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 1,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6711));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 2,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6719));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 3,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6723));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 4,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6727));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 5,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6730));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 6,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6733));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 7,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6737));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 8,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6740));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 9,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6743));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(5888), new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(5895) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(5911), new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(5911) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(5919), new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(5919) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(5926), new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(5927) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(5932), new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(5933) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(5939), new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(5940) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6016), new DateTime(2025, 7, 2, 12, 13, 37, 981, DateTimeKind.Utc).AddTicks(6018) });

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UserId_RoleId",
                table: "UserRoles",
                columns: new[] { "UserId", "RoleId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_RoleId_PermissionId",
                table: "RolePermissions",
                columns: new[] { "RoleId", "PermissionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DrillPatterns_CreatedById",
                table: "DrillPatterns",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_BlastSequences_CreatedById",
                table: "BlastSequences",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_BlastSequences_Users_CreatedById",
                table: "BlastSequences",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DrillPatterns_Users_CreatedById",
                table: "DrillPatterns",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlastSequences_Users_CreatedById",
                table: "BlastSequences");

            migrationBuilder.DropForeignKey(
                name: "FK_DrillPatterns_Users_CreatedById",
                table: "DrillPatterns");

            migrationBuilder.DropIndex(
                name: "IX_UserRoles_UserId_RoleId",
                table: "UserRoles");

            migrationBuilder.DropIndex(
                name: "IX_RolePermissions_RoleId_PermissionId",
                table: "RolePermissions");

            migrationBuilder.DropIndex(
                name: "IX_DrillPatterns_CreatedById",
                table: "DrillPatterns");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DrillHoles",
                table: "DrillHoles");

            migrationBuilder.DropIndex(
                name: "IX_BlastSequences_CreatedById",
                table: "BlastSequences");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "DrillPatterns");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "BlastSequences");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "UserRoles",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "RolePermissions",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Regions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SerialNumber",
                table: "DrillHoles",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<double>(
                name: "Northing",
                table: "DrillHoles",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)");

            migrationBuilder.AlterColumn<double>(
                name: "Length",
                table: "DrillHoles",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)");

            migrationBuilder.AlterColumn<double>(
                name: "Elevation",
                table: "DrillHoles",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)");

            migrationBuilder.AlterColumn<double>(
                name: "Easting",
                table: "DrillHoles",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)");

            migrationBuilder.AlterColumn<double>(
                name: "Depth",
                table: "DrillHoles",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "DrillHoles",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DrillHoles",
                table: "DrillHoles",
                column: "SerialNumber");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(4813), new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(4814) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(4820), new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(4820) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(4823), new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(4824) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(4826), new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(4826) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(4828), new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(4829) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(4831), new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(4831) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(4833), new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(4834) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(4836), new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(4836) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(4963), new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(4963) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(5035), new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(5035) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(5039), new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(5039) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(5041), new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(5042) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(5044), new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(5044) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(5047), new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(5047) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(5049), new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(5050) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(5052), new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(5052) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(5054), new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(5054) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(5056), new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(5057) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(5058), new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(5059) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 1,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(4893));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 2,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(4897));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 3,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(4899));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 4,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(4900));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 5,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(4902));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 6,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(4903));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 7,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(4906));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 8,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(4908));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 9,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(4910));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(4438), new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(4442) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(4449), new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(4450) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(4453), new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(4453) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(4455), new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(4456) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(4458), new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(4458) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(4460), new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(4460) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(4462), new DateTime(2025, 7, 2, 10, 48, 0, 757, DateTimeKind.Utc).AddTicks(4463) });

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UserId",
                table: "UserRoles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SiteBlastingData_ProjectId",
                table: "SiteBlastingData",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_RoleId",
                table: "RolePermissions",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_DrillPatterns_CreatedByUserId",
                table: "DrillPatterns",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_DrillPatterns_ProjectId",
                table: "DrillPatterns",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_BlastSequences_CreatedByUserId",
                table: "BlastSequences",
                column: "CreatedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BlastSequences_Users_CreatedByUserId",
                table: "BlastSequences",
                column: "CreatedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DrillPatterns_Users_CreatedByUserId",
                table: "DrillPatterns",
                column: "CreatedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
