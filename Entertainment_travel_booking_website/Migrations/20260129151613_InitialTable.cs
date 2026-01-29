using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entertainment_travel_booking_website.Migrations
{
    /// <inheritdoc />
    public partial class InitialTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "additianActivites",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Place = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_additianActivites", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "hotels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PricePerNight = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Availability = table.Column<bool>(type: "bit", nullable: false),
                    Rate = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hotels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "trips",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Place = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DiscountedPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AvailableSeats = table.Column<int>(type: "int", nullable: false),
                    Rate = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MaxPeople = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_trips", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "activitiesSupImgs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdditianActivitiesId = table.Column<int>(type: "int", nullable: false),
                    SupImg = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_activitiesSupImgs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_activitiesSupImgs_additianActivites_AdditianActivitiesId",
                        column: x => x.AdditianActivitiesId,
                        principalTable: "additianActivites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "hotelSupImgs",
                columns: table => new
                {
                    HotelId = table.Column<int>(type: "int", nullable: false),
                    SupImg = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hotelSupImgs", x => new { x.SupImg, x.HotelId });
                    table.ForeignKey(
                        name: "FK_hotelSupImgs_hotels_HotelId",
                        column: x => x.HotelId,
                        principalTable: "hotels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "rooms",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    locationInHotel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Availability = table.Column<bool>(type: "bit", nullable: false),
                    HotelId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rooms", x => x.ID);
                    table.ForeignKey(
                        name: "FK_rooms_hotels_HotelId",
                        column: x => x.HotelId,
                        principalTable: "hotels",
                        principalColumn: "Id");
                });

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

            migrationBuilder.CreateTable(
                name: "tripAdditianActivities",
                columns: table => new
                {
                    tripId = table.Column<int>(type: "int", nullable: false),
                    additianActivitiesId = table.Column<int>(type: "int", nullable: false),
                    NotesOBT = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tripAdditianActivities", x => new { x.tripId, x.additianActivitiesId });
                    table.ForeignKey(
                        name: "FK_tripAdditianActivities_additianActivites_additianActivitiesId",
                        column: x => x.additianActivitiesId,
                        principalTable: "additianActivites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tripAdditianActivities_trips_tripId",
                        column: x => x.tripId,
                        principalTable: "trips",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TripSupimage",
                columns: table => new
                {
                    TripId = table.Column<int>(type: "int", nullable: false),
                    SupImg = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TripSupimage", x => new { x.SupImg, x.TripId });
                    table.ForeignKey(
                        name: "FK_TripSupimage_trips_TripId",
                        column: x => x.TripId,
                        principalTable: "trips",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_activitiesSupImgs_AdditianActivitiesId",
                table: "activitiesSupImgs",
                column: "AdditianActivitiesId");

            migrationBuilder.CreateIndex(
                name: "IX_AdditianActivitiesTrip_TripsId",
                table: "AdditianActivitiesTrip",
                column: "TripsId");

            migrationBuilder.CreateIndex(
                name: "IX_hotelSupImgs_HotelId",
                table: "hotelSupImgs",
                column: "HotelId");

            migrationBuilder.CreateIndex(
                name: "IX_rooms_HotelId",
                table: "rooms",
                column: "HotelId");

            migrationBuilder.CreateIndex(
                name: "IX_tripAdditianActivities_additianActivitiesId",
                table: "tripAdditianActivities",
                column: "additianActivitiesId");

            migrationBuilder.CreateIndex(
                name: "IX_TripSupimage_TripId",
                table: "TripSupimage",
                column: "TripId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "activitiesSupImgs");

            migrationBuilder.DropTable(
                name: "AdditianActivitiesTrip");

            migrationBuilder.DropTable(
                name: "hotelSupImgs");

            migrationBuilder.DropTable(
                name: "rooms");

            migrationBuilder.DropTable(
                name: "tripAdditianActivities");

            migrationBuilder.DropTable(
                name: "TripSupimage");

            migrationBuilder.DropTable(
                name: "hotels");

            migrationBuilder.DropTable(
                name: "additianActivites");

            migrationBuilder.DropTable(
                name: "trips");
        }
    }
}
