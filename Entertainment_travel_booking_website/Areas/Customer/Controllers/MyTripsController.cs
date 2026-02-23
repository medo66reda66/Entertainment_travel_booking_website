using Entertainment_travel_booking_website.DataBase;
using Entertainment_travel_booking_website.Models;
using Entertainment_travel_booking_website.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

[Area("Customer")]
[Authorize]
public class MyTripsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IRepository<Trip> _tripGenericRepo;
    private readonly IEmailSender _emailSender;


    public MyTripsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IRepository<Trip> tripGenericRepo, IEmailSender emailSender)
    {
        _context = context;
        _userManager = userManager;
        _tripGenericRepo = tripGenericRepo;
        _emailSender = emailSender;
    }

    public async Task<IActionResult> success()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound();
        }
        var order = await _context.Orders
            .Where(o => o.UserId == user.Id )
            .OrderByDescending(o => o.Id).FirstOrDefaultAsync();

        await _emailSender.SendEmailAsync(
user.Email!,
"Your Trip Is Confirmed ✅",
$@"
    <div style='font-family: Arial, sans-serif; background-color:#f4f6f8; padding:20px;'>
        <div style='max-width:600px; margin:auto; background-color:#ffffff; 
                    border-radius:12px; padding:25px; box-shadow:0 4px 10px rgba(0,0,0,0.1);'>
            
            <h2 style='color:#0d6efd; text-align:center;'>
                ✈️ Trip Confirmed Successfully!
            </h2>

            <p style='font-size:16px; color:#333;'>
                Hello <strong style='color:#000;'>{user.UserName}</strong>,
            </p>

            <p style='font-size:15px; color:#555;'>
                We’re excited to let you know that your booking has been
                <strong style='color:green;'>successfully confirmed</strong> 🎉
            </p>

            <div style='background-color:#f1f8ff; padding:15px; border-radius:8px; margin:20px 0;'>
                <p style='margin:0; font-size:15px; color:#333;'>
                    <strong>📄 Booking Number:</strong>
                </p>
                <p style='margin:5px 0 0; font-size:18px; color:#0d6efd; font-weight:bold;'>
                    {order.CardNumber}
                </p>
            </div>

            <p style='font-size:15px; color:#555;'>
                You are all set and ready for your trip. Pack your bags and get ready for an
                amazing experience 🌍
            </p>

            <p style='font-size:15px; color:#555;'>
                We wish you a <strong>wonderful journey</strong> and unforgettable memories!
            </p>

            <hr style='margin:25px 0;' />

            <p style='font-size:14px; color:#888; text-align:center;'>
                💙 Travel Booking Team <br/>
                <span style='font-size:12px;'>Thank you for choosing us</span>
            </p>
        </div>
    </div>
    "
);


        return View(); 
    }
    public async Task<IActionResult> cancel()
    {
        return View();
    }
    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);

        var orders = await _context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(i => i.Trip.Hotel)
            .Where(o => o.UserId == user.Id).OrderByDescending(e=>e.Id)
            .ToListAsync();

        return View(orders);
    }
}
