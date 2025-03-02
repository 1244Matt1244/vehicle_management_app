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
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState); // 400 - Bad Request for invalid model state
        }

        await _vehicleService.AddModelAsync(_mapper.Map<VehicleModelDTO>(model));
        return Ok(); // 200 - OK for successful creation
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var model = await _vehicleService.GetModelByIdAsync(id);
        if (model == null)
        {
            return NotFound(); // 404 - Not Found
        }

        var vm = _mapper.Map<VehicleModelEditVM>(model);
        vm.AvailableMakes = _mapper.Map<List<VehicleMakeVM>>(await _vehicleService.GetAllMakesAsync());

        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(VehicleModelEditVM model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState); // 400 - Bad Request for invalid model state
        }

        await _vehicleService.UpdateModelAsync(_mapper.Map<VehicleModelDTO>(model));
        return Ok(); // 200 - OK for successful update
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var model = await _vehicleService.GetModelByIdAsync(id);
        if (model == null)
        {
            return NotFound(); // 404 - Not Found
        }
        return View(_mapper.Map<VehicleModelVM>(model)); // Display model details
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var model = await _vehicleService.GetModelByIdAsync(id);
        if (model == null)
        {
            return NotFound(); // 404 - Not Found
        }
        return View(_mapper.Map<VehicleModelVM>(model)); // Show confirmation for deletion
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _vehicleService.DeleteModelAsync(id);
        return Ok(); // 200 - OK for successful deletion
    }
}
