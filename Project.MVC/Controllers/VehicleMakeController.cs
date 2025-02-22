using Microsoft.AspNetCore.Mvc;
using Project.Service.Data.DTOs;
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

        public async Task<IActionResult> Details(int id)
        {
            var make = await _vehicleService.GetMakeByIdAsync(id);
            if (make == null) return NotFound(); // 404
            return View(make);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _vehicleService.DeleteMakeAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (KeyNotFoundException)
            {
                return NotFound(); // 404
            }
        }
    }
}