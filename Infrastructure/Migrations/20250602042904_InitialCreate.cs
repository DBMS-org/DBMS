using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DrillHoles",
                columns: table => new
                {
                    SerialNumber = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Easting = table.Column<double>(type: "float", nullable: false),
                    Northing = table.Column<double>(type: "float", nullable: false),
                    Elevation = table.Column<double>(type: "float", nullable: false),
                    Length = table.Column<double>(type: "float", nullable: false),
                    Depth = table.Column<double>(type: "float", nullable: false),
                    Azimuth = table.Column<double>(type: "float", nullable: false),
                    Dip = table.Column<double>(type: "float", nullable: false),
                    ActualDepth = table.Column<double>(type: "float", nullable: false),
                    Stemming = table.Column<double>(type: "float", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrillHoles", x => x.SerialNumber);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Module = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Action = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    NormalizedName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Area = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Region = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Country = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    OmanPhone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CountryPhone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RolePermissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    PermissionId = table.Column<int>(type: "int", nullable: false),
                    GrantedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RevokedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RolePermissions_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePermissions_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    AssignedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RevokedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "Action", "CreatedAt", "Description", "IsActive", "Module", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "View", new DateTime(2025, 6, 2, 4, 29, 3, 959, DateTimeKind.Utc).AddTicks(5398), "Can view user list", true, "Users", "View Users", new DateTime(2025, 6, 2, 4, 29, 3, 959, DateTimeKind.Utc).AddTicks(5398) },
                    { 2, "Create", new DateTime(2025, 6, 2, 4, 29, 3, 959, DateTimeKind.Utc).AddTicks(5401), "Can create new users", true, "Users", "Create User", new DateTime(2025, 6, 2, 4, 29, 3, 959, DateTimeKind.Utc).AddTicks(5401) },
                    { 3, "Edit", new DateTime(2025, 6, 2, 4, 29, 3, 959, DateTimeKind.Utc).AddTicks(5403), "Can edit user details", true, "Users", "Edit User", new DateTime(2025, 6, 2, 4, 29, 3, 959, DateTimeKind.Utc).AddTicks(5403) },
                    { 4, "Delete", new DateTime(2025, 6, 2, 4, 29, 3, 959, DateTimeKind.Utc).AddTicks(5405), "Can delete users", true, "Users", "Delete User", new DateTime(2025, 6, 2, 4, 29, 3, 959, DateTimeKind.Utc).AddTicks(5405) },
                    { 5, "View", new DateTime(2025, 6, 2, 4, 29, 3, 959, DateTimeKind.Utc).AddTicks(5406), "Can view dashboard", true, "Dashboard", "View Dashboard", new DateTime(2025, 6, 2, 4, 29, 3, 959, DateTimeKind.Utc).AddTicks(5407) },
                    { 6, "Upload", new DateTime(2025, 6, 2, 4, 29, 3, 959, DateTimeKind.Utc).AddTicks(5408), "Can upload CSV files", true, "DrillHole", "Upload CSV", new DateTime(2025, 6, 2, 4, 29, 3, 959, DateTimeKind.Utc).AddTicks(5408) },
                    { 7, "View", new DateTime(2025, 6, 2, 4, 29, 3, 959, DateTimeKind.Utc).AddTicks(5472), "Can view drill holes", true, "DrillHole", "View DrillHoles", new DateTime(2025, 6, 2, 4, 29, 3, 959, DateTimeKind.Utc).AddTicks(5472) }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedAt", "Description", "IsActive", "Name", "NormalizedName", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 6, 2, 4, 29, 3, 959, DateTimeKind.Utc).AddTicks(5283), "Administrator with full access", true, "Admin", "ADMIN", new DateTime(2025, 6, 2, 4, 29, 3, 959, DateTimeKind.Utc).AddTicks(5285) },
                    { 2, new DateTime(2025, 6, 2, 4, 29, 3, 959, DateTimeKind.Utc).AddTicks(5290), "Blasting Engineer with technical access", true, "BlastingEngineer", "BLASTINGENGINEER", new DateTime(2025, 6, 2, 4, 29, 3, 959, DateTimeKind.Utc).AddTicks(5290) },
                    { 3, new DateTime(2025, 6, 2, 4, 29, 3, 959, DateTimeKind.Utc).AddTicks(5292), "Regular user with limited access", true, "User", "USER", new DateTime(2025, 6, 2, 4, 29, 3, 959, DateTimeKind.Utc).AddTicks(5292) }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Area", "Country", "CountryPhone", "CreatedAt", "Email", "Name", "OmanPhone", "Region", "Role", "Status", "UpdatedAt" },
                values: new object[] { 1, "Central", "Oman", "+968 9999 9999", new DateTime(2025, 6, 2, 4, 29, 3, 959, DateTimeKind.Utc).AddTicks(5492), "admin@dbms.com", "System Administrator", "+968 9999 9999", "Muscat", "Admin", "Active", new DateTime(2025, 6, 2, 4, 29, 3, 959, DateTimeKind.Utc).AddTicks(5492) });

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_Module_Action",
                table: "Permissions",
                columns: new[] { "Module", "Action" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_PermissionId",
                table: "RolePermissions",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_RoleId",
                table: "RolePermissions",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Name",
                table: "Roles",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UserId",
                table: "UserRoles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DrillHoles");

            migrationBuilder.DropTable(
                name: "RolePermissions");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
