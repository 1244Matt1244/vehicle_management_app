using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Project.Service.Interfaces;
using Project.MVC.ViewModels;
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

        // GET: VehicleModel
        public async Task<IActionResult> Index(int page = 1, string sortOrder = "", string searchString = "", int? makeId = null)
        {
            var models = await _vehicleService.GetModelsAsync(page, 10, sortOrder, searchString, makeId);
            var makes = await _vehicleService.GetAllMakesAsync();

            var vm = new VehicleModelIndexVM
            {
                Items = _mapper.Map<List<VehicleModelVM>>(models.Items),
                Pagination = new PaginationVM { Page = page, PageSize = 10, TotalItems = models.TotalCount },
                Makes = _mapper.Map<List<VehicleMakeVM>>(makes),
                CurrentMakeId = makeId,
                CurrentSort = sortOrder,
                SearchString = searchString
            };

            return View(vm);
        }
    }
}