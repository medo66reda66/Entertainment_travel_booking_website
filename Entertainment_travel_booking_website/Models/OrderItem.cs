namespace Entertainment_travel_booking_website.Models
{
    public class OrderItem
    {
        public int Id { get; set; }

        public int TripId { get; set; }
        public string TripName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }
    }
}
