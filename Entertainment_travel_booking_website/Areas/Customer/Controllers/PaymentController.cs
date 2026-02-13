using Entertainment_travel_booking_website.DataBase;
using Entertainment_travel_booking_website.Models;
using Entertainment_travel_booking_website.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Entertainment_travel_booking_website.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class PaymentController : Controller
    {
        private readonly ITripRepository _tripRepo;
        private readonly IOrderRepository _orderRepo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public PaymentController(ITripRepository tripRepo, IOrderRepository orderRepo, UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _tripRepo = tripRepo;
            _orderRepo = orderRepo;
            _userManager = userManager;
            _context = context;
        }

        // GET: عرض صفحة الدفع
        [HttpGet]
        public async Task<IActionResult> Checkout(int tripId, List<int>? SelectedActivityIds, int Quantity = 1)
        {
            var trip = await _tripRepo.GetAsync(tripId);
            if (trip == null) return NotFound();

            SelectedActivityIds ??= new List<int>();

            // حساب سعر الرحلة بعد الخصم
            decimal tripPrice = trip.Price;
            if (trip.DiscountedPrice.HasValue && trip.DiscountedPrice > 0)
            {
                tripPrice -= trip.Price * (trip.DiscountedPrice.Value / 100m);
            }

            // جلب الأنشطة المختارة بشكل منفصل (بدون الاعتماد على Trip.TripAdditianActivities)
            decimal activitiesTotal = 0;
            List<AdditianActivities> selectedActivities = new List<AdditianActivities>();

            if (SelectedActivityIds.Any())
            {
                selectedActivities = await _context.additianActivites
                    .Where(a => SelectedActivityIds.Contains(a.Id))
                    .ToListAsync();

                activitiesTotal = selectedActivities.Sum(a => a.Price);
            }

            decimal unitPrice = tripPrice + activitiesTotal;
            decimal totalPrice = unitPrice * Quantity;

            var orderForDisplay = new Order
            {
                Quantity = Quantity,
                TotalPrice = totalPrice,
                OrderItems = new List<OrderItem>
        {
            new OrderItem
            {
                TripId = trip.Id,
                TripName = trip.Place,
                Price = unitPrice,
                Quantity = Quantity
            }
        }
            };

            ViewBag.Trip = trip;
            ViewBag.TripPriceAfterDiscount = tripPrice;
            ViewBag.SelectedActivityIds = SelectedActivityIds;
            ViewBag.AvailableActivities = selectedActivities;  // للعرض في الـ view
            ViewBag.AllPossibleActivities = await _context.additianActivites.ToListAsync(); // اختياري لو عايز تعرض كل الأنشطة

            return View(orderForDisplay);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> checkout(int tripId, List<int>? SelectedActivityIds, int Quantity = 1)
        {
            var trip = await _tripRepo.GetAsync(tripId);
            if (trip == null) return NotFound();

            SelectedActivityIds ??= new List<int>();

            // نفس الحساب بالضبط في الـ POST (للأمان)
            decimal tripPrice = trip.Price;
            if (trip.DiscountedPrice.HasValue && trip.DiscountedPrice > 0)
            {
                tripPrice -= trip.Price * (trip.DiscountedPrice.Value / 100m);
            }

            decimal activitiesTotal = 0;
            if (SelectedActivityIds.Any())
            {
                activitiesTotal = await _context.additianActivites
                    .Where(a => SelectedActivityIds.Contains(a.Id))
                    .Select(a => a.Price)
                    .SumAsync();
            }

            decimal unitPrice = tripPrice + activitiesTotal;
            decimal finalTotal = unitPrice * Quantity;

            var user = await _userManager.GetUserAsync(User);

            var newOrder = new Order
            {
                UserId = user.Id,
                OrderDate = DateTime.Now,
                Quantity = Quantity,
                TotalPrice = finalTotal,
                OrderItems = new List<OrderItem>
        {
            new OrderItem
            {
                TripId = trip.Id,
                TripName = trip.Place,
                Price = unitPrice,
                Quantity = Quantity
            }
        }
            };

            await _orderRepo.AddOrderAsync(newOrder);

            return RedirectToAction(nameof(Success));
        }
        public IActionResult Success()
        {
            return View();
        }
    }
}