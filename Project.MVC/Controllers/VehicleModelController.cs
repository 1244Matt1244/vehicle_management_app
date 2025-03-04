using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Project.MVC.Helpers;
using Project.MVC.ViewModels;
using Project.Service.Data.DTOs;
using Project.Service.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        int pageNumber = 1,
        int pageSize = 10,
        string sortBy = "Name",
        string sortOrder = "asc",
        string searchString = "",
        int? makeId = null)
    {
        // Fetch models with paging, sorting, and filtering
        var serviceResult = await _vehicleService.GetModelsAsync(
            pageNumber,
            pageSize,
            sortBy,
            sortOrder,
            searchString,
            makeId);

        var makes = await _vehicleService.GetAllMakesAsync();

        // Create ViewModel with mapped results
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

    // GET: VehicleModel/Create
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

    // POST: VehicleModel/Create
    [HttpPost]
    public async Task<IActionResult> Create(VehicleModelCreateVM model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState); // Return 400 Bad Request for invalid model state
        }

        await _vehicleService.AddModelAsync(_mapper.Map<VehicleModelDTO>(model));
        return Ok(); // Return 200 OK for successful creation
    }

    // GET: VehicleModel/Edit/5
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var model = await _vehicleService.GetModelByIdAsync(id);
        if (model == null)
        {
            return NotFound(); // Return 404 Not Found
        }

        var vm = _mapper.Map<VehicleModelEditVM>(model);
        vm.AvailableMakes = _mapper.Map<List<VehicleMakeVM>>(await _vehicleService.GetAllMakesAsync());

        return View(vm);
    }

    // POST: VehicleModel/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(VehicleModelEditVM model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState); // Return 400 Bad Request for invalid model state
        }

        await _vehicleService.UpdateModelAsync(_mapper.Map<VehicleModelDTO>(model));
        return Ok(); // Return 200 OK for successful update
    }

    // GET: VehicleModel/Details/5
    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var model = await _vehicleService.GetModelByIdAsync(id);
        if (model == null)
        {
            return NotFound(); // Return 404 Not Found
        }
        return View(_mapper.Map<VehicleModelVM>(model)); // Display model details
    }

    // GET: VehicleModel/Delete/5
    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var model = await _vehicleService.GetModelByIdAsync(id);
        if (model == null)
        {
            return NotFound(); // Return 404 Not Found
        }
        return View(_mapper.Map<VehicleModelVM>(model)); // Show confirmation for deletion
    }

    // POST: VehicleModel/Delete/5
    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _vehicleService.DeleteModelAsync(id);
        return Ok(); // Return 200 OK for successful deletion
    }
}
