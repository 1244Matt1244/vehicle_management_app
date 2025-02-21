// Project.MVC/Controllers/VehicleModelController.cs
using Microsoft.AspNetCore.Mvc;
using Project.Service.Interfaces;
using Project.MVC.ViewModels;
using AutoMapper;

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

        public async Task<IActionResult> Index()
        {
            var models = await _vehicleService.GetModelsAsync();
            var viewModels = _mapper.Map<IEnumerable<VehicleModelVM>>(models);
            return View(viewModels);
        }

        [HttpPost]
        public async Task<IActionResult> Create(VehicleModelVM model)
        {
            if (ModelState.IsValid)
            {
                var dto = _mapper.Map<VehicleModelDTO>(model);
                await _vehicleService.CreateModelAsync(dto);
                return RedirectToAction("Index");
            }
            return View(model);
        }
    }
}
