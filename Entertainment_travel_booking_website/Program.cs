using Entertainment_travel_booking_website.DataBase;
using Entertainment_travel_booking_website.Models;
using Entertainment_travel_booking_website.Repository;
using Entertainment_travel_booking_website.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Entertainment_travel_booking_website
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container
            builder.Services.AddControllersWithViews();

            // DbContext
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("default")
                )
            );

            // Generic Repository
            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            // Trip
            builder.Services.AddScoped<IRepository<Trip>, Repository<Trip>>();
            builder.Services.AddScoped<IRepository<TripSupimage>, Repository<TripSupimage>>();
            builder.Services.AddScoped<TripSupimgIRepository, TripSupImgsRepository>();
            builder.Services.AddScoped<TripRepository>();

            // Hotel
            builder.Services.AddScoped<IRepository<Hotel>, Repository<Hotel>>();
            builder.Services.AddScoped<IRepository<HotelSupImg>, Repository<HotelSupImg>>();
            builder.Services.AddScoped<HotelSupimgIRepository, HotelSupImgsRepository>();
            builder.Services.AddScoped<HotelRepository>();
            // Additional Activities
            builder.Services.AddScoped<IRepository<AdditianActivities>, Repository<AdditianActivities>>();
            builder.Services.AddScoped<IRepository<ActivitiesSupImg>, Repository<ActivitiesSupImg>>();
            builder.Services.AddScoped<IAdditionalActivitySubImageRepository, AdditionalActivitySubImageRepository>();
            builder.Services.AddScoped<IAdditianActivitiesRepository, AdditianActivitiesRepository>();

            var app = builder.Build();

            // Configure the HTTP request pipeline
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();

            app.MapStaticAssets();

            app.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
            );

            app.MapControllerRoute(
                name: "default",
                pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}"
            ).WithStaticAssets();

            app.Run();
        }
    }
}
