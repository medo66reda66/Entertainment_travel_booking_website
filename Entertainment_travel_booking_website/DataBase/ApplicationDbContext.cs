using Entertainment_travel_booking_website.DataBase.Entitytypeconficration;
using Entertainment_travel_booking_website.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Entertainment_travel_booking_website.DataBase
{
    public class ApplicationDbContext:IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Trip> trips { get; set; }
        public DbSet<AdditianActivities> additianActivites { get; set; }
        public DbSet<TripAdditianActivities> tripAdditianActivities { get; set; }
        public DbSet<ActivitiesSupImg> activitiesSupImgs { get; set; }
        public DbSet<Hotel> hotels { get; set; }
        public DbSet<HotelSupImg> hotelSupImgs { get; set; }
        public DbSet<TripSupimage> tripSupimages { get; set; }
        public DbSet<ApplicationUserOtp> ApplicationUserOtps { get; set; }
        public DbSet<Room> rooms { get; set; }
        
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    base.OnConfiguring(optionsBuilder);
        //    optionsBuilder.UseSqlServer("Data Source=.;Initial catalog =Trips; Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");  base.OnConfiguring(optionsBuilder);
            //optionsBuilder.UseSqlServer("Data Source=(localdb)\\ProjectModels;Initial Catalog=Trips;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
        //}

        override protected void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(HotelSupImgEntitytypeconficration).Assembly);
        }
    }
}
