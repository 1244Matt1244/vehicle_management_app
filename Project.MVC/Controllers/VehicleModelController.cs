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
            ViewBag.Makes =