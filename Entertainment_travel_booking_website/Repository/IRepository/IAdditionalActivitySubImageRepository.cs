using Entertainment_travel_booking_website.Models;

namespace Entertainment_travel_booking_website.Repository.IRepository
{
    public interface IAdditionalActivitySubImageRepository : IRepository<ActivitiesSupImg>
    {
        // إضافة مجموعة صور فرعية
        Task AddAdditionActivitySupImagesAsync(List<ActivitiesSupImg> activitiesSupImgs);

        // حذف مجموعة صور فرعية
        void RemoveAdditionActivitySupImages(List<ActivitiesSupImg> activitiesSupImgs);

        Task CommitAsync();
    }
}