namespace Entertainment_travel_booking_website.Models
{
    public class TripAdditianActivities
    {
        public int tripId { get; set; }
        public Trip? trip { get; set; }
        public int additianActivitiesId { get; set; }
        public AdditianActivities? additianActivities { get; set; }
        public string? NotesOBT { get; set; }=string.Empty;
    }
}
