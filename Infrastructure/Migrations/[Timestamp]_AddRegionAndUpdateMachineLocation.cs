using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRegionAndUpdateMachineLocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Create Regions table
            migrationBuilder.CreateTable(
                name: "Regions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Regions_Name",
                table: "Regions",
                column: "Name",
                unique: true);

            // Insert default regions
            migrationBuilder.InsertData(
                table: "Regions",
                columns: new[] { "Name", "Description", "IsActive", "CreatedAt", "UpdatedAt" },
                values: new object[,]
                {
                    { "Muscat", "Muscat Governorate", true, DateTime.UtcNow, DateTime.UtcNow },
                    { "Dhofar", "Dhofar Governorate", true, DateTime.UtcNow, DateTime.UtcNow },
                    { "Musandam", "Musandam Governorate", true, DateTime.UtcNow, DateTime.UtcNow },
                    { "Al Buraimi", "Al Buraimi Governorate", true, DateTime.UtcNow, DateTime.UtcNow },
                    { "Al Dakhiliyah", "Al Dakhiliyah Governorate", true, DateTime.UtcNow, DateTime.UtcNow },
                    { "Al Dhahirah", "Al Dhahirah Governorate", true, DateTime.UtcNow, DateTime.UtcNow },
                    { "Al Wusta", "Al Wusta Governorate", true, DateTime.UtcNow, DateTime.UtcNow },
                    { "Al Batinah North", "Al Batinah North Governorate", true, DateTime.UtcNow, DateTime.UtcNow },
                    { "Al Batinah South", "Al Batinah South Governorate", true, DateTime.UtcNow, DateTime.UtcNow },
                    { "Ash Sharqiyah North", "Ash Sharqiyah North Governorate", true, DateTime.UtcNow, DateTime.UtcNow },
                    { "Ash Sharqiyah South", "Ash Sharqiyah South Governorate", true, DateTime.UtcNow, DateTime.UtcNow }
                });

            // Add RegionId column to Machines table
            migrationBuilder.AddColumn<int>(
                name: "RegionId",
                table: "Machines",
                type: "int",
                nullable: true);

            // Update Projects table to use RegionId instead of Region string
            migrationBuilder.AddColumn<int>(
                name: "RegionId",
                table: "Projects",
                type: "int",
                nullable: false,
                defaultValue: 1);

            // Migrate existing Projects data
            migrationBuilder.Sql(@"
                UPDATE p 
                SET p.RegionId = r.Id 
                FROM Projects p 
                INNER JOIN Regions r ON p.Region = r.Name
            ");

            // Drop the old Region column from Projects
            migrationBuilder.DropColumn(
                name: "Region",
                table: "Projects");

            // Drop the old CurrentLocation column from Machines
            migrationBuilder.DropColumn(
                name: "CurrentLocation",
                table: "Machines");

            // Create foreign key constraints
            migrationBuilder.CreateIndex(
                name: "IX_Machines_RegionId",
                table: "Machines",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_RegionId",
                table: "Projects",
                column: "RegionId");

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
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop foreign keys
            migrationBuilder.DropForeignKey(
                name: "FK_Machines_Regions_RegionId",
                table: "Machines");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Regions_RegionId",
                table: "Projects");

            // Drop indexes
            migrationBuilder.DropIndex(
                name: "IX_Machines_RegionId",
                table: "Machines");

            migrationBuilder.DropIndex(
                name: "IX_Projects_RegionId",
                table: "Projects");

            // Add back old columns
            migrationBuilder.AddColumn<string>(
                name: "Region",
                table: "Projects",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CurrentLocation",
                table: "Machines",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            // Drop new columns
            migrationBuilder.DropColumn(
                name: "RegionId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "RegionId",
                table: "Machines");

            // Drop Regions table
            migrationBuilder.DropTable(
                name: "Regions");
        }
    }
} 