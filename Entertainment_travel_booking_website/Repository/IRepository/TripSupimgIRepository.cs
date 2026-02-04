using Entertainment_travel_booking_website.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Entertainment_travel_booking_website.Repository.IRepository
{
    public interface TripSupimgIRepository : IRepository<TripSupimage>
    {
         Task AddTripSupImagesAsync(List<TripSupimage> tripSupImages, CancellationToken cancellationToken = default);


         void RemoveTripSupImages(List<TripSupimage> tripSupImages);
        
    }
}
