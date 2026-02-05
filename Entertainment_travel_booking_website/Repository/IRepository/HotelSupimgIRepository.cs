using Entertainment_travel_booking_website.Models;

namespace Entertainment_travel_booking_website.Repository.IRepository
{
    public interface HotelSupimgIRepository: IRepository<HotelSupImg>
    {
        Task AddHotelSupImagesAsync(List<HotelSupImg> hotelSupImages, CancellationToken cancellationToken = default);


        void RemoveHotelSupImages(List<HotelSupImg> hotelSupImages);

    }
}
