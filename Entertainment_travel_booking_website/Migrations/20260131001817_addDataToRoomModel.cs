using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entertainment_travel_booking_website.Migrations
{
    /// <inheritdoc />
    public partial class addDataToRoomModel : Migration
    {
        
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.InsertData(
                table: "rooms", // تأكد لو اسم الجدول عندك "Room" شيل حرف الـ s
                columns: new[] { "ID", "Description", "Type", "locationInHotel", "Availability" },
                values: new object[,]
                {
            { 1, "Standard single room with sea view", 0, "Floor 1, Room 101", true },
            { 2, "Spacious double room for couples", 1, "Floor 1, Room 102", true },
            { 3, "Luxury VIP suite with private pool", 2, "Floor 5, Suite 501", true },
            { 4, "Cozy single room near the elevator", 0, "Floor 2, Room 205", false },
            { 5, "Double room with garden view", 1, "Floor 2, Room 208", true },
            { 6, "Exclusive VIP room with balcony", 2, "Floor 5, Suite 505", true }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "rooms",
                keyColumn: "ID",
                keyValue: new object[] { 1, 2, 3, 4, 5, 6 });
        }
    }
}
