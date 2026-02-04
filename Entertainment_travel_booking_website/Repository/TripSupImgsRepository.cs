using Entertainment_travel_booking_website.DataBase;
using Entertainment_travel_booking_website.Models;
using Entertainment_travel_booking_website.Repository.IRepository;

namespace Entertainment_travel_booking_website.Repository
{
    public class TripSupImgsRepository :Repository<TripSupimage>, TripSupimgIRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public TripSupImgsRepository(ApplicationDbContext dbContext): base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddTripSupImagesAsync(List<TripSupimage> tripSupImages, CancellationToken cancellationToken = default)
        {
            await _dbContext.AddRangeAsync(tripSupImages, cancellationToken);
        }
        public void RemoveTripSupImages(List<TripSupimage> tripSupImages)
        {
            _dbContext.RemoveRange(tripSupImages);
        }


    }
}
