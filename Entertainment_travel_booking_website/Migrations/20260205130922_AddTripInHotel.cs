using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entertainment_travel_booking_website.Migrations
{
    /// <inheritdoc />
    public partial class AddTripInHotel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TripSupimage_trips_TripId",
                table: "TripSupimage");

            migrationBuilder.DropTable(
                name: "AdditianActivitiesTrip");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TripSupimage",
                table: "TripSupimage");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "TripSupimage");

            migrationBuilder.RenameTable(
                name: "TripSupimage",
                newName: "tripSupimages");

            migrationBuilder.RenameIndex(
                name: "IX_TripSupimage_TripId",
                table: "tripSupimages",
                newName: "IX_tripSupimages_TripId");

            migrationBuilder.AddColumn<int>(
                name: "AdditionalActivityId",
                table: "trips",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HotelId",
                table: "trips",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_tripSupimages",
                table: "tripSupimages",
                columns: new[] { "SupImg", "TripId" });

            migrationBuilder.CreateIndex(
                name: "IX_trips_AdditionalActivityId",
                table: "trips",
                column: "AdditionalActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_trips_HotelId",
                table: "trips",
                column: "HotelId");

            migrationBuilder.AddForeignKey(
                name: "FK_trips_additianActivites_AdditionalActivityId",
                table: "trips",
                column: "AdditionalActivityId",
                principalTable: "additianActivites",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_trips_hotels_HotelId",
                table: "trips",
                column: "HotelId",
                principalTable: "hotels",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_tripSupimages_trips_TripId",
                table: "tripSupimages",
                column: "TripId",
                principalTable: "trips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_trips_additianActivites_AdditionalActivityId",
                table: "trips");

            migrationBuilder.DropForeignKey(
                name: "FK_trips_hotels_HotelId",
                table: "trips");

            migrationBuilder.DropForeignKey(
                name: "FK_tripSupimages_trips_TripId",
                table: "tripSupimages");

            migrationBuilder.DropIndex(
                name: "IX_trips_AdditionalActivityId",
                table: "trips");

            migrationBuilder.DropIndex(
                name: "IX_trips_HotelId",
                table: "trips");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tripSupimages",
                table: "tripSupimages");

            migrationBuilder.DropColumn(
                name: "AdditionalActivityId",
                table: "trips");

            migrationBuilder.DropColumn(
                name: "HotelId",
                table: "trips");

            migrationBuilder.RenameTable(
                name: "tripSupimages",
                newName: "TripSupimage");

            migrationBuilder.RenameIndex(
                name: "IX_tripSupimages_TripId",
                table: "TripSupimage",
                newName: "IX_TripSupimage_TripId");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "TripSupimage",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TripSupimage",
                table: "TripSupimage",
                columns: new[] { "SupImg", "TripId" });

            migrationBuilder.CreateTable(
                name: "AdditianActivitiesTrip",
                columns: table => new
                {
                    AdditianActivitiesId = table.Column<int>(type: "int", nullable: false),
                    TripsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdditianActivitiesTrip", x => new { x.AdditianActivitiesId, x.TripsId });
                    table.ForeignKey(
                        name: "FK_AdditianActivitiesTrip_additianActivites_AdditianActivitiesId",
                        column: x => x.AdditianActivitiesId,
                        principalTable: "additianActivites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AdditianActivitiesTrip_trips_TripsId",
                        column: x => x.TripsId,
                        principalTable: "trips",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdditianActivitiesTrip_TripsId",
                table: "AdditianActivitiesTrip",
                column: "TripsId");

            migrationBuilder.AddForeignKey(
                name: "FK_TripSupimage_trips_TripId",
                table: "TripSupimage",
                column: "TripId",
                principalTable: "trips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
