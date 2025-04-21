using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PropertyRentalDAL.Migrations
{
    /// <inheritdoc />
    public partial class v3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PropertyServices");

            migrationBuilder.AddColumn<int>(
                name: "PropertyId",
                table: "Services",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FeesPerMonth",
                table: "Properties",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Services_PropertyId",
                table: "Services",
                column: "PropertyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Properties_PropertyId",
                table: "Services",
                column: "PropertyId",
                principalTable: "Properties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Services_Properties_PropertyId",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_Services_PropertyId",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "PropertyId",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "FeesPerMonth",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "PropertyServices",
                columns: table => new
                {
                    PropertyId = table.Column<int>(type: "int", nullable: false),
                    ServiceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyServices", x => new { x.PropertyId, x.ServiceId });
                    table.ForeignKey(
                        name: "FK_PropertyServices_Properties_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Properties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PropertyServices_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PropertyServices_ServiceId",
                table: "PropertyServices",
                column: "ServiceId");
        }
    }
}
