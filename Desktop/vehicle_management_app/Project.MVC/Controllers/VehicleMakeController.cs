// Project.MVC/Controllers/VehicleMakeController.cs
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using Project.MVC.ViewModels;
using Project.Service.Models;
using Project.Service.Services;

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

        // Prikaz liste s sortiranje, filtriranjem i pagingom
        public async Task<ActionResult> Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParam = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

            if (searchString != null)
            {
                page = 1; // Ako je unesena nova pretraga, kreni od prve stranice
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            var makes = await _vehicleService.GetVehicleMakesAsync(sortOrder, searchString, pageNumber, pageSize);
            var totalCount = await _vehicleService.GetTotalMakesAsync(searchString);

            var viewModels = _mapper.Map<IEnumerable<VehicleMakeViewModel>>(makes);

            var pagedList = new StaticPagedList<VehicleMakeViewModel>(viewModels, pageNumber, pageSize, totalCount);

            return View(pagedList);
        }

        // GET: VehicleMake/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                // Neispravan zahtjev bez potrebnog ID-a
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var make = await _vehicleService.GetVehicleMakeByIdAsync(id.Value);

            if (make == null)
            {
                // Nije pronađena marka vozila s danim ID-em
                return HttpNotFound();
            }

            var viewModel = _mapper.Map<VehicleMakeViewModel>(make);
            return View(viewModel);
        }

        // GET: VehicleMake/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: VehicleMake/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(VehicleMakeViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var make = _mapper.Map<VehicleMake>(viewModel);
                    await _vehicleService.CreateVehicleMakeAsync(make);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    // Logirajte grešku ako koristite logiranje (npr. NLog, Serilog)
                    // logger.Error(ex);

                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, contact your system administrator.");
                }
            }
            return View(viewModel);
        }

        // GET: VehicleMake/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                // Neispravan zahtjev
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var make = await _vehicleService.GetVehicleMakeByIdAsync(id.Value);
            if (make == null)
            {
                return HttpNotFound();
            }

            var viewModel = _mapper.Map<VehicleMakeViewModel>(make);
            return View(viewModel);
        }

        // POST: VehicleMake/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(VehicleMakeViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var make = _mapper.Map<VehicleMake>(viewModel);
                    await _vehicleService.UpdateVehicleMakeAsync(make);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    // Logiranje greške
                    // logger.Error(ex);

                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, contact your system administrator.");
                }
            }
            return View(viewModel);
        }

        // GET: VehicleMake/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                // Neispravan zahtjev
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var make = await _vehicleService.GetVehicleMakeByIdAsync(id.Value);
            if (make == null)
            {
                return HttpNotFound();
            }

            var viewModel = _mapper.Map<VehicleMakeViewModel>(make);
            return View(viewModel);
        }

        // POST: VehicleMake/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _vehicleService.DeleteVehicleMakeAsync(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Logiranje greške
                // logger.Error(ex);

                return RedirectToAction("Delete", new { id = id, saveChangesError = true });
            }
        }
    }
}
