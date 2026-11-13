using Microsoft.AspNetCore.Mvc;
using ZombieParty.Models;
using ZombieParty.Services;
using ZombieParty.Utility;
using ZombieParty.ViewModels;

namespace ZombieParty.Controllers
{
    public class ZombieTypeController : Controller
    {
        private IZombieTypeService _serviceZT { get; set; }

        public ZombieTypeController(IZombieTypeService serviceZT)
        {
            _serviceZT = serviceZT;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<ZombieType> zombieTypesList = await _serviceZT.GetAllAsync();
            return View(zombieTypesList);
        }

        //public async Task<IActionResult> Details(int id)
        //{
        //    ... (à faire plus tard avec IZombieService)
        //}

        //GET CREATE
        public IActionResult Create()
        {
            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ZombieType zombieType)
        {
            if (ModelState.IsValid)
            {
                await _serviceZT.CreateAsync(zombieType);   // le SaveChangesAsync est déjà inclus
                TempData[AppConstants.Success] = $"{zombieType.TypeName} zombie type added";
                return this.RedirectToAction("Index");
            }

            return this.View(zombieType);
        }

        public async Task<IActionResult> Edit(int id)
        {
            ZombieType? zombieType = await _serviceZT.GetByIdAsync(id);
            return View(zombieType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ZombieType zombieType)
        {
            if (ModelState.IsValid)
            {
                await _serviceZT.EditAsync(zombieType);
                TempData[AppConstants.Success] = $"ZombieType {zombieType.TypeName} has been modified";
                return this.RedirectToAction("Index");
            }

            return View(zombieType);
        }

        public async Task<IActionResult> Delete(int id)
        {
            ZombieType? zombieType = await _serviceZT.GetByIdAsync(id);
            if (zombieType == null)
            {
                return NotFound();
            }

            return View(zombieType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePost(int id)
        {
            ZombieType? zombieType = await _serviceZT.GetByIdAsync(id);
            if (zombieType == null)
            {
                return NotFound();
            }

            await _serviceZT.DeleteAsync(id);
            TempData[AppConstants.Success] = $"ZombieType {zombieType.TypeName} has been removed";
            return RedirectToAction("Index");
        }
    }
}