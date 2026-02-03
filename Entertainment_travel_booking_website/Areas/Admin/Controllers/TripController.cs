using Entertainment_travel_booking_website.Models;
using Entertainment_travel_booking_website.modelVM;
using Entertainment_travel_booking_website.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Entertainment_travel_booking_website.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TripController : Controller
    {

        private readonly ITripRepository _tripRepository;

        public TripController(ITripRepository tripRepository)
        {
            _tripRepository = tripRepository;
        }

        public async Task<IActionResult> Index()
        {
            var trips = await _tripRepository.GetAllAsync();
            return View(trips);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TripVM tripVM)
        {
            if (!ModelState.IsValid)
                return View(tripVM);

            string tripsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/trips");
            if (!Directory.Exists(tripsFolder))
                Directory.CreateDirectory(tripsFolder);

            string supImagesFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/trip-sup");
            if (!Directory.Exists(supImagesFolder))
                Directory.CreateDirectory(supImagesFolder);

            string mainImageName = Guid.NewGuid() + Path.GetExtension(tripVM.MainImage.FileName);
            string mainImagePath = Path.Combine(tripsFolder, mainImageName);
            using (var stream = new FileStream(mainImagePath, FileMode.Create))
            {
                await tripVM.MainImage.CopyToAsync(stream);
            }

            var trip = new Trip
            {
                Place = tripVM.Place,
                StartDate = tripVM.StartDate,
                EndDate = tripVM.EndDate,
                Description = tripVM.Description,
                Price = tripVM.Price,
                DiscountedPrice = tripVM.DiscountedPrice,
                Image = mainImageName,
                AvailableSeats = tripVM.AvailableSeats,
                MaxPeople = tripVM.MaxPeople,
                Status = tripVM.Status,
                TripSupimages = new List<TripSupimage>()
            };

            if (tripVM.SupImages != null)
            {
                foreach (var image in tripVM.SupImages)
                {
                    string imageName = Guid.NewGuid() + Path.GetExtension(image.FileName);
                    string imagePath = Path.Combine(supImagesFolder, imageName);

                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }

                    trip.TripSupimages.Add(new TripSupimage
                    {
                        SupImg = imageName
                    });
                }
            }

            await _tripRepository.AddAsync(trip);
            return RedirectToAction("Index");
        }
        //-----Edit-------
        public async Task<IActionResult> Edit(int id)
        {
            var trip = await _tripRepository.GetAsync(id);
            if (trip == null) return NotFound();

            var tripVM = new TripVM
            {
                Id = trip.Id,
                Place = trip.Place,
                StartDate = trip.StartDate,
                EndDate = trip.EndDate,
                Description = trip.Description,
                Price = trip.Price,
                DiscountedPrice = trip.DiscountedPrice,
                AvailableSeats = trip.AvailableSeats,
                MaxPeople = trip.MaxPeople,
                Status = trip.Status
            };

            return View(tripVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TripVM tripVM)
        {
            if (id != tripVM.Id) return BadRequest();
            if (!ModelState.IsValid) return View(tripVM);

            var trip = await _tripRepository.GetAsync(id);
            if (trip == null) return NotFound();

            trip.Place = tripVM.Place;
            trip.StartDate = tripVM.StartDate;
            trip.EndDate = tripVM.EndDate;
            trip.Description = tripVM.Description;
            trip.Price = tripVM.Price;
            trip.DiscountedPrice = tripVM.DiscountedPrice;
            trip.AvailableSeats = tripVM.AvailableSeats;
            trip.MaxPeople = tripVM.MaxPeople;
            trip.Status = tripVM.Status;

            string tripsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/trips");
            string supImagesFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/trip-sup");
            if (!Directory.Exists(tripsFolder)) Directory.CreateDirectory(tripsFolder);
            if (!Directory.Exists(supImagesFolder)) Directory.CreateDirectory(supImagesFolder);

            if (tripVM.MainImage != null)
            {
                string mainImageName = Guid.NewGuid() + Path.GetExtension(tripVM.MainImage.FileName);
                string mainImagePath = Path.Combine(tripsFolder, mainImageName);

                using (var stream = new FileStream(mainImagePath, FileMode.Create))
                {
                    await tripVM.MainImage.CopyToAsync(stream);
                }

                trip.Image = mainImageName;
            }

            if (tripVM.SupImages != null)
            {
                foreach (var image in tripVM.SupImages)
                {
                    string imageName = Guid.NewGuid() + Path.GetExtension(image.FileName);
                    string imagePath = Path.Combine(supImagesFolder, imageName);

                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }

                    trip.TripSupimages.Add(new TripSupimage { SupImg = imageName });
                }
            }

            await _tripRepository.UpdateAsync(trip);

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(int id)
        {
            await _tripRepository.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}
