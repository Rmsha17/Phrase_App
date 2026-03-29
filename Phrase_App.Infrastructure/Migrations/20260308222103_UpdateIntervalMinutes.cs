using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Phrase_App.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateIntervalMinutes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "IntervalMinutes",
                table: "OverlaySettings",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "IntervalMinutes",
                table: "OverlaySettings",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }
    }
}
