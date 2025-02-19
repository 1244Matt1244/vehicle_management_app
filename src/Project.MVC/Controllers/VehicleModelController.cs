using Microsoft.AspNetCore.Mvc;
using Project.Service.Data.DTOs;
using Project.Service.Data.Helpers;
using Project.Service.Data;
using Project.Service.Interfaces;

namespace Project.MVC.Controllers
{
    public class VehicleModelController : Controller
    {
        private readonly IVehicleService _vehicleService;

        public VehicleModelController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        public async Task<IActionResult> Index(string sortOrder, string searchString, int pageNumber = 1, int pageSize = 10)
        {
            var models = await _vehicleService.GetModelsAsync(sortOrder, searchString, pageNumber, pageSize);
            return View(models);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(VehicleModelDTO modelDto)
        {
            if (ModelState.IsValid)
            {
                await _vehicleService.CreateModelAsync(modelDto);
                return RedirectToAction(nameof(Index));
            }
            return View(modelDto);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var model = await _vehicleService.GetModelByIdAsync(id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(VehicleModelDTO modelDto)
        {
            if (ModelState.IsValid)
            {
                await _vehicleService.UpdateModelAsync(modelDto);
                return RedirectToAction(nameof(Index));
            }
            return View(modelDto);
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _vehicleService.DeleteModelAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}