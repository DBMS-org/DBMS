using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDrillPatternIdToDrillPoint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add DrillPatternId column to DrillPoints table
            migrationBuilder.AddColumn<int>(
                name: "DrillPatternId",
                table: "DrillPoints",
                type: "int",
                nullable: true);

            // Create foreign key constraint
            migrationBuilder.CreateIndex(
                name: "IX_DrillPoints_DrillPatternId",
                table: "DrillPoints",
                column: "DrillPatternId");

            migrationBuilder.AddForeignKey(
                name: "FK_DrillPoints_DrillPatterns_DrillPatternId",
                table: "DrillPoints",
                column: "DrillPatternId",
                principalTable: "DrillPatterns",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove foreign key constraint
            migrationBuilder.DropForeignKey(
                name: "FK_DrillPoints_DrillPatterns_DrillPatternId",
                table: "DrillPoints");

            // Remove index
            migrationBuilder.DropIndex(
                name: "IX_DrillPoints_DrillPatterns_DrillPatternId",
                table: "DrillPoints");

            // Remove column
            migrationBuilder.DropColumn(
                name: "DrillPatternId",
                table: "DrillPoints");
        }
    }
} 