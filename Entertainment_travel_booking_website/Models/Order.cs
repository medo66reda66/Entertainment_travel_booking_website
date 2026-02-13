using System.ComponentModel.DataAnnotations;


namespace Entertainment_travel_booking_website.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; } // يرتبط بـ ApplicationUser
        public ApplicationUser User { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.Now;
        public decimal TotalPrice { get; set; }
        public int Quantity { get; set; }

        [Required]
        public string CardName { get; set; }
        [Required]
        public string CardNumber { get; set; }
        [Required]
        public string ExpiryDate { get; set; }
        [Required]
        public string CVV { get; set; }

        public List<OrderItem> OrderItems { get; set; }
    }
}
