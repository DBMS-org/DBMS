using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCompanyToMachine : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Company",
                table: "Machines");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4371), new DateTime(2025, 6, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4371) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4376), new DateTime(2025, 6, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4376) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4380), new DateTime(2025, 6, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4380) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4383), new DateTime(2025, 6, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4383) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4386), new DateTime(2025, 6, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4386) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4388), new DateTime(2025, 6, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4389) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4391), new DateTime(2025, 6, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4391) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4394), new DateTime(2025, 6, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4394) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4396), new DateTime(2025, 6, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4397) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4399), new DateTime(2025, 6, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4399) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4401), new DateTime(2025, 6, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4402) });

            migrationBuilder.UpdateData(
                table: "ProjectSites",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4626), new DateTime(2025, 6, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4627) });

            migrationBuilder.UpdateData(
                table: "ProjectSites",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4632), new DateTime(2025, 6, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4633) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4562), new DateTime(2025, 6, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4563) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4570), new DateTime(2025, 6, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4570) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 12, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4576), new DateTime(2025, 5, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4588) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4117), new DateTime(2025, 6, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4120) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4127), new DateTime(2025, 6, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4128) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4131), new DateTime(2025, 6, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4131) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4134), new DateTime(2025, 6, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4134) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4136), new DateTime(2025, 6, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4137) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4139), new DateTime(2025, 6, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4139) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4141), new DateTime(2025, 6, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4142) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4504), new DateTime(2025, 6, 22, 9, 58, 20, 354, DateTimeKind.Utc).AddTicks(4504) });
        }
    }
}
