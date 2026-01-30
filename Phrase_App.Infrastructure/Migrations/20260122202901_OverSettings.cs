using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Phrase_App.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class OverSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "OverlaySettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FontSize = table.Column<double>(type: "float", nullable: false),
                    FontColor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FontFamily = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Opacity = table.Column<double>(type: "float", nullable: false),
                    BackgroundType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BackgroundValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AnimationType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IntervalMinutes = table.Column<int>(type: "int", nullable: false),
                    Position = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VibrationEnabled = table.Column<bool>(type: "bit", nullable: false),
                    SoundEffect = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OverlaySettings", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "ColorHex", "IconKey", "IsActive", "Name" },
                values: new object[,]
                {
                    { new Guid("06c8c8e0-d431-4bdb-ac2e-44f87f667baf"), "#00E676", "growth", true, "Growth" },
                    { new Guid("0a0ce44d-2afe-410b-a395-3d137433bd5c"), "#FF80AB", "gratitude", true, "Gratitude" },
                    { new Guid("0c5d4c37-c2a3-4b8d-9edd-fc4c75745609"), "#FF9100", "career", true, "Career" },
                    { new Guid("0e180c18-937e-4919-8e06-12fdec3d5b39"), "#A1887F", "resilience", true, "Resilience" },
                    { new Guid("17815288-c1bd-4e4a-8b4d-229dadc114d7"), "#2962FF", "fitness", true, "Fitness" },
                    { new Guid("243877e0-c6d4-498a-9ed4-813ae66e7ff9"), "#607D8B", "discipline", true, "Discipline" },
                    { new Guid("25fd56c2-9839-4cc7-9448-0538b98379d3"), "#00B8D4", "peace", true, "Peace" },
                    { new Guid("45475541-c73b-4c45-b93e-9056be60a096"), "#B2FF59", "hope", true, "Hope" },
                    { new Guid("8cc62e42-285f-4105-a54b-f71a087ce526"), "#40C4FF", "zen", true, "Zen" },
                    { new Guid("9c3f3bb9-8c4d-4bd8-a1b9-85d6d0539b3f"), "#D50000", "courage", true, "Courage" },
                    { new Guid("9c4a7362-6428-4804-97b0-0f53552b7e78"), "#FFD740", "energy", true, "Energy" },
                    { new Guid("af473f6b-78b4-466c-a471-f89e27501d72"), "#FBC02D", "happiness", true, "Happiness" },
                    { new Guid("b61dd8ce-9e86-41d9-8271-0d6a353efd26"), "#FF4081", "love", true, "Love" },
                    { new Guid("ca3e71ea-ecc7-4f75-aa9a-33b82f2dbc5a"), "#FF5252", "focus", true, "Focus" },
                    { new Guid("d0d76d8e-7c04-4c77-900d-0dc7d1275457"), "#00C853", "wealth", true, "Wealth" },
                    { new Guid("eddf4497-f3bd-4109-b768-11ca0f5f804e"), "#7C4DFF", "wisdom", true, "Wisdom" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OverlaySettings");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("06c8c8e0-d431-4bdb-ac2e-44f87f667baf"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("0a0ce44d-2afe-410b-a395-3d137433bd5c"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("0c5d4c37-c2a3-4b8d-9edd-fc4c75745609"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("0e180c18-937e-4919-8e06-12fdec3d5b39"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("17815288-c1bd-4e4a-8b4d-229dadc114d7"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("243877e0-c6d4-498a-9ed4-813ae66e7ff9"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("25fd56c2-9839-4cc7-9448-0538b98379d3"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("45475541-c73b-4c45-b93e-9056be60a096"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("8cc62e42-285f-4105-a54b-f71a087ce526"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("9c3f3bb9-8c4d-4bd8-a1b9-85d6d0539b3f"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("9c4a7362-6428-4804-97b0-0f53552b7e78"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("af473f6b-78b4-466c-a471-f89e27501d72"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("b61dd8ce-9e86-41d9-8271-0d6a353efd26"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("ca3e71ea-ecc7-4f75-aa9a-33b82f2dbc5a"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("d0d76d8e-7c04-4c77-900d-0dc7d1275457"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("eddf4497-f3bd-4109-b768-11ca0f5f804e"));

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
    }
}
