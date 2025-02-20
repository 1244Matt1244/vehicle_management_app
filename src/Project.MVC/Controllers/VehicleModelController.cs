using Microsoft.AspNetCore.Mvc;
using Project.Service.Interfaces;
using Project.Service.Data.DTOs;

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
            var makes = await _vehicleService.GetAllMakesAsync();
            ViewBag.Makes = makes; // If you want to stick with ViewBag
            return View(makes); // Pass makes directly to the view
        }
    }
}
