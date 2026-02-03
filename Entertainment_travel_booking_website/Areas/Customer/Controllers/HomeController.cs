using Entertainment_travel_booking_website.DataBase;
using Entertainment_travel_booking_website.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Entertainment_travel_booking_website.Areas.Home.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _Context ;
        public HomeController(ILogger<HomeController> logger,ApplicationDbContext context)
        {
            _logger = logger;
            _Context = context;
        }
        public IActionResult Index(string? destination, DateTime? startDate, DateTime? endDate, decimal? maxPrice, int page = 1)
        {
           
            var trips = _Context.trips.AsNoTracking().AsQueryable();

           
            if (!string.IsNullOrEmpty(destination))
            {
                trips = trips.Where(t => t.Place.Contains(destination));
            }

            if (maxPrice.HasValue && maxPrice > 0)
            {
                trips = trips.Where(t => t.Price <= maxPrice.Value);
            }

            if (startDate.HasValue)
            {
                trips = trips.Where(t => t.StartDate.Date >= startDate.Value.Date);
            }

            if (endDate.HasValue)
            {
                trips = trips.Where(t => t.EndDate.Date <= endDate.Value.Date);
            }

           
            int pageSize = 4;
            var totalTrips = trips.Count();


            ViewBag.TotalPages = (int)Math.Ceiling(totalTrips / (double)pageSize);
            ViewBag.CurrentPage = page;

           
            var filteredTrips = trips.AsNoTracking()
                                     .OrderByDescending(t => t.Id) 
                                     .Skip((page - 1) * pageSize)
                                     .Take(pageSize)
                                     .ToList();

           
            ViewBag.Hotels = _Context.hotels.AsNoTracking().Take(4).ToList();

         
            ViewBag.PopularDestinations = _Context.trips
                .GroupBy(t => t.Place)
                .Select(g => new
                {
                    Name = g.Key,
                    Count = g.Count(),
                    Image = g.FirstOrDefault() != null ? g.FirstOrDefault().Image : ""
                })
                .OrderByDescending(g => g.Count)
                .Take(4)
                .ToList();

          
            return View(filteredTrips);
        }
    }

    }
