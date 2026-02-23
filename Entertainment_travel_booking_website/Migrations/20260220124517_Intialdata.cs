using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Travel_booking_website.Migrations
{
    /// <inheritdoc />
    public partial class Intialdata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            // ======================
            // Hotels (15 rows)
            // ======================
            migrationBuilder.Sql(@"
                INSERT INTO Hotels (Name, Location, PricePerNight, Description, Image, Availability, Rate)
                VALUES
                ('Fairmont Nile City', 'Cairo, Egypt', 4500, 'Luxury five-star hotel overlooking the Nile.', 'fairmont_nile.jpg', 1, 4.8),
                ('Marriott Mena House', 'Giza, Egypt', 5000, 'Historic hotel with Pyramid views.', 'mena_house.jpg', 1, 4.9),
                ('Four Seasons Nile Plaza', 'Cairo, Egypt', 6000, 'Upscale hotel with Nile panorama.', 'four_seasons.jpg', 1, 4.7),
                ('Steigenberger Cecil', 'Alexandria, Egypt', 3000, 'Historic hotel in downtown Alexandria.', 'cecil.jpg', 1, 4.5),
                ('Hilton Hurghada', 'Hurghada, Egypt', 4000, 'Beachfront resort with Red Sea views.', 'hilton_hurghada.jpg', 1, 4.6),
                ('Sofitel Winter Palace', 'Luxor, Egypt', 5500, 'Iconic hotel by the Nile.', 'winter_palace.jpg', 1, 4.8),
                ('Movenpick Aswan', 'Aswan, Egypt', 4800, 'Luxury Nile-side resort.', 'movenpick_aswan.jpg', 1, 4.7),
                ('Tolip Hotel Mansoura', 'Mansoura, Egypt', 2000, 'Modern downtown hotel.', 'tolip_mansoura.jpg', 1, 4.2),
                ('Ismailia Hotel', 'Ismailia, Egypt', 1800, 'Comfortable city hotel.', 'ismailia.jpg', 1, 4.1),
                ('Suez Canal Hotel', 'Suez, Egypt', 2200, 'Modern hotel near Suez Canal.', 'suez.jpg', 1, 4.0),
                ('Mercure Port Said', 'Port Said, Egypt', 2500, 'International chain hotel.', 'port_said.jpg', 1, 4.3),
                ('El Mahalla Hotel', 'El Mahalla El Kubra, Egypt', 1700, 'Family-friendly hotel.', 'elmahalla.jpg', 1, 4.1),
                ('Fayoum Palace', 'Fayoum, Egypt', 1900, 'Boutique hotel in Fayoum.', 'fayoum_palace.jpg', 1, 4.4),
                ('Sheraton Sharm', 'Sharm El Sheikh, Egypt', 4200, 'Luxury beach resort.', 'sheraton_sharm.jpg', 1, 4.5),
                ('The Nile Ritz-Carlton', 'Cairo, Egypt', 5800, 'Elegant riverside hotel.', 'ritz_carlton.jpg', 1, 4.9);
            ");

            // ======================
            // Trips (15 rows)
            // ======================
            migrationBuilder.Sql(@"
            INSERT INTO trips (Place, StartDate, EndDate, Description, Price, Count, DiscountedPrice, AvailableSeats, Rate, MaxPeople, Status, HotelId, Image)
            VALUES
            ('Giza Pyramids', '2026-03-15', '2026-03-16', 'Explore the Pyramids with guided tours.', 1200, 25, 1000, 25, 4.8, 30, 1, 2, 'giza_pyramids.jpg'),
            (' Cairo', '2026-03-16', '2026-03-17', 'Dinner cruise along the Nile River.', 1500, 20, 1300, 20, 4.9, 25, 1, 1, 'nile_cruise.jpg'),
            ('Hurghada ', '2026-03-20', '2026-03-21', 'Scuba diving in Red Sea.', 2200, 15, 2000, 15, 4.7, 20, 1, 5, 'hurghada_diving.jpg'),
            ('Luxor', '2026-03-18', '2026-03-18', 'Guided tour of Luxor Temple.', 800, 30, 700, 30, 4.6, 35, 1, 6, 'luxor_temple.jpg'),
            ('Aswan', '2026-03-19', '2026-03-19', 'Traditional sailboat trip on Nile.', 500, 40, 450, 40, 4.7, 45, 1, 7, 'aswan_felucca.jpg'),
            ('Sharm', '2026-03-22', '2026-03-23', 'Desert safari adventure.', 600, 25, 550, 25, 4.5, 30, 1, 14, 'sharm_safari.jpg'),
            ('Alexandria', '2026-03-17', '2026-03-17', 'Visit Bibliotheca Alexandrina.', 300, 35, 250, 35, 4.4, 40, 1, 4, 'alexandria_library.jpg'),
            ('Fayoum', '2026-03-21', '2026-03-21', 'Relax at Fayoum natural oasis.', 400, 20, 350, 20, 4.3, 25, 1, 13, 'fayoum_oasis.jpg'),
            ('Suez', '2026-03-23', '2026-03-23', 'Boat tour along Suez Canal.', 350, 15, 300, 15, 4.2, 20, 1, 10, 'suez_canal.jpg'),
            ('Port', '2026-03-24', '2026-03-24', 'Enjoy the Port Marina view.', 300, 10, 250, 10, 4.1, 15, 1, 11, 'port_said_marina.jpg'),
            ('Ismailia', '2026-03-25', '2026-03-25', 'Leisure walk along the canal.', 150, 20, 120, 20, 4.0, 25, 1, 9, 'ismailia_canal.jpg'),
            ('El Mahalla Textile Workshop', '2026-03-26', '2026-03-26', 'See local textile production.', 200, 15, 180, 15, 4.1, 20, 1, 12, 'el_mahalla_textile.jpg'),
            ('Tanta', '2026-03-27', '2026-03-27', 'Visit historical churches.', 100, 30, 90, 30, 4.0, 35, 1, 12, 'tanta_church.jpg'),
            ('Dahab', '2026-03-28', '2026-03-29', 'Red Sea diving adventure.', 2100, 12, 2000, 12, 4.7, 15, 1, 5, 'dahab_diving.jpg'),
            ('Hurghada', '2026-03-29', '2026-03-29', 'View sea life through glass boat.', 400, 20, 350, 20, 4.3, 25, 1, 5, 'hurghada_glass_boat.jpg');
            ");

            // ======================
            // Rooms (15 rows)
            // ======================
            migrationBuilder.Sql(@"
                INSERT INTO rooms (Description, Type, locationInHotel, Availability, HotelId)
                VALUES
                ('Deluxe Nile View', 2, '5th Floor, Nile Side', 1, 1),
                ('Junior Suite', 2, '6th Floor, Garden View', 1, 1),
                ('Superior Room', 1, '3rd Floor', 1, 2),
                ('Executive Suite', 2, '8th Floor, River View', 1, 3),
                ('Presidential Suite', 2, 'Top Floor, Pyramid View', 1, 2),
                ('Standard Room', 1, '2nd Floor', 1, 4),
                ('Family Suite', 2, '4th Floor, Beach View', 1, 5),
                ('Nile Suite', 2, '7th Floor, Nile View', 1, 6),
                ('Luxury Suite', 2, 'Top Floor', 1, 7),
                ('Double Room', 2, '3rd Floor', 1, 8),
                ('Single Room', 1, '1st Floor', 1, 9),
                ('Canal View Room', 2, '2nd Floor', 1, 10),
                ('Port View Suite', 2, '4th Floor', 1, 11),
                ('Classic Room', 1, '2nd Floor', 1, 12),
                ('Deluxe Sea View', 2, '6th Floor, Beachside', 1, 14);
            ");

            // ======================
            // Additional Activities (15 rows)
            // ======================

            migrationBuilder.Sql(@"
                INSERT INTO additianActivites (Place, Description, Price, Date, MainImg)
                    VALUES
                    ('Giza Pyramids, Giza, Egypt', 'Camel ride experience near the Great Pyramids.', 300.00, '2026-03-15', 'giza_pyramids.jpg'),
                    ('Nile River Dinner Cruise, Cairo, Egypt', 'Luxury Nile dinner cruise with live show.', 800.00, '2026-03-16', 'nile_dinner_cruise.jpg'),
                    ('Scuba Diving, Hurghada, Egypt', 'Professional scuba diving trip in the Red Sea.', 1200.00, '2026-03-20', 'hurghada_diving.jpg'),
                    ('Luxor Temple, Luxor, Egypt', 'Guided historical tour inside Luxor Temple.', 500.00, '2026-03-18', 'luxor_temple.jpg'),
                    ('Felucca Ride, Aswan, Egypt', 'Traditional felucca sailing experience on the Nile.', 400.00, '2026-03-19', 'aswan_felucca.jpg'),
                    ('Desert Safari, Sharm El Sheikh, Egypt', 'ATV desert safari with Bedouin dinner.', 600.00, '2026-03-22', 'sharm_safari.jpg'),
                    ('Bibliotheca Alexandrina, Alexandria, Egypt', 'Cultural visit to Alexandria Library.', 200.00, '2026-03-17', 'alex_library.jpg'),
                    ('Wadi El Rayan, Fayoum, Egypt', 'Waterfalls and sandboarding adventure.', 350.00, '2026-03-21', 'wadi_elrayan.jpg'),
                    ('Suez Canal Boat Tour, Suez, Egypt', 'Boat trip along the Suez Canal.', 300.00, '2026-03-23', 'suez_canal.jpg'),
                    ('Port Said Marina, Port Said, Egypt', 'Evening marina walk and seafood tasting.', 250.00, '2026-03-24', 'port_said_marina.jpg'),
                    ('Ismailia Canal Walk, Ismailia, Egypt', 'Relaxing walk along the canal.', 150.00, '2026-03-25', 'ismailia_canal.jpg'),
                    ('Textile Factory Tour, El Mahalla, Egypt', 'Educational textile industry tour.', 100.00, '2026-03-26', 'mahalla_factory.jpg'),
                    ('Tanta Religious Landmarks Tour, Tanta, Egypt', 'Visit to famous historical mosque and church.', 50.00, '2026-03-27', 'tanta_landmarks.jpg'),
                    ('Blue Hole Diving, Dahab, Egypt', 'World-famous Blue Hole diving experience.', 1100.00, '2026-03-28', 'dahab_bluehole.jpg'),
                    ('Glass Boat Tour, Hurghada, Egypt', 'Sea exploration with glass-bottom boat.', 400.00, '2026-03-29', 'hurghada_glassboat.jpg');
            ");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Trips;");
            migrationBuilder.Sql("DELETE FROM Hotels;");
            migrationBuilder.Sql("DELETE FROM AdditianActivities;");
            migrationBuilder.Sql("DELETE FROM Rooms;");
           
        }
    }
}
