using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Phrase_App.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NewColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("21ccff8a-0ff8-40fe-86e5-995cb3fdda33"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("23e1b037-776e-427a-9d4b-7f436f248683"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("2f3a2303-0ffa-4a88-84cc-f341a8e3322e"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("5a82cf91-cb2a-4058-8a71-bdd7ed747867"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("693c7394-6489-4fc3-8b6f-3522d8e90b94"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("6a3ecd0c-24e6-4004-9d32-61148468e29d"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("7397688d-0d0a-4670-a360-42ef19880267"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("8a7b694f-858c-4558-bfec-542184dbc679"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("8b674b21-0da8-4f9a-991c-f32e7820d9e7"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("8df4ba25-f077-4075-a253-8a72a47ec5e3"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("9dda7289-7d96-4d78-b614-003f816e6429"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("bb5ded02-08d1-4ba0-88de-da5f6d7567ce"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("bfa959a3-36d7-42b7-9494-dfc67044c8f2"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("d8b68648-1de4-4a68-bab5-03b039d5ca57"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("fed11992-9ec7-4ab1-be00-25bb292ed9e3"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("ffa285af-ac35-49cc-abf7-daa2d5eb7c4d"));

            migrationBuilder.AddColumn<string>(
                name: "Bio",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DarkMode",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "ColorHex", "IconKey", "IsActive", "Name" },
                values: new object[,]
                {
                    { new Guid("470fc196-b9a5-4303-973b-6e00d903eea9"), "#FF80AB", "gratitude", true, "Gratitude" },
                    { new Guid("4e1d6085-70aa-489d-be66-de91a66603a6"), "#FF4081", "love", true, "Love" },
                    { new Guid("67a460f7-53bf-443a-95b0-98ee49fafab5"), "#2962FF", "fitness", true, "Fitness" },
                    { new Guid("6c2812ca-d41d-4d26-8474-d8c0872d9094"), "#40C4FF", "zen", true, "Zen" },
                    { new Guid("6f9d7afb-8dd6-44ec-ab1e-3e5037942605"), "#B2FF59", "hope", true, "Hope" },
                    { new Guid("7df0bdda-f264-42db-90dd-34e9440e550d"), "#00E676", "growth", true, "Growth" },
                    { new Guid("7e38166e-1a0a-4913-ac04-a676047c4cd4"), "#00C853", "wealth", true, "Wealth" },
                    { new Guid("b36299fe-9174-4236-bae4-e3e6da549bdc"), "#00B8D4", "peace", true, "Peace" },
                    { new Guid("c2b7d457-7285-48b2-85e6-6d52f41019ce"), "#A1887F", "resilience", true, "Resilience" },
                    { new Guid("c4a36d70-063f-4c3d-ae71-131ea4a6350e"), "#FBC02D", "happiness", true, "Happiness" },
                    { new Guid("c87c5ecd-51ff-48d6-9573-78539113999e"), "#D50000", "courage", true, "Courage" },
                    { new Guid("caa5953f-1501-4168-8634-d01550f6bdb3"), "#607D8B", "discipline", true, "Discipline" },
                    { new Guid("d410e73c-f2d9-4941-8580-c75ccc8535bb"), "#FF5252", "focus", true, "Focus" },
                    { new Guid("d53badb1-8b92-4b91-b463-1c5afcbd6163"), "#FF9100", "career", true, "Career" },
                    { new Guid("ed9a023e-406c-4b8b-8751-e8770b1ac96b"), "#FFD740", "energy", true, "Energy" },
                    { new Guid("fdc444b0-3121-4f5c-8041-feebea120e42"), "#7C4DFF", "wisdom", true, "Wisdom" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("470fc196-b9a5-4303-973b-6e00d903eea9"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("4e1d6085-70aa-489d-be66-de91a66603a6"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("67a460f7-53bf-443a-95b0-98ee49fafab5"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("6c2812ca-d41d-4d26-8474-d8c0872d9094"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("6f9d7afb-8dd6-44ec-ab1e-3e5037942605"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("7df0bdda-f264-42db-90dd-34e9440e550d"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("7e38166e-1a0a-4913-ac04-a676047c4cd4"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("b36299fe-9174-4236-bae4-e3e6da549bdc"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("c2b7d457-7285-48b2-85e6-6d52f41019ce"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("c4a36d70-063f-4c3d-ae71-131ea4a6350e"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("c87c5ecd-51ff-48d6-9573-78539113999e"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("caa5953f-1501-4168-8634-d01550f6bdb3"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("d410e73c-f2d9-4941-8580-c75ccc8535bb"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("d53badb1-8b92-4b91-b463-1c5afcbd6163"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("ed9a023e-406c-4b8b-8751-e8770b1ac96b"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("fdc444b0-3121-4f5c-8041-feebea120e42"));

            migrationBuilder.DropColumn(
                name: "Bio",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DarkMode",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "ColorHex", "IconKey", "IsActive", "Name" },
                values: new object[,]
                {
                    { new Guid("21ccff8a-0ff8-40fe-86e5-995cb3fdda33"), "#FF9100", "career", true, "Career" },
                    { new Guid("23e1b037-776e-427a-9d4b-7f436f248683"), "#FF80AB", "gratitude", true, "Gratitude" },
                    { new Guid("2f3a2303-0ffa-4a88-84cc-f341a8e3322e"), "#FF5252", "focus", true, "Focus" },
                    { new Guid("5a82cf91-cb2a-4058-8a71-bdd7ed747867"), "#FFD740", "energy", true, "Energy" },
                    { new Guid("693c7394-6489-4fc3-8b6f-3522d8e90b94"), "#D50000", "courage", true, "Courage" },
                    { new Guid("6a3ecd0c-24e6-4004-9d32-61148468e29d"), "#00B8D4", "peace", true, "Peace" },
                    { new Guid("7397688d-0d0a-4670-a360-42ef19880267"), "#607D8B", "discipline", true, "Discipline" },
                    { new Guid("8a7b694f-858c-4558-bfec-542184dbc679"), "#FBC02D", "happiness", true, "Happiness" },
                    { new Guid("8b674b21-0da8-4f9a-991c-f32e7820d9e7"), "#FF4081", "love", true, "Love" },
                    { new Guid("8df4ba25-f077-4075-a253-8a72a47ec5e3"), "#7C4DFF", "wisdom", true, "Wisdom" },
                    { new Guid("9dda7289-7d96-4d78-b614-003f816e6429"), "#40C4FF", "zen", true, "Zen" },
                    { new Guid("bb5ded02-08d1-4ba0-88de-da5f6d7567ce"), "#00E676", "growth", true, "Growth" },
                    { new Guid("bfa959a3-36d7-42b7-9494-dfc67044c8f2"), "#2962FF", "fitness", true, "Fitness" },
                    { new Guid("d8b68648-1de4-4a68-bab5-03b039d5ca57"), "#00C853", "wealth", true, "Wealth" },
                    { new Guid("fed11992-9ec7-4ab1-be00-25bb292ed9e3"), "#A1887F", "resilience", true, "Resilience" },
                    { new Guid("ffa285af-ac35-49cc-abf7-daa2d5eb7c4d"), "#B2FF59", "hope", true, "Hope" }
                });
        }
    }
}
