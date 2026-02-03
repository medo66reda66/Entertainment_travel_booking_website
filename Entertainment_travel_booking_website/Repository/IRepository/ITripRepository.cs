using Entertainment_travel_booking_website.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Entertainment_travel_booking_website.Repository.IRepository
{
    public interface ITripRepository
    {
        Task<IEnumerable<Trip>> GetAllAsync();    
        Task<Trip?> GetAsync(int id);             
        Task AddAsync(Trip trip);                 
        Task UpdateAsync(Trip trip);             
        Task DeleteAsync(int id);                
    }
}
