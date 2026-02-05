using Entertainment_travel_booking_website.Models;
using System.ComponentModel.DataAnnotations;

namespace Entertainment_travel_booking_website.modelVM
{
    public class HotelEditVM
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be positive")]
        public decimal PricePerNight { get; set; }

        public int AvailableRooms { get; set; }

        public bool Status { get; set; } = true;

        // Main Image جديدة (اختياري)
        [Display(Name = "Main Image")]
        public IFormFile? MainImage { get; set; }

        // Additional Images جديدة (اختياري)
        [Display(Name = "Additional Images")]
        public List<IFormFile>? SupImages { get; set; }

        // Existing Main Image
        public string? ExistingMainImage { get; set; }

        // Existing Sub Images
        public IEnumerable<HotelSupImg> ExistingSupImages { get; set; } = new List<HotelSupImg>();
        public int? HotelId { get; set; }
        public int? AdditionalActivityId { get; set; }

    }
}
