using System.ComponentModel.DataAnnotations;

namespace Entertainment_travel_booking_website.modelVM
{
    public class NewPasswordVM
    {
        public int id { get; set; }
        [Required, DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
        [Required, DataType(DataType.Password), Compare(nameof(Password))]
        public string Confirmpassword { get; set; } = String.Empty;
        public string Applicationuserid { get; set; } = string.Empty;
    }
}
