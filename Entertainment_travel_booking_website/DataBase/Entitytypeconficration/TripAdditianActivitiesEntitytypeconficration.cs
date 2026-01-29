using Entertainment_travel_booking_website.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entertainment_travel_booking_website.DataBase.Entitytypeconficration
{
    public class TripAdditianActivitiesEntitytypeconficration : IEntityTypeConfiguration<TripAdditianActivities>
    {
        public void Configure(EntityTypeBuilder<TripAdditianActivities> builder)
        {
           builder.HasKey(e => new { e.tripId, e.additianActivitiesId });
        }
    }
}
