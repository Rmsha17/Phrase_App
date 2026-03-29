using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Phrase_App.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddShowAuthorSnapToGrid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ShowAuthor",
                table: "OverlaySettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SnapToGrid",
                table: "OverlaySettings",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShowAuthor",
                table: "OverlaySettings");

            migrationBuilder.DropColumn(
                name: "SnapToGrid",
                table: "OverlaySettings");
        }
    }
}
