using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace Entertainment_travel_booking_website.modelVM
{
    public class TripCreateVM
    {
        public string Place { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal? DiscountedPrice { get; set; }
        public int AvailableSeats { get; set; }
        public int MaxPeople { get; set; }
        public bool Status { get; set; }

        // Main Image
        public IFormFile MainImage { get; set; }

        // Sub Images
        public List<IFormFile> SupImages { get; set; }

        // Optional selections
        public int? HotelId { get; set; }                    // 🆕
        public int? AdditionalActivityId { get; set; }       // 🆕
    }
}
