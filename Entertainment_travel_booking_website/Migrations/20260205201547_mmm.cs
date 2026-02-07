using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entertainment_travel_booking_website.Migrations
{
    /// <inheritdoc />
    public partial class mmm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_rooms_hotels_HotelId",
                table: "rooms");

            migrationBuilder.AlterColumn<int>(
                name: "HotelId",
                table: "rooms",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_rooms_hotels_HotelId",
                table: "rooms",
                column: "HotelId",
                principalTable: "hotels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_rooms_hotels_HotelId",
                table: "rooms");

            migrationBuilder.AlterColumn<int>(
                name: "HotelId",
                table: "rooms",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_rooms_hotels_HotelId",
                table: "rooms",
                column: "HotelId",
                principalTable: "hotels",
                principalColumn: "Id");
        }
    }
}
