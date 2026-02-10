using Entertainment_travel_booking_website.Models;
using System.Collections.Generic;

namespace Entertainment_travel_booking_website.modelVM
{
    public class CartVM
    {
        public Trip Trip { get; set; } = new Trip();
        public List<AdditianActivities> Activities { get; set; } = new List<AdditianActivities>();
        public decimal TotalPrice { get; set; }
        public int CartItemId { get; set; } // لزر Remove
    }
}
