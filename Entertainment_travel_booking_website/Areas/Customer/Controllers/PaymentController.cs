using Entertainment_travel_booking_website.Models;
using Entertainment_travel_booking_website.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic; // عشان List<OrderItem>
using System.Threading.Tasks;

[Area("Customer")]
[Authorize]
public class PaymentController : Controller
{
    private readonly ITripRepository _tripRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly UserManager<ApplicationUser> _userManager;

    public PaymentController(
        ITripRepository tripRepository,
        IOrderRepository orderRepository,
        UserManager<ApplicationUser> userManager)
    {
        _tripRepository = tripRepository;
        _orderRepository = orderRepository;
        _userManager = userManager;
    }

    // GET
    public async Task<IActionResult> Checkout(int tripId)
    {
        var trip = await _tripRepository.GetAsync(tripId);
        if (trip == null) return NotFound();

        var model = new Order
        {
            TotalPrice = trip.Price,
            OrderItems = new List<OrderItem>
            {
                new OrderItem
                {
                    TripId = trip.Id,
                    TripName = trip.Place,
                    Price = trip.Price,
                    Quantity = 1
                }
            }
        };

        return View(model);
    }

    // POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Checkout(Order model, int tripId)
    {
        var trip = await _tripRepository.GetAsync(tripId);
        if (trip == null) return NotFound();

        if (!ModelState.IsValid)
        {
            model.TotalPrice = trip.Price;
            return View(model);
        }

        var user = await _userManager.GetUserAsync(User);
        model.UserId = user.Id;
        model.OrderDate = DateTime.Now;
        model.TotalPrice = trip.Price;

        model.OrderItems = new List<OrderItem>
        {
            new OrderItem
            {
                TripId = trip.Id,
                TripName = trip.Place,
                Price = trip.Price,
                Quantity = 1
            }
        };

        await _orderRepository.AddOrderAsync(model);

        return RedirectToAction("Success");
    }

    public IActionResult Success()
    {
        return View();
    }
}
