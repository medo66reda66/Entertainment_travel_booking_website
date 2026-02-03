using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entertainment_travel_booking_website.Migrations
{
    /// <inheritdoc />
    public partial class addDataToHotelModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "hotels", // اتأكد من اسم الـ DbSet عندك (Hotels ولا Hotel؟)
                columns: new[] { "Id", "Name", "Location", "PricePerNight", "Description", "Image", "Availability", "Rate" },
                values: new object[,]
                {
            { 1, "Steigenberger Al Dau", "Hurghada", 4500m, "Luxury beach resort", "h1.jpg", true, 4.9m },
            { 2, "Four Seasons Resort", "Sharm El Sheikh", 7000m, "World-class service", "h2.jpg", true, 4.8m },
            { 3, "Old Cataract", "Aswan", 5500m, "Historic hotel with Nile view", "h3.jpg", true, 4.7m },
            { 4, "Mena House", "Cairo", 4000m, "Stay right in front of Pyramids", "h4.jpg", true, 4.9m },
            { 5, "Tolip Hotel", "Alexandria", 2500m, "Beautiful Mediterranean view", "h5.jpg", false, 4.2m },
            { 6, "Basma Hotel", "Aswan", 1800m, "Comfortable stay with great vibes", "h6.jpg", true, 4.0m }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "hotels",
                keyColumn: "Id",
                keyValue: new object[] { 1, 2, 3, 4, 5, 6 });
        }
    }
}
