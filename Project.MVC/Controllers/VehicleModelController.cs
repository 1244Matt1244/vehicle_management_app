// Project.MVC/Controllers/VehicleModelController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using AutoMapper;
using Project.Service.Interfaces;
using Project.MVC.ViewModels;
using System.Threading.Tasks;
using Project.Service.Data.Helpers;
using System.Collections.Generic;

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

        // GET: VehicleModel/Index
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
            ViewBag.CurrentSort = sortBy;
            ViewBag.SortOrder = sortOrder;
            ViewBag.SearchString = searchString;
            ViewBag.MakeId = makeId;

            return View(modelsVm);
        }

        // GET: VehicleModel/Create
        public async Task<IActionResult> Create()
        {
            var makesDto = await _vehicleService.GetAllMakesAsync();
            ViewBag.Makes = new SelectList(makesDto, "Id", "Name");
            return View();
        }

        // POST: VehicleModel/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VehicleModelVM modelVm)
        {
            if (ModelState.IsValid)
            {
                var modelDto = _mapper.Map<VehicleModelDTO>(modelVm);
                await _vehicleService.CreateModelAsync(modelDto);
                TempData["SuccessMessage"] = "Vehicle model created successfully!";
                return RedirectToAction(nameof(Index));
            }

            var makesDto = await _vehicleService.GetAllMakesAsync();
            ViewBag.Makes = new SelectList(makesDto, "Id", "Name", modelVm.MakeId);
            TempData["ErrorMessage"] = "Failed to create vehicle model.";
            return View(modelVm);
        }

        // GET: VehicleModel/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var modelDto = await _vehicleService.GetModelByIdAsync(id);
            if (modelDto == null)
            {
                return NotFound();
            }

            var modelVm = _mapper.Map<VehicleModelVM>(modelDto);
            var makesDto = await _vehicleService.GetAllMakesAsync();
            ViewBag.Makes = new SelectList(makesDto, "Id", "Name", modelVm.MakeId);
            return View(modelVm);
        }

        // POST: VehicleModel/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, VehicleModelVM modelVm)
        {
            if (id != modelVm.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var modelDto = _mapper.Map<VehicleModelDTO>(modelVm);
                await _vehicleService.UpdateModelAsync(modelDto);
                TempData["SuccessMessage"] = "Vehicle model updated successfully!";
                return RedirectToAction(nameof(Index));
            }

            var makesDto = await _vehicleService.GetAllMakesAsync();
            ViewBag.Makes = new SelectList(makesDto, "Id", "Name", modelVm.MakeId);
            TempData["ErrorMessage"] = "Failed to update vehicle model.";
            return View(modelVm);
        }

        // GET: VehicleModel/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var modelDto = await _vehicleService.GetModelByIdAsync(id);
            if (modelDto == null)
            {
                return NotFound();
            }

            var modelVm = _mapper.Map<VehicleModelVM>(modelDto);
            return View(modelVm);
        }

        // POST: VehicleModel/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _vehicleService.DeleteModelAsync(id);
            TempData["SuccessMessage"] = "Vehicle model deleted successfully!";
            return RedirectToAction(nameof(Index));
        }
    }
}
