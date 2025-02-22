using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Project.Service.Interfaces;
using Project.Service.Data.DTOs;
using AutoMapper;
using Project.MVC.ViewModels;
using Project.Service.Data.Helpers; // Ensure you have a helper for PaginatedList

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

        // Index Action (List with Pagination, Sorting, and Searching)
        public async Task<IActionResult> Index(
            int page = 1,
            int pageSize = 10,
            string sortBy = "Name",
            string sortOrder = "asc",
            string searchString = "")
        {
            var makesDto = await _vehicleService.GetMakesAsync(page, pageSize, sortBy, sortOrder, searchString);
            var makesVm = _mapper.Map<PaginatedList<VehicleMakeVM>>(makesDto);

            var viewModel = new VehicleMakeVM
            {
                PaginatedList = makesVm,
                CurrentSort = sortBy,
                SortOrder = sortOrder,
                SearchString = searchString
            };

            return View(viewModel);
        }

        // Edit Vehicle Make
        public async Task<IActionResult> Edit(int id)
        {
            var makeDto = await _vehicleService.GetMakeByIdAsync(id);
            if (makeDto == null) return NotFound();

            return View(_mapper.Map<VehicleMakeVM>(makeDto));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, VehicleMakeVM makeVm)
        {
            if (!ModelState.IsValid) return View(makeVm);

            var makeDto = _mapper.Map<VehicleMakeDTO>(makeVm);
            await _vehicleService.UpdateMakeAsync(id, makeDto);

            return RedirectToAction(nameof(Index));
        }

        // Delete Vehicle Make
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

        // Create Vehicle Make
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
