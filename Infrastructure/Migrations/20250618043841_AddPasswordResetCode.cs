using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPasswordResetCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PasswordResetCodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsUsed = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AttemptCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PasswordResetCodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PasswordResetCodes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 18, 4, 38, 40, 460, DateTimeKind.Utc).AddTicks(4663), new DateTime(2025, 6, 18, 4, 38, 40, 460, DateTimeKind.Utc).AddTicks(4663) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 18, 4, 38, 40, 460, DateTimeKind.Utc).AddTicks(4669), new DateTime(2025, 6, 18, 4, 38, 40, 460, DateTimeKind.Utc).AddTicks(4669) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 18, 4, 38, 40, 460, DateTimeKind.Utc).AddTicks(4672), new DateTime(2025, 6, 18, 4, 38, 40, 460, DateTimeKind.Utc).AddTicks(4673) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 18, 4, 38, 40, 460, DateTimeKind.Utc).AddTicks(4675), new DateTime(2025, 6, 18, 4, 38, 40, 460, DateTimeKind.Utc).AddTicks(4675) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 18, 4, 38, 40, 460, DateTimeKind.Utc).AddTicks(4678), new DateTime(2025, 6, 18, 4, 38, 40, 460, DateTimeKind.Utc).AddTicks(4678) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 18, 4, 38, 40, 460, DateTimeKind.Utc).AddTicks(4681), new DateTime(2025, 6, 18, 4, 38, 40, 460, DateTimeKind.Utc).AddTicks(4681) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 18, 4, 38, 40, 460, DateTimeKind.Utc).AddTicks(4684), new DateTime(2025, 6, 18, 4, 38, 40, 460, DateTimeKind.Utc).AddTicks(4684) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 18, 4, 38, 40, 460, DateTimeKind.Utc).AddTicks(4687), new DateTime(2025, 6, 18, 4, 38, 40, 460, DateTimeKind.Utc).AddTicks(4687) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 18, 4, 38, 40, 460, DateTimeKind.Utc).AddTicks(4689), new DateTime(2025, 6, 18, 4, 38, 40, 460, DateTimeKind.Utc).AddTicks(4690) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 18, 4, 38, 40, 460, DateTimeKind.Utc).AddTicks(4692), new DateTime(2025, 6, 18, 4, 38, 40, 460, DateTimeKind.Utc).AddTicks(4692) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 18, 4, 38, 40, 460, DateTimeKind.Utc).AddTicks(4695), new DateTime(2025, 6, 18, 4, 38, 40, 460, DateTimeKind.Utc).AddTicks(4695) });

            migrationBuilder.UpdateData(
                table: "ProjectSites",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 18, 4, 38, 40, 460, DateTimeKind.Utc).AddTicks(4934), new DateTime(2025, 6, 18, 4, 38, 40, 460, DateTimeKind.Utc).AddTicks(4935) });

            migrationBuilder.UpdateData(
                table: "ProjectSites",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 18, 4, 38, 40, 460, DateTimeKind.Utc).AddTicks(4940), new DateTime(2025, 6, 18, 4, 38, 40, 460, DateTimeKind.Utc).AddTicks(4940) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 18, 4, 38, 40, 460, DateTimeKind.Utc).AddTicks(4809), new DateTime(2025, 6, 18, 4, 38, 40, 460, DateTimeKind.Utc).AddTicks(4810) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 18, 4, 38, 40, 460, DateTimeKind.Utc).AddTicks(4816), new DateTime(2025, 6, 18, 4, 38, 40, 460, DateTimeKind.Utc).AddTicks(4817) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 12, 18, 4, 38, 40, 460, DateTimeKind.Utc).AddTicks(4823), new DateTime(2025, 5, 18, 4, 38, 40, 460, DateTimeKind.Utc).AddTicks(4835) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 18, 4, 38, 40, 460, DateTimeKind.Utc).AddTicks(4409), new DateTime(2025, 6, 18, 4, 38, 40, 460, DateTimeKind.Utc).AddTicks(4412) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 18, 4, 38, 40, 460, DateTimeKind.Utc).AddTicks(4419), new DateTime(2025, 6, 18, 4, 38, 40, 460, DateTimeKind.Utc).AddTicks(4419) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 18, 4, 38, 40, 460, DateTimeKind.Utc).AddTicks(4422), new DateTime(2025, 6, 18, 4, 38, 40, 460, DateTimeKind.Utc).AddTicks(4423) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 18, 4, 38, 40, 460, DateTimeKind.Utc).AddTicks(4425), new DateTime(2025, 6, 18, 4, 38, 40, 460, DateTimeKind.Utc).AddTicks(4426) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 18, 4, 38, 40, 460, DateTimeKind.Utc).AddTicks(4745), new DateTime(2025, 6, 18, 4, 38, 40, 460, DateTimeKind.Utc).AddTicks(4745) });

            migrationBuilder.CreateIndex(
                name: "IX_PasswordResetCodes_ExpiresAt",
                table: "PasswordResetCodes",
                column: "ExpiresAt");

            migrationBuilder.CreateIndex(
                name: "IX_PasswordResetCodes_UserId_Code",
                table: "PasswordResetCodes",
                columns: new[] { "UserId", "Code" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PasswordResetCodes");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 13, 26, 55, 41, DateTimeKind.Utc).AddTicks(7885), new DateTime(2025, 6, 16, 13, 26, 55, 41, DateTimeKind.Utc).AddTicks(7886) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 13, 26, 55, 41, DateTimeKind.Utc).AddTicks(7891), new DateTime(2025, 6, 16, 13, 26, 55, 41, DateTimeKind.Utc).AddTicks(7892) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 13, 26, 55, 41, DateTimeKind.Utc).AddTicks(7895), new DateTime(2025, 6, 16, 13, 26, 55, 41, DateTimeKind.Utc).AddTicks(7895) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 13, 26, 55, 41, DateTimeKind.Utc).AddTicks(7897), new DateTime(2025, 6, 16, 13, 26, 55, 41, DateTimeKind.Utc).AddTicks(7898) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 13, 26, 55, 41, DateTimeKind.Utc).AddTicks(7901), new DateTime(2025, 6, 16, 13, 26, 55, 41, DateTimeKind.Utc).AddTicks(7901) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 13, 26, 55, 41, DateTimeKind.Utc).AddTicks(7903), new DateTime(2025, 6, 16, 13, 26, 55, 41, DateTimeKind.Utc).AddTicks(7904) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 13, 26, 55, 41, DateTimeKind.Utc).AddTicks(7906), new DateTime(2025, 6, 16, 13, 26, 55, 41, DateTimeKind.Utc).AddTicks(7907) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 13, 26, 55, 41, DateTimeKind.Utc).AddTicks(7909), new DateTime(2025, 6, 16, 13, 26, 55, 41, DateTimeKind.Utc).AddTicks(7909) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 13, 26, 55, 41, DateTimeKind.Utc).AddTicks(7911), new DateTime(2025, 6, 16, 13, 26, 55, 41, DateTimeKind.Utc).AddTicks(7912) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 13, 26, 55, 41, DateTimeKind.Utc).AddTicks(7914), new DateTime(2025, 6, 16, 13, 26, 55, 41, DateTimeKind.Utc).AddTicks(7914) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 13, 26, 55, 41, DateTimeKind.Utc).AddTicks(7916), new DateTime(2025, 6, 16, 13, 26, 55, 41, DateTimeKind.Utc).AddTicks(7917) });

            migrationBuilder.UpdateData(
                table: "ProjectSites",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 13, 26, 55, 41, DateTimeKind.Utc).AddTicks(8086), new DateTime(2025, 6, 16, 13, 26, 55, 41, DateTimeKind.Utc).AddTicks(8087) });

            migrationBuilder.UpdateData(
                table: "ProjectSites",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 13, 26, 55, 41, DateTimeKind.Utc).AddTicks(8092), new DateTime(2025, 6, 16, 13, 26, 55, 41, DateTimeKind.Utc).AddTicks(8093) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 13, 26, 55, 41, DateTimeKind.Utc).AddTicks(8022), new DateTime(2025, 6, 16, 13, 26, 55, 41, DateTimeKind.Utc).AddTicks(8022) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 13, 26, 55, 41, DateTimeKind.Utc).AddTicks(8029), new DateTime(2025, 6, 16, 13, 26, 55, 41, DateTimeKind.Utc).AddTicks(8030) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 12, 16, 13, 26, 55, 41, DateTimeKind.Utc).AddTicks(8036), new DateTime(2025, 5, 16, 13, 26, 55, 41, DateTimeKind.Utc).AddTicks(8047) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 13, 26, 55, 41, DateTimeKind.Utc).AddTicks(7567), new DateTime(2025, 6, 16, 13, 26, 55, 41, DateTimeKind.Utc).AddTicks(7574) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 13, 26, 55, 41, DateTimeKind.Utc).AddTicks(7581), new DateTime(2025, 6, 16, 13, 26, 55, 41, DateTimeKind.Utc).AddTicks(7581) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 13, 26, 55, 41, DateTimeKind.Utc).AddTicks(7584), new DateTime(2025, 6, 16, 13, 26, 55, 41, DateTimeKind.Utc).AddTicks(7585) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 13, 26, 55, 41, DateTimeKind.Utc).AddTicks(7587), new DateTime(2025, 6, 16, 13, 26, 55, 41, DateTimeKind.Utc).AddTicks(7588) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 16, 13, 26, 55, 41, DateTimeKind.Utc).AddTicks(7965), new DateTime(2025, 6, 16, 13, 26, 55, 41, DateTimeKind.Utc).AddTicks(7966) });
        }
    }
}
