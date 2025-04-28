using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PropertyRentalDAL.Migrations
{
    /// <inheritdoc />
    public partial class v5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Ratings_RatingId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_RatingId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "RatingId",
                table: "Bookings");

            migrationBuilder.AddColumn<int>(
                name: "BookingId",
                table: "Ratings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_BookingId",
                table: "Ratings",
                column: "BookingId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Bookings_BookingId",
                table: "Ratings",
                column: "BookingId",
                principalTable: "Bookings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Bookings_BookingId",
                table: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_Ratings_BookingId",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "BookingId",
                table: "Ratings");

            migrationBuilder.AddColumn<int>(
                name: "RatingId",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_RatingId",
                table: "Bookings",
                column: "RatingId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Ratings_RatingId",
                table: "Bookings",
                column: "RatingId",
                principalTable: "Ratings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
