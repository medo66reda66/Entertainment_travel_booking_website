using Entertainment_travel_booking_website.Models;
using Entertainment_travel_booking_website.Repository.IRepository;
using System.Linq.Expressions;

namespace Entertainment_travel_booking_website.Repository
{
    public class AdditianActivitiesRepository : IAdditianActivitiesRepository
    {
        private readonly IRepository<AdditianActivities> _repo;

        public AdditianActivitiesRepository(IRepository<AdditianActivities> repo)
        {
            _repo = repo;
        }

        // 1. تنفيذ الدالة المطلوبة لجلب قائمة بيانات مع الفلترة والترتيب
        public async Task<IEnumerable<AdditianActivities>> GetAsync(
            Expression<Func<AdditianActivities, bool>>? expression = null,
            Func<IQueryable<AdditianActivities>, IOrderedQueryable<AdditianActivities>>? orderBy = null,
            Expression<Func<AdditianActivities, object>>[]? includes = null,
            bool tracked = true)
        {
            // نفترض أن المستودع العام يدعم orderBy؛ إذا لم يكن كذلك، أزلها من الواجهة وهنا.
            return await _repo.GetAsync(
                expression: expression,
                includes: includes,
                tracked: tracked
            );
        }

        // 2. تنفيذ الدالة المطلوبة لجلب عنصر واحد فقط
        public async Task<AdditianActivities?> GetOneAsync(
            Expression<Func<AdditianActivities, bool>> expression,
            Expression<Func<AdditianActivities, object>>[]? includes = null,
            bool tracked = true)
        {
            return await _repo.GetOneAsync(
                expression: expression,
                includes: includes,
                tracked: tracked
            );
        }

        // 3. دالة مخصصة لجلب الكل مع الصور (طريقة مساعدة)
        public async Task<IEnumerable<AdditianActivities>> GetAllAsync()
        {
            return await _repo.GetAsync(
                expression: null,
                includes: new Expression<Func<AdditianActivities, object>>[] { a => a.ActivitiesSupImgs }
            );
        }

        // 4. إضافة سجل جديد
        public async Task AddAsync(AdditianActivities activity)
        {
            await _repo.AddAsync(activity);
            await _repo.CommitAsync();
        }

        // 5. تحديث سجل
        public async Task UpdateAsync(AdditianActivities activity)
        {
            _repo.Update(activity);
            await _repo.CommitAsync();
        }

        // 6. حذف سجل بواسطة الـ ID
        public async Task DeleteAsync(int id)
        {
            // البحث عن العنصر أولاً باستخدام الـ Repo
            var activity = await _repo.GetOneAsync(expression: a => a.Id == id);

            if (activity != null)
            {
                _repo.Delete(activity);
                await _repo.CommitAsync();
            }
        }
    }
}