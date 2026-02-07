using Entertainment_travel_booking_website.Models;
using Entertainment_travel_booking_website.modelVM;
using Entertainment_travel_booking_website.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq.Expressions;

namespace Entertainment_travel_booking_website.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoomController : Controller
    {
        private readonly IRepository<Room> _roomRepo;
        private readonly IRepository<Hotel> _hotelRepo;

        public RoomController(IRepository<Room> roomRepo, IRepository<Hotel> hotelRepo)
        {
            _roomRepo = roomRepo;
            _hotelRepo = hotelRepo;
        }

        // ----------------- Index -----------------
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var rooms = await _roomRepo.GetAsync(cancellationToken: cancellationToken, tracked: false);
            return View(rooms);
        }

        // ----------------- Create -----------------
        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            var hotels = await _hotelRepo.GetAsync(cancellationToken: cancellationToken);
            var vm = new RoomVM
            {
                HotelList = hotels.Select(h => new SelectListItem
                {
                    Text = h.Name,
                    Value = h.Id.ToString()
                })
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RoomVM vm, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                // لو الـ Model مش فاليد لازم نعيد ملء القائمة تاني
                var hotels = await _hotelRepo.GetAsync(cancellationToken: cancellationToken);
                vm.HotelList = hotels.Select(h => new SelectListItem { Text = h.Name, Value = h.Id.ToString() });
                return View(vm);
            }

            var room = new Room
            {
                Description = vm.Description,
                Type = vm.Type,
                locationInHotel = vm.locationInHotel,
                Availability = vm.Availability,
                HotelId = vm.HotelId // السطر ده هو اللي كان ناقصك!
            };

            await _roomRepo.AddAsync(room, cancellationToken);
            await _roomRepo.CommitAsync(cancellationToken);
            return RedirectToAction(nameof(Index));
        }

        // ----------------- Edit -----------------
        public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
        {
            var room = await _roomRepo.GetOneAsync(r => r.ID == id, cancellationToken: cancellationToken);
            if (room == null) return NotFound();

            var vm = new RoomVM
            {
                ID = room.ID,
                Description = room.Description!,
                Type = room.Type,
                locationInHotel = room.locationInHotel,
                Availability = room.Availability
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RoomVM vm, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid) return View(vm);

            var room = await _roomRepo.GetOneAsync(r => r.ID == vm.ID);
            if (room == null) return NotFound();

            room.Description = vm.Description;
            room.Type = vm.Type;
            room.locationInHotel = vm.locationInHotel;
            room.Availability = vm.Availability;

            _roomRepo.Update(room);
            await _roomRepo.CommitAsync(cancellationToken);

            return RedirectToAction(nameof(Index));
        }

        // ----------------- Delete -----------------
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var room = await _roomRepo.GetOneAsync(r => r.ID == id, cancellationToken: cancellationToken);
            if (room is null)
                return RedirectToAction("NotFoundPage", "Home");

            _roomRepo.Delete(room);
            await _roomRepo.CommitAsync();
            return RedirectToAction(nameof(Index));

        }
    }
}
