/* This C# code snippet represents a controller class named `VehicleModelController` in an ASP.NET Core
MVC project. Here's a breakdown of what the code is doing: */
using Microsoft.AspNetCore.Mvc;
using Project.MVC.ViewModels;
using Project.Service.Data.DTOs; // Add this for DTOs
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
        public async Task<IActionResult> Index(int page = 1, string sortOrder = "name", string searchString = "", int? makeId = null)
        {
            var models = await _vehicleService.GetModelsAsync(page, 10, sortOrder, searchString, makeId);
            var makes = await _vehicleService.GetAllMakesAsync();

            var vm = new VehicleModelIndexVM
            {
                Items = models.Items,
                Pagination = new PaginationVM
                {
                    Page = page,
                    PageSize = 10,
                    TotalItems = models.TotalCount
                },
                Makes = makes.Items, // Ensure IVehicleService returns a list of VehicleMakeVM
                CurrentMakeId = makeId,
                CurrentSort = sortOrder,
                SortOrder = sortOrder == "name_desc" ? "name_asc" : "name_desc"
            };

            return View(vm);
        }

        // GET: VehicleModel/Create
        public async Task<IActionResult> Create()
        {
            var makes = await _vehicleService.GetAllMakesAsync();
            var vm = new VehicleModelCreateVM
            {
                Makes = makes.Items // Ensure this returns a list of VehicleMakeVM
            };
            return View(vm);
        }

        // POST: VehicleModel/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VehicleModelCreateVM vm)
        {
            if (ModelState.IsValid)
            {
                var modelDto = new VehicleModelDTO
                {
                    Name = vm.Name,
                    MakeId = vm.MakeId // Ensure MakeId is part of the ViewModel
                };

                await _vehicleService.CreateModelAsync(modelDto);
                return RedirectToAction(nameof(Index));
            }

            // If model state is invalid, return to the view with the current data
            var makes = await _vehicleService.GetAllMakesAsync();
            vm.Makes = makes.Items; // Populate makes again
            return View(vm);
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
            var vm = new VehicleModelEditVM
            {
                Id = modelDto.Id,
                Name = modelDto.Name,
                MakeId = modelDto.MakeId,
                Makes = makes.Items // Ensure this returns a list of VehicleMakeVM
            };

            return View(vm);
        }

        // POST: VehicleModel/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(VehicleModelEditVM vm)
        {
            if (ModelState.IsValid)
            {
                var modelDto = new VehicleModelDTO
                {
                    Id = vm.Id,
                    Name = vm.Name,
                    MakeId = vm.MakeId
                };

                await _vehicleService.UpdateModelAsync(modelDto);
                return RedirectToAction(nameof(Index));
            }

            // If model state is invalid, return to the view with the current data
            var makes = await _vehicleService.GetAllMakesAsync();
            vm.Makes = makes.Items; // Populate makes again
            return View(vm);
        }

        // GET: VehicleModel/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var modelDto = await _vehicleService.GetModelByIdAsync(id);
            if (modelDto == null)
            {
                return NotFound();
            }

            var vm = new VehicleModelDeleteVM
            {
                Id = modelDto.Id,
                Name = modelDto.Name
            };

            return View(vm);
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
