using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Phrase_App.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CategortTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("0557fdf7-80e4-42a5-956e-aea07a8a5011"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("3ad3c552-8570-4d0c-bd2a-83abb2752d40"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("4c4e56c1-8255-4574-8e7d-39fa6774d0db"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("507e5003-e002-4bd6-9d9f-30cfe41e9dff"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("5c2af74b-a93e-49cf-afc2-ac87601d46d5"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("663d2343-2919-4ec4-8163-6c1557ce81c0"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("68ea134d-d5a8-41dc-ae62-5f40f4acb868"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("8b0f4530-6969-4ed1-a094-5cd5ed29d32f"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("b0ce7b4e-90f7-434d-b58e-1011778d4162"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("b92311a5-2029-4886-a85d-a61b9b39773c"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("bcc304ab-2a9b-4520-ad28-b566d74273fa"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("bfbfd438-3921-4dd4-a9f5-36455b86696a"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("c5024082-c63e-4231-900d-726b888c8df5"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("c73d18c9-0453-4647-84e1-bbfc163a077a"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("d8192f77-9d6b-4441-8bc5-1b82b2c83ff7"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("fee1e3fd-7ec2-4237-be4a-a603afa646eb"));

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "ColorHex", "IconKey", "IsActive", "Name" },
                values: new object[,]
                {
                    { new Guid("0557fdf7-80e4-42a5-956e-aea07a8a5011"), "#FF9100", "career", true, "Career" },
                    { new Guid("3ad3c552-8570-4d0c-bd2a-83abb2752d40"), "#00B8D4", "peace", true, "Peace" },
                    { new Guid("4c4e56c1-8255-4574-8e7d-39fa6774d0db"), "#FF4081", "love", true, "Love" },
                    { new Guid("507e5003-e002-4bd6-9d9f-30cfe41e9dff"), "#A1887F", "resilience", true, "Resilience" },
                    { new Guid("5c2af74b-a93e-49cf-afc2-ac87601d46d5"), "#607D8B", "discipline", true, "Discipline" },
                    { new Guid("663d2343-2919-4ec4-8163-6c1557ce81c0"), "#00C853", "wealth", true, "Wealth" },
                    { new Guid("68ea134d-d5a8-41dc-ae62-5f40f4acb868"), "#D50000", "courage", true, "Courage" },
                    { new Guid("8b0f4530-6969-4ed1-a094-5cd5ed29d32f"), "#FF5252", "focus", true, "Focus" },
                    { new Guid("b0ce7b4e-90f7-434d-b58e-1011778d4162"), "#2962FF", "fitness", true, "Fitness" },
                    { new Guid("b92311a5-2029-4886-a85d-a61b9b39773c"), "#40C4FF", "zen", true, "Zen" },
                    { new Guid("bcc304ab-2a9b-4520-ad28-b566d74273fa"), "#B2FF59", "hope", true, "Hope" },
                    { new Guid("bfbfd438-3921-4dd4-a9f5-36455b86696a"), "#FFD740", "energy", true, "Energy" },
                    { new Guid("c5024082-c63e-4231-900d-726b888c8df5"), "#7C4DFF", "wisdom", true, "Wisdom" },
                    { new Guid("c73d18c9-0453-4647-84e1-bbfc163a077a"), "#00E676", "growth", true, "Growth" },
                    { new Guid("d8192f77-9d6b-4441-8bc5-1b82b2c83ff7"), "#FF80AB", "gratitude", true, "Gratitude" },
                    { new Guid("fee1e3fd-7ec2-4237-be4a-a603afa646eb"), "#FBC02D", "happiness", true, "Happiness" }
                });
        }
    }
}
