using Entertainment_travel_booking_website.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entertainment_travel_booking_website.Migrations
{
    /// <inheritdoc />
    public partial class AddSixTripsSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "trips", // تأكد إن اسم الجدول في الداتا بيز عندك "Trips"
                columns: new[] { "Id", "Place", "StartDate", "EndDate", "Description", "Price", "Image", "AvailableSeats", "MaxPeople", "Status", "Rate" },
                values: new object[,]
                {
            { 1, "Dahab", new DateTime(2026, 5, 1), new DateTime(2026, 5, 7), "Diving and relaxation", 5000m, "dahab.jpg", 20, 25, true, 4.8m },
            { 2, "Siwa Oasis", new DateTime(2026, 2, 10), new DateTime(2026, 2, 15), "Safari and salt lakes", 4500m, "siwa.jpg", 15, 20, true, 4.9m },
            { 3, "Sharm El Sheikh", new DateTime(2026, 6, 20), new DateTime(2026, 6, 25), "Luxury resorts", 8500m, "sharm.jpg", 30, 40, true, 4.5m },
            { 4, "Luxor & Aswan", new DateTime(2026, 12, 15), new DateTime(2026, 12, 22), "Ancient history", 7000m, "luxor.jpg", 25, 30, true, 4.7m },
            { 5, "Hurghada", new DateTime(2026, 7, 5), new DateTime(2026, 7, 10), "Boat trips", 6200m, "hurghada.jpg", 10, 15, true, 4.6m },
            { 6, "Alexandria", new DateTime(2026, 8, 1), new DateTime(2026, 8, 3), "Mediterranean sea", 2500m, "alex.jpg", 40, 50, true, 4.2m }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // عشان لو حبيت تعمل Rollback يمسح الداتا دي
            migrationBuilder.DeleteData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: new object[] { 1, 2, 3, 4, 5, 6 });
        }
    }
}
