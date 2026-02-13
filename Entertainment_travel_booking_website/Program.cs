using Ecommerce.Utilities;
using Entertainment_travel_booking_website.DataBase;
using Entertainment_travel_booking_website.Models;
using Entertainment_travel_booking_website.Repository;
using Entertainment_travel_booking_website.Repository.IRepository;
using Entertainment_travel_booking_website.Utilities;
using Entertainment_travel_booking_website.Utilities.IDbInitial;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Stripe;
using System.Globalization;

namespace Entertainment_travel_booking_website
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ================= Add services =================
            builder.Services.AddControllersWithViews();

            // ================= DbContext =================
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("default")
                )
            );

            // ================= Identity =================
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            builder.Services.AddTransient<IEmailSender, EmailSender>();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Identity/Account/Login";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
            });

            // ================= Generic Repository =================
            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            // ================= Trip Repositories =================
            builder.Services.AddScoped<ITripRepository, TripRepository>();
            builder.Services.AddScoped<IRepository<Trip>, Repository<Trip>>();
            builder.Services.AddScoped<IRepository<TripSupimage>, Repository<TripSupimage>>();
            builder.Services.AddScoped<TripSupimgIRepository, TripSupImgsRepository>();

            // ================= Hotel Repositories =================
            builder.Services.AddScoped<IRepository<Hotel>, Repository<Hotel>>();
            builder.Services.AddScoped<IRepository<HotelSupImg>, Repository<HotelSupImg>>();
            builder.Services.AddScoped<IRepository<ApplicationUserOtp>, Repository<ApplicationUserOtp>>();
            builder.Services.AddScoped<HotelSupimgIRepository, HotelSupImgsRepository>();
            builder.Services.AddScoped<HotelRepository>();

            // ================= Additional Activities =================
            builder.Services.AddScoped<IRepository<AdditianActivities>, Repository<AdditianActivities>>();
            builder.Services.AddScoped<IRepository<ActivitiesSupImg>, Repository<ActivitiesSupImg>>();
            builder.Services.AddScoped<IAdditionalActivitySubImageRepository, AdditionalActivitySubImageRepository>();
            builder.Services.AddScoped<IAdditianActivitiesRepository, AdditianActivitiesRepository>();

            // ================= Cart =================
            builder.Services.AddScoped<ICartRepository, CartRepository>();

            // ================= Orders =================
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddLocalization(options => options.ResourcesPath = "Resourse");
            const string culture = "ar";
            var supportedCultures = new[] 
            {
                new CultureInfo(culture),
                new CultureInfo("en"),
                new CultureInfo("es"),
            };
            builder.Services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture(culture);
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

            // External Login With Google
            //builder.Services.AddAuthentication()
            //.AddGoogle("google", opt =>
            //{
            //    var googleAuth = builder.Configuration.GetSection("Authentication:Google");
            //    opt.ClientId = googleAuth["ClientId"] ?? "";
            //    opt.ClientSecret = googleAuth["ClientSecret"] ?? "";
            //    opt.SignInScheme = IdentityConstants.ExternalScheme;
            //});

            // ================= External Login (Google) =================


            builder.Services.AddAuthentication()
                .AddGoogle("google", opt =>
                {
                    var googleAuth = builder.Configuration.GetSection("Authentication:Google");
                    opt.ClientId = googleAuth["ClientId"] ?? "";
                    opt.ClientSecret = googleAuth["ClientSecret"] ?? "";
                    opt.SignInScheme = IdentityConstants.ExternalScheme;
                });

            StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];

            // ================= Database Initializer =================

            builder.Services.AddScoped<IDbIntializer, DbInitializer>();

            var app = builder.Build();

            // ================= Initialize Database =================
            //using (var scope = app.Services.CreateScope())
            //{
            //    var dbInit = scope.ServiceProvider.GetRequiredService<IDbIntializer>();
            //    dbInit.Initializ();
            //}

            // ================= HTTP Request Pipeline =================
            var scopeS = app.Services.CreateScope();
            var serviceProvider = scopeS.ServiceProvider.GetService<IDbIntializer>();
            serviceProvider?.Initializ();

            app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

            // Configure the HTTP request pipeline
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            // ================= Routes =================
            app.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
            );

            app.MapControllerRoute(
                name: "default",
                pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}"
            );

            app.Run();
        }
    }
}
