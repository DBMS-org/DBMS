using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveDrillPatternEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlastSequences_DrillPatterns_DrillPatternId",
                table: "BlastSequences");

            migrationBuilder.DropForeignKey(
                name: "FK_BlastSequences_ProjectSites_SiteId",
                table: "BlastSequences");

            migrationBuilder.DropForeignKey(
                name: "FK_BlastSequences_Users_CreatedById",
                table: "BlastSequences");

            migrationBuilder.DropForeignKey(
                name: "FK_DrillPoints_DrillPatterns_DrillPatternId",
                table: "DrillPoints");

            migrationBuilder.DropForeignKey(
                name: "FK_DrillPoints_ProjectSites_SiteId",
                table: "DrillPoints");

            migrationBuilder.DropTable(
                name: "DrillPatterns");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DrillPoints",
                table: "DrillPoints");

            migrationBuilder.DropIndex(
                name: "IX_DrillPoints_DrillPatternId",
                table: "DrillPoints");

            migrationBuilder.DropIndex(
                name: "IX_BlastSequences_CreatedById",
                table: "BlastSequences");

            migrationBuilder.DropIndex(
                name: "IX_BlastSequences_DrillPatternId",
                table: "BlastSequences");

            migrationBuilder.DropColumn(
                name: "DrillPatternId",
                table: "DrillPoints");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "BlastSequences");

            migrationBuilder.DropColumn(
                name: "DrillPatternId",
                table: "BlastSequences");

            migrationBuilder.AlterColumn<string>(
                name: "SimulationSettingsJson",
                table: "BlastSequences",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldDefaultValue: "{}");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "BlastSequences",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "BlastSequences",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AddColumn<double>(
                name: "DelayBetweenHoles",
                table: "BlastSequences",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "DelayBetweenRows",
                table: "BlastSequences",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DrillPoints",
                table: "DrillPoints",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6575), new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6576) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6581), new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6582) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6585), new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6585) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6587), new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6588) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6590), new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6590) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6592), new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6593) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6595), new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6595) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6597), new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6597) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6856), new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6857) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6879), new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6879) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6882), new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6883) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6885), new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6885) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6887), new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6888) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6890), new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6891) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6892), new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6893) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6895), new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6895) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6897), new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6897) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6899), new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6900) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6902), new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6902) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 1,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6684));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 2,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6698));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 3,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6700));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 4,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6702));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 5,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6703));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 6,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6705));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 7,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6706));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 8,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6708));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 9,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6709));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6153), new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6157) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6166), new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6167) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6169), new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6170) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6172), new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6173) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6175), new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6175) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6177), new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6178) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6180), new DateTime(2025, 7, 6, 6, 52, 22, 612, DateTimeKind.Utc).AddTicks(6180) });

            migrationBuilder.CreateIndex(
                name: "IX_DrillPoints_X_Y_ProjectId_SiteId",
                table: "DrillPoints",
                columns: new[] { "X", "Y", "ProjectId", "SiteId" });

            migrationBuilder.CreateIndex(
                name: "IX_BlastSequences_CreatedByUserId",
                table: "BlastSequences",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BlastSequences_IsActive",
                table: "BlastSequences",
                column: "IsActive");

            migrationBuilder.AddForeignKey(
                name: "FK_BlastSequences_ProjectSites_SiteId",
                table: "BlastSequences",
                column: "SiteId",
                principalTable: "ProjectSites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BlastSequences_Users_CreatedByUserId",
                table: "BlastSequences",
                column: "CreatedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DrillPoints_ProjectSites_SiteId",
                table: "DrillPoints",
                column: "SiteId",
                principalTable: "ProjectSites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlastSequences_ProjectSites_SiteId",
                table: "BlastSequences");

            migrationBuilder.DropForeignKey(
                name: "FK_BlastSequences_Users_CreatedByUserId",
                table: "BlastSequences");

            migrationBuilder.DropForeignKey(
                name: "FK_DrillPoints_ProjectSites_SiteId",
                table: "DrillPoints");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DrillPoints",
                table: "DrillPoints");

            migrationBuilder.DropIndex(
                name: "IX_DrillPoints_X_Y_ProjectId_SiteId",
                table: "DrillPoints");

            migrationBuilder.DropIndex(
                name: "IX_BlastSequences_CreatedByUserId",
                table: "BlastSequences");

            migrationBuilder.DropIndex(
                name: "IX_BlastSequences_IsActive",
                table: "BlastSequences");

            migrationBuilder.DropColumn(
                name: "DelayBetweenHoles",
                table: "BlastSequences");

            migrationBuilder.DropColumn(
                name: "DelayBetweenRows",
                table: "BlastSequences");

            migrationBuilder.AddColumn<int>(
                name: "DrillPatternId",
                table: "DrillPoints",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SimulationSettingsJson",
                table: "BlastSequences",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "{}",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "BlastSequences",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "BlastSequences",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "BlastSequences",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DrillPatternId",
                table: "BlastSequences",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DrillPoints",
                table: "DrillPoints",
                columns: new[] { "Id", "ProjectId", "SiteId" });

            migrationBuilder.CreateTable(
                name: "DrillPatterns",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    SiteId = table.Column<int>(type: "int", nullable: false),
                    Burden = table.Column<double>(type: "float", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Depth = table.Column<double>(type: "float", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Spacing = table.Column<double>(type: "float", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrillPatterns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DrillPatterns_ProjectSites_SiteId",
                        column: x => x.SiteId,
                        principalTable: "ProjectSites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DrillPatterns_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DrillPatterns_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7759), new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7759) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7763), new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7764) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7767), new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7767) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7769), new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7770) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7772), new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7773) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7775), new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7775) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7777), new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7778) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7780), new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7780) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7889), new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7889) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7898), new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7898) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7900), new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7901) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7903), new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7904) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7906), new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7906) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7908), new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7909) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7911), new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7911) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7913), new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7913) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7915), new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7915) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7917), new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7918) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7919), new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7920) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 1,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7823));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 2,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7827));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 3,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7829));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 4,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7831));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 5,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7832));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 6,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7834));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 7,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7835));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 8,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7837));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 9,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7838));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7441), new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7446) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7455), new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7455) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7458), new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7459) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7521), new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7521) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7523), new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7524) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7526), new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7526) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7528), new DateTime(2025, 7, 5, 10, 6, 2, 969, DateTimeKind.Utc).AddTicks(7528) });

            migrationBuilder.CreateIndex(
                name: "IX_DrillPoints_DrillPatternId",
                table: "DrillPoints",
                column: "DrillPatternId");

            migrationBuilder.CreateIndex(
                name: "IX_BlastSequences_CreatedById",
                table: "BlastSequences",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_BlastSequences_DrillPatternId",
                table: "BlastSequences",
                column: "DrillPatternId");

            migrationBuilder.CreateIndex(
                name: "IX_DrillPatterns_CreatedByUserId",
                table: "DrillPatterns",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_DrillPatterns_IsActive",
                table: "DrillPatterns",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_DrillPatterns_ProjectId_SiteId",
                table: "DrillPatterns",
                columns: new[] { "ProjectId", "SiteId" });

            migrationBuilder.CreateIndex(
                name: "IX_DrillPatterns_SiteId",
                table: "DrillPatterns",
                column: "SiteId");

            migrationBuilder.AddForeignKey(
                name: "FK_BlastSequences_DrillPatterns_DrillPatternId",
                table: "BlastSequences",
                column: "DrillPatternId",
                principalTable: "DrillPatterns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BlastSequences_ProjectSites_SiteId",
                table: "BlastSequences",
                column: "SiteId",
                principalTable: "ProjectSites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BlastSequences_Users_CreatedById",
                table: "BlastSequences",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DrillPoints_DrillPatterns_DrillPatternId",
                table: "DrillPoints",
                column: "DrillPatternId",
                principalTable: "DrillPatterns",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DrillPoints_ProjectSites_SiteId",
                table: "DrillPoints",
                column: "SiteId",
                principalTable: "ProjectSites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
