using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Phrase_App.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ScheduleQuote : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("19c88010-8352-45b8-b99c-bd87b776ce8c"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("2f62e12c-5583-4c62-834f-e748870e315c"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("35dbc698-f3fc-4822-ad31-c26b2d8e46fe"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("3a0c9b0e-721f-4721-a182-ca3704a9c99c"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("4e6fa705-ca38-4f07-bda3-807819247715"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("5ec6981d-b21b-47f8-8f45-4b685755c911"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("74f57f77-e7ba-4e78-bef8-45008bbb5acb"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("7e6991b9-c9fc-4423-bfe0-feccb8541183"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("8a8c5063-fb01-4ff7-acc2-3271a7053014"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("9d2c8633-f920-4b39-8e07-7230c98fd692"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("a177519d-6324-4195-b4d2-f56d241f02cb"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("bb7dac56-9239-4b24-995f-b299acd8a1dc"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("ce1e2894-4989-4bc2-aa01-d0624e5167ee"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("d9f82dfd-2a6e-490d-ac37-df25952d533a"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("e12aea12-e36b-4dcd-8990-f94a0c24fac0"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("ec17e632-6454-4fb8-b6d9-3e853e161bd9"));

            migrationBuilder.AddColumn<string>(
                name: "ProfileImageUrl",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserQuotes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuoteId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CustomContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomAuthor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsFavorite = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserQuotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserQuotes_Quotes_QuoteId",
                        column: x => x.QuoteId,
                        principalTable: "Quotes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "QuoteSchedules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserQuoteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DailyStartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    DailyEndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuoteSchedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuoteSchedules_UserQuotes_UserQuoteId",
                        column: x => x.UserQuoteId,
                        principalTable: "UserQuotes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ScheduledDays",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ScheduleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DayOfWeek = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduledDays", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScheduledDays_QuoteSchedules_ScheduleId",
                        column: x => x.ScheduleId,
                        principalTable: "QuoteSchedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "ColorHex", "IconKey", "IsActive", "Name" },
                values: new object[,]
                {
                    { new Guid("0398ae60-4e96-4880-90ac-460eacea1801"), "#7C4DFF", "wisdom", true, "Wisdom" },
                    { new Guid("0672c7b1-d37a-4f26-bc10-e396c5e05f15"), "#B2FF59", "hope", true, "Hope" },
                    { new Guid("1778e498-5f67-48be-b398-349f7a260d2f"), "#FF4081", "love", true, "Love" },
                    { new Guid("344e4be3-a452-4e6d-aa26-2930b16d18c5"), "#607D8B", "discipline", true, "Discipline" },
                    { new Guid("4b26bd95-8435-41a8-8a9b-9c956e205458"), "#D50000", "courage", true, "Courage" },
                    { new Guid("5201afc4-bd8f-41f9-b432-92341c1705e5"), "#2962FF", "fitness", true, "Fitness" },
                    { new Guid("5e6159db-bd78-459a-8394-35c8044b6dc9"), "#FBC02D", "happiness", true, "Happiness" },
                    { new Guid("65b45d1b-36e7-469d-8116-c07b766dc944"), "#00B8D4", "peace", true, "Peace" },
                    { new Guid("71006d66-df71-4327-a29b-8f692b6f8d73"), "#00C853", "wealth", true, "Wealth" },
                    { new Guid("8ea2d429-6377-4093-af30-fe3779557109"), "#40C4FF", "zen", true, "Zen" },
                    { new Guid("98989d18-dc69-48d6-b245-48e563aafc91"), "#FF80AB", "gratitude", true, "Gratitude" },
                    { new Guid("aa4be26b-2481-4662-9f22-99b2b3f277ea"), "#FF9100", "career", true, "Career" },
                    { new Guid("c8965de2-6841-4c2b-8fd1-7e1ae579cb55"), "#FF5252", "focus", true, "Focus" },
                    { new Guid("cde90656-d5dc-4912-8d00-d464f4719f6b"), "#00E676", "growth", true, "Growth" },
                    { new Guid("e5442af1-3d37-41c0-8dbc-d188dfe65a2e"), "#FFD740", "energy", true, "Energy" },
                    { new Guid("e724d7f4-e8a6-4ad6-822e-1e76307b40b3"), "#A1887F", "resilience", true, "Resilience" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuoteSchedules_UserQuoteId",
                table: "QuoteSchedules",
                column: "UserQuoteId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledDays_ScheduleId",
                table: "ScheduledDays",
                column: "ScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserQuotes_QuoteId",
                table: "UserQuotes",
                column: "QuoteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScheduledDays");

            migrationBuilder.DropTable(
                name: "QuoteSchedules");

            migrationBuilder.DropTable(
                name: "UserQuotes");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("0398ae60-4e96-4880-90ac-460eacea1801"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("0672c7b1-d37a-4f26-bc10-e396c5e05f15"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("1778e498-5f67-48be-b398-349f7a260d2f"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("344e4be3-a452-4e6d-aa26-2930b16d18c5"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("4b26bd95-8435-41a8-8a9b-9c956e205458"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("5201afc4-bd8f-41f9-b432-92341c1705e5"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("5e6159db-bd78-459a-8394-35c8044b6dc9"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("65b45d1b-36e7-469d-8116-c07b766dc944"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("71006d66-df71-4327-a29b-8f692b6f8d73"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("8ea2d429-6377-4093-af30-fe3779557109"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("98989d18-dc69-48d6-b245-48e563aafc91"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aa4be26b-2481-4662-9f22-99b2b3f277ea"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("c8965de2-6841-4c2b-8fd1-7e1ae579cb55"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("cde90656-d5dc-4912-8d00-d464f4719f6b"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("e5442af1-3d37-41c0-8dbc-d188dfe65a2e"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("e724d7f4-e8a6-4ad6-822e-1e76307b40b3"));

            migrationBuilder.DropColumn(
                name: "ProfileImageUrl",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "ColorHex", "IconKey", "IsActive", "Name" },
                values: new object[,]
                {
                    { new Guid("19c88010-8352-45b8-b99c-bd87b776ce8c"), "#2962FF", "fitness", true, "Fitness" },
                    { new Guid("2f62e12c-5583-4c62-834f-e748870e315c"), "#A1887F", "resilience", true, "Resilience" },
                    { new Guid("35dbc698-f3fc-4822-ad31-c26b2d8e46fe"), "#40C4FF", "zen", true, "Zen" },
                    { new Guid("3a0c9b0e-721f-4721-a182-ca3704a9c99c"), "#00B8D4", "peace", true, "Peace" },
                    { new Guid("4e6fa705-ca38-4f07-bda3-807819247715"), "#00E676", "growth", true, "Growth" },
                    { new Guid("5ec6981d-b21b-47f8-8f45-4b685755c911"), "#D50000", "courage", true, "Courage" },
                    { new Guid("74f57f77-e7ba-4e78-bef8-45008bbb5acb"), "#B2FF59", "hope", true, "Hope" },
                    { new Guid("7e6991b9-c9fc-4423-bfe0-feccb8541183"), "#FF80AB", "gratitude", true, "Gratitude" },
                    { new Guid("8a8c5063-fb01-4ff7-acc2-3271a7053014"), "#00C853", "wealth", true, "Wealth" },
                    { new Guid("9d2c8633-f920-4b39-8e07-7230c98fd692"), "#607D8B", "discipline", true, "Discipline" },
                    { new Guid("a177519d-6324-4195-b4d2-f56d241f02cb"), "#FF9100", "career", true, "Career" },
                    { new Guid("bb7dac56-9239-4b24-995f-b299acd8a1dc"), "#7C4DFF", "wisdom", true, "Wisdom" },
                    { new Guid("ce1e2894-4989-4bc2-aa01-d0624e5167ee"), "#FFD740", "energy", true, "Energy" },
                    { new Guid("d9f82dfd-2a6e-490d-ac37-df25952d533a"), "#FF4081", "love", true, "Love" },
                    { new Guid("e12aea12-e36b-4dcd-8990-f94a0c24fac0"), "#FF5252", "focus", true, "Focus" },
                    { new Guid("ec17e632-6454-4fb8-b6d9-3e853e161bd9"), "#FBC02D", "happiness", true, "Happiness" }
                });
        }
    }
}
