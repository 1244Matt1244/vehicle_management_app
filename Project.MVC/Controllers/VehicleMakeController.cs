using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Project.MVC.ViewModels;
using Project.Service.Interfaces;
using Project.Service.Data.DTOs;

namespace Project.MVC.Controllers
{
    public class VehicleMakeController : Controller
    {
        private readonly IVehicleService _vehicleService;
        private readonly IMapper _mapper;

        public VehicleMakeController(IVehicleService vehicleService, IMapper mapper)
        {
            _vehicleService = vehicleService ?? throw new ArgumentNullException(nameof(vehicleService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        // Displays a paginated, sorted, and searchable list of vehicle makes
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10, string sortBy = "Name", string sortOrder = "asc", string search = "")
        {
            var result = await _vehicleService.GetMakesAsync(page, pageSize, sortBy, sortOrder, search);
            
            var viewModel = new VehicleMakeIndexVM
            {
                Items = _mapper.Map<List<VehicleMakeVM>>(result.Items),
                Pagination = new PaginationVM
                {
                    Page = result.PageIndex,
                    PageSize = result.PageSize,
                    TotalItems = result.TotalCount
                },
                CurrentSort = sortBy,
                SortOrder = sortOrder,
                SearchString = search
            };

            return View(viewModel);
        }

        // Creates a new vehicle make
        [HttpPost]
        public async Task<IActionResult> Create(VehicleMakeVM viewModel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _vehicleService.CreateMakeAsync(_mapper.Map<VehicleMakeDTO>(viewModel));
            return RedirectToAction(nameof(Index));
        }

        // Retrieves a vehicle make for editing
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var makeDto = await _vehicleService.GetMakeByIdAsync(id);
            return makeDto == null ? NotFound() : View(_mapper.Map<VehicleMakeVM>(makeDto));
        }

        // Updates the vehicle make
        [HttpPost]
        public async Task<IActionResult> Edit(VehicleMakeVM viewModel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _vehicleService.UpdateMakeAsync(_mapper.Map<VehicleMakeDTO>(viewModel));
            return RedirectToAction(nameof(Index));
        }

        // Deletes a vehicle make
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _vehicleService.DeleteMakeAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // Displays detailed information about a specific vehicle make
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var makeDto = await _vehicleService.GetMakeByIdAsync(id);
            return makeDto == null ? NotFound() : View(_mapper.Map<VehicleMakeVM>(makeDto));
        }
    }
}
