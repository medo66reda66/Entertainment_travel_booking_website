using Entertainment_travel_booking_website.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Entertainment_travel_booking_website.modelVM
{
    public class RoomVM
    {
        public int ID { get; set; }
        public string? Description { get; set; }
        public RoomType Type { get; set; }
        public string locationInHotel { get; set; } = string.Empty;
        public bool Availability { get; set; }
        public int HotelId { get; set; } // عشان نربطها بالفندق
        public IEnumerable<SelectListItem>? HotelList { get; set; }
    }
}
