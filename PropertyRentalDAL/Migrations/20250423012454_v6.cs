using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PropertyRentalDAL.Migrations
{
    /// <inheritdoc />
    public partial class v6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HostId",
                table: "Bookings",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_HostId",
                table: "Bookings",
                column: "HostId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_AspNetUsers_HostId",
                table: "Bookings",
                column: "HostId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_AspNetUsers_HostId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_HostId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "HostId",
                table: "Bookings");
        }
    }
}
