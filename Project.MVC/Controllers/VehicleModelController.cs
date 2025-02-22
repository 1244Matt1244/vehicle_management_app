using Microsoft.AspNetCore.Mvc;
using Project.Service.Data.DTOs;
using Project.Service.Data.Helpers;
using Project.Service.Interfaces;
using System.Threading.Tasks;

namespace Project.MVC.Controllers
{
    public class VehicleModelController : Controller
    {
        private readonly IVehicleService _vehicleService;

        public VehicleModelController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        // GET: VehicleModel
        public async Task<IActionResult> Index(
            int? makeId,
            int page = 1,
            int pageSize = 10,
            string sortBy = "Name",
            string sortOrder = "asc",
            string searchString = "")
        {
            var models = await _vehicleService.GetModelsAsync(
                page,
                pageSize,
                sortBy,
                sortOrder,
                searchString,
                makeId
            );

            ViewBag.MakeId = makeId;
            ViewBag.PageSize = pageSize;
            ViewBag.SortBy = sortBy;
            ViewBag.SortOrder = sortOrder;
            ViewBag.SearchString = searchString;

            return View(models);
        }

        // GET: VehicleModel/Create
        public async Task<IActionResult> Create()
        {
            var makes = await _vehicleService.GetAllMakesAsync();
            ViewBag.Makes = makes;
            return View();
        }

        // POST: VehicleModel/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VehicleModelDTO modelDto)
        {
            if (ModelState.IsValid)
            {
                await _vehicleService.CreateModelAsync(modelDto);
                return RedirectToAction(nameof(Index));
            }
            return View(modelDto);
        }

        // GET: VehicleModel/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var modelDto = await _vehicleService.GetModelByIdAsync(id);
            if (modelDto == null)
            {
                return NotFound();
            }

            var makes = await _vehicleService.GetAllMakesAsync();
                        ViewBag.Makes = makes;
            return View(modelDto);
        }

        // POST: VehicleModel/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, VehicleModelDTO modelDto)
        {
            if (id != modelDto.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                await _vehicleService.UpdateModelAsync(modelDto);
                return RedirectToAction(nameof(Index));
            }

            // If we got this far, something failed; redisplay the form.
            var makes = await _vehicleService.GetAllMakesAsync();
            ViewBag.Makes = makes;
            return View(modelDto);
        }

        // GET: VehicleModel/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var modelDto = await _vehicleService.GetModelByIdAsync(id);
            if (modelDto == null)
            {
                return NotFound();
            }

            return View(modelDto);
        }

        // POST: VehicleModel/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _vehicleService.DeleteModelAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
