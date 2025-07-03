using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NewMigration : Migration
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

            migrationBuilder.DropIndex(
                name: "IX_Regions_IsActive",
                table: "Regions");

            migrationBuilder.DropIndex(
                name: "IX_PasswordResetCodes_ExpiresAt",
                table: "PasswordResetCodes");

            migrationBuilder.DropIndex(
                name: "IX_PasswordResetCodes_UserId_Code",
                table: "PasswordResetCodes");

            migrationBuilder.DropIndex(
                name: "IX_Machines_Name",
                table: "Machines");

            migrationBuilder.DropIndex(
                name: "IX_Machines_SerialNumber",
                table: "Machines");

            migrationBuilder.DropIndex(
                name: "IX_Machines_Type",
                table: "Machines");

            migrationBuilder.DropIndex(
                name: "IX_BlastSequences_IsActive",
                table: "BlastSequences");

            migrationBuilder.DropIndex(
                name: "IX_BlastSequences_ProjectId",
                table: "BlastSequences");

            migrationBuilder.DropIndex(
                name: "IX_Regions_Name",
                table: "Regions");

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

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Regions",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Regions",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Regions",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Regions",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<bool>(
                name: "IsUsed",
                table: "PasswordResetCodes",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "PasswordResetCodes",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(6)",
                oldMaxLength: 6);

            migrationBuilder.AlterColumn<int>(
                name: "AttemptCount",
                table: "PasswordResetCodes",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "PasswordResetCodes",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Machines",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Machines",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BlastConnections",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    SiteId = table.Column<int>(type: "int", nullable: false),
                    FromHoleId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ToHoleId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConnectorType = table.Column<int>(type: "int", nullable: false),
                    Delay = table.Column<int>(type: "int", nullable: false),
                    Sequence = table.Column<int>(type: "int", nullable: false),
                    StartPointJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EndPointJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlastConnections", x => new { x.Id, x.ProjectId, x.SiteId });
                    table.ForeignKey(
                        name: "FK_BlastConnections_ProjectSites_SiteId",
                        column: x => x.SiteId,
                        principalTable: "ProjectSites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BlastConnections_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DetonatorInfos",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    SiteId = table.Column<int>(type: "int", nullable: false),
                    HoleId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Delay = table.Column<int>(type: "int", nullable: false),
                    Sequence = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetonatorInfos", x => new { x.Id, x.ProjectId, x.SiteId });
                    table.ForeignKey(
                        name: "FK_DetonatorInfos_ProjectSites_SiteId",
                        column: x => x.SiteId,
                        principalTable: "ProjectSites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DetonatorInfos_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DrillPoints",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    SiteId = table.Column<int>(type: "int", nullable: false),
                    X = table.Column<double>(type: "float(18)", precision: 18, scale: 2, nullable: false),
                    Y = table.Column<double>(type: "float(18)", precision: 18, scale: 2, nullable: false),
                    Depth = table.Column<double>(type: "float(18)", precision: 18, scale: 2, nullable: false),
                    Spacing = table.Column<double>(type: "float(18)", precision: 18, scale: 2, nullable: false),
                    Burden = table.Column<double>(type: "float(18)", precision: 18, scale: 2, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrillPoints", x => new { x.Id, x.ProjectId, x.SiteId });
                    table.ForeignKey(
                        name: "FK_DrillPoints_ProjectSites_SiteId",
                        column: x => x.SiteId,
                        principalTable: "ProjectSites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DrillPoints_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PatternSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    SiteId = table.Column<int>(type: "int", nullable: false),
                    Spacing = table.Column<double>(type: "float", nullable: false),
                    Burden = table.Column<double>(type: "float", nullable: false),
                    Depth = table.Column<double>(type: "float(18)", precision: 18, scale: 2, nullable: false, defaultValue: 10.0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatternSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PatternSettings_ProjectSites_SiteId",
                        column: x => x.SiteId,
                        principalTable: "ProjectSites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatternSettings_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Action", "CreatedAt", "Description", "Module", "Name", "UpdatedAt" },
                values: new object[] { "Create", new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(8894), "Allows creating a new user", "UserManagement", "Create User", new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(8895) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Action", "CreatedAt", "Description", "Module", "Name", "UpdatedAt" },
                values: new object[] { "Read", new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(8902), "Allows viewing user details", "UserManagement", "Read User", new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(8902) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Action", "CreatedAt", "Description", "Module", "Name", "UpdatedAt" },
                values: new object[] { "Update", new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(8906), "Allows editing user details", "UserManagement", "Update User", new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(8907) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "Description", "Module", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(8910), "Allows deleting a user", "UserManagement", new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(8910) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Action", "CreatedAt", "Description", "Module", "Name", "UpdatedAt" },
                values: new object[] { "Create", new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(8913), "Allows creating a new project", "ProjectManagement", "Create Project", new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(8914) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Action", "CreatedAt", "Description", "Module", "Name", "UpdatedAt" },
                values: new object[] { "Read", new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(8916), "Allows viewing project details", "ProjectManagement", "Read Project", new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(8917) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Action", "CreatedAt", "Description", "Module", "Name", "UpdatedAt" },
                values: new object[] { "Update", new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(8920), "Allows editing project details", "ProjectManagement", "Update Project", new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(8921) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Action", "CreatedAt", "Description", "Module", "Name", "UpdatedAt" },
                values: new object[] { "Delete", new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(8923), "Allows deleting a project", "ProjectManagement", "Delete Project", new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(8924) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Country", "CreatedAt", "Description", "UpdatedAt" },
                values: new object[] { "Oman", new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(9071), null, new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(9072) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Country", "CreatedAt", "Description", "UpdatedAt" },
                values: new object[] { "Oman", new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(9079), null, new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(9079) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Country", "CreatedAt", "Description", "UpdatedAt" },
                values: new object[] { "Oman", new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(9082), null, new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(9083) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Country", "CreatedAt", "Description", "UpdatedAt" },
                values: new object[] { "Oman", new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(9085), null, new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(9085) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Country", "CreatedAt", "Description", "Name", "UpdatedAt" },
                values: new object[] { "Oman", new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(9087), null, "Ad Dakhiliyah", new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(9088) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Country", "CreatedAt", "Description", "Name", "UpdatedAt" },
                values: new object[] { "Oman", new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(9090), null, "Al Batinah North", new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(9091) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Country", "CreatedAt", "Description", "Name", "UpdatedAt" },
                values: new object[] { "Oman", new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(9093), null, "Al Batinah South", new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(9094) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Country", "CreatedAt", "Description", "Name", "UpdatedAt" },
                values: new object[] { "Oman", new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(9096), null, "Ash Sharqiyah South", new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(9097) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Country", "CreatedAt", "Description", "Name", "UpdatedAt" },
                values: new object[] { "Oman", new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(9099), null, "Ash Sharqiyah North", new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(9099) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Country", "CreatedAt", "Description", "Name", "UpdatedAt" },
                values: new object[] { "Oman", new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(9102), null, "Ad Dhahirah", new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(9102) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "Country", "CreatedAt", "Description", "Name", "UpdatedAt" },
                values: new object[] { "Oman", new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(9104), null, "Al Wusta", new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(9105) });

            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "Id", "GrantedAt", "IsActive", "PermissionId", "RevokedAt", "RoleId" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(8993), true, 1, null, 1 },
                    { 2, new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(8998), true, 2, null, 1 },
                    { 3, new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(9000), true, 3, null, 1 },
                    { 4, new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(9002), true, 4, null, 1 },
                    { 5, new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(9004), true, 5, null, 1 },
                    { 6, new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(9006), true, 6, null, 1 },
                    { 7, new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(9008), true, 7, null, 1 },
                    { 8, new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(9010), true, 8, null, 1 },
                    { 9, new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(9012), true, 6, null, 2 }
                });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(8333), new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(8338) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "Description", "Name", "NormalizedName", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(8347), "Manages blasting operations", "Blasting Engineer", "BLASTING_ENGINEER", new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(8347) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "Description", "Name", "NormalizedName", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(8351), "Manages mechanical tasks", "Mechanical Engineer", "MECHANICAL_ENGINEER", new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(8352) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "Description", "Name", "NormalizedName", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(8355), "Manages machine inventory and assignments", "Machine Manager", "MACHINE_MANAGER", new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(8355) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "Description", "Name", "NormalizedName", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(8358), "Manages explosive materials", "Explosive Manager", "EXPLOSIVE_MANAGER", new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(8359) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "Description", "Name", "NormalizedName", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(8361), "Manages store inventory", "Store Manager", "STORE_MANAGER", new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(8362) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "Description", "Name", "NormalizedName", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(8364), "Operates machinery", "Operator", "OPERATOR", new DateTime(2025, 7, 2, 8, 47, 46, 346, DateTimeKind.Utc).AddTicks(8365) });

            migrationBuilder.CreateIndex(
                name: "IX_PasswordResetCodes_Email",
                table: "PasswordResetCodes",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_PasswordResetCodes_UserId",
                table: "PasswordResetCodes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BlastConnections_ProjectId",
                table: "BlastConnections",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_BlastConnections_SiteId",
                table: "BlastConnections",
                column: "SiteId");

            migrationBuilder.CreateIndex(
                name: "IX_DetonatorInfos_ProjectId",
                table: "DetonatorInfos",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_DetonatorInfos_SiteId",
                table: "DetonatorInfos",
                column: "SiteId");

            migrationBuilder.CreateIndex(
                name: "IX_DrillPoints_ProjectId_SiteId",
                table: "DrillPoints",
                columns: new[] { "ProjectId", "SiteId" });

            migrationBuilder.CreateIndex(
                name: "IX_DrillPoints_SiteId",
                table: "DrillPoints",
                column: "SiteId");

            migrationBuilder.CreateIndex(
                name: "IX_PatternSettings_ProjectId",
                table: "PatternSettings",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_PatternSettings_SiteId",
                table: "PatternSettings",
                column: "SiteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Machines_Projects_ProjectId",
                table: "Machines",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Machines_Users_OperatorId",
                table: "Machines",
                column: "OperatorId",
                principalTable: "Users",
                principalColumn: "Id");
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

            migrationBuilder.DropTable(
                name: "BlastConnections");

            migrationBuilder.DropTable(
                name: "DetonatorInfos");

            migrationBuilder.DropTable(
                name: "DrillPoints");

            migrationBuilder.DropTable(
                name: "PatternSettings");

            migrationBuilder.DropIndex(
                name: "IX_PasswordResetCodes_Email",
                table: "PasswordResetCodes");

            migrationBuilder.DropIndex(
                name: "IX_PasswordResetCodes_UserId",
                table: "PasswordResetCodes");

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Regions");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "PasswordResetCodes");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Machines");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Regions",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Regions",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Regions",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<bool>(
                name: "IsUsed",
                table: "PasswordResetCodes",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "PasswordResetCodes",
                type: "nvarchar(6)",
                maxLength: 6,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<int>(
                name: "AttemptCount",
                table: "PasswordResetCodes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Machines",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Action", "CreatedAt", "Description", "Module", "Name", "UpdatedAt" },
                values: new object[] { "View", new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(1793), "Can view user list", "Users", "View Users", new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(1793) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Action", "CreatedAt", "Description", "Module", "Name", "UpdatedAt" },
                values: new object[] { "Create", new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(1798), "Can create new users", "Users", "Create User", new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(1799) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Action", "CreatedAt", "Description", "Module", "Name", "UpdatedAt" },
                values: new object[] { "Edit", new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(1802), "Can edit user details", "Users", "Edit User", new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(1802) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "Description", "Module", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(1805), "Can delete users", "Users", new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(1805) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Action", "CreatedAt", "Description", "Module", "Name", "UpdatedAt" },
                values: new object[] { "View", new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(1808), "Can view dashboard", "Dashboard", "View Dashboard", new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(1808) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Action", "CreatedAt", "Description", "Module", "Name", "UpdatedAt" },
                values: new object[] { "Upload", new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(1810), "Can upload CSV files", "DrillHole", "Upload CSV", new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(1811) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Action", "CreatedAt", "Description", "Module", "Name", "UpdatedAt" },
                values: new object[] { "View", new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(1813), "Can view drill holes", "DrillHole", "View DrillHoles", new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(1813) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Action", "CreatedAt", "Description", "Module", "Name", "UpdatedAt" },
                values: new object[] { "View", new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(1816), "Can view project list", "Projects", "View Projects", new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(1816) });

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "Action", "CreatedAt", "Description", "IsActive", "Module", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { 9, "Create", new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(1818), "Can create new projects", true, "Projects", "Create Project", new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(1818) },
                    { 10, "Edit", new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(1821), "Can edit project details", true, "Projects", "Edit Project", new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(1821) },
                    { 11, "Delete", new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(1823), "Can delete projects", true, "Projects", "Delete Project", new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(1823) }
                });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Description", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(2051), "Capital Governorate", new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(2052) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "Description", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(2056), "Southern Governorate", new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(2057) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "Description", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(2060), "Northern Governorate", new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(2061) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "Description", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(2064), "Western Governorate", new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(2064) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "Description", "Name", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(2067), "Interior Governorate", "Al Dakhiliyah", new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(2068) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "Description", "Name", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(2071), "Al Dhahirah Governorate", "Al Dhahirah", new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(2071) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "Description", "Name", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(2074), "Central Governorate", "Al Wusta", new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(2075) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "Description", "Name", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(2077), "Northern Batinah Governorate", "Al Batinah North", new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(2078) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "Description", "Name", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(2081), "Southern Batinah Governorate", "Al Batinah South", new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(2081) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "Description", "Name", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(2084), "Northern Sharqiyah Governorate", "Ash Sharqiyah North", new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(2085) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "Description", "Name", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(2087), "Southern Sharqiyah Governorate", "Ash Sharqiyah South", new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(2088) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(1487), new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(1490) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "Description", "Name", "NormalizedName", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(1497), "Blasting Engineer with technical access", "BlastingEngineer", "BLASTINGENGINEER", new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(1497) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "Description", "Name", "NormalizedName", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(1501), "Mechanical Engineer with analysis and design access", "MechanicalEngineer", "MECHANICALENGINEER", new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(1501) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "Description", "Name", "NormalizedName", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(1503), "Operator with operational access", "Operator", "OPERATOR", new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(1504) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "Description", "Name", "NormalizedName", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(1509), "Machine Manager with machinery operational access", "MachineManager", "MACHINEMANAGER", new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(1509) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "Description", "Name", "NormalizedName", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(1540), "Explosive Manager with explosives handling and safety access", "ExplosiveManager", "EXPLOSIVEMANAGER", new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(1541) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "Description", "Name", "NormalizedName", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(1544), "Store Manager with inventory and supply chain access", "StoreManager", "STOREMANAGER", new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(1545) });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Country", "CountryPhone", "CreatedAt", "Email", "LastLoginAt", "Name", "OmanPhone", "PasswordHash", "PasswordResetCode", "PasswordResetCodeExpiry", "Region", "Role", "Status", "UpdatedAt" },
                values: new object[] { 1, "Oman", "+968 9999 9999", new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(1875), "admin@dbms.com", null, "System Administrator", "+968 9999 9999", "$2a$11$K8QQfR6Z5j6XgkHjWo9xXeNqO7QDj9qQVvjBZjR8g1jzQzKL9Yd3W", null, null, "Muscat", "Admin", "Active", new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(1875) });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Id", "AssignedUserId", "CreatedAt", "Description", "EndDate", "Name", "Region", "RegionId", "StartDate", "Status", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(1940), "Major highway development project in Muscat region", new DateTime(2024, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "Muscat Infrastructure Development - Highway Construction", "Muscat", null, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Active", new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(1941) },
                    { 2, 1, new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(1947), "Mining site expansion project in Dhofar region", new DateTime(2024, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Dhofar Mining Operations - Site Development", "Dhofar", null, new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Active", new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(1948) },
                    { 3, 1, new DateTime(2025, 1, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(1954), "Industrial zone construction project in Sohar", new DateTime(2023, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sohar Industrial Zone - Development", "Al Batinah North", null, new DateTime(2023, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Completed", new DateTime(2025, 6, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(1963) }
                });

            migrationBuilder.InsertData(
                table: "ProjectSites",
                columns: new[] { "Id", "Coordinates", "CreatedAt", "Description", "IsOperatorCompleted", "IsPatternApproved", "IsSimulationConfirmed", "Location", "Name", "ProjectId", "Status", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "{\"Latitude\":23.5880,\"Longitude\":58.3829}", new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(2006), "Primary construction site for highway project", false, false, false, "Muscat Highway Junction", "Muscat Main Site", 1, "Active", new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(2007) },
                    { 2, "{\"Latitude\":17.0194,\"Longitude\":54.1085}", new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(2013), "Main mining pit for ore extraction", false, false, false, "Dhofar Mining Area", "Dhofar Mining Pit A", 2, "Active", new DateTime(2025, 7, 1, 8, 50, 51, 527, DateTimeKind.Utc).AddTicks(2014) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Regions_IsActive",
                table: "Regions",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_PasswordResetCodes_ExpiresAt",
                table: "PasswordResetCodes",
                column: "ExpiresAt");

            migrationBuilder.CreateIndex(
                name: "IX_PasswordResetCodes_UserId_Code",
                table: "PasswordResetCodes",
                columns: new[] { "UserId", "Code" });

            migrationBuilder.CreateIndex(
                name: "IX_Machines_Name",
                table: "Machines",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Machines_SerialNumber",
                table: "Machines",
                column: "SerialNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Machines_Type",
                table: "Machines",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_BlastSequences_IsActive",
                table: "BlastSequences",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_BlastSequences_ProjectId",
                table: "BlastSequences",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Machines_Projects_ProjectId",
                table: "Machines",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Machines_Users_OperatorId",
                table: "Machines",
                column: "OperatorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
