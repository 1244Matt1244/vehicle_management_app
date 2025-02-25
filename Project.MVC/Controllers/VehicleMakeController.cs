using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Project.MVC.Helpers;
using Project.MVC.ViewModels;
using Project.Service.Interfaces;
using System.Collections.Generic;
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

        public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 10, 
            string sortBy = "Name", string sortOrder = "asc", string searchString = "")
        {
            var serviceResult = await _vehicleService.GetMakesAsync(pageIndex, pageSize, sortBy, sortOrder, searchString);
            
            var pagedResult = new PagedResult<VehicleMakeVM>
            {
                Items = _mapper.Map<List<VehicleMakeVM>>(serviceResult.Items),
                TotalCount = serviceResult.TotalCount,
                PageIndex = pageIndex,
                PageSize = pageSize
            };

            return View(pagedResult);
        }
    }
}
