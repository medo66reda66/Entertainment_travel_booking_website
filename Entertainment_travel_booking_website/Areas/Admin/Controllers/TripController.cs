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

        private readonly IRepository<Trip> _tripRepository;
        private readonly IRepository<TripSupimage> _tripSupimageRepository;
        private readonly TripSupimgIRepository _tripSupimgIRepository;

        public TripController(IRepository<Trip> tripRepository, IRepository<TripSupimage> tripSupimageRepository, TripSupimgIRepository tripSupimgIRepository)
        {
            _tripRepository = tripRepository;
            _tripSupimageRepository = tripSupimageRepository;
            _tripSupimgIRepository = tripSupimgIRepository;
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var trips = await _tripRepository.GetAsync(tracked:false,cancellationToken:cancellationToken);
            return View(trips);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TripCreateVM tripcreateVM,CancellationToken cancellationtoken)
        {
            if (!ModelState.IsValid)
                return View(tripcreateVM);
          try
          { 
                var trip = new Trip
                {
                    Place = tripcreateVM.Place,
                    StartDate = tripcreateVM.StartDate,
                    EndDate = tripcreateVM.EndDate,
                    Description = tripcreateVM.Description,
                    Price = tripcreateVM.Price,
                    DiscountedPrice = tripcreateVM.DiscountedPrice,
                    AvailableSeats = tripcreateVM.AvailableSeats,
                    MaxPeople = tripcreateVM.MaxPeople,
                    Status = tripcreateVM.Status,
                };

                if (tripcreateVM.MainImage is not null && tripcreateVM.MainImage.Length > 0)
                {
                    var tripsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/trips");
                    if (!Directory.Exists(tripsFolder))
                        Directory.CreateDirectory(tripsFolder);



                    var mainImageName = Guid.NewGuid().ToString() + Path.GetExtension(tripcreateVM.MainImage.FileName);
                    var mainImagePath = Path.Combine(tripsFolder, mainImageName);
                    using (var stream = new FileStream(mainImagePath, FileMode.Create))
                    {
                        await tripcreateVM.MainImage.CopyToAsync(stream);
                    }

                    trip.Image = mainImageName;
                }

                await _tripRepository.AddAsync(trip, cancellationtoken);
                await _tripRepository.CommitAsync(cancellationtoken);

                if (tripcreateVM.SupImages != null)
                {
                    var supImagesFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/trip-sup");
                    if (!Directory.Exists(supImagesFolder))
                        Directory.CreateDirectory(supImagesFolder);

                    foreach (var image in tripcreateVM.SupImages)
                    {
                        var imageName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                        var imagePath = Path.Combine(supImagesFolder, imageName);

                        using (var stream = new FileStream(imagePath, FileMode.Create))
                        {
                            await image.CopyToAsync(stream);
                        }

                        await _tripSupimageRepository.AddAsync(new TripSupimage
                        {
                            TripId = trip.Id,
                            SupImg = imageName
                        }, cancellationtoken);
                    }
                    await _tripSupimageRepository.CommitAsync(cancellationtoken);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while uploading images: " + ex.Message);
                return View(tripcreateVM);
            }

            return RedirectToAction("Index");
        }
        //-----Edit-------
        public async Task<IActionResult> Edit(int id,CancellationToken cancellationToken)
        {
            var trip = await _tripRepository.GetOneAsync(e=>e.Id == id, includes:[e=>e.TripSupimages],cancellationToken:cancellationToken);
            if (trip == null) return NotFound();

            var tripVM = new TripEditVM
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
                Status = trip.Status,
            };

            return View(tripVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TripEditVM tripeEditVM,CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid) return View(tripeEditVM);
    
            var trip = await _tripRepository.GetOneAsync(e=>e.Id== tripeEditVM.Id,cancellationToken:cancellationToken);
            if (trip == null) return NotFound();

            trip.Place = tripeEditVM.Place;
            trip.StartDate = tripeEditVM.StartDate;
            trip.EndDate = tripeEditVM.EndDate;
            trip.Description = tripeEditVM.Description;
            trip.Price = tripeEditVM.Price;
            trip.DiscountedPrice = tripeEditVM.DiscountedPrice;
            trip.AvailableSeats = tripeEditVM.AvailableSeats;
            trip.MaxPeople = tripeEditVM.MaxPeople;
            trip.Status = tripeEditVM.Status;

            var tripsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/trips");
            var supImagesFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/trip-sup");
            if (!Directory.Exists(tripsFolder)) Directory.CreateDirectory(tripsFolder);
            if (!Directory.Exists(supImagesFolder)) Directory.CreateDirectory(supImagesFolder);

            if (tripeEditVM.MainImage != null)
            {
                var existingMainImagePath = Path.Combine(tripsFolder, trip.Image);
                if (System.IO.File.Exists(existingMainImagePath))
                {
                    System.IO.File.Delete(existingMainImagePath);
                }

                var mainImageName = Guid.NewGuid().ToString() + Path.GetExtension(tripeEditVM.MainImage.FileName);
                var mainImagePath = Path.Combine(tripsFolder, mainImageName);

                using (var stream = new FileStream(mainImagePath, FileMode.Create))
                {
                    await tripeEditVM.MainImage.CopyToAsync(stream);
                }

                trip.Image = mainImageName;
            }

            if (tripeEditVM.SupImages != null)
            {
                foreach (var existingSupImage in trip.TripSupimages)
                {
                    var existingImagePath = Path.Combine(supImagesFolder, existingSupImage.SupImg);
                    if (System.IO.File.Exists(existingImagePath))
                    {
                        System.IO.File.Delete(existingImagePath);
                    }
                }

                _tripSupimgIRepository.RemoveTripSupImages(trip.TripSupimages);
               await _tripSupimageRepository.CommitAsync(cancellationToken);

                foreach (var image in tripeEditVM.SupImages)
                {
                    var imageName = Guid.NewGuid() + Path.GetExtension(image.FileName);
                    var imagePath = Path.Combine(supImagesFolder, imageName);

                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }

                   await _tripSupimageRepository.AddAsync(new TripSupimage
                    {
                        TripId = trip.Id,
                        SupImg = imageName
                    },cancellationToken);
                }
            }

             _tripRepository.Update(trip);
             await _tripRepository.CommitAsync(cancellationToken);

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(int id,CancellationToken cancellationToken)
        {
           var trip = await _tripRepository.GetOneAsync(e => e.Id == id,cancellationToken:cancellationToken);
              if (trip == null) return NotFound();

            var tripsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/trips");
            var supImagesFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/trip-sup");
            var existingMainImagePath = Path.Combine(tripsFolder, trip.Image);

            if (System.IO.File.Exists(existingMainImagePath))
            {
                System.IO.File.Delete(existingMainImagePath);
            }

            foreach (var existingSupImage in trip.TripSupimages)
            {
                var existingImagePath = Path.Combine(supImagesFolder, existingSupImage.SupImg);
                if (System.IO.File.Exists(existingImagePath))
                {
                    System.IO.File.Delete(existingImagePath);
                }
            }

            _tripRepository.Delete(trip);
            _tripSupimgIRepository.RemoveTripSupImages(trip.TripSupimages);

            await _tripRepository.CommitAsync(cancellationToken);
           await _tripSupimgIRepository.CommitAsync(cancellationToken);

            return RedirectToAction("Index");
        }
    }
}
