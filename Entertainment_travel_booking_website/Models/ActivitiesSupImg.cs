namespace Entertainment_travel_booking_website.Models
{
    public class ActivitiesSupImg
    {
        public int Id { get; set; }
        public int AdditianActivitiesId { get; set; }
        public AdditianActivities? AdditianActivities { get; set; }  
        public string? SupImg { get; set; }
    }
}
