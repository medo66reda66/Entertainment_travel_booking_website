using Ecommerce.Utilities;
using Entertainment_travel_booking_website.Models;
using Entertainment_travel_booking_website.modelVM;
using Entertainment_travel_booking_website.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Linq.Expressions;
using Travel_booking_website;

namespace Entertainment_travel_booking_website.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = $"{DS.SUPER_ADMIN_ROLE},{DS.ADMIN_ROLE},{DS.EMPLOYEE_ROLE}")]
    public class TripController : Controller
    {
        private readonly IRepository<Trip> _tripRepository;
        private readonly IRepository<TripSupimage> _tripSupimageRepository;
        private readonly TripSupimgIRepository _tripSupimgIRepository;
        private readonly IStringLocalizer<Trip1Controller> _localizer;

        // ====== ADD ONLY ======
        private readonly IRepository<Hotel> _hotelRepository;
        private readonly IRepository<AdditianActivities> _activitiesRepository;
        // ======================

        public TripController(
            IRepository<Trip> tripRepository,
            IRepository<TripSupimage> tripSupimageRepository,
            TripSupimgIRepository tripSupimgIRepository,

            // ====== ADD ONLY ======
            IRepository<Hotel> hotelRepository,
            IRepository<AdditianActivities> activitiesRepository
,
            IStringLocalizer<Trip1Controller> localizer
        // ======================
        )
        {
            _tripRepository = tripRepository;
            _tripSupimageRepository = tripSupimageRepository;
            _tripSupimgIRepository = tripSupimgIRepository;

            // ====== ADD ONLY ======
            _hotelRepository = hotelRepository;
            _activitiesRepository = activitiesRepository;
            _localizer = localizer;
            // ======================
        }

        // ----- Index -----
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var trips = await _tripRepository.GetAsync(
                includes: new Expression<Func<Trip, object>>[]
                {
            t => t.TripSupimages,
            t => t.Hotel,
            t => t.TripAdditianActivities  // فقط الـ Collection بدون Select
                },
                tracked: false,
                cancellationToken: cancellationToken
            );
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_TripsListPartial", trips);
            }
            return View(trips);
        }



        // ----- Create -----
        [HttpGet]
        [Authorize(Roles = $"{DS.SUPER_ADMIN_ROLE},{DS.ADMIN_ROLE},{DS.EMPLOYEE_ROLE}")]
        public async Task<IActionResult> Create()
        {
            ViewBag.Hotels = await _hotelRepository.GetAsync() ?? new List<Hotel>();
            ViewBag.Activities = await _activitiesRepository.GetAsync() ?? new List<AdditianActivities>();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{DS.SUPER_ADMIN_ROLE},{DS.ADMIN_ROLE},{DS.EMPLOYEE_ROLE}")]
        public async Task<IActionResult> Create(TripCreateVM tripcreateVM, CancellationToken cancellationtoken)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Hotels = await _hotelRepository.GetAsync() ?? new List<Hotel>();
                ViewBag.Activities = await _activitiesRepository.GetAsync() ?? new List<AdditianActivities>();
                return View(tripcreateVM);
            }

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
                    HotelId = tripcreateVM.HotelId
                };

                // Multiple Activities
                if (tripcreateVM.SelectedActivityIds != null && tripcreateVM.SelectedActivityIds.Count > 0)
                {
                    foreach (var activityId in tripcreateVM.SelectedActivityIds)
                    {
                        trip.TripAdditianActivities.Add(new TripAdditianActivities
                        {
                            additianActivitiesId = activityId
                        });
                    }
                }

                // Main Image
                if (tripcreateVM.MainImage != null && tripcreateVM.MainImage.Length > 0)
                {
                    var tripsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/trips");
                    if (!Directory.Exists(tripsFolder)) Directory.CreateDirectory(tripsFolder);

                    var mainImageName = Guid.NewGuid().ToString() + Path.GetExtension(tripcreateVM.MainImage.FileName);
                    var mainImagePath = Path.Combine(tripsFolder, mainImageName);

                    using (var stream = new FileStream(mainImagePath, FileMode.Create))
                        await tripcreateVM.MainImage.CopyToAsync(stream);

                    trip.Image = mainImageName;
                }

                await _tripRepository.AddAsync(trip, cancellationtoken);
                await _tripRepository.CommitAsync(cancellationtoken);

                // Sub Images
                if (tripcreateVM.SupImages != null)
                {
                    var supImagesFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/trip-sup");
                    if (!Directory.Exists(supImagesFolder)) Directory.CreateDirectory(supImagesFolder);

                    foreach (var image in tripcreateVM.SupImages)
                    {
                        var imageName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                        var imagePath = Path.Combine(supImagesFolder, imageName);

                        using (var stream = new FileStream(imagePath, FileMode.Create))
                            await image.CopyToAsync(stream);

                        await _tripSupimageRepository.AddAsync(new TripSupimage
                        {
                            TripId = trip.Id,
                            SupImg = imageName
                        }, cancellationtoken);
                    }

                    await _tripSupimageRepository.CommitAsync(cancellationtoken);
                }

                TempData["sucess-Notification"] =_localizer["AddTrip"].Value;
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error: " + ex.Message);
                ViewBag.Hotels = await _hotelRepository.GetAsync() ?? new List<Hotel>();
                ViewBag.Activities = await _activitiesRepository.GetAsync() ?? new List<AdditianActivities>();
                return View(tripcreateVM);
            }
        }

        // ----- Edit -----
        [HttpGet]
        [Authorize(Roles = $"{DS.SUPER_ADMIN_ROLE},{DS.ADMIN_ROLE}")]
        public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
        {
            var trip = await _tripRepository.GetOneAsync(
                e => e.Id == id,
                includes: new Expression<Func<Trip, object>>[] { e => e.TripSupimages, e => e.TripAdditianActivities },
                cancellationToken: cancellationToken
            );

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
                ExistingMainImage = trip.Image,
                ExistingSupImages = trip.TripSupimages,
                HotelId = trip.HotelId,
                SelectedActivityIds = trip.TripAdditianActivities.Select(a => a.additianActivitiesId).ToList()
            };

            ViewBag.Hotels = await _hotelRepository.GetAsync() ?? new List<Hotel>();
            ViewBag.Activities = await _activitiesRepository.GetAsync() ?? new List<AdditianActivities>();

            return View(tripVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{DS.SUPER_ADMIN_ROLE},{DS.ADMIN_ROLE}")]
        public async Task<IActionResult> Edit(TripEditVM tripeEditVM, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid) return View(tripeEditVM);

            var trip = await _tripRepository.GetOneAsync(
                e => e.Id == tripeEditVM.Id,
                includes: new Expression<Func<Trip, object>>[] { e => e.TripSupimages, e => e.TripAdditianActivities },
                cancellationToken: cancellationToken
            );

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
            trip.HotelId = tripeEditVM.HotelId;

            // Update Activities
            trip.TripAdditianActivities.Clear();
            if (tripeEditVM.SelectedActivityIds != null && tripeEditVM.SelectedActivityIds.Count > 0)
            {
                foreach (var activityId in tripeEditVM.SelectedActivityIds)
                {
                    trip.TripAdditianActivities.Add(new TripAdditianActivities
                    {
                        additianActivitiesId = activityId
                    });
                }
            }

            // Main Image
            var tripsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/trips");
            if (tripeEditVM.MainImage != null)
            {
                var existingMainImagePath = Path.Combine(tripsFolder, trip.Image);
                if (System.IO.File.Exists(existingMainImagePath)) System.IO.File.Delete(existingMainImagePath);

                var mainImageName = Guid.NewGuid().ToString() + Path.GetExtension(tripeEditVM.MainImage.FileName);
                var mainImagePath = Path.Combine(tripsFolder, mainImageName);

                using (var stream = new FileStream(mainImagePath, FileMode.Create))
                    await tripeEditVM.MainImage.CopyToAsync(stream);

                trip.Image = mainImageName;
            }

            // Sub Images
            var supImagesFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/trip-sup");
            if (tripeEditVM.SupImages != null)
            {
                foreach (var existingSupImage in trip.TripSupimages)
                {
                    var existingImagePath = Path.Combine(supImagesFolder, existingSupImage.SupImg);
                    if (System.IO.File.Exists(existingImagePath)) System.IO.File.Delete(existingImagePath);
                }

                _tripSupimgIRepository.RemoveTripSupImages(trip.TripSupimages);
                await _tripSupimageRepository.CommitAsync(cancellationToken);

                foreach (var image in tripeEditVM.SupImages)
                {
                    var imageName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                    var imagePath = Path.Combine(supImagesFolder, imageName);

                    using (var stream = new FileStream(imagePath, FileMode.Create))
                        await image.CopyToAsync(stream);

                    await _tripSupimageRepository.AddAsync(new TripSupimage
                    {
                        TripId = trip.Id,
                        SupImg = imageName
                    }, cancellationToken);
                }
            }

            _tripRepository.Update(trip);
            await _tripRepository.CommitAsync(cancellationToken);

            var successMessage = _localizer["EditTrip"].Value;
            TempData["sucess-Notification"] = successMessage;

            return RedirectToAction("Index");
        }


        // ----------------- Delete Trip -----------------
        [HttpGet]
        [Authorize(Roles = $"{DS.SUPER_ADMIN_ROLE},{DS.ADMIN_ROLE}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            // جلب الرحلة مع الصور الفرعية
            var trip = await _tripRepository.GetOneAsync(
                t => t.Id == id,
                includes: new Expression<Func<Trip, object>>[] { t => t.TripSupimages },
                cancellationToken: cancellationToken
            );

            if (trip == null)
                return NotFound();

            // مجلدات الصور
            var mainFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/trips");
            var supFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/trip-sup");

            // حذف الصورة الرئيسية إذا موجودة
            if (!string.IsNullOrEmpty(trip.Image))
            {
                var mainImagePath = Path.Combine(mainFolder, trip.Image);
                if (System.IO.File.Exists(mainImagePath))
                    System.IO.File.Delete(mainImagePath);
            }

            // حذف الصور الفرعية من القرص وقاعدة البيانات
            if (trip.TripSupimages != null && trip.TripSupimages.Any())
            {
                foreach (var supImg in trip.TripSupimages)
                {
                    var supImagePath = Path.Combine(supFolder, supImg.SupImg);
                    if (System.IO.File.Exists(supImagePath))
                        System.IO.File.Delete(supImagePath);
                }

                _tripSupimgIRepository.RemoveTripSupImages(trip.TripSupimages.ToList());
                await _tripSupimageRepository.CommitAsync(cancellationToken);
            }

            // حذف الرحلة نفسها
            _tripRepository.Delete(trip);
            await _tripRepository.CommitAsync(cancellationToken);
            TempData["sucess-Notification"] = _localizer["DeleteTrip"].Value;

            return RedirectToAction(nameof(Index));
        }

    }
}