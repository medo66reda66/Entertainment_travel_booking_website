using Ecommerce.Utilities;
using Entertainment_travel_booking_website.DataBase;
using Entertainment_travel_booking_website.Models;
using Entertainment_travel_booking_website.Utilities.IDbInitial;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Entertainment_travel_booking_website.Utilities
{
    public class DbInitializer: IDbIntializer
    {
        private readonly ApplicationDbContext _context;
        private readonly Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> _userManager;
        private readonly Microsoft.AspNetCore.Identity.RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<DbInitializer> _logger;

        public DbInitializer(ApplicationDbContext context, Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager,
            Microsoft.AspNetCore.Identity.RoleManager<IdentityRole> roleManager, ILogger<DbInitializer> logger)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        public void Initializ()
        {
            try
            {
                if (_context.Database.GetAppliedMigrations().Any())
                {
                    _context.Database.Migrate();
                }
                if (_roleManager.Roles.IsNullOrEmpty())
                {
                    _roleManager.CreateAsync(new(DS.SUPER_ADMIN_ROLE)).GetAwaiter().GetResult();
                    _roleManager.CreateAsync(new(DS.ADMIN_ROLE)).GetAwaiter().GetResult();
                    _roleManager.CreateAsync(new(DS.EMPLOYEE_ROLE)).GetAwaiter().GetResult();
                    _roleManager.CreateAsync(new(DS.CUSTOMER_ROLE)).GetAwaiter().GetResult();

                    _userManager.CreateAsync(new ApplicationUser
                    {
                        UserName = "superAdmin@Errasoft.edu.eg",
                        Email = "superAdmin@Errasoft.edu.eg",
                        EmailConfirmed = true,
                        firstName = "Super",
                        lastName = "Admin",
                    }, "SuperAdmin123*").GetAwaiter().GetResult();

                    var user = _userManager.FindByEmailAsync("superAdmin@Errasoft.edu.eg").GetAwaiter().GetResult();
                    _userManager.AddToRoleAsync(user!, DS.SUPER_ADMIN_ROLE).GetAwaiter().GetResult();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while initializing the database.");

            }
        }

    }
}
