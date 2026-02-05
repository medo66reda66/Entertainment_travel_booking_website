using Entertainment_travel_booking_website.DataBase;
using Entertainment_travel_booking_website.Models;
using Entertainment_travel_booking_website.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Entertainment_travel_booking_website.Repository
{
    public class AdditionalActivitySubImageRepository : Repository<ActivitiesSupImg>, IAdditionalActivitySubImageRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public AdditionalActivitySubImageRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        // إضافة مجموعة صور فرعية
        public async Task AddAdditionActivitySupImagesAsync(List<ActivitiesSupImg> subImages, CancellationToken cancellationToken = default)
        {
            if (subImages == null || subImages.Count == 0) return;
            await _dbContext.AddRangeAsync(subImages, cancellationToken);
        }

        // حذف مجموعة صور فرعية
        public void RemoveAdditionActivitySupImages(List<ActivitiesSupImg> subImages)
        {
            if (subImages == null || subImages.Count == 0) return;
            _dbContext.Set<ActivitiesSupImg>().RemoveRange(subImages);
        }

        // حفظ التغييرات في قاعدة البيانات
        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        // تنفيذ طرق الواجهة بدون CancellationToken من خلال استدعاء الإصدارات المحملة بالزيادة
        public async Task AddAdditionActivitySupImagesAsync(List<ActivitiesSupImg> activitiesSupImgs)
        {
            await AddAdditionActivitySupImagesAsync(activitiesSupImgs, default);
        }

        public async Task CommitAsync()
        {
            await CommitAsync(default);
        }
    }
}