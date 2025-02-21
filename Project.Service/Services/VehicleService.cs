using AutoMapper;
using Project.Data.Models;
using Project.Service.Data.DTOs;
using Project.Service.Interfaces;
using Project.Service.Helpers;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Project.Service.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly IVehicleRepository _repository;
        private readonly IMapper _mapper;

        public VehicleService(IVehicleRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // VehicleMake Methods

        // Fetches a paginated list of vehicle makes
        public async Task<PaginatedList<VehicleMakeDTO>> GetMakesPaginatedAsync(int page, int pageSize, string sortOrder, string searchString)
        {
            var paginatedMakes = await _repository.GetMakesPaginatedAsync(page, pageSize, sortOrder, searchString);
            return _mapper.Map<PaginatedList<VehicleMakeDTO>>(paginatedMakes);
        }

        // Fetches a vehicle make by its ID
        public async Task<VehicleMakeDTO> GetMakeByIdAsync(int id)
        {
            var make = await _repository.GetMakeByIdAsync(id);
            return _mapper.Map<VehicleMakeDTO>(make);
        }

        // Creates a new vehicle make
        public async Task CreateMakeAsync(VehicleMakeDTO makeDto)
        {
            var make = _mapper.Map<VehicleMake>(makeDto);
            await _repository.AddMakeAsync(make);
        }

        // Updates an existing vehicle make
        public async Task UpdateMakeAsync(VehicleMakeDTO makeDto)
        {
            var make = _mapper.Map<VehicleMake>(makeDto);
            await _repository.UpdateMakeAsync(make);
        }

        // Deletes a vehicle make by its ID
        public async Task DeleteMakeAsync(int id)
        {
            await _repository.DeleteMakeAsync(id);
        }

        // VehicleModel Methods

        // Fetches a paginated list of vehicle models
        public async Task<PaginatedList<VehicleModelDTO>> GetModelsPaginatedAsync(int page, int pageSize, string sortOrder, string searchString)
        {
            var query = _repository.GetVehicleModels();  // Assuming repository has GetVehicleModels method for retrieving models.

            // Apply search filter
            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(m => m.Name.Contains(searchString));
            }

            // Apply sorting
            query = sortOrder switch
            {
                "name_desc" => query.OrderByDescending(m => m.Name),
                _ => query.OrderBy(m => m.Name),
            };

            // Pagination
            var totalItems = await query.CountAsync();
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedList<VehicleModelDTO>(
                _mapper.Map<List<VehicleModelDTO>>(items),
                page,
                (int)Math.Ceiling(totalItems / (double)pageSize),
                totalItems
            );
        }

        // Fetches a vehicle model by its ID
        public async Task<VehicleModelDTO> GetModelByIdAsync(int id)
        {
            var model = await _repository.GetModelByIdAsync(id);
            return _mapper.Map<VehicleModelDTO>(model);
        }

        // Creates a new vehicle model
        public async Task CreateModelAsync(VehicleModelDTO modelDto)
        {
            var model = _mapper.Map<VehicleModel>(modelDto);
            await _repository.AddModelAsync(model);
        }

        // Updates an existing vehicle model
        public async Task UpdateModelAsync(VehicleModelDTO modelDto)
        {
            var model = _mapper.Map<VehicleModel>(modelDto);
            await _repository.UpdateModelAsync(model);
        }

        // Deletes a vehicle model by its ID
        public async Task DeleteModelAsync(int id)
        {
            await _repository.DeleteModelAsync(id);
        }
    }
}
