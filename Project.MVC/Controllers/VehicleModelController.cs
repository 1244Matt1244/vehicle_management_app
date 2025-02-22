using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Project.Service.Interfaces;
using Project.MVC.ViewModels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Project.Service.Data.DTOs; 

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
            int? makeId,
            int page = 1,
            int pageSize = 10,
            string sortBy = "Name",
            string sortOrder = "asc",
            string searchString = "")
        {
            var modelsDto = await _vehicleService.GetModelsAsync(
                page,
                pageSize,
                sortBy,
                sortOrder,
                searchString,
                makeId
            );

            var modelsVm = _mapper.Map<PaginatedList<VehicleModelVM>>(modelsDto);
            var makesDto = await _vehicleService.GetAllMakesAsync();

            ViewBag.Makes = new SelectList(makesDto, "Id", "Name", makeId);
            ViewBag.PageSize = pageSize;
            ViewBag.SortBy = sortBy;
            ViewBag.SortOrder = sortOrder;
            ViewBag.SearchString = searchString;

            return View(modelsVm);
        }

        public async Task<IActionResult> Create()
        {
            var makesDto = await _vehicleService.GetAllMakesAsync();
            ViewBag.Makes = new SelectList(makesDto, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VehicleModelVM modelVm)
        {
            if (ModelState.IsValid)
            {
                var modelDto = _mapper.Map<VehicleModelDTO>(modelVm);
                await _vehicleService.CreateModelAsync(modelDto);
                return RedirectToAction(nameof(Index));
            }
            return View(modelVm);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var modelDto = await _vehicleService.GetModelByIdAsync(id);
            if (modelDto == null) return NotFound();

            var modelVm = _mapper.Map<VehicleModelVM>(modelDto);
            var makesDto = await _vehicleService.GetAllMakesAsync();
            ViewBag.Makes = new SelectList(makesDto, "Id", "Name");

            return View(modelVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, VehicleModelVM modelVm)
        {
            if (id != modelVm.Id) return BadRequest();

            if (ModelState.IsValid)
            {
                var modelDto = _mapper.Map<VehicleModelDTO>(modelVm);
                await _vehicleService.UpdateModelAsync(id, modelDto);
                return RedirectToAction(nameof(Index));
            }

            var makesDto = await _vehicleService.GetAllMakesAsync();
            ViewBag.Makes = new SelectList(makesDto, "Id", "Name");
            return View(modelVm);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var modelDto = await _vehicleService.GetModelByIdAsync(id);
            if (modelDto == null) return NotFound();

            var modelVm = _mapper.Map<VehicleModelVM>(modelDto);
            return View(modelVm);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _vehicleService.DeleteModelAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}