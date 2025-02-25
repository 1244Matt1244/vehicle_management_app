using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Project.MVC.Helpers;
using Project.MVC.ViewModels;
using Project.Service.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            int pageIndex = 1, 
            int pageSize = 10, 
            string sortBy = "Name", 
            string sortOrder = "asc", 
            string searchString = "", 
            int? makeId = null
        )
        {
            var modelResult = await _vehicleService.GetModelsAsync(pageIndex, pageSize, sortBy, sortOrder, searchString, makeId);
            var makes = await _vehicleService.GetAllMakesAsync();

            var viewModel = new VehicleModelIndexVM
            {
                PagedResults = new PagedResult<VehicleModelVM>
                {
                    Items = _mapper.Map<List<VehicleModelVM>>(modelResult.Items),
                    TotalCount = modelResult.TotalCount,
                    PageIndex = pageIndex,
                    PageSize = pageSize
                },
                Makes = _mapper.Map<List<VehicleMakeVM>>(makes)
            };

            return View(viewModel);
        }
    }
}