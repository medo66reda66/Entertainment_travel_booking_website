using Entertainment_travel_booking_website.Models;

public class CartItem
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public int TripId { get; set; }
    public Trip Trip { get; set; } = new Trip();
    public List<AdditianActivities> SelectedActivities { get; set; } = new List<AdditianActivities>();
    public decimal TotalPrice { get; set; }
}
