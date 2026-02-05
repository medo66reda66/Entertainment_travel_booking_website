using Entertainment_travel_booking_website.Models;
using Entertainment_travel_booking_website.Repository.IRepository;
using System.Linq.Expressions;

namespace Entertainment_travel_booking_website.Repository
{
    public class HotelRepository: IHotelRepository
    {
        private readonly IRepository<Hotel> _repo;

        public HotelRepository(IRepository<Hotel> repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Hotel>> GetAllAsync()
        {
            return await _repo.GetAsync(
                expression: null,
                includes: new Expression<Func<Hotel, object>>[] { t => t.HotelSupImgs }
            );
        }

        public async Task<Hotel?> GetAsync(int id)
        {
            return await _repo.GetOneAsync(
                expression: t => t.Id == id,
                includes: new Expression<Func<Hotel, object>>[] { t => t.HotelSupImgs }
            );
        }

        public async Task AddAsync(Hotel hotel)
        {
            await _repo.AddAsync(hotel);
            await _repo.CommitAsync();
        }

        public async Task UpdateAsync(Hotel hotel)
        {
            _repo.Update(hotel);
            await _repo.CommitAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var hotel = await GetAsync(id);
            if (hotel != null)
            {
                _repo.Delete(hotel);
                await _repo.CommitAsync();
            }
        }

    }
}
