namespace Entertainment_travel_booking_website.Models
{
    public class ApplicationUserOtp
    {
        public string Id { get; set; }
        public string OtpCode { get; set; } = string.Empty;
        public DateTime validto { get; set; }
        public DateTime createAt { get; set; }= DateTime.Now;
        public bool isvalid { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

    }
}
