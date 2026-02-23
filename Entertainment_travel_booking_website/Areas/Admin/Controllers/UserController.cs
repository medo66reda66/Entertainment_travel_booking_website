using Entertainment_travel_booking_website.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ecommerce.Utilities;

namespace Travel_booking_website.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = $"{DS.SUPER_ADMIN_ROLE}")]
    // تأكد أن الأدمن فقط هو من يدخل
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        // عرض قائمة المستخدمين
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            return View(users);
        }

        // ميثود لعمل بلوك أو فك البلوك
        [HttpPost]
        public async Task<IActionResult> LockUnlock([FromBody] string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return Json(new { success = false, message = "User not found" });

            if (await _userManager.IsInRoleAsync(user, DS.SUPER_ADMIN_ROLE))
            {
                TempData["error-Notification"] = "no super Admin lock";
                return RedirectToAction(nameof(Index));
            }

            // إذا كان اليوزر معموله بلوك حالياً، نفكه
            if (user.LockoutEnd != null && user.LockoutEnd > DateTime.Now)
            {
                user.LockoutEnd = DateTime.Now; // فك البلوك
            }
            else
            {
                user.LockoutEnd = DateTime.Now.AddDays(30); 
            }

            await _userManager.UpdateAsync(user);
            return Json(new { success = true, message = "Operation Successful" });
        }
    }
}
