// Controllers/MakeController.cs
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Project.Service.Interfaces;
using Project.MVC.ViewModels;

namespace Project.MVC.Controllers
{
    public class MakeController : Controller
    {
        private readonly IVehicleService _service;
        private readonly IMapper _mapper;

        public MakeController(IVehicleService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, string sortOrder = "name_asc", string searchString = "")
        {
            var makes = await _service.GetMakesAsync(page, 10, sortOrder, searchString);
            return View(_mapper.Map<PaginatedList<VehicleMakeVM>>(makes));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VehicleMakeVM vm)
        {
            if (!ModelState.IsValid) return View(vm);
            
            await _service.CreateMakeAsync(_mapper.Map<VehicleMakeDTO>(vm));
            return RedirectToAction(nameof(Index));
        }
    }
}