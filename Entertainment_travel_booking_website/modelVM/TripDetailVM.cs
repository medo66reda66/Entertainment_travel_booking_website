using Entertainment_travel_booking_website.modelVM;
using System.Collections.Generic;

namespace Entertainment_travel_booking_website.Models.ViewModels
{
    public class TripDetailVM
    {
        public Trip Trip { get; set; } = new Trip();
        public int TripId { get; set; }
        public Hotel Hotel { get; set; } = new Hotel();
        public List<AdditianActivities> AdditionalActivities { get; set; } = new List<AdditianActivities>();

        public List<int> SelectedActivityIds { get; set; } = new List<int>(); // لتخزين الأنشطة المختارة
        public decimal TotalPrice { get; set; } // السعر النهائي بعد إضافة الأنشطة
    
        public int Count { get; set; }
    
    }
}
