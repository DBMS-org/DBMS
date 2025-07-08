using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DropBlastSequencesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlastSequences");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7388), new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7389) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7398), new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7398) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7401), new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7401) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7403), new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7404) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7405), new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7406) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7408), new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7408) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7410), new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7411) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7412), new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7413) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7572), new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7573) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7584), new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7585) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7587), new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7588) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7590), new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7590) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7592), new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7593) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7595), new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7595) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7597), new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7598) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7600), new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7600) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7602), new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7602) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7604), new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7605) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7607), new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7607) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 1,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7473));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 2,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7481));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 3,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7483));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 4,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7484));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 5,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7486));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 6,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7487));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 7,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7488));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 8,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7490));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 9,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7491));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(6999), new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7006) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7016), new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7017) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7020), new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7020) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7022), new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7023) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7025), new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7025) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7027), new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7027) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7029), new DateTime(2025, 7, 7, 8, 26, 56, 414, DateTimeKind.Utc).AddTicks(7030) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BlastSequences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    SiteId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DelayBetweenHoles = table.Column<double>(type: "float", nullable: false),
                    DelayBetweenRows = table.Column<double>(type: "float", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SimulationSettingsJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlastSequences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlastSequences_ProjectSites_SiteId",
                        column: x => x.SiteId,
                        principalTable: "ProjectSites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BlastSequences_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BlastSequences_Users_CreatedByUserId",
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
                values: new object[] { new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(986), new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(987) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(1000), new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(1001) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(1007), new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(1008) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(1013), new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(1014) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(1020), new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(1021) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(1026), new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(1027) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(1032), new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(1033) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(1038), new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(1038) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(1296), new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(1297) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(1312), new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(1313) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(1319), new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(1320) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(1325), new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(1325) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(1330), new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(1331) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(1336), new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(1337) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(1342), new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(1343) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(1347), new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(1348) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(1353), new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(1353) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(1358), new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(1359) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(1364), new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(1364) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 1,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(1137));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 2,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(1144));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 3,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(1147));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 4,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(1150));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 5,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(1153));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 6,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(1156));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 7,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(1159));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 8,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(1163));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 9,
                column: "GrantedAt",
                value: new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(1166));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(356), new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(364) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(378), new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(379) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(385), new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(386) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(392), new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(393) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(398), new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(399) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(404), new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(405) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(410), new DateTime(2025, 7, 6, 15, 20, 0, 966, DateTimeKind.Utc).AddTicks(411) });

            migrationBuilder.CreateIndex(
                name: "IX_BlastSequences_CreatedByUserId",
                table: "BlastSequences",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BlastSequences_IsActive",
                table: "BlastSequences",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_BlastSequences_ProjectId_SiteId",
                table: "BlastSequences",
                columns: new[] { "ProjectId", "SiteId" });

            migrationBuilder.CreateIndex(
                name: "IX_BlastSequences_SiteId",
                table: "BlastSequences",
                column: "SiteId");
        }
    }
}
