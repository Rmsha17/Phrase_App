using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Phrase_App.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class OverlaySetting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "ColorHex", "IconKey", "IsActive", "Name" },
                values: new object[,]
                {
                    { new Guid("0db7446f-ee13-4639-a690-6f2c15b26b64"), "#00C853", "wealth", true, "Wealth" },
                    { new Guid("1a933e03-ced0-4bfe-abe3-3f8e031151d4"), "#B2FF59", "hope", true, "Hope" },
                    { new Guid("34ad237c-c7aa-4549-a15f-ee27d8340797"), "#607D8B", "discipline", true, "Discipline" },
                    { new Guid("39988adc-ce4b-4610-8f6e-151eabea56f8"), "#FBC02D", "happiness", true, "Happiness" },
                    { new Guid("60792d66-bd81-454d-8c73-5ad3dfeb2259"), "#FF5252", "focus", true, "Focus" },
                    { new Guid("60fcc47b-1cba-4a88-95aa-b1af054faa8c"), "#D50000", "courage", true, "Courage" },
                    { new Guid("62c3b888-62ba-4d61-ac29-18582e7034d1"), "#00B8D4", "peace", true, "Peace" },
                    { new Guid("6afa3e9e-f634-4ea6-948b-6e01a2263dc2"), "#FF9100", "career", true, "Career" },
                    { new Guid("7607301d-ec10-4783-9170-488e4f5bfb45"), "#40C4FF", "zen", true, "Zen" },
                    { new Guid("9bf961e2-833e-45f3-8568-6e5f4c6b01fd"), "#00E676", "growth", true, "Growth" },
                    { new Guid("ae95f438-4ee0-4fc4-b2b6-4f317533bd66"), "#7C4DFF", "wisdom", true, "Wisdom" },
                    { new Guid("b5d4e3f5-f847-423a-9717-125bf9120a34"), "#FF80AB", "gratitude", true, "Gratitude" },
                    { new Guid("b7bc4d1f-954a-4fd8-bb6f-f43284aca499"), "#A1887F", "resilience", true, "Resilience" },
                    { new Guid("e5b8831a-d2e3-4b5e-b995-03e9a1a12540"), "#2962FF", "fitness", true, "Fitness" },
                    { new Guid("ec3b6a60-24d4-42f2-979d-82f6c928fe50"), "#FFD740", "energy", true, "Energy" },
                    { new Guid("fd7d25b6-9c33-4dbf-abdd-f7314b8e05d2"), "#FF4081", "love", true, "Love" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("0db7446f-ee13-4639-a690-6f2c15b26b64"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("1a933e03-ced0-4bfe-abe3-3f8e031151d4"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("34ad237c-c7aa-4549-a15f-ee27d8340797"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("39988adc-ce4b-4610-8f6e-151eabea56f8"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("60792d66-bd81-454d-8c73-5ad3dfeb2259"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("60fcc47b-1cba-4a88-95aa-b1af054faa8c"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("62c3b888-62ba-4d61-ac29-18582e7034d1"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("6afa3e9e-f634-4ea6-948b-6e01a2263dc2"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("7607301d-ec10-4783-9170-488e4f5bfb45"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("9bf961e2-833e-45f3-8568-6e5f4c6b01fd"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("ae95f438-4ee0-4fc4-b2b6-4f317533bd66"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("b5d4e3f5-f847-423a-9717-125bf9120a34"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("b7bc4d1f-954a-4fd8-bb6f-f43284aca499"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("e5b8831a-d2e3-4b5e-b995-03e9a1a12540"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("ec3b6a60-24d4-42f2-979d-82f6c928fe50"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("fd7d25b6-9c33-4dbf-abdd-f7314b8e05d2"));

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
    }
}
