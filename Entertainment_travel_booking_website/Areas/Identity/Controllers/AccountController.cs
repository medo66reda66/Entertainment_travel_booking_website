using Entertainment_travel_booking_website.Models;
using Entertainment_travel_booking_website.modelVM;
using Entertainment_travel_booking_website.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;


namespace Entertainment_travel_booking_website.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly IRepository<ApplicationUserOtp> _otpRepository;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IEmailSender emailSender, IRepository<ApplicationUserOtp> otpRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _otpRepository = otpRepository;
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid)
            {
                return View(registerVM);
            }

            var user = new ApplicationUser
            {
                firstName = registerVM.Firstname,
                lastName = registerVM.Lastname,
                Address = registerVM.Address,
                UserName = registerVM.Email,
                Email = registerVM.Email
            };
            var result = await _userManager.CreateAsync(user, registerVM.Password);

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var link = Url.Action(nameof(ConfirmEmail), "Account", new { userId = user.Id, token }, Request.Scheme);

            await _emailSender.SendEmailAsync(user.Email!,
                 "Confirm your email", $"Please confirm your account by clicking this link: <a href='{link}'>Confirm Email</a>");

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(registerVM);
            }
            return RedirectToAction(nameof(Login));
        }
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return NotFound();

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Email confirmation failed. Please try again.");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Email confirm");
            }

            return RedirectToAction("index", "Home", new { area = "Customer" });
        }
        public IActionResult ResendEmailConfirm()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResendEmailConfirm(ResendEmailConfirmVM resendEmailConfirmVM)
        {
            if (!ModelState.IsValid)
            {
                return View(resendEmailConfirmVM);
            }

            var user = await _userManager.FindByEmailAsync(resendEmailConfirmVM.Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "No user found with this email.");
                return View(resendEmailConfirmVM);
            }

            if (user.EmailConfirmed)
            {
                ModelState.AddModelError(string.Empty, "This email is already confirmed. Please log in.");
                return View(resendEmailConfirmVM);
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var link = Url.Action(nameof(ConfirmEmail), "Account", new { userId = user.Id, token }, Request.Scheme);

            await _emailSender.SendEmailAsync(user.Email!,
                 "Confirm your Resent email", $"Please confirm your account by clicking this link: <a href='{link}'>Confirm Email</a>");


            return View(resendEmailConfirmVM);
        }
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordVM forgotPasswordVM, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return View(forgotPasswordVM);
            }

            var user = await _userManager.FindByEmailAsync(forgotPasswordVM.Email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                ModelState.AddModelError(string.Empty, "No user found with this email or email not confirmed.");
                return View(forgotPasswordVM);
            }

            var OTP = new Random().Next(100000, 999999).ToString();
            var userOtp = new ApplicationUserOtp
            {
                Id = Guid.NewGuid().ToString(),
                OtpCode = OTP,
                ApplicationUserId = user.Id,
                validto = DateTime.UtcNow.AddMinutes(60),
                createAt = DateTime.UtcNow,
                isvalid = true
            };
            await _otpRepository.AddAsync(userOtp);
            await _otpRepository.CommitAsync(cancellationToken);

            var userOtps = await _otpRepository.GetAsync(u => u.ApplicationUserId == user.Id, cancellationToken: cancellationToken);
            var totalOtps = userOtps.Count(e => (DateTime.UtcNow - e.createAt).TotalHours < 24);
            if (totalOtps > 5)
            {
                ModelState.AddModelError(string.Empty, "You have exceeded the maximum number of OTP requests. Please try again later.");
                return View(forgotPasswordVM);
            }

            await _emailSender.SendEmailAsync(user.Email!,
                 "Reset Password", $"Please reset your password The Otp: {OTP}</a>");
            return RedirectToAction("ValidateOtp", new { userid = user.Id });
        }
        [HttpGet]
        public IActionResult ValidateOtp(string userid)
        {
            return View(new ValidateOtpVM
            {
                ApplicationUserId = userid
            });
        }
        [HttpPost]
        public async Task<IActionResult> ValidateOtp(ValidateOtpVM validateOtp, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return View(validateOtp);
            }
            var userOtps = await _otpRepository.GetAsync(u => u.ApplicationUserId == validateOtp.ApplicationUserId && u.OtpCode == validateOtp.OtpCode, cancellationToken: cancellationToken);
            var validOtp = userOtps.FirstOrDefault(e => e.isvalid && (DateTime.UtcNow - e.createAt).TotalMinutes < 60);
            if (validOtp == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid or expired OTP. Please try again.");
                return View(validateOtp);
            }
            validOtp.isvalid = false;
            _otpRepository.Update(validOtp);
            await _otpRepository.CommitAsync(cancellationToken);
            return RedirectToAction(nameof(NewPassword), new { userid = validateOtp.ApplicationUserId });
        }
        [HttpGet]
        public IActionResult NewPassword(string userid)
        {
            return View(new NewPasswordVM
            {
                Applicationuserid = userid
            });
        }
        [HttpPost]
        public async Task<IActionResult> NewPassword(NewPasswordVM newPassword, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return View(newPassword);
            }

            var user = await _userManager.FindByIdAsync(newPassword.Applicationuserid);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "User not found.");
                return View(newPassword);
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(newPassword);
            }
            return RedirectToAction(nameof(Login));
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid)
            {
                return View(loginVM);
            }
            if (loginVM.Email == null || loginVM.Password == null)
            {
                ModelState.AddModelError(string.Empty, "Email and Password are required.");
                return View(loginVM);
            }

            var user = await _userManager.FindByEmailAsync(loginVM.Email);
            var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, loginVM.RememberMe, true);

            if (!result.Succeeded)
            {
                if (result.IsLockedOut)
                {
                    ModelState.AddModelError(string.Empty, "Your account is locked out. Please try again later.");
                    return View(loginVM);
                }
                else if (!user.EmailConfirmed)
                {
                    ModelState.AddModelError(string.Empty, "You need to confirm your email before logging in. Please check your email for the confirmation link.");
                    return View(loginVM);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt. Please check your email and password.");
                    return View(loginVM);
                }
            }
            return RedirectToAction("Index", "Home", new { area = "Customer" });
        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }
    }
}
