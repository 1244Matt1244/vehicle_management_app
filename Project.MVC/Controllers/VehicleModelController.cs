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
    public class VehicleModelController : Controller
    {
        private readonly IVehicleService _vehicleService;
        private readonly IMapper _mapper;

        public VehicleModelController(IVehicleService vehicleService, IMapper mapper)
        {
            _vehicleService = vehicleService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(
            int pageNumber = 1,
            int pageSize = 10,
            string sortBy = "Name",
            string sortOrder = "asc",
            string searchString = "",
            int? makeId = null)
        {
            var serviceResult = await _vehicleService.GetModelsAsync(
                pageNumber,
                pageSize,
                sortBy,
                sortOrder,
                searchString,
                makeId);

            var makes = await _vehicleService.GetAllMakesAsync();

            var viewModel = new VehicleModelIndexVM
            {
                PagedResults = new PagedResult<VehicleModelVM>
                {
                    Items = _mapper.Map<List<VehicleModelVM>>(serviceResult.Items),
                    TotalCount = serviceResult.TotalCount,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                },
                Makes = _mapper.Map<List<VehicleMakeVM>>(makes)
            };

            ViewBag.SortOrder = sortOrder;
            ViewBag.CurrentSort = sortBy;
            ViewBag.CurrentFilter = searchString;

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var makes = await _vehicleService.GetAllMakesAsync();
            var vm = new VehicleModelCreateVM
            {
                AvailableMakes = _mapper.Map<List<VehicleMakeVM>>(makes)
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(VehicleModelCreateVM model)
        {
            if (ModelState.IsValid)
            {
                await _vehicleService.AddModelAsync(_mapper.Map<VehicleModelDTO>(model));
                return RedirectToAction(nameof(Index)); // Redirect to Index after creation
            }
            model.AvailableMakes = _mapper.Map<List<VehicleMakeVM>>(await _vehicleService.GetAllMakesAsync());
            return View(model); // Return to Create view if validation fails
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _vehicleService.GetModelByIdAsync(id);
            if (model == null) return NotFound(); // Return 404 if model not found

            var vm = _mapper.Map<VehicleModelEditVM>(model);
            vm.AvailableMakes = _mapper.Map<List<VehicleMakeVM>>(await _vehicleService.GetAllMakesAsync());
            
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(VehicleModelEditVM model)
        {
            if (ModelState.IsValid)
            {
                await _vehicleService.UpdateModelAsync(_mapper.Map<VehicleModelDTO>(model));
                return RedirectToAction(nameof(Index)); // Redirect to Index after successful update
            }
            
            model.AvailableMakes = _mapper.Map<List<VehicleMakeVM>>(
                await _vehicleService.GetAllMakesAsync()
            );
            return View(model); // Return view with validation errors if any
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var model = await _vehicleService.GetModelByIdAsync(id);
            if (model == null) return NotFound(); // Return 404 if not found
            return View(_mapper.Map<VehicleModelVM>(model)); // Display model details
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var model = await _vehicleService.GetModelByIdAsync(id);
            if (model == null) return NotFound(); // Return 404 if model not found
            return View(_mapper.Map<VehicleModelVM>(model)); // Show confirmation for deletion
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _vehicleService.DeleteModelAsync(id);
            return RedirectToAction(nameof(Index)); // Redirect to Index after deletion
        }
    }
}
