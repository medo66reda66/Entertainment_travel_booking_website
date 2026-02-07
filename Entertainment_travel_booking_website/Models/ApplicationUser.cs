using Microsoft.AspNetCore.Identity;

namespace Entertainment_travel_booking_website.Models
{
    public class ApplicationUser: IdentityUser
    {
        public string firstName { get; set; } = string.Empty;
        public string lastName { get; set; } = string.Empty;
        public string? Address { get; set; }

    }
}
