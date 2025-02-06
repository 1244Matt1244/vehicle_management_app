// Project.MVC/Controllers/VehicleModelController.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using Project.MVC.ViewModels;
using Project.Service.Models;
using Project.Service.Services;

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

        // Prikaz modela, uz mogućnost filtriranja po MakeId
        public async Task<ActionResult> Index(string sortOrder, int makeId = 0, int page = 1)
        {
            int pageSize = 10;
            IEnumerable<VehicleModel> models = await _vehicleService.GetVehicleModelsAsync(sortOrder, makeId, page, pageSize);
            var viewModels = _mapper.Map<IEnumerable<VehicleModelViewModel>>(models);
            return View(viewModels);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(VehicleModelViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                VehicleModel model = _mapper.Map<VehicleModel>(viewModel);
                await _vehicleService.CreateVehicleModelAsync(model);
                return RedirectToAction("Index");
            }
            return View(viewModel);
        }

        public async Task<ActionResult> Edit(int id)
        {
            VehicleModel model = await _vehicleService.GetVehicleModelByIdAsync(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            var viewModel = _mapper.Map<VehicleModelViewModel>(model);
            return View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(VehicleModelViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                VehicleModel model = _mapper.Map<VehicleModel>(viewModel);
                await _vehicleService.UpdateVehicleModelAsync(model);
                return RedirectToAction("Index");
            }
            return View(viewModel);
        }

        public async Task<ActionResult> Delete(int id)
        {
            VehicleModel model = await _vehicleService.GetVehicleModelByIdAsync(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            var viewModel = _mapper.Map<VehicleModelViewModel>(model);
            return View(viewModel);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await _vehicleService.DeleteVehicleModelAsync(id);
            return RedirectToAction("Index");
        }
    }
}
