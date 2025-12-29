using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Phrase_App.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class QuoteTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("169ed876-a4ed-4221-a977-a879fe8711e1"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("298158de-1f42-4981-aa3d-5810cc57e9a1"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("2e638968-a4aa-4e9d-b2a3-4f3d48e2099c"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("32f46a7e-e95b-4c92-8c17-65b428b37f97"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("3af82598-6366-40eb-8f1e-6575fd627951"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("409f924a-f733-4f15-b62f-72e53e946213"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("46e20e13-6177-419a-a348-d4b0265d7d65"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("532a41f3-7e94-4f0c-ba55-0ebe7f139585"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("82731a8e-770f-4c65-9cb7-3f9a65e19aef"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("957ac687-dc10-4025-bd86-32a135a0edb4"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("b043b9d4-4ee8-4a0b-bbba-9a193c88dabb"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("c2ac8532-ec6f-40d8-9ee6-756376ec9e90"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("c3268450-402a-4866-a1bd-f172e6c50c44"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("e53374a6-683c-4c00-a484-cf03352ee664"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("e63e51df-8cfc-4fd6-933d-e9415768698a"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("f53d60cc-c1c5-4357-9820-4d136de66ce4"));

            migrationBuilder.CreateTable(
                name: "Quotes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Quotes_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_Quotes_CategoryId",
                table: "Quotes",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Quotes");

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

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "ColorHex", "IconKey", "IsActive", "Name" },
                values: new object[,]
                {
                    { new Guid("169ed876-a4ed-4221-a977-a879fe8711e1"), "#A1887F", "resilience", true, "Resilience" },
                    { new Guid("298158de-1f42-4981-aa3d-5810cc57e9a1"), "#B2FF59", "hope", true, "Hope" },
                    { new Guid("2e638968-a4aa-4e9d-b2a3-4f3d48e2099c"), "#FF4081", "love", true, "Love" },
                    { new Guid("32f46a7e-e95b-4c92-8c17-65b428b37f97"), "#FBC02D", "happiness", true, "Happiness" },
                    { new Guid("3af82598-6366-40eb-8f1e-6575fd627951"), "#FFD740", "energy", true, "Energy" },
                    { new Guid("409f924a-f733-4f15-b62f-72e53e946213"), "#FF80AB", "gratitude", true, "Gratitude" },
                    { new Guid("46e20e13-6177-419a-a348-d4b0265d7d65"), "#FF9100", "career", true, "Career" },
                    { new Guid("532a41f3-7e94-4f0c-ba55-0ebe7f139585"), "#40C4FF", "zen", true, "Zen" },
                    { new Guid("82731a8e-770f-4c65-9cb7-3f9a65e19aef"), "#7C4DFF", "wisdom", true, "Wisdom" },
                    { new Guid("957ac687-dc10-4025-bd86-32a135a0edb4"), "#00B8D4", "peace", true, "Peace" },
                    { new Guid("b043b9d4-4ee8-4a0b-bbba-9a193c88dabb"), "#D50000", "courage", true, "Courage" },
                    { new Guid("c2ac8532-ec6f-40d8-9ee6-756376ec9e90"), "#FF5252", "focus", true, "Focus" },
                    { new Guid("c3268450-402a-4866-a1bd-f172e6c50c44"), "#2962FF", "fitness", true, "Fitness" },
                    { new Guid("e53374a6-683c-4c00-a484-cf03352ee664"), "#00E676", "growth", true, "Growth" },
                    { new Guid("e63e51df-8cfc-4fd6-933d-e9415768698a"), "#607D8B", "discipline", true, "Discipline" },
                    { new Guid("f53d60cc-c1c5-4357-9820-4d136de66ce4"), "#00C853", "wealth", true, "Wealth" }
                });
        }
    }
}
