using Entertainment_travel_booking_website.DataBase;
using Entertainment_travel_booking_website.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

[Area("Customer")]
[Authorize]
public class MyTripsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public MyTripsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public IActionResult success()
    {

        return View();
    }
    public IActionResult cancel()
    {
        return View();
    }
    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);

        var orders = await _context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(i => i.Trip.Hotel)
            .Where(o => o.UserId == user.Id)
            .ToListAsync();

        return View(orders);
    }
}
