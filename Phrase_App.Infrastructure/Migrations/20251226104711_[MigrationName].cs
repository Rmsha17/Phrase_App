using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Phrase_App.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MigrationName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IconKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ColorHex = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
