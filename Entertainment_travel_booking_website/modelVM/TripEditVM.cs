using Microsoft.AspNetCore.Http;
using Entertainment_travel_booking_website.Models;
using System.Collections.Generic;

namespace Entertainment_travel_booking_website.modelVM
{
    public class TripEditVM
    {
        public int Id { get; set; }
        public string Place { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal? DiscountedPrice { get; set; }
        public int AvailableSeats { get; set; }
        public int MaxPeople { get; set; }
        public bool Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Description { get; set; }

        public int? HotelId { get; set; }

        // ❌ شيل دي
        // public int? AdditionalActivityId { get; set; }

        // ✅ اختيارات متعددة
        public List<int> SelectedActivityIds { get; set; } = new();

        // صور جديدة
        public IFormFile? MainImage { get; set; }
        public List<IFormFile>? SupImages { get; set; }

        // صور موجودة
        public string? ExistingMainImage { get; set; }
        public List<TripSupimage>? ExistingSupImages { get; set; }
    }
}
