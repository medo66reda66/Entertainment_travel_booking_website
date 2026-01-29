using Entertainment_travel_booking_website.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entertainment_travel_booking_website.DataBase.Entitytypeconficration
{
    public class HotelSupImgEntitytypeconficration: IEntityTypeConfiguration<HotelSupImg>
    {
        public void Configure(EntityTypeBuilder<HotelSupImg> builder)
        {
            builder.HasKey(e => new {  e.SupImg,e.HotelId });
        }
    }
    
}

