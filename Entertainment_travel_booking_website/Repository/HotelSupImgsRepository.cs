using Entertainment_travel_booking_website.DataBase;
using Entertainment_travel_booking_website.Models;
using Entertainment_travel_booking_website.Repository.IRepository;

namespace Entertainment_travel_booking_website.Repository
{
    public class HotelSupImgsRepository : Repository<HotelSupImg>, HotelSupimgIRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public HotelSupImgsRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddHotelSupImagesAsync(List<HotelSupImg> hotelSupImages, CancellationToken cancellationToken = default)
        {
            await _dbContext.AddRangeAsync(hotelSupImages, cancellationToken);
        }

        // حذف الصور الفرعية من قاعدة البيانات
        public void RemoveHotelSupImages(List<HotelSupImg> hotelSupImages)
        {
            if (hotelSupImages == null || hotelSupImages.Count == 0) return;
            _dbContext.hotelSupImgs.RemoveRange(hotelSupImages);
        }

        public void RemoveTripSupImages(List<HotelSupImg> hotelSupImgs)
        {
            _dbContext.RemoveRange(hotelSupImgs);
        }
    }
}
