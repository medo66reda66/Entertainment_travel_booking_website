namespace Entertainment_travel_booking_website.Models
{
    public class HotelSupImg
    {
        public int Id { get; set; }
        public int HotelId { get; set; }
        public Hotel? Hotel { get; set; }  
        public string? SupImg { get; set; }
    }
}
