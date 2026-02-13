using Entertainment_travel_booking_website.DataBase;
using Entertainment_travel_booking_website.Models;
using Entertainment_travel_booking_website.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe.Checkout;
using System.Security.Claims;

namespace Entertainment_travel_booking_website.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class PaymentController : Controller
    {
        
        private readonly IRepository<Trip> _tripGenericRepo;
        private readonly IRepository<Order> _orderGenericRepo;
        private readonly IRepository<OrderItem> _orderItemGenericRepo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public PaymentController(UserManager<ApplicationUser> userManager, ApplicationDbContext context, IRepository<Trip> tripGenericRepo, IRepository<Order> orderGenericRepo, IRepository<OrderItem> orderItemGenericRepo)
        {
            _userManager = userManager;
            _context = context;
            _tripGenericRepo = tripGenericRepo;
            _orderGenericRepo = orderGenericRepo;
            _orderItemGenericRepo = orderItemGenericRepo;
        }

        public async Task<IActionResult> Pay(int tripId, List<int>? SelectedActivityIds, CancellationToken cancellationToken, int Quantity = 1)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user is null)
                return NotFound();

            var trip = await _tripGenericRepo.GetOneAsync(e => e.Id == tripId);

            if (trip is null) return NotFound();

            SelectedActivityIds ??= new List<int>();

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
            decimal totalPrice = unitPrice;
            
             Random random = new Random();
                int sixDigitNumber = random.Next(100000, 1000000);
            try
            {
                var order = new Order
                {
                    UserId = user.Id,
                    OrderDate = DateTime.UtcNow,
                    TotalPrice = (totalPrice * Quantity),
                    Quantity = Quantity,
                    ExpiryDate = trip.StartDate,
                    CardNumber = sixDigitNumber.ToString(),
                    OrderItems = new List<OrderItem>
                {
                    new OrderItem
                    {
                        TripId = trip.Id,
                        Quantity = Quantity,
                        Price = (totalPrice*Quantity),
                        TripName = trip.Place
                    }
                }
                };
                await _orderGenericRepo.AddAsync(order, cancellationToken);
                await _orderGenericRepo.CommitAsync(cancellationToken);

                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes = new List<string> { "card" },
                    LineItems = new List<SessionLineItemOptions>(),
                    Mode = "payment",
                    SuccessUrl = $"{Request.Scheme}://{Request.Host}/customer/MyTrips/success",
                    CancelUrl = $"{Request.Scheme}://{Request.Host}/customer/MyTrips/cancel/{trip.Id}",
                };

                options.LineItems.Add(new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "egp",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = trip.Place,
                            Description = trip.Description
                        },
                        UnitAmount = (long)totalPrice * 100,
                    },
                    Quantity = Quantity
                });

                var service = new SessionService();
                var session = service.Create(options);
                return Redirect(session.Url);
            }
            catch (Exception ex)
            {
                // Log the exception (ex) here as needed
                return RedirectToAction("cancel", "MyTrips", new { area = "Customer" });
            }
        }

    }
}