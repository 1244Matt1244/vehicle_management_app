using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Project.MVC.Models;
using Project.Service.DTOs;
using Project.Service.Interfaces;
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

        // GET: VehicleMake
        public async Task<IActionResult> Index(string search, string sortBy, string sortOrder, int page = 1)
        {
            var parameters = new QueryParams
            {
                Search = search,
                SortBy = sortBy,
                SortOrder = sortOrder,
                Page = page,
                PageSize = 10
            };

            var paginatedList = await _vehicleService.GetMakesAsync(parameters);
            var viewModelList = _mapper.Map<IEnumerable<VehicleMakeViewModel>>(paginatedList.Items);

            // Prepare pagination info for the view
            ViewBag.TotalPages = (int)Math.Ceiling((double)paginatedList.TotalCount / parameters.PageSize);
            ViewBag.CurrentPage = parameters.Page;

            return View(viewModelList);
        }

        // GET: VehicleMake/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var makeDto = await _vehicleService.GetMakeByIdAsync(id);
            if (makeDto == null)
            {
                return NotFound();
            }
            var viewModel = _mapper.Map<VehicleMakeViewModel>(makeDto);
            return View(viewModel);
        }

        // GET: VehicleMake/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: VehicleMake/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VehicleMakeViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var makeDto = _mapper.Map<VehicleMakeDTO>(viewModel);
                await _vehicleService.CreateMakeAsync(makeDto);
                return RedirectToAction(nameof(Index));
            }
            return View(viewModel);
        }

        // Similar actions for Edit and Delete
    }
}
