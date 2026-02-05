using System.ComponentModel.DataAnnotations;

namespace Entertainment_travel_booking_website.modelVM
{
    public class HotelCreateVM
    {
        // ----------------- Create VM -----------------
        
            [Required]
            public string Name { get; set; }

            [Required]
            public string Location { get; set; }

            [Required]
            public string Description { get; set; }

            [Required]
            [Range(0, double.MaxValue, ErrorMessage = "Price must be positive")]
            public decimal PricePerNight { get; set; }

            [Required]
            public int AvailableRooms { get; set; }

            public bool Status { get; set; } = true;

            [Display(Name = "Main Image")]
            public IFormFile MainImage { get; set; }

            [Display(Name = "Additional Images")]
            public List<IFormFile> SupImages { get; set; }
        }
}
