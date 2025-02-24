// Project.MVC/Controllers/VehicleMakeController.cs
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Project.Service.Interfaces;
using Project.Service.Data.DTOs;
using Project.MVC.ViewModels;
using Project.Service.Data.Helpers;
using System.Threading.Tasks;

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

        // GET: VehicleMake/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var makeDto = await _vehicleService.GetMakeByIdAsync(id);
            if (makeDto == null) return NotFound();
            return View(_mapper.Map<VehicleMakeVM>(makeDto));
        }

        // POST: VehicleMake/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, VehicleMakeVM makeVm)
        {
            if (id != makeVm.Id) return BadRequest();

            if (ModelState.IsValid)
            {
                await _vehicleService.UpdateMakeAsync(
                    id, 
                    _mapper.Map<VehicleMakeDTO>(makeVm) // Fixed parameter
                );
                return RedirectToAction(nameof(Index));
            }
            return View(makeVm);
        }
    }
}