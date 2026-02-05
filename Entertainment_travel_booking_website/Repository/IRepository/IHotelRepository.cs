using Entertainment_travel_booking_website.Models;

namespace Entertainment_travel_booking_website.Repository.IRepository
{
    public interface IHotelRepository
    {
        Task<IEnumerable<Hotel>> GetAllAsync();
        Task<Hotel?> GetAsync(int id);
        Task AddAsync(Hotel hotel);
        Task UpdateAsync(Hotel hotel);
        Task DeleteAsync(int id);
    }
}
