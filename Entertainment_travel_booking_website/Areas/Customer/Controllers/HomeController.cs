using Entertainment_travel_booking_website.DataBase;
using Entertainment_travel_booking_website.Models;
using Entertainment_travel_booking_website.Models.ViewModels;
using Entertainment_travel_booking_website.modelVM;
using Entertainment_travel_booking_website.Repository;
using Entertainment_travel_booking_website.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Entertainment_travel_booking_website.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]

    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _Context;
        private readonly ICartRepository _cartRepo;

        public HomeController(ApplicationDbContext context, ICartRepository cartRepo)
        {
            _Context = context;
            _cartRepo = cartRepo;
        }

        // ------------------- الصفحة الرئيسية + Pagination -------------------
        public IActionResult Index(int page = 1, string? destination = null, decimal? minPrice = null, decimal? maxPrice = null, int? hotelId = null, bool? available = null)
        {
            int pageSize = 4;
            var query = _Context.trips.Include(t => t.Hotel).AsQueryable();

            // ======== فلترة الوجهة ========
            if (!string.IsNullOrEmpty(destination))
                query = query.Where(t => t.Place.Contains(destination));

            // ======== فلترة السعر ========
            if (minPrice.HasValue)
                query = query.Where(t => t.Price >= minPrice.Value);

            if (maxPrice.HasValue)
                query = query.Where(t => t.Price <= maxPrice.Value);

            // ======== فلترة الفندق ========
            if (hotelId.HasValue)
                query = query.Where(t => t.Hotel != null && t.Hotel.Id == hotelId.Value);

            // ======== فلترة متاح فقط ========
            if (available.HasValue && available.Value)
                query = query.Where(t => t.Status == true);

            // ======== Pagination ========
            var totalItems = query.Count();
            var trips = query
                .OrderByDescending(t => t.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            // ======== الوجهات الشائعة ========
            ViewBag.PopularDestinations = _Context.trips
                .GroupBy(t => t.Place)
                .Select(g => new { Name = g.Key, Count = g.Count(), Image = g.First().Image })
                .Take(4)
                .ToList();

            // ======== قائمة الفنادق للفلتر ========
            ViewBag.Hotels = _Context.hotels.ToList();
            // تشيك لو الطلب AJAX
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                // رجع Partial View فيه كروت الرحلات بس (بدون Layout)
                return PartialView("_TripsListPartial", trips);
            }

            return View(trips);
        }

        // ------------------- تفاصيل الرحلة -------------------
        public IActionResult Detail(int id)
        {
            var trip = _Context.trips
                .Include(t => t.TripSupimages)
                .Include(t => t.Hotel)
                    .ThenInclude(h => h.HotelSupImgs)
                .FirstOrDefault(t => t.Id == id);

            if (trip == null) return NotFound();

            var activities = _Context.additianActivites
                .Include(a => a.ActivitiesSupImgs)
                .Where(a => a.TripAdditianActivities.Any(t => t.tripId == id))
                .ToList();

            foreach (var act in activities)
            {
                act.MainImg = act.ActivitiesSupImgs?.FirstOrDefault()?.SupImg ?? "default-activity.jpg";
            }

            var vm = new TripDetailVM
            {
                Trip = trip,
                Hotel = trip.Hotel,
                AdditionalActivities = activities,
                TotalPrice = trip.Price
            };

            return View(vm);
        }
        public IActionResult ActivityDetail(int id)
        {
            var activity = _Context.additianActivites
                .Include(a => a.ActivitiesSupImgs)
                .FirstOrDefault(a => a.Id == id);

            if (activity == null) return NotFound();

            return View(activity);
        }
        public IActionResult HotelDetail(int id)
        {
            var hotel = _Context.hotels
                .Include(h => h.HotelSupImgs)  
                .Include(h => h.Rooms)         
                .FirstOrDefault(h => h.Id == id);

            if (hotel == null) return NotFound();

        
            var vm = new HotelDetailVM
            {
                Hotel = hotel,
                Images = hotel.HotelSupImgs.ToList(),
                Rooms = hotel.Rooms.ToList()
            };

            return View(vm);
        }



        // ------------------- صفحة Cart -------------------
        [HttpGet]
        public IActionResult Cart()
        {
            var userId = User.Identity?.Name ?? "guest";
         
            var cartItems = _cartRepo.GetCartItems(userId);
            return View(cartItems);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult cart(int TripId, List<int> SelectedActivityIds, int Quantity = 1) // <-- إضافة Quantity
        {
            var userId = User.Identity?.Name ?? "guest";

            // التأكد من وجود الرحلة
            var trip = _Context.trips.FirstOrDefault(t => t.Id == TripId);
            if (trip == null) return NotFound();

          
            SelectedActivityIds ??= new List<int>();
  var activities = _Context.additianActivites
                .Where(a => SelectedActivityIds.Contains(a.Id))
                .ToList();

       
            decimal totalPrice = (trip.Price + activities.Sum(a => a.Price)) * Quantity;

           
            _cartRepo.AddToCart(userId, TripId, SelectedActivityIds, totalPrice, Quantity);

   
            return RedirectToAction("Cart");
        }

        [HttpPost]
        public IActionResult RemoveFromCart(int cartItemId)
        {
            _cartRepo.RemoveCartItem(cartItemId);
            return RedirectToAction("Cart");
        }

        [HttpPost]
        public IActionResult Payment()
        {
            var userId = User.Identity?.Name ?? "guest";
            _cartRepo.ClearCart(userId);
            TempData["PaymentMessage"] = "تمت عملية الدفع بنجاح ✅";
            return RedirectToAction("Cart");
        }
    }
}
