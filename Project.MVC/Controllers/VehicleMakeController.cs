using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Project.Service.Interfaces;
using Project.Service.Data.DTOs;
using Project.Service.Data.Helpers;

namespace Project.MVC.Controllers
{
    public class VehicleMakeController : Controller
    {
        private readonly IVehicleService _vehicleService;

        public VehicleMakeController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 10, string sortBy = "Name", string sortOrder = "asc", string search = "")
        {
            var makes = await _vehicleService.GetMakesAsync(pageIndex, pageSize, sortBy, sortOrder, search);
            return View(makes);
        }

        public async Task<IActionResult> Details(int id)
        {
            var make = await _vehicleService.GetMakeByIdAsync(id);
            if (make == null)
            {
                return NotFound();
            }
            return View(make);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VehicleMakeDTO make)
        {
            if (ModelState.IsValid)
            {
                await _vehicleService.CreateMakeAsync(make);
                return RedirectToAction("Index");
            }
            return View(make);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var make = await _vehicleService.GetMakeByIdAsync(id);
            if (make == null)
            {
                return NotFound();
            }
            return View(make);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(VehicleMakeDTO make)
        {
            if (ModelState.IsValid)
            {
                await _vehicleService.UpdateMakeAsync(make);
                return RedirectToAction("Index");
            }
            return View(make);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var make = await _vehicleService.GetMakeByIdAsync(id);
            if (make == null)
            {
                return NotFound();
            }
            return View(make);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _vehicleService.DeleteMakeAsync(id);
            return RedirectToAction("Index");
        }
    }
}
