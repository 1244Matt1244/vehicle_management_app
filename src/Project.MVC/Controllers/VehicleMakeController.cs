using Microsoft.AspNetCore.Mvc;
using Project.Service.Data.DTOs;
using Project.Service.Data.Helpers;
using Project.Service.Data;
using Project.Service.Interfaces;


namespace Project.MVC.Controllers
{
    public class VehicleMakeController : Controller
    {
        private readonly IVehicleService _vehicleService;

        public VehicleMakeController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        public async Task<IActionResult> Index()
        {
            var makes = await _vehicleService.GetAllMakesAsync();
            return View(makes);
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
        public async Task<IActionResult> Edit(VehicleMakeDTO makeDto)
        {
            if (ModelState.IsValid)
            {
                await _vehicleService.UpdateMakeAsync(makeDto);
                return RedirectToAction(nameof(Index));
            }
            return View(makeDto);
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _vehicleService.DeleteMakeAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}