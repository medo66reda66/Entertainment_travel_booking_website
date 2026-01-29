using Entertainment_travel_booking_website.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entertainment_travel_booking_website.DataBase.Entitytypeconficration
{
    public class TripSupImgEntitytypeconficration : IEntityTypeConfiguration<TripSupimage>
    {
        public void Configure(EntityTypeBuilder<TripSupimage> builder)
        {
            builder.HasKey(e => new {e.SupImg,e.TripId});
        }
    }
}
