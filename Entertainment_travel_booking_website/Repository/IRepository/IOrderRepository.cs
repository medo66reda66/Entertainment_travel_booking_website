using Entertainment_travel_booking_website.Models;

namespace Entertainment_travel_booking_website.Repository.IRepository
{
    public interface IOrderRepository
    {
        Task AddOrderAsync(Order order);

    }
}
