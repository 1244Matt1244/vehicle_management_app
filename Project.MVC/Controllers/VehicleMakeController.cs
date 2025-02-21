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

        // Index - Displays a paginated list of vehicle makes
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

        // Create - Displays the create form and handles the post request
        [HttpPost]
        public async Task<IActionResult> Create(VehicleMakeVM viewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var dto = _mapper.Map<VehicleMakeDTO>(viewModel);
            await _vehicleService.CreateMakeAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        // Edit (GET) - Retrieves a specific vehicle make for editing
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var makeDto = await _vehicleService.GetMakeByIdAsync(id);
            if (makeDto == null)
                return NotFound();

            var viewModel = _mapper.Map<VehicleMakeVM>(makeDto);
            return View(viewModel);
        }

        // Edit (POST) - Updates the vehicle make
        [HttpPost]
        public async Task<IActionResult> Edit(VehicleMakeVM viewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var dto = _mapper.Map<VehicleMakeDTO>(viewModel);
            await _vehicleService.UpdateMakeAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        // Delete - Handles the deletion of a vehicle make
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _vehicleService.DeleteMakeAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // Details - Displays detailed information about a specific vehicle make
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var makeDto = await _vehicleService.GetMakeByIdAsync(id);
            if (makeDto == null)
                return NotFound();

            var viewModel = _mapper.Map<VehicleMakeVM>(makeDto);
            return View(viewModel);
        }
    }
}
