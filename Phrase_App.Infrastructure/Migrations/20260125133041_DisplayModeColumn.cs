using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Phrase_App.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DisplayModeColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AddColumn<string>(
                name: "DisplayMode",
                table: "OverlaySettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisplayMode",
                table: "OverlaySettings");

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
    }
}
