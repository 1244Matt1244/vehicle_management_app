using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Project.Service.Interfaces;
using Project.Service.Data.DTOs;
using Project.Service.Data.Helpers;
using Project.MVC.ViewModels;
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

        // GET: VehicleMake (List with Pagination, Sorting, and Searching)
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

        // GET: VehicleMake/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var makeDto = await _vehicleService.GetMakeByIdAsync(id);
            if (makeDto == null) return NotFound();
            return View(_mapper.Map<VehicleMakeVM>(makeDto));
        }

        // GET: VehicleMake/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: VehicleMake/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VehicleMakeVM makeVm)
        {
            if (!ModelState.IsValid) return View(makeVm);

            var makeDto = _mapper.Map<VehicleMakeDTO>(makeVm);
            await _vehicleService.CreateMakeAsync(makeDto);
            return RedirectToAction(nameof(Index));
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
                await _vehicleService.UpdateMakeAsync(id, _mapper.Map<VehicleMakeDTO>(makeVm));
                return RedirectToAction(nameof(Index));
            }
            return View(makeVm);
        }

        // GET: VehicleMake/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var makeDto = await _vehicleService.GetMakeByIdAsync(id);
            if (makeDto == null) return NotFound();
            return View(_mapper.Map<VehicleMakeVM>(makeDto));
        }

        // POST: VehicleMake/DeleteConfirmed
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
    }
}
