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

        public async Task<IActionResult> Index(
            int pageNumber = 1,
            int pageSize = 10,
            string sortBy = "Name",
            string sortOrder = "asc",
            string searchString = "")
        {
            var serviceResult = await _vehicleService.GetMakesAsync(pageNumber, pageSize, sortBy, sortOrder, searchString);

            var pagedResult = new PagedResult<VehicleMakeVM>
            {
                Items = _mapper.Map<List<VehicleMakeVM>>(serviceResult.Items),
                TotalCount = serviceResult.TotalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            ViewBag.SortOrder = sortOrder;
            ViewBag.CurrentSort = sortBy;
            ViewBag.CurrentFilter = searchString;

            return View(pagedResult);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VehicleMakeVM vehicleMake)
        {
            if (ModelState.IsValid)
            {
                var vehicleMakeDTO = _mapper.Map<VehicleMakeDTO>(vehicleMake);
                await _vehicleService.AddMakeAsync(vehicleMakeDTO);
                return RedirectToAction(nameof(Index));
            }
            return View(vehicleMake);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var make = await _vehicleService.GetMakeByIdAsync(id);
            if (make == null) return NotFound();
            return View(_mapper.Map<VehicleMakeVM>(make));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, VehicleMakeVM vehicleMake)
        {
            if (id != vehicleMake.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var vehicleMakeDTO = _mapper.Map<VehicleMakeDTO>(vehicleMake);
                await _vehicleService.UpdateMakeAsync(vehicleMakeDTO);
                return RedirectToAction(nameof(Index));
            }
            return View(vehicleMake);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var make = await _vehicleService.GetMakeByIdAsync(id);
            if (make == null) return NotFound();
            return View(_mapper.Map<VehicleMakeVM>(make));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var make = await _vehicleService.GetMakeByIdAsync(id);
            if (make == null) return NotFound();
            return View(_mapper.Map<VehicleMakeVM>(make));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _vehicleService.DeleteMakeAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
