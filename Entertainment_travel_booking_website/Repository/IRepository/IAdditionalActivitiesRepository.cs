using Entertainment_travel_booking_website.Models;
using System.Linq.Expressions;

namespace Entertainment_travel_booking_website.Repository.IRepository
{
    public interface IAdditianActivitiesRepository
    {
        // جلب مجموعة
        Task<IEnumerable<AdditianActivities>> GetAsync(
            Expression<Func<AdditianActivities, bool>>? expression = null,
            Func<IQueryable<AdditianActivities>, IOrderedQueryable<AdditianActivities>>? orderBy = null,
            Expression<Func<AdditianActivities, object>>[]? includes = null,
            bool tracked = true
        );

        // جلب عنصر واحد
        Task<AdditianActivities?> GetOneAsync(
            Expression<Func<AdditianActivities, bool>> expression,
            Expression<Func<AdditianActivities, object>>[]? includes = null,
            bool tracked = true
        );

        // عمليات CRUD
        Task AddAsync(AdditianActivities entity);
        Task UpdateAsync(AdditianActivities entity);
        Task DeleteAsync(int id);
    }
}