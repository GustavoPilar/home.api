using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace home.api.Migrations
{
    /// <inheritdoc />
    public partial class add_family_and_shared_home : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "FamilyId",
                table: "Homes",
                type: "RAW(16)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Families",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    Name = table.Column<string>(type: "NVARCHAR2(150)", maxLength: 150, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Families", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserFamilies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    FamilyId = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UserId = table.Column<Guid>(type: "RAW(16)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFamilies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserFamilies_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserFamilies_Families_FamilyId",
                        column: x => x.FamilyId,
                        principalTable: "Families",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Homes_FamilyId",
                table: "Homes",
                column: "FamilyId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFamilies_FamilyId",
                table: "UserFamilies",
                column: "FamilyId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFamilies_UserId_FamilyId",
                table: "UserFamilies",
                columns: new[] { "UserId", "FamilyId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Homes_Families_FamilyId",
                table: "Homes",
                column: "FamilyId",
                principalTable: "Families",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Homes_Families_FamilyId",
                table: "Homes");

            migrationBuilder.DropTable(
                name: "UserFamilies");

            migrationBuilder.DropTable(
                name: "Families");

            migrationBuilder.DropIndex(
                name: "IX_Homes_FamilyId",
                table: "Homes");

            migrationBuilder.DropColumn(
                name: "FamilyId",
                table: "Homes");
        }
    }
}
