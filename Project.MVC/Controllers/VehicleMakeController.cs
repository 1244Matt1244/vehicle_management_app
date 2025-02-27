using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Project.MVC.Helpers;
using Project.MVC.ViewModels;
using Project.Service.Data.DTOs;
using Project.Service.Interfaces;
using System.Collections.Generic;
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

        // Existing Index action
        public async Task<IActionResult> Index(
            int pageNumber = 1,
            int pageSize = 10,
            string sortBy = "Name",
            string sortOrder = "asc",
            string searchString = "")
        {
            var serviceResult = await _vehicleService.GetMakesAsync(
                pageNumber, 
                pageSize, 
                sortBy, 
                sortOrder, 
                searchString);

            var pagedResult = new PagedResult<VehicleMakeVM>
            {
                Items = _mapper.Map<List<VehicleMakeVM>>(serviceResult.Items),
                TotalCount = serviceResult.TotalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            return View(pagedResult);
        }

        // Create action - GET
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // Create action - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] VehicleMakeVM make)
        {
            if (ModelState.IsValid)
            {
                await _vehicleService.AddMakeAsync(_mapper.Map<VehicleMakeDTO>(make));
                return CreatedAtAction(nameof(Details), new { id = make.Id }, make); // 201 Created
            }
            return View(make); // 400 Bad Request
        }

        // Edit action - GET
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var make = await _vehicleService.GetMakeByIdAsync(id);
            if (make == null) return NotFound();
            return View(_mapper.Map<VehicleMakeVM>(make));
        }

        // Edit action - POST
        [HttpPost]
        public async Task<IActionResult> Edit(VehicleMakeVM model)
        {
            if (ModelState.IsValid)
            {
                await _vehicleService.UpdateMakeAsync(_mapper.Map<VehicleMakeDTO>(model));
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // Details action - GET
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var make = await _vehicleService.GetMakeByIdAsync(id);
            if (make == null) return NotFound();
            return View(_mapper.Map<VehicleMakeVM>(make));
        }

        // Delete action - GET
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var make = await _vehicleService.GetMakeByIdAsync(id);
            if (make == null) return NotFound();
            return View(_mapper.Map<VehicleMakeVM>(make));
        }

        // Delete action - POST
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _vehicleService.DeleteMakeAsync(id);
                return NoContent(); // 204 No Content
            }
            catch (KeyNotFoundException)
            {
                return NotFound(); // 404 Not Found
            }
        }
    }
}
