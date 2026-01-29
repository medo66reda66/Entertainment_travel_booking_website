namespace Entertainment_travel_booking_website.Models
{
    public class Hotel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Location { get; set; }
        public decimal? PricePerNight { get; set; }
        public string? Description { get; set; }=string.Empty;
        public string? Image { get; set; }
        public bool Availability { get; set; }
        public decimal? Rate { get; set; }
        public List<Room>? Rooms { get; set; }
        public List<HotelSupImg>? HotelSupImgs { get; set; }


    }
}
