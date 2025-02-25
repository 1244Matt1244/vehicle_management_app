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

        #region VehicleMake
        public async Task<PagedResult<VehicleMakeDTO>> GetMakesAsync(int page, int pageSize, string sortBy, string sortOrder, string searchString)
        {
            var (makes, totalCount) = await _repository.GetMakesPaginatedAsync(page, pageSize, sortBy, sortOrder, searchString);
            return new PagedResult<VehicleMakeDTO>
            {
                Items = _mapper.Map<List<VehicleMakeDTO>>(makes),
                TotalCount = totalCount,
                PageNumber = page,
                PageSize = pageSize
            };
        }
        
        public async Task<VehicleMakeDTO> GetMakeByIdAsync(int id)
        {
            var make = await _repository.GetMakeByIdAsync(id);
            return _mapper.Map<VehicleMakeDTO>(make);
        }
        
        public async Task<VehicleMakeDTO> CreateMakeAsync(VehicleMakeDTO makeDto)
        {
            var make = _mapper.Map<VehicleMake>(makeDto);
            await _repository.AddMakeAsync(make);
            return _mapper.Map<VehicleMakeDTO>(make);
        }

        public async Task UpdateMakeAsync(int id, VehicleMakeDTO makeDto)
        {
            var make = await _repository.GetMakeByIdAsync(id) ?? throw new KeyNotFoundException();
            _mapper.Map(makeDto, make);
            await _repository.UpdateMakeAsync(make);
        }

        public async Task DeleteMakeAsync(int id)
        {
            var make = await _repository.GetMakeByIdAsync(id) ?? throw new KeyNotFoundException();
            await _repository.DeleteMakeAsync(make);
        }
        #endregion

        #region VehicleModel
        public async Task<PagedResult<VehicleModelDTO>> GetModelsAsync(int page, int pageSize, string sortBy, string sortOrder, string searchString, int? makeId)
        {
            var (models, totalCount) = await _repository.GetModelsPaginatedAsync(page, pageSize, sortBy, sortOrder, searchString, makeId);
            return new PagedResult<VehicleModelDTO>
            {
                Items = _mapper.Map<List<VehicleModelDTO>>(models),
                TotalCount = totalCount,
                PageNumber = page,
                PageSize = pageSize
            };
        }
        
        public async Task<VehicleModelDTO> GetModelByIdAsync(int id)
        {
            var model = await _repository.GetModelByIdAsync(id);
            return _mapper.Map<VehicleModelDTO>(model);
        }

        public async Task<VehicleModelDTO> CreateModelAsync(VehicleModelDTO modelDto)
        {
            var model = _mapper.Map<VehicleModel>(modelDto);
            await _repository.AddModelAsync(model);
            return _mapper.Map<VehicleModelDTO>(model);
        }

        public async Task UpdateModelAsync(int id, VehicleModelDTO modelDto)
        {
            var model = await _repository.GetModelByIdAsync(id) ?? throw new KeyNotFoundException();
            _mapper.Map(modelDto, model);
            await _repository.UpdateModelAsync(model);
        }

        public async Task DeleteModelAsync(int id)
        {
            var model = await _repository.GetModelByIdAsync(id) ?? throw new KeyNotFoundException();
            await _repository.DeleteModelAsync(model);
        }
        #endregion

        #region Common
        public async Task<List<VehicleMakeDTO>> GetAllMakesAsync()
        {
            var makes = await _repository.GetAllMakesAsync();
            return _mapper.Map<List<VehicleMakeDTO>>(makes);
        }

        public async Task<List<VehicleModelDTO>> GetAllModelsAsync()
        {
            var models = await _repository.GetAllModelsAsync();
            return _mapper.Map<List<VehicleModelDTO>>(models);
        }
        #endregion
    }

    public class PagedResult<T>
    {
        public List<T> Items { get; set; }
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    }
}
