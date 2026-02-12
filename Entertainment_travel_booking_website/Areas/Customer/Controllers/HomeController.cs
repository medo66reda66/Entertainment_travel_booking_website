using Entertainment_travel_booking_website.DataBase;
using Entertainment_travel_booking_website.Models;
using Entertainment_travel_booking_website.Models.ViewModels;
using Entertainment_travel_booking_website.modelVM;
using Entertainment_travel_booking_website.Repository;
using Entertainment_travel_booking_website.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

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

            // 🔥 حساب السعر بعد الخصم
            decimal finalTripPrice = trip.Price;

            if (trip.DiscountedPrice != null && trip.DiscountedPrice > 0)
            {
                finalTripPrice -= (trip.Price * trip.DiscountedPrice.Value / 100);
            }

            var vm = new TripDetailVM
            {
                Trip = trip,
                Hotel = trip.Hotel,
                AdditionalActivities = activities,
                TotalPrice = finalTripPrice
            };

            return View(vm);
        }


        [ValidateAntiForgeryToken]
        public IActionResult BookNow(int tripId, int quantity, List<int>? SelectedActivityIds)
        {
            var trip = _Context.trips.FirstOrDefault(t => t.Id == tripId);
            if (trip == null) return NotFound();

            SelectedActivityIds ??= new List<int>();

            // حساب سعر الرحلة بعد الخصم
            decimal finalPrice = trip.Price;
            if (trip.DiscountedPrice != null && trip.DiscountedPrice > 0)
            {
                finalPrice -= (trip.Price * trip.DiscountedPrice.Value / 100);
            }

            // ===== حساب سعر الأنشطة الإضافية =====
            decimal activitiesTotal = 0;
            if (SelectedActivityIds != null && SelectedActivityIds.Count > 0)
            {
                activitiesTotal = _Context.tripAdditianActivities
                    .Where(x => SelectedActivityIds.Contains(x.additianActivitiesId))
                    .Select(x => x.additianActivities.Price)
                    .Sum();
            }

            // السعر النهائي للفرد الواحد = سعر الرحلة + سعر الأنشطة
            decimal finalUnitPrice = finalPrice + activitiesTotal;

            // السعر النهائي الإجمالي = السعر للفرد × الكمية
            decimal totalPrice = finalUnitPrice * quantity;

            // إنشاء Order مع OrderItems (للتوافق مع PaymentController)
            var order = new Order
            {
                Quantity = quantity,
                TotalPrice = totalPrice,
                OrderDate = DateTime.Now,
                UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                OrderItems = new List<OrderItem>
        {
            new OrderItem
            {
                TripId = trip.Id,
                TripName = trip.Place,
                Price = finalUnitPrice, // سعر الفرد الواحد شامل الأنشطة
                Quantity = quantity
            }
        }
            };

            _Context.Orders.Add(order);
            _Context.SaveChanges();

            return RedirectToAction("Index");
        }
        // ------------------- صفحة Cart -------------------

        //[Authorize]
        //public IActionResult MyTrips()
        //{
        //    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        //    var orders = oreder.GetAll(o => o.UserId == userId, includeProperties: "Trip,Trip.Hotel");

        //    var tripsList = orders.Select(o => new MyTripsVM
        //    {
        //        OrderId = o.Id,
        //        TripName = o.Trip.Place,
        //        HotelName = o.Trip.Hotel != null ? o.Trip.Hotel.Name : "",
        //        Price = o.TotalPrice,
        //        Quantity = o.Quantity,
        //        BookingDate = o.OrderDate
        //    }).ToList();

        //    return View(tripsList);
        //}

    }
}
