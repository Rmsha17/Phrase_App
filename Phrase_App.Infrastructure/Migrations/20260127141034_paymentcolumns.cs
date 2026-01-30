using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Phrase_App.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class paymentcolumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPremium",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "PremiumExpiryDate",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubscriptionType",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPremium",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PremiumExpiryDate",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SubscriptionType",
                table: "AspNetUsers");
        }
    }
}
