using Ecommerce.Utilities;
using Entertainment_travel_booking_website.Models;
using Entertainment_travel_booking_website.modelVM;
using Entertainment_travel_booking_website.Repository;
using Entertainment_travel_booking_website.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Entertainment_travel_booking_website.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =$"{DS.SUPER_ADMIN_ROLE},{DS.ADMIN_ROLE},{DS.EMPLOYEE_ROLE}")]
    public class AdditionalActivitiesController : Controller
    {
        private readonly IAdditianActivitiesRepository _activitiesRepo;
        private readonly IAdditionalActivitySubImageRepository _subImageRepo;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AdditionalActivitiesController(
            IAdditianActivitiesRepository activitiesRepo,
            IAdditionalActivitySubImageRepository subImageRepo,
            IWebHostEnvironment webHostEnvironment)
        {
            _activitiesRepo = activitiesRepo;
            _subImageRepo = subImageRepo;
            _webHostEnvironment = webHostEnvironment;
        }

        // ----------------- Index -----------------
        public async Task<IActionResult> Index()
        {
            var activities = await _activitiesRepo.GetAsync(
                includes: new Expression<Func<AdditianActivities, object>>[]
                {
            a => a.ActivitiesSupImgs 
                }
            );

            return View(activities);
        }

        // ----------------- Create -----------------
        [HttpGet]
        [Authorize(Roles = $"{DS.SUPER_ADMIN_ROLE},{DS.ADMIN_ROLE},{DS.EMPLOYEE_ROLE}")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{DS.SUPER_ADMIN_ROLE},{DS.ADMIN_ROLE},{DS.EMPLOYEE_ROLE}")]
        public async Task<IActionResult> Create(AdditionalActivityVM vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var activity = new AdditianActivities
            {
                Place = vm.Place,
                Description = vm.Description,
                Price = vm.Price,
                Date = vm.Date,
                ActivitiesSupImgs = new List<ActivitiesSupImg>()
            };

            if (vm.SubImageFiles != null && vm.SubImageFiles.Any())
            {
                string folder = Path.Combine(_webHostEnvironment.WebRootPath, "images/additionalActivities");
                if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

                foreach (var file in vm.SubImageFiles)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string path = Path.Combine(folder, fileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    activity.ActivitiesSupImgs.Add(new ActivitiesSupImg
                    {
                        SupImg = fileName 
                    });
                }
            }

            await _activitiesRepo.AddAsync(activity);
            TempData["sucess-Notification"] = "Activity Create Successfully";

            return RedirectToAction(nameof(Index));
        }

        // ----------------- Edit -----------------
        [HttpGet]
        [Authorize(Roles = $"{DS.SUPER_ADMIN_ROLE},{DS.ADMIN_ROLE}")]
        public async Task<IActionResult> Edit(int id)
        {
            var activity = await _activitiesRepo.GetOneAsync(
                e => e.Id == id,
                includes: new Expression<Func<AdditianActivities, object>>[] { e => e.ActivitiesSupImgs }
            );
            if (activity == null) return NotFound();

            var vm = new AdditionalActivityVM
            {
                Id = activity.Id,
                Place = activity.Place,
                Description = activity.Description,
                Price = activity.Price,
                Date = activity.Date,
                ExistingSubImages = activity.ActivitiesSupImgs?.Select(s => s.SupImg).ToList() 
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{DS.SUPER_ADMIN_ROLE},{DS.ADMIN_ROLE}")]
        public async Task<IActionResult> Edit(AdditionalActivityVM vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var activity = await _activitiesRepo.GetOneAsync(
                e => e.Id == vm.Id,
                includes: new Expression<Func<AdditianActivities, object>>[] { e => e.ActivitiesSupImgs }
            );
            if (activity == null) return NotFound();

            activity.Place = vm.Place;
            activity.Description = vm.Description;
            activity.Price = vm.Price;
            activity.Date = vm.Date;

        
            if (vm.SubImageFiles != null && vm.SubImageFiles.Any())
            {
                string folder = Path.Combine(_webHostEnvironment.WebRootPath, "images/additionalActivities");
                if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

                var subImages = new List<ActivitiesSupImg>();

                foreach (var file in vm.SubImageFiles)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string path = Path.Combine(folder, fileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    subImages.Add(new ActivitiesSupImg
                    {
                        SupImg = fileName, 
                        AdditianActivitiesId = activity.Id
                    });
                }

                await _subImageRepo.AddAdditionActivitySupImagesAsync(subImages);
            }

            await _activitiesRepo.UpdateAsync(activity);
            TempData["sucess-Notification"] = "Activity Edit Successfully";

            return RedirectToAction(nameof(Index));
        }

        // ----------------- Delete -----------------
        [HttpGet]
        [Authorize(Roles = $"{DS.SUPER_ADMIN_ROLE},{DS.ADMIN_ROLE}")]
        public async Task<IActionResult> Delete(int id)
        {
            var activity = await _activitiesRepo.GetOneAsync(
                e => e.Id == id,
                includes: new Expression<Func<AdditianActivities, object>>[] { e => e.ActivitiesSupImgs }
            );

            if (activity == null) return NotFound();

          
            var folder = Path.Combine(_webHostEnvironment.WebRootPath, "images/additionalActivities");

        
            if (activity.ActivitiesSupImgs != null && activity.ActivitiesSupImgs.Any())
            {
                foreach (var img in activity.ActivitiesSupImgs)
                {
                    var imgPath = Path.Combine(folder, img.SupImg); 
                    if (System.IO.File.Exists(imgPath))
                    {
                        System.IO.File.Delete(imgPath);
                    }
                }

                
                _subImageRepo.RemoveAdditionActivitySupImages(activity.ActivitiesSupImgs.ToList());
                await _subImageRepo.CommitAsync();
            }

          
            await _activitiesRepo.DeleteAsync(activity.Id);
            TempData["sucess-Notification"] = "Activity Delete Successfully";

            return RedirectToAction(nameof(Index));
        }
    }
}