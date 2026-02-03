using Entertainment_travel_booking_website.Models;
using Entertainment_travel_booking_website.Repository.IRepository;
using System.Linq.Expressions;

namespace Entertainment_travel_booking_website.Repository
{
    public class TripRepository : ITripRepository
    {
        private readonly IRepository<Trip> _repo;

        public TripRepository(IRepository<Trip> repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Trip>> GetAllAsync()
        {
            return await _repo.GetAsync(
                expression: null,
                includes: new Expression<Func<Trip, object>>[] { t => t.TripSupimages }
            );
        }

        public async Task<Trip?> GetAsync(int id)
        {
            return await _repo.GetOneAsync(
                expression: t => t.Id == id,
                includes: new Expression<Func<Trip, object>>[] { t => t.TripSupimages }
            );
        }

        public async Task AddAsync(Trip trip)
        {
            await _repo.AddAsync(trip);
            await _repo.CommitAsync();
        }

        public async Task UpdateAsync(Trip trip)
        {
            _repo.Update(trip);
            await _repo.CommitAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var trip = await GetAsync(id);
            if (trip != null)
            {
                _repo.Delete(trip);
                await _repo.CommitAsync();
            }
        }
    }
}
