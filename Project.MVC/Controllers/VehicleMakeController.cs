using Microsoft.AspNetCore.Mvc;
using Project.Service.Data.DTOs;
using Project.Service.Interfaces;
using Project.MVC.ViewModels;
using AutoMapper;
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

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VehicleMakeVM model)
        {
            if (ModelState.IsValid)
            {
                var makeDto = _mapper.Map<VehicleMakeDTO>(model);
                await _vehicleService.AddMakeAsync(makeDto);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var makeDto = await _vehicleService.GetMakeByIdAsync(id);
            if (makeDto == null)
            {
                return NotFound();
            }
            var model = _mapper.Map<VehicleMakeVM>(makeDto);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, VehicleMakeVM model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }
            
            if (ModelState.IsValid)
            {
                var makeDto = _mapper.Map<VehicleMakeDTO>(model);
                await _vehicleService.UpdateMakeAsync(makeDto);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var makeDto = await _vehicleService.GetMakeByIdAsync(id);
            if (makeDto == null)
            {
                return NotFound();
            }
            var model = _mapper.Map<VehicleMakeVM>(makeDto);
            return View(model);
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
