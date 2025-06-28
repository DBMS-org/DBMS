using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCompanyFromMachine : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Company",
                table: "Machines");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9337), new DateTime(2025, 6, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9338) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9343), new DateTime(2025, 6, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9343) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9346), new DateTime(2025, 6, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9346) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9349), new DateTime(2025, 6, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9349) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9352), new DateTime(2025, 6, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9352) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9354), new DateTime(2025, 6, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9355) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9357), new DateTime(2025, 6, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9357) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9360), new DateTime(2025, 6, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9360) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9362), new DateTime(2025, 6, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9363) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9365), new DateTime(2025, 6, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9365) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9367), new DateTime(2025, 6, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9368) });

            migrationBuilder.UpdateData(
                table: "ProjectSites",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9546), new DateTime(2025, 6, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9547) });

            migrationBuilder.UpdateData(
                table: "ProjectSites",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9552), new DateTime(2025, 6, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9553) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9478), new DateTime(2025, 6, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9479) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9485), new DateTime(2025, 6, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9486) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 12, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9491), new DateTime(2025, 5, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9503) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9005), new DateTime(2025, 6, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9010) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9017), new DateTime(2025, 6, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9017) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9021), new DateTime(2025, 6, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9021) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9024), new DateTime(2025, 6, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9024) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9026), new DateTime(2025, 6, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9027) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9029), new DateTime(2025, 6, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9030) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9032), new DateTime(2025, 6, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9032) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9418), new DateTime(2025, 6, 22, 10, 30, 1, 759, DateTimeKind.Utc).AddTicks(9418) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Company",
                table: "Machines",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 10, 21, 4, 718, DateTimeKind.Utc).AddTicks(8836), new DateTime(2025, 6, 22, 10, 21, 4, 718, DateTimeKind.Utc).AddTicks(8837) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 10, 21, 4, 718, DateTimeKind.Utc).AddTicks(8843), new DateTime(2025, 6, 22, 10, 21, 4, 718, DateTimeKind.Utc).AddTicks(8844) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 10, 21, 4, 718, DateTimeKind.Utc).AddTicks(8847), new DateTime(2025, 6, 22, 10, 21, 4, 718, DateTimeKind.Utc).AddTicks(8847) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 10, 21, 4, 718, DateTimeKind.Utc).AddTicks(8850), new DateTime(2025, 6, 22, 10, 21, 4, 718, DateTimeKind.Utc).AddTicks(8850) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 10, 21, 4, 718, DateTimeKind.Utc).AddTicks(8853), new DateTime(2025, 6, 22, 10, 21, 4, 718, DateTimeKind.Utc).AddTicks(8853) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 10, 21, 4, 718, DateTimeKind.Utc).AddTicks(8856), new DateTime(2025, 6, 22, 10, 21, 4, 718, DateTimeKind.Utc).AddTicks(8856) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 10, 21, 4, 718, DateTimeKind.Utc).AddTicks(8858), new DateTime(2025, 6, 22, 10, 21, 4, 718, DateTimeKind.Utc).AddTicks(8859) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 10, 21, 4, 718, DateTimeKind.Utc).AddTicks(8861), new DateTime(2025, 6, 22, 10, 21, 4, 718, DateTimeKind.Utc).AddTicks(8861) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 10, 21, 4, 718, DateTimeKind.Utc).AddTicks(8864), new DateTime(2025, 6, 22, 10, 21, 4, 718, DateTimeKind.Utc).AddTicks(8864) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 10, 21, 4, 718, DateTimeKind.Utc).AddTicks(8866), new DateTime(2025, 6, 22, 10, 21, 4, 718, DateTimeKind.Utc).AddTicks(8867) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 10, 21, 4, 718, DateTimeKind.Utc).AddTicks(8869), new DateTime(2025, 6, 22, 10, 21, 4, 718, DateTimeKind.Utc).AddTicks(8869) });

            migrationBuilder.UpdateData(
                table: "ProjectSites",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 10, 21, 4, 718, DateTimeKind.Utc).AddTicks(9047), new DateTime(2025, 6, 22, 10, 21, 4, 718, DateTimeKind.Utc).AddTicks(9048) });

            migrationBuilder.UpdateData(
                table: "ProjectSites",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 10, 21, 4, 718, DateTimeKind.Utc).AddTicks(9052), new DateTime(2025, 6, 22, 10, 21, 4, 718, DateTimeKind.Utc).AddTicks(9053) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 10, 21, 4, 718, DateTimeKind.Utc).AddTicks(8981), new DateTime(2025, 6, 22, 10, 21, 4, 718, DateTimeKind.Utc).AddTicks(8982) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 10, 21, 4, 718, DateTimeKind.Utc).AddTicks(8988), new DateTime(2025, 6, 22, 10, 21, 4, 718, DateTimeKind.Utc).AddTicks(8989) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 12, 22, 10, 21, 4, 718, DateTimeKind.Utc).AddTicks(8996), new DateTime(2025, 5, 22, 10, 21, 4, 718, DateTimeKind.Utc).AddTicks(9007) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 10, 21, 4, 718, DateTimeKind.Utc).AddTicks(8496), new DateTime(2025, 6, 22, 10, 21, 4, 718, DateTimeKind.Utc).AddTicks(8500) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 10, 21, 4, 718, DateTimeKind.Utc).AddTicks(8510), new DateTime(2025, 6, 22, 10, 21, 4, 718, DateTimeKind.Utc).AddTicks(8511) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 10, 21, 4, 718, DateTimeKind.Utc).AddTicks(8514), new DateTime(2025, 6, 22, 10, 21, 4, 718, DateTimeKind.Utc).AddTicks(8514) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 10, 21, 4, 718, DateTimeKind.Utc).AddTicks(8517), new DateTime(2025, 6, 22, 10, 21, 4, 718, DateTimeKind.Utc).AddTicks(8517) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 10, 21, 4, 718, DateTimeKind.Utc).AddTicks(8519), new DateTime(2025, 6, 22, 10, 21, 4, 718, DateTimeKind.Utc).AddTicks(8520) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 10, 21, 4, 718, DateTimeKind.Utc).AddTicks(8522), new DateTime(2025, 6, 22, 10, 21, 4, 718, DateTimeKind.Utc).AddTicks(8522) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 10, 21, 4, 718, DateTimeKind.Utc).AddTicks(8524), new DateTime(2025, 6, 22, 10, 21, 4, 718, DateTimeKind.Utc).AddTicks(8525) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 10, 21, 4, 718, DateTimeKind.Utc).AddTicks(8923), new DateTime(2025, 6, 22, 10, 21, 4, 718, DateTimeKind.Utc).AddTicks(8923) });
        }
    }
}
