using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Project.Service.Interfaces;
using Project.Service.Data.DTOs;
using AutoMapper;
using Project.MVC.ViewModels;

namespace Project.MVC.Controllers
{
    public class VehicleMakeController : Controller
    {
        private readonly IVehicleService _vehicleService;
        private readonly IMapper _mapper;

        public VehicleMakeController(IVehicleService vehicleService, IMapper mapper)
        {
            _vehicleService = vehicleService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Edit(int id)
        {
            var makeDto = await _vehicleService.GetMakeByIdAsync(id);
            if (makeDto == null) return NotFound();
            
            return View(_mapper.Map<VehicleMakeVM>(makeDto));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _vehicleService.DeleteMakeAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (System.Collections.Generic.KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VehicleMakeVM makeVm)
        {
            if (!ModelState.IsValid) return View(makeVm);

            var makeDto = _mapper.Map<VehicleMakeDTO>(makeVm);
            await _vehicleService.CreateMakeAsync(makeDto);
            return RedirectToAction(nameof(Index));
        }
    }
}