using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace home.api.Migrations
{
    /// <inheritdoc />
    public partial class add_family_titles_and_membership_role : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "FamilyTitleId",
                table: "UserFamilies",
                type: "RAW(16)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "UserFamilies",
                type: "NVARCHAR2(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "Member");

            migrationBuilder.CreateTable(
                name: "FamilyTitles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    Name = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    FamilyId = table.Column<Guid>(type: "RAW(16)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FamilyTitles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FamilyTitles_Families_FamilyId",
                        column: x => x.FamilyId,
                        principalTable: "Families",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "FamilyTitles",
                columns: new[] { "Id", "CreatedAt", "FamilyId", "LastUpdatedAt", "Name" },
                values: new object[,]
                {
                    { new Guid("a1b2c3d4-0000-4000-8000-000000000001"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Pai" },
                    { new Guid("a1b2c3d4-0000-4000-8000-000000000002"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Mãe" },
                    { new Guid("a1b2c3d4-0000-4000-8000-000000000003"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Cônjuge" },
                    { new Guid("a1b2c3d4-0000-4000-8000-000000000004"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Filho" },
                    { new Guid("a1b2c3d4-0000-4000-8000-000000000005"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Filha" },
                    { new Guid("a1b2c3d4-0000-4000-8000-000000000006"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Irmão" },
                    { new Guid("a1b2c3d4-0000-4000-8000-000000000007"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Irmã" },
                    { new Guid("a1b2c3d4-0000-4000-8000-000000000008"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Avô" },
                    { new Guid("a1b2c3d4-0000-4000-8000-000000000009"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Avó" },
                    { new Guid("a1b2c3d4-0000-4000-8000-000000000010"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Neto" },
                    { new Guid("a1b2c3d4-0000-4000-8000-000000000011"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Neta" },
                    { new Guid("a1b2c3d4-0000-4000-8000-000000000012"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Tio" },
                    { new Guid("a1b2c3d4-0000-4000-8000-000000000013"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Tia" },
                    { new Guid("a1b2c3d4-0000-4000-8000-000000000014"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Sobrinho" },
                    { new Guid("a1b2c3d4-0000-4000-8000-000000000015"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Sobrinha" },
                    { new Guid("a1b2c3d4-0000-4000-8000-000000000016"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Primo" },
                    { new Guid("a1b2c3d4-0000-4000-8000-000000000017"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Prima" },
                    { new Guid("a1b2c3d4-0000-4000-8000-000000000018"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Padrasto" },
                    { new Guid("a1b2c3d4-0000-4000-8000-000000000019"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Madrasta" },
                    { new Guid("a1b2c3d4-0000-4000-8000-000000000020"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Enteado" },
                    { new Guid("a1b2c3d4-0000-4000-8000-000000000021"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Enteada" },
                    { new Guid("a1b2c3d4-0000-4000-8000-000000000022"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Sogro" },
                    { new Guid("a1b2c3d4-0000-4000-8000-000000000023"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Sogra" },
                    { new Guid("a1b2c3d4-0000-4000-8000-000000000024"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Genro" },
                    { new Guid("a1b2c3d4-0000-4000-8000-000000000025"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Nora" },
                    { new Guid("a1b2c3d4-0000-4000-8000-000000000026"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Cunhado" },
                    { new Guid("a1b2c3d4-0000-4000-8000-000000000027"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Cunhada" },
                    { new Guid("a1b2c3d4-0000-4000-8000-000000000028"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Agregado" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserFamilies_FamilyTitleId",
                table: "UserFamilies",
                column: "FamilyTitleId");

            migrationBuilder.CreateIndex(
                name: "IX_FamilyTitles_FamilyId_Name",
                table: "FamilyTitles",
                columns: new[] { "FamilyId", "Name" },
                unique: true,
                filter: "\"FamilyId\" IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_UserFamilies_FamilyTitles_FamilyTitleId",
                table: "UserFamilies",
                column: "FamilyTitleId",
                principalTable: "FamilyTitles",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFamilies_FamilyTitles_FamilyTitleId",
                table: "UserFamilies");

            migrationBuilder.DropTable(
                name: "FamilyTitles");

            migrationBuilder.DropIndex(
                name: "IX_UserFamilies_FamilyTitleId",
                table: "UserFamilies");

            migrationBuilder.DropColumn(
                name: "FamilyTitleId",
                table: "UserFamilies");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "UserFamilies");
        }
    }
}
