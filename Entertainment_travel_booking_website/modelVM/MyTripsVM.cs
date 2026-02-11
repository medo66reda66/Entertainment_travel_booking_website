namespace Entertainment_travel_booking_website.modelVM
{
    public class MyTripsVM
    {
        public int OrderId { get; set; }
        public string TripName { get; set; }
        public string HotelName { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public DateTime BookingDate { get; set; }
    }
}
