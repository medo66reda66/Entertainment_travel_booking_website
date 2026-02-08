using Entertainment_travel_booking_website.Models;
using Entertainment_travel_booking_website.modelVM;
using Entertainment_travel_booking_website.Repository;
using Entertainment_travel_booking_website.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Entertainment_travel_booking_website.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HotelController : Controller
    {
        private readonly IRepository<Hotel> _hotelRepository;
        private readonly IRepository<HotelSupImg> _hotelSupimageRepository;
        private readonly HotelSupimgIRepository _hotelSupImgsRepository;
        private readonly IRepository<Room> _roomRepository;

        public HotelController(
            IRepository<Hotel> hotelRepository,
            IRepository<Room> roomRepository,
            IRepository<HotelSupImg> hotelSupimageRepository,
            HotelSupimgIRepository hotelSupImgsRepository) 
        {
            _hotelRepository = hotelRepository;
            _roomRepository = roomRepository;
            _hotelSupimageRepository = hotelSupimageRepository;
            _hotelSupImgsRepository = hotelSupImgsRepository;
        }

        //----------------- Index -----------------
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var hotels = await _hotelRepository.GetAsync(
                includes: new Expression<Func<Hotel, object>>[] { h => h.HotelSupImgs,h=>h.Rooms },
                tracked: false,
                cancellationToken: cancellationToken
            
            
    );
            return View(hotels);
        }

        //----------------- Create -----------------
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HotelCreateVM hotelCreateVM, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid) return View(hotelCreateVM);

            try
            {
                var hotel = new Hotel
                {
                    Name = hotelCreateVM.Name,
                    Location = hotelCreateVM.Location,
                    Description = hotelCreateVM.Description,
                    PricePerNight = hotelCreateVM.PricePerNight,
                    Availability = hotelCreateVM.Status
                };

                // حفظ الصورة الرئيسية
                if (hotelCreateVM.MainImage != null && hotelCreateVM.MainImage.Length > 0)
                {
                    var hotelFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/hotels");
                    if (!Directory.Exists(hotelFolder))
                        Directory.CreateDirectory(hotelFolder);

                    var mainImageName = Guid.NewGuid().ToString() + Path.GetExtension(hotelCreateVM.MainImage.FileName);
                    var mainImagePath = Path.Combine(hotelFolder, mainImageName);
                    using (var stream = new FileStream(mainImagePath, FileMode.Create))
                    {
                        await hotelCreateVM.MainImage.CopyToAsync(stream);
                    }
                    hotel.Image = mainImageName;
                }

                await _hotelRepository.AddAsync(hotel, cancellationToken);
                await _hotelRepository.CommitAsync(cancellationToken);

                // حفظ الصور الإضافية
                if (hotelCreateVM.SupImages != null)
                {
                    var supImagesFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/hotel-sup");
                    if (!Directory.Exists(supImagesFolder))
                        Directory.CreateDirectory(supImagesFolder);

                    foreach (var image in hotelCreateVM.SupImages)
                    {
                        var imageName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                        var imagePath = Path.Combine(supImagesFolder, imageName);
                        using (var stream = new FileStream(imagePath, FileMode.Create))
                        {
                            await image.CopyToAsync(stream);
                        }

                        await _hotelSupimageRepository.AddAsync(new HotelSupImg
                        {
                            HotelId = hotel.Id,
                            SupImg = imageName
                        }, cancellationToken);
                    }
                    await _hotelSupimageRepository.CommitAsync(cancellationToken);
                }
       
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "حدث خطأ أثناء رفع الصور: " + ex.Message);
                return View(hotelCreateVM);
            }

            return RedirectToAction("Index");
        }

        // GET: Edit Hotel
        public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
        {
            var hotel = await _hotelRepository.GetOneAsync(
                h => h.Id == id,
                includes: new Expression<Func<Hotel, object>>[] { h => h.HotelSupImgs, h=>h.Rooms },
                cancellationToken: cancellationToken
            );

            if (hotel == null) return NotFound();

            var hotelVM = new HotelEditVM
            {
                Id = hotel.Id,
                Name = hotel.Name,
                Location = hotel.Location,
                Description = hotel.Description,
                Status = hotel.Availability,
                ExistingMainImage = hotel.Image,
                ExistingSupImages = hotel.HotelSupImgs
            };

            return View(hotelVM);
        }

        // POST: Edit Hotel
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(HotelEditVM hotelEditVM, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid) return View(hotelEditVM);

            // جلب الفندق مع الصور الفرعية
            var hotel = await _hotelRepository.GetOneAsync(
                h => h.Id == hotelEditVM.Id,
                includes: new Expression<Func<Hotel, object>>[] { h => h.HotelSupImgs, h=>h.Rooms },
                cancellationToken: cancellationToken
            );

            if (hotel == null) return NotFound();

            // تحديث البيانات الأساسية
            hotel.Name = hotelEditVM.Name;
            hotel.Location = hotelEditVM.Location;
            hotel.Description = hotelEditVM.Description;
            hotel.PricePerNight = hotelEditVM.PricePerNight;
            hotel.Availability = hotelEditVM.Status;

            // مسارات حفظ الصور
            var hotelFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/hotels");
            var supImagesFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/hotel-sup");
            if (!Directory.Exists(hotelFolder)) Directory.CreateDirectory(hotelFolder);
            if (!Directory.Exists(supImagesFolder)) Directory.CreateDirectory(supImagesFolder);

            // تحديث الصورة الرئيسية
            if (hotelEditVM.MainImage != null)
            {
                // حذف الصورة القديمة
                if (!string.IsNullOrEmpty(hotel.Image))
                {
                    var existingMainImagePath = Path.Combine(hotelFolder, hotel.Image);
                    if (System.IO.File.Exists(existingMainImagePath))
                        System.IO.File.Delete(existingMainImagePath);
                }

                var mainImageName = Guid.NewGuid() + Path.GetExtension(hotelEditVM.MainImage.FileName);
                var mainImagePath = Path.Combine(hotelFolder, mainImageName);
                using (var stream = new FileStream(mainImagePath, FileMode.Create))
                {
                    await hotelEditVM.MainImage.CopyToAsync(stream);
                }
                hotel.Image = mainImageName;
            }

            // تحديث الصور الإضافية
            if (hotelEditVM.SupImages != null && hotelEditVM.SupImages.Any())
            {
                // حذف الصور القديمة
                foreach (var existingSupImage in hotel.HotelSupImgs)
                {
                    var existingPath = Path.Combine(supImagesFolder, existingSupImage.SupImg);
                    if (System.IO.File.Exists(existingPath))
                        System.IO.File.Delete(existingPath);
                }

                _hotelSupImgsRepository.RemoveHotelSupImages(hotel.HotelSupImgs);

                // حفظ الصور الجديدة
                foreach (var image in hotelEditVM.SupImages)
                {
                    var imageName = Guid.NewGuid() + Path.GetExtension(image.FileName);
                    var imagePath = Path.Combine(supImagesFolder, imageName);
                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }

                    await _hotelSupimageRepository.AddAsync(new HotelSupImg
                    {
                        HotelId = hotel.Id,
                        SupImg = imageName
                    }, cancellationToken);
                }

                await _hotelSupimageRepository.CommitAsync(cancellationToken);
            }

            // حفظ التحديثات
            _hotelRepository.Update(hotel);
            await _hotelRepository.CommitAsync(cancellationToken);

            return RedirectToAction("Index");
        }
        //----------------- Delete -----------------
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            // جلب الفندق مع الصور الفرعية
            var hotel = await _hotelRepository.GetOneAsync(
                h => h.Id == id,
                includes: new Expression<Func<Hotel, object>>[] { h => h.HotelSupImgs },
                cancellationToken: cancellationToken
            );

            if (hotel == null)
                return NotFound();

            // مجلدات الصور
            var hotelFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/hotels");
            var supImagesFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/hotel-sup");

            // حذف الصورة الرئيسية إذا موجودة
            if (!string.IsNullOrEmpty(hotel.Image))
            {
                var mainImagePath = Path.Combine(hotelFolder, hotel.Image);
                if (System.IO.File.Exists(mainImagePath))
                    System.IO.File.Delete(mainImagePath);
            }

            // حذف الصور الفرعية من القرص
            if (hotel.HotelSupImgs != null && hotel.HotelSupImgs.Any())
            {
                foreach (var supImg in hotel.HotelSupImgs)
                {
                    var supImgPath = Path.Combine(supImagesFolder, supImg.SupImg);
                    if (System.IO.File.Exists(supImgPath))
                        System.IO.File.Delete(supImgPath);
                }

                // حذف الصور الفرعية من قاعدة البيانات
                _hotelSupImgsRepository.RemoveHotelSupImages(hotel.HotelSupImgs.ToList());
                await _hotelSupImgsRepository.CommitAsync(cancellationToken);
            }

            // حذف الفندق نفسه
            _hotelRepository.Delete(hotel);
            await _hotelRepository.CommitAsync(cancellationToken);

            return RedirectToAction("Index");
        }

    }
}
