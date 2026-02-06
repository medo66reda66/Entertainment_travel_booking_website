using System.ComponentModel.DataAnnotations;

namespace Entertainment_travel_booking_website.modelVM
{
    public class ValidateOtpVM
    {
        public int Id { get; set; }
        [Required]
        public string OtpCode { get; set; } = string.Empty;
        public string ApplicationUserId { get; set; } = string.Empty;
    }
}
