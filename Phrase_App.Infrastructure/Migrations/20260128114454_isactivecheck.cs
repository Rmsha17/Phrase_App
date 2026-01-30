using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Phrase_App.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class isactivecheck : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "UserQuotes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "QuoteSchedules",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "UserQuotes");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "QuoteSchedules");
        }
    }
}
