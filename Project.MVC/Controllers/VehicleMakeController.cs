using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Project.MVC.Helpers;
using Project.MVC.ViewModels;
using Project.Service.Data.DTOs;
using Project.Service.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
public class VehicleMakeController : Controller
{
    private readonly IVehicleService _vehicleService;
    private readonly IMapper _mapper;

    public VehicleMakeController(IVehicleService vehicleService, IMapper mapper)
    {
        _vehicleService = vehicleService;
        _mapper = mapper;
    }

    // Index action
    public async Task<IActionResult> Index(
        int pageNumber = 1,
        int pageSize = 10,
        string sortBy = "Name",
        string sortOrder = "asc",
        string searchString = "")
    {
        var serviceResult = await _vehicleService.GetMakesAsync(
            pageNumber, 
            pageSize, 
            sortBy, 
            sortOrder, 
            searchString);

        var pagedResult = new PagedResult<VehicleMakeVM>
        {
            Items = _mapper.Map<List<VehicleMakeVM>>(serviceResult.Items),
            TotalCount = serviceResult.TotalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        // Sorting and filtering data for view
        ViewBag.SortOrder = sortOrder;
        ViewBag.CurrentSort = sortBy;
        ViewBag.CurrentFilter = searchString;

        return View(pagedResult);
    }

    // Create action - GET
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    // Create action - POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(VehicleMakeVM vehicleMake)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState); // 400 - Bad Request
        }

        var vehicleMakeDTO = _mapper.Map<VehicleMakeDTO>(vehicleMake);
        await _vehicleService.AddMakeAsync(vehicleMakeDTO);
        return Ok(); // 200 - OK, successful creation
    }

    // Edit action - GET
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var make = await _vehicleService.GetMakeByIdAsync(id);
        if (make == null)
        {
            return NotFound(); // 404 - Not Found
        }
        return View(_mapper.Map<VehicleMakeVM>(make)); // Map to ViewModel for the edit view
    }

    // Edit action - POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, VehicleMakeVM vehicleMake)
    {
        if (id != vehicleMake.Id)
        {
            return NotFound(); // 404 - Not Found if ID mismatch
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState); // 400 - Bad Request for invalid model state
        }

        var vehicleMakeDTO = _mapper.Map<VehicleMakeDTO>(vehicleMake);
        await _vehicleService.UpdateMakeAsync(vehicleMakeDTO);
        return Ok(); // 200 - OK for successful update
    }

    // Details action - GET
    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var make = await _vehicleService.GetMakeByIdAsync(id);
        if (make == null)
        {
            return NotFound(); // 404 - Not Found
        }
        return View(_mapper.Map<VehicleMakeVM>(make)); // Display details
    }

    // Delete action - GET
    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var make = await _vehicleService.GetMakeByIdAsync(id);
        if (make == null)
        {
            return NotFound(); // 404 - Not Found
        }
        return View(_mapper.Map<VehicleMakeVM>(make)); // Show confirmation for deletion
    }

    // Delete action - POST
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _vehicleService.DeleteMakeAsync(id);
        return Ok(); // 200 - OK for successful deletion
    }
}
