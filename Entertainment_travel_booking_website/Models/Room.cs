namespace Entertainment_travel_booking_website.Models
{
    public enum RoomType
    {
        Single,
        Double,
        Vip,
    }
    public class Room
    {
        public int ID { get; set; }
        public string? Description { get; set; } = string.Empty;
        public RoomType Type { get; set; }
        public string locationInHotel { get; set; } = string.Empty;
        public bool Availability { get; set; }
        public int HotelId { get; set; } // رقم الفندق (الرابط)
        public Hotel? Hotel { get; set; } // الفندق نفسه (للوصول لبياناته)

    }
}
