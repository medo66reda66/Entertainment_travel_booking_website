using Entertainment_travel_booking_website.Models; // ApplicationUser
using Entertainment_travel_booking_website.modelVM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Entertainment_travel_booking_website.Areas.Identity.Controllers
{
    [Area("Identity")]
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfileController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> UpdateProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account", new { area = "Identity" });

            var model = new ApplicationUserVM
            {
                FirstName = user.firstName,
                LastName = user.lastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber ?? "",
                Address = user.Address
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(ApplicationUserVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account", new { area = "Identity" });

            // تحديث البيانات الأساسية
            user.firstName = model.FirstName;
            user.lastName = model.LastName;
            user.Address = model.Address;
            user.PhoneNumber = model.PhoneNumber;

            // تحديث البريد الإلكتروني إذا تغير
            if (user.Email != model.Email)
            {
                var setEmailResult = await _userManager.SetEmailAsync(user, model.Email);
                if (!setEmailResult.Succeeded)
                {
                    foreach (var error in setEmailResult.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);

                    return View(model);
                }

                // تحديث اسم المستخدم ليكون مطابق للبريد الإلكتروني
                user.UserName = model.Email;
            }

            // تحديث كلمة المرور إذا تم إدخالها
            if (!string.IsNullOrEmpty(model.NewPassword))
            {
                if (string.IsNullOrEmpty(model.CurrentPassword))
                {
                    ModelState.AddModelError(string.Empty, "Current password is required to change the password.");
                    return View(model);
                }

                var passwordCheck = await _userManager.CheckPasswordAsync(user, model.CurrentPassword);
                if (!passwordCheck)
                {
                    ModelState.AddModelError(string.Empty, "Current password is incorrect.");
                    return View(model);
                }

                var changePassResult = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                if (!changePassResult.Succeeded)
                {
                    foreach (var error in changePassResult.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);

                    return View(model);
                }
            }

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                foreach (var error in updateResult.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);

                return View(model);
            }

            TempData["SuccessMessage"] = "Profile updated successfully.";
            return RedirectToAction(nameof(UpdateProfile));
        }
    }
}