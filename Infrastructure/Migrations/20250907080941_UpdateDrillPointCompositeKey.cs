using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDrillPointCompositeKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlastConnections_DrillPoints_Point1DrillPointId",
                table: "BlastConnections");

            migrationBuilder.DropForeignKey(
                name: "FK_BlastConnections_DrillPoints_Point2DrillPointId",
                table: "BlastConnections");

            migrationBuilder.DropForeignKey(
                name: "FK_DetonatorInfos_DrillPoints_DrillPointId",
                table: "DetonatorInfos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DrillPoints",
                table: "DrillPoints");

            migrationBuilder.DropIndex(
                name: "IX_BlastConnections_Point1DrillPointId",
                table: "BlastConnections");

            migrationBuilder.DropIndex(
                name: "IX_BlastConnections_Point2DrillPointId",
                table: "BlastConnections");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DrillPoints",
                table: "DrillPoints",
                columns: new[] { "Id", "ProjectId", "SiteId" });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(6766), new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(6767) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(6776), new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(6776) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(6782), new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(6783) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(6787), new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(6787) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(6791), new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(6792) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(6796), new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(6796) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(6800), new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(6800) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(6806), new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(6823) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(7030), new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(7031) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(7044), new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(7045) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(7050), new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(7050) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(7054), new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(7054) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(7058), new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(7058) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(7062), new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(7063) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(7066), new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(7067) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(7273), new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(7274) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(7279), new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(7280) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(7283), new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(7284) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(7287), new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(7288) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 1,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(6909));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 2,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(6916));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 3,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(6919));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 4,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(6922));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 5,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(6925));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 6,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(6927));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 7,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(6929));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 8,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(6932));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 9,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(6935));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(6319), new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(6325) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(6342), new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(6343) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(6349), new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(6350) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(6354), new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(6354) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(6359), new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(6360) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(6363), new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(6364) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(6368), new DateTime(2025, 9, 7, 8, 9, 39, 570, DateTimeKind.Utc).AddTicks(6369) });

            migrationBuilder.CreateIndex(
                name: "IX_DetonatorInfos_DrillPointId_ProjectId_SiteId",
                table: "DetonatorInfos",
                columns: new[] { "DrillPointId", "ProjectId", "SiteId" });

            migrationBuilder.CreateIndex(
                name: "IX_BlastConnections_Point1DrillPointId_ProjectId_SiteId",
                table: "BlastConnections",
                columns: new[] { "Point1DrillPointId", "ProjectId", "SiteId" });

            migrationBuilder.CreateIndex(
                name: "IX_BlastConnections_Point2DrillPointId_ProjectId_SiteId",
                table: "BlastConnections",
                columns: new[] { "Point2DrillPointId", "ProjectId", "SiteId" });

            migrationBuilder.AddForeignKey(
                name: "FK_BlastConnections_DrillPoints_Point1DrillPointId_ProjectId_SiteId",
                table: "BlastConnections",
                columns: new[] { "Point1DrillPointId", "ProjectId", "SiteId" },
                principalTable: "DrillPoints",
                principalColumns: new[] { "Id", "ProjectId", "SiteId" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BlastConnections_DrillPoints_Point2DrillPointId_ProjectId_SiteId",
                table: "BlastConnections",
                columns: new[] { "Point2DrillPointId", "ProjectId", "SiteId" },
                principalTable: "DrillPoints",
                principalColumns: new[] { "Id", "ProjectId", "SiteId" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DetonatorInfos_DrillPoints_DrillPointId_ProjectId_SiteId",
                table: "DetonatorInfos",
                columns: new[] { "DrillPointId", "ProjectId", "SiteId" },
                principalTable: "DrillPoints",
                principalColumns: new[] { "Id", "ProjectId", "SiteId" },
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlastConnections_DrillPoints_Point1DrillPointId_ProjectId_SiteId",
                table: "BlastConnections");

            migrationBuilder.DropForeignKey(
                name: "FK_BlastConnections_DrillPoints_Point2DrillPointId_ProjectId_SiteId",
                table: "BlastConnections");

            migrationBuilder.DropForeignKey(
                name: "FK_DetonatorInfos_DrillPoints_DrillPointId_ProjectId_SiteId",
                table: "DetonatorInfos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DrillPoints",
                table: "DrillPoints");

            migrationBuilder.DropIndex(
                name: "IX_DetonatorInfos_DrillPointId_ProjectId_SiteId",
                table: "DetonatorInfos");

            migrationBuilder.DropIndex(
                name: "IX_BlastConnections_Point1DrillPointId_ProjectId_SiteId",
                table: "BlastConnections");

            migrationBuilder.DropIndex(
                name: "IX_BlastConnections_Point2DrillPointId_ProjectId_SiteId",
                table: "BlastConnections");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DrillPoints",
                table: "DrillPoints",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3768), new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3769) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3773), new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3773) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3776), new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3776) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3780), new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3781) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3782), new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3783) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3785), new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3785) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3790), new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3791) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3793), new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3793) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3906), new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3906) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3914), new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3915) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3918), new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3918) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3920), new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3920) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3922), new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3923) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3924), new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3925) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3927), new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3927) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3929), new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3930) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3931), new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3932) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3934), new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3934) });

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3936), new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3936) });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 1,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3836));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 2,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3841));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 3,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3843));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 4,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3844));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 5,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3846));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 6,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3847));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 7,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3849));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 8,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3850));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 9,
                column: "GrantedAt",
                value: new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3852));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3463), new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3467) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3473), new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3474) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3477), new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3477) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3479), new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3480) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3482), new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3482) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3484), new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3484) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3486), new DateTime(2025, 9, 6, 8, 35, 38, 856, DateTimeKind.Utc).AddTicks(3487) });

            migrationBuilder.CreateIndex(
                name: "IX_BlastConnections_Point1DrillPointId",
                table: "BlastConnections",
                column: "Point1DrillPointId");

            migrationBuilder.CreateIndex(
                name: "IX_BlastConnections_Point2DrillPointId",
                table: "BlastConnections",
                column: "Point2DrillPointId");

            migrationBuilder.AddForeignKey(
                name: "FK_BlastConnections_DrillPoints_Point1DrillPointId",
                table: "BlastConnections",
                column: "Point1DrillPointId",
                principalTable: "DrillPoints",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BlastConnections_DrillPoints_Point2DrillPointId",
                table: "BlastConnections",
                column: "Point2DrillPointId",
                principalTable: "DrillPoints",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DetonatorInfos_DrillPoints_DrillPointId",
                table: "DetonatorInfos",
                column: "DrillPointId",
                principalTable: "DrillPoints",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
