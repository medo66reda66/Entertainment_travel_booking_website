using Entertainment_travel_booking_website.Models;
using System.Collections.Generic;

namespace Entertainment_travel_booking_website.Models.ViewModels
{
    public class HotelDetailVM
    {
        public Hotel Hotel { get; set; }                  // بيانات الفندق الأساسية
        public List<HotelSupImg> Images { get; set; }   // الصور الفرعية للفندق
        public List<Room> Rooms { get; set; }             // الغرف المرتبطة بالفندق
    
    }
}
