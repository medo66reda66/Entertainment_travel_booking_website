using System.ComponentModel.DataAnnotations;

namespace Entertainment_travel_booking_website.modelVM
{
    public class TripCreateVM
    {
       public int Id { get; set; }
        [Required]
        public string Place { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string? Description { get; set; }

        public decimal Price { get; set; }
        public decimal? DiscountedPrice { get; set; }

        public IFormFile MainImage { get; set; }  

        public List<IFormFile>? SupImages { get; set; } 

        public int AvailableSeats { get; set; }
        public int MaxPeople { get; set; }
        public bool Status { get; set; }
    }
}
