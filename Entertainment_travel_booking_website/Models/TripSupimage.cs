namespace Entertainment_travel_booking_website.Models
{
    public class TripSupimage
    {
        public int Id { get; set; }
        public int TripId { get; set; }
        public Trip? Trip { get; set; }  
        public string? SupImg { get; set; }
    }
}
