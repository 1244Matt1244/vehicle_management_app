using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Project.MVC.Helpers;
using Project.MVC.ViewModels;
using Project.Service.Data.DTOs;
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
    }
}