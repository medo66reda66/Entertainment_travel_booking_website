using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace Entertainment_travel_booking_website.modelVM
{
    public class AdditionalActivityVM
    {
        public int Id { get; set; }
        public string Place { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public DateTime Date { get; set; }

        public IFormFile? MainImageFile { get; set; }   
        public string? ExistingMainImage { get; set; }

   
        public List<IFormFile>? SubImageFiles { get; set; } 
        public List<string>? ExistingSubImages { get; set; }
    }
}
