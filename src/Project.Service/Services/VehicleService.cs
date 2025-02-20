// Project.Service/Services/VehicleService.cs
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Project.Service.Data.Context;
using Project.Service.Interfaces;
using Project.Service.Shared.DTOs;
using Project.Service.Shared.Helpers;
using Project.Service.Models;
using System.Linq.Dynamic.Core;

namespace Project.Service.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public VehicleService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        #region Vehicle Makes
        public async Task<PaginatedList<VehicleMakeDTO>> GetMakesAsync(
            int pageIndex = 1,
            int pageSize = 10,
            string sortBy = "Name",
            string sortOrder = "asc",
            string searchString = "")
        {
            var query = _context.VehicleMakes
                .AsNoTracking()
                .AsQueryable();

            // Filtering
            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(m => 
                    m.Name.Contains(searchString) || 
                    m.Abrv.Contains(searchString));
            }

            // Sorting
            query = query.OrderBy($"{sortBy} {sortOrder}");

            // Projection and Pagination
            return await PaginatedList<VehicleMakeDTO>.CreateAsync(
                query.ProjectTo<VehicleMakeDTO>(_mapper.ConfigurationProvider),
                pageIndex,
                pageSize
            );
        }

        public async Task<VehicleMakeDTO> GetMakeByIdAsync(int id)
        {
            var make = await _context.VehicleMakes.FindAsync(id);
            return _mapper.Map<VehicleMakeDTO>(make);
        }

        public async Task CreateMakeAsync(VehicleMakeDTO makeDto)
        {
            var make = _mapper.Map<VehicleMake>(makeDto);
            _context.VehicleMakes.Add(make);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateMakeAsync(VehicleMakeDTO makeDto)
        {
            var make = await _context.VehicleMakes.FindAsync(makeDto.Id);
            _mapper.Map(makeDto, make);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMakeAsync(int id)
        {
            var make = await _context.VehicleMakes.FindAsync(id);
            if (make != null)
            {
                _context.VehicleMakes.Remove(make);
                await _context.SaveChangesAsync();
            }
        }
        #endregion

        #region Vehicle Models
        public async Task<PaginatedList<VehicleModelDTO>> GetModelsAsync(
            int pageIndex = 1,
            int pageSize = 10,
            string sortBy = "Name",
            string sortOrder = "asc",
            string searchString = "",
            int? makeId = null)
        {
            var query = _context.VehicleModels
                .AsNoTracking()
                .Include(m => m.Make)
                .AsQueryable();

            // Filtering
            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(m => 
                    m.Name.Contains(searchString) || 
                    m.Abrv.Contains(searchString) ||
                    m.Make.Name.Contains(searchString));
            }

            if (makeId.HasValue)
            {
                query = query.Where(m => m.MakeId == makeId.Value);
            }

            // Sorting
            query = query.OrderBy($"{sortBy} {sortOrder}");

            // Projection and Pagination
            return await PaginatedList<VehicleModelDTO>.CreateAsync(
                query.ProjectTo<VehicleModelDTO>(_mapper.ConfigurationProvider),
                pageIndex,
                pageSize
            );
        }

        public async Task<VehicleModelDTO> GetModelByIdAsync(int id)
        {
            var model = await _context.VehicleModels
                .Include(m => m.Make)
                .FirstOrDefaultAsync(m => m.Id == id);
            return _mapper.Map<VehicleModelDTO>(model);
        }

        public async Task CreateModelAsync(VehicleModelDTO modelDto)
        {
            var model = _mapper.Map<VehicleModel>(modelDto);
            _context.VehicleModels.Add(model);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateModelAsync(VehicleModelDTO modelDto)
        {
            var model = await _context.VehicleModels.FindAsync(modelDto.Id);
            _mapper.Map(modelDto, model);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteModelAsync(int id)
        {
            var model = await _context.VehicleModels.FindAsync(id);
            if (model != null)
            {
                _context.VehicleModels.Remove(model);
                await _context.SaveChangesAsync();
            }
        }
        #endregion
    }
}