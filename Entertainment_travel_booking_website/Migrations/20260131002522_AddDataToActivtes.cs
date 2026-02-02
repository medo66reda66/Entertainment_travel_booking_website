using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entertainment_travel_booking_website.Migrations
{
    /// <inheritdoc />
    public partial class AddDataToActivtes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "additianActivites", // تأكد من الاسم في الـ DbSet (ممكن يكون AdditionalActivities)
                columns: new[] { "Id", "Place", "Description", "Price", "Date" },
                values: new object[,]
                {
            { 1, "Dahab", "Snorkeling at Blue Hole", 500m, new DateTime(2026, 5, 2) },
            { 2, "Siwa", "Sandboarding in Great Sand Sea", 300m, new DateTime(2026, 2, 12) },
            { 3, "Sharm El Sheikh", "Parasailing over the Red Sea", 800m, new DateTime(2026, 6, 21) },
            { 4, "Luxor", "Hot Air Balloon at sunrise", 1500m, new DateTime(2026, 12, 16) },
            { 5, "Hurghada", "Glass Boat trip", 400m, new DateTime(2026, 7, 7) },
            { 6, "Aswan", "Felucca ride at sunset", 250m, new DateTime(2026, 12, 18) }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "additianActivites",
                keyColumn: "Id",
                keyValue: new object[] { 1, 2, 3, 4, 5, 6 });
        }
    }
}
