using Microsoft.AspNetCore.Mvc;
using Project.Service.Interfaces;
using Project.MVC.ViewModels;

namespace Project.MVC.Controllers
{
    public class VehicleModelController : Controller
    {
        private readonly IVehicleService _vehicleService;

        public VehicleModelController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        public async Task<IActionResult> Index()
        {
            var models = await _vehicleService.GetModelsAsync();
            return View(models);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(VehicleModelVM model)
        {
            if (ModelState.IsValid)
            {
                await _vehicleService.CreateModelAsync(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }
    }
}
