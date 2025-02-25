using AutoMapper;
using Project.Service.Data.DTOs;
using Project.Service.Data.Helpers;
using Project.Service.Interfaces;
using Project.Service.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Project.Service.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly IVehicleRepository _repository;
        private readonly IMapper _mapper;

        public VehicleService(IVehicleRepository repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        // Get paginated vehicle makes
        public async Task<PaginatedList<VehicleMakeDTO>> GetMakesAsync(int page, int pageSize, string sortBy, string sortOrder, string searchString)
        {
            var (makes, totalCount) = await _repository.GetMakesPaginatedAsync(page, pageSize, sortBy, sortOrder, searchString);
            return new PaginatedList<VehicleMakeDTO>(
                _mapper.Map<List<VehicleMakeDTO>>(makes),
                totalCount,
                page,
                pageSize
            );
        }

        // Get a specific make by ID
        public async Task<VehicleMakeDTO> GetMakeByIdAsync(int id)
        {
            var make = await _repository.GetMakeByIdAsync(id);
            if (make == null) throw new KeyNotFoundException("Vehicle make not found");
            return _mapper.Map<VehicleMakeDTO>(make);
        }

        // Create a new make
        public async Task<VehicleMakeDTO> CreateMakeAsync(VehicleMakeDTO makeDto)
        {
            var make = _mapper.Map<VehicleMake>(makeDto);
            await _repository.CreateMakeAsync(make);
            return _mapper.Map<VehicleMakeDTO>(make);
        }

        // Update an existing make
        public async Task<bool> UpdateMakeAsync(int id, VehicleMakeDTO makeDto)
        {
            var existingMake = await _repository.GetMakeByIdAsync(id);
            if (existingMake == null) return false;
            
            _mapper.Map(makeDto, existingMake);
            await _repository.UpdateMakeAsync(existingMake);
            return true;
        }

        // Delete a make by ID
        public async Task<bool> DeleteMakeAsync(int id)
        {
            var make = await _repository.GetMakeByIdAsync(id);
            if (make == null) return false;
            
            await _repository.DeleteMakeAsync(make);
            return true;
        }

        // Get paginated vehicle models
        public async Task<PaginatedList<VehicleModelDTO>> GetModelsAsync(int page, int pageSize, string sortBy, string sortOrder, string searchString, int? makeId)
        {
            var (models, totalCount) = await _repository.GetModelsPaginatedAsync(page, pageSize, sortBy, sortOrder, searchString, makeId);
            return new PaginatedList<VehicleModelDTO>(
                _mapper.Map<List<VehicleModelDTO>>(models),
                totalCount,
                page,
                pageSize
            );
        }

        // Get a specific model by ID
        public async Task<VehicleModelDTO> GetModelByIdAsync(int id)
        {
            var model = await _repository.GetModelByIdAsync(id);
            if (model == null) throw new KeyNotFoundException("Vehicle model not found");
            return _mapper.Map<VehicleModelDTO>(model);
        }

        // Create a new model
        public async Task<VehicleModelDTO> CreateModelAsync(VehicleModelDTO modelDto)
        {
            var model = _mapper.Map<VehicleModel>(modelDto);
            await _repository.CreateModelAsync(model);
            return _mapper.Map<VehicleModelDTO>(model);
        }

        // Update an existing model
        public async Task<bool> UpdateModelAsync(int id, VehicleModelDTO modelDto)
        {
            var existingModel = await _repository.GetModelByIdAsync(id);
            if (existingModel == null) return false;
            
            _mapper.Map(modelDto, existingModel);
            await _repository.UpdateModelAsync(existingModel);
            return true;
        }

        // Delete a model by ID
        public async Task<bool> DeleteModelAsync(int id)
        {
            var model = await _repository.GetModelByIdAsync(id);
            if (model == null) return false;
            
            await _repository.DeleteModelAsync(model);
            return true;
        }
    }
}
