using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Project.MVC.ViewModels;
using Project.Service.Data.Helpers;
using Project.Service.Interfaces;
using Project.Service.Services;
using Project.Service.Data.DTOs;

namespace Project.MVC.Controllers
{
    public class VehicleMakeController : Controller
    {
        private readonly IVehicleService _service;
        private readonly IMapper _mapper;

        public VehicleMakeController(IVehicleService service, IMapper mapper)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 10, 
            string sortBy = "Name", string sortOrder = "asc", string search = "")
        {
            var result = await _service.GetMakesAsync(page, pageSize, sortBy, sortOrder, search);
            var viewModel = new VehicleMakeIndexVM
            {
                Items = _mapper.Map<List<VehicleMakeVM>>(result.Items),
                Pagination = new PaginationVM
                {
                    Page = result.PageIndex,
                    PageSize = result.PageSize,
                    TotalItems = result.TotalCount
                },
                CurrentSort = sortBy,
                SortOrder = sortOrder,
                SearchString = search
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(VehicleMakeVM viewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var dto = _mapper.Map<VehicleMakeDTO>(viewModel);
            await _service.CreateMakeAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var makeDto = await _service.GetMakeByIdAsync(id);
            if (makeDto == null)
                return NotFound();

            var viewModel = _mapper.Map<VehicleMakeVM>(makeDto);
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(VehicleMakeVM viewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var dto = _mapper.Map<VehicleMakeDTO>(viewModel);
            await _service.UpdateMakeAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteMakeAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var makeDto = await _service.GetMakeByIdAsync(id);
            if (makeDto == null)
                return NotFound();

            var viewModel = _mapper.Map<VehicleMakeVM>(makeDto);
            return View(viewModel);
        }
    }
}
