using Microsoft.AspNetCore.Mvc;
using Project.Service.Data.DTOs;
using Project.Service.Data.Helpers;
using Project.Service.Interfaces;
using System.Threading.Tasks;

namespace Project.MVC.Controllers
{
    public class VehicleMakeController : Controller
    {
        private readonly IVehicleService _vehicleService;

        public VehicleMakeController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        public async Task<IActionResult> Index(
            int page = 1,
            int pageSize = 10,
            string sortBy = "Name",
            string sortOrder = "asc",
            string searchString = "")
        {
            var makes = await _vehicleService.GetMakesAsync(page, pageSize, sortBy, sortOrder, searchString);
            return View(makes);
        }

        public async Task<IActionResult> Details(int id)
        {
            var make = await _vehicleService.GetMakeByIdAsync(id);
            return View(make);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(VehicleMakeDTO makeDto)
        {
            if (ModelState.IsValid)
            {
                await _vehicleService.CreateMakeAsync(makeDto);
                return RedirectToAction(nameof(Index));
            }
            return View(makeDto);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var make = await _vehicleService.GetMakeByIdAsync(id);
            return View(make);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, VehicleMakeDTO makeDto)
        {
            if (id != makeDto.Id) return NotFound();
            
            if (ModelState.IsValid)
            {
                await _vehicleService.UpdateMakeAsync(id, makeDto);
                return RedirectToAction(nameof(Index));
            }
            return View(makeDto);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var make = await _vehicleService.GetMakeByIdAsync(id);
            return View(make);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _vehicleService.DeleteMakeAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}