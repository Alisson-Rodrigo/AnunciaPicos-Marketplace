using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnunciaPicos.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentUniqueNewCollum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPlanChange",
                table: "Payments",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "PreviousPlanExpirationDate",
                table: "Payments",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PreviousPlanId",
                table: "Payments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PreviousPlanPurchaseDate",
                table: "Payments",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PreviousPlanRemainingDays",
                table: "Payments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReplacedByPaymentId",
                table: "Payments",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPlanChange",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "PreviousPlanExpirationDate",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "PreviousPlanId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "PreviousPlanPurchaseDate",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "PreviousPlanRemainingDays",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "ReplacedByPaymentId",
                table: "Payments");
        }
    }
}
