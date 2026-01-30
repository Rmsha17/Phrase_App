using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Phrase_App.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CurrentPurchaseTokenColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CurrentPurchaseToken",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentPurchaseToken",
                table: "AspNetUsers");
        }
    }
}
