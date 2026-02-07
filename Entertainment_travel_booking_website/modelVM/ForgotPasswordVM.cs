using System.ComponentModel.DataAnnotations;

namespace Entertainment_travel_booking_website.modelVM
{
    public class ForgotPasswordVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;
    }
}
