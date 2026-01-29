namespace Entertainment_travel_booking_website.Models
{
    public class Trip
    {
        public int Id { get; set; }
        public string Place { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal? DiscountedPrice { get; set; }
        public string Image { get; set; } = string.Empty;
        public int AvailableSeats { get; set; }
        public decimal? Rate { get; set; }
        public int MaxPeople { get; set; }
        public bool Status { get; set; }
        public List<TripSupimage>? TripSupimages { get; set; }
        public List<AdditianActivities>? AdditianActivities { get; set; }


    }
}
