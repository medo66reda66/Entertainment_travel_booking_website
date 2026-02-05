namespace Entertainment_travel_booking_website.Models
{
    public class AdditianActivities
    {
        public int Id { get; set; }
        public string Place { get; set; }=string.Empty;
        public string? Description { get; set; }=string.Empty;
        public decimal Price { get; set; }
        public DateTime Date { get; set; }
        public List<Trip>? Trips { get; set; }
        //public string MainImage { get; set; }
        public List<ActivitiesSupImg>? ActivitiesSupImgs { get; set; }

    }
}
