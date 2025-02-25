using Microsoft.AspNetCore.Mvc;
using Project.MVC.Helpers;
using Project.MVC.ViewModels;
using Project.Service.Interfaces;

public class VehicleMakeController : Controller
{
    private readonly IVehicleService _vehicleService;

    public VehicleMakeController(IVehicleService vehicleService)
    {
        _vehicleService = vehicleService;
    }

    public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
    {
        var serviceResult = await _vehicleService.GetMakesAsync(page, pageSize, "Name", "asc", "");
        
        var pagedResult = new PagedResult<VehicleMakeVM>
        {
            Items = serviceResult.Items,
            TotalCount = serviceResult.TotalCount,
            PageNumber = page,
            PageSize = pageSize
        };

        return View(pagedResult);
    }
}