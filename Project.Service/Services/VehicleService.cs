using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Project.Service.Data.Context;
using Project.Service.Data.DTOs;
using Project.Service.Data.Helpers;
using Project.Service.Interfaces;
using Project.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        #region Makes

        public async Task<PaginatedList<VehicleMakeDTO>> GetMakesAsync(QueryParams parameters)
        {
            var query = _context.VehicleMakes.AsNoTracking();

            // Filtering
            if (!string.IsNullOrEmpty(parameters.Search))
            {
                query = query.Where(m => m.Name.Contains(parameters.Search) || m.Abrv.Contains(parameters.Search));
            }

            // Sorting
            var sortBy = !string.IsNullOrEmpty(parameters.SortBy) ? parameters.SortBy : "Name";
            var property = typeof(VehicleMake).GetProperty(sortBy);
            if (property != null)
            {
                query = parameters.SortOrder == "desc"
                    ? query.OrderByDescending(m => EF.Property<object>(m, property.Name))
                    : query.OrderBy(m => EF.Property<object>(m, property.Name));
            }
            else
            {
                // Default to sorting by Name if property not found
                query = query.OrderBy(m => m.Name);
            }

            // Pagination
            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((parameters.Page - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ProjectTo<VehicleMakeDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return new PaginatedList<VehicleMakeDTO>(items, totalCount, parameters.Page, parameters.PageSize);
        }

        public async Task<VehicleMakeDTO?> GetMakeByIdAsync(int id)
        {
            var make = await _context.VehicleMakes.FindAsync(id);
            if (make == null)
            {
                return null;
            }
            return _mapper.Map<VehicleMakeDTO>(make);
        }

        public async Task CreateMakeAsync(VehicleMakeDTO makeDto)
        {
            var entity = _mapper.Map<VehicleMake>(makeDto);
            _context.VehicleMakes.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateMakeAsync(VehicleMakeDTO makeDto)
        {
            var entity = await _context.VehicleMakes.FindAsync(makeDto.Id);
            if (entity != null)
            {
                _mapper.Map(makeDto, entity);
                await _context.SaveChangesAsync();
            }
            else
            {
                // Optionally handle the case where the entity is not found
                throw new KeyNotFoundException("Vehicle Make not found.");
            }
        }

        public async Task DeleteMakeAsync(int id)
        {
            var entity = await _context.VehicleMakes.FindAsync(id);
            if (entity != null)
            {
                _context.VehicleMakes.Remove(entity);
                await _context.SaveChangesAsync();
            }
            else
            {
                // Optionally handle the case where the entity is not found
                throw new KeyNotFoundException("Vehicle Make not found.");
            }
        }

        #endregion

        #region Models

        public async Task<PaginatedList<VehicleModelDTO>> GetModelsAsync(QueryParams parameters)
        {
            var query = _context.VehicleModels
                .Include(m => m.Make)
                .AsNoTracking();

            // Filtering
            if (parameters.MakeId.HasValue)
            {
                query = query.Where(m => m.MakeId == parameters.MakeId.Value);
            }

            if (!string.IsNullOrEmpty(parameters.Search))
            {
                query = query.Where(m => m.Name.Contains(parameters.Search) || m.Abrv.Contains(parameters.Search));
            }

            // Sorting
            var sortBy = !string.IsNullOrEmpty(parameters.SortBy) ? parameters.SortBy : "Name";
            var property = typeof(VehicleModel).GetProperty(sortBy);
            if (property != null)
            {
                query = parameters.SortOrder == "desc"
                    ? query.OrderByDescending(m => EF.Property<object>(m, property.Name))
                    : query.OrderBy(m => EF.Property<object>(m, property.Name));
            }
            else
            {
                // Default to sorting by Name if property not found
                query = query.OrderBy(m => m.Name);
            }

            // Pagination
            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((parameters.Page - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ProjectTo<VehicleModelDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return new PaginatedList<VehicleModelDTO>(items, totalCount, parameters.Page, parameters.PageSize);
        }

        public async Task<VehicleModelDTO?> GetModelByIdAsync(int id)
        {
            var model = await _context.VehicleModels
                .Include(m => m.Make)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (model == null)
            {
                return null;
            }

            return _mapper.Map<VehicleModelDTO>(model);
        }

        public async Task CreateModelAsync(VehicleModelDTO modelDto)
        {
            var entity = _mapper.Map<VehicleModel>(modelDto);
            _context.VehicleModels.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateModelAsync(VehicleModelDTO modelDto)
        {
            var entity = await _context.VehicleModels.FindAsync(modelDto.Id);
            if (entity != null)
            {
                _mapper.Map(modelDto, entity);
                await _context.SaveChangesAsync();
            }
            else
            {
                // Optionally handle the case where the entity is not found
                throw new KeyNotFoundException("Vehicle Model not found.");
            }
        }

        public async Task DeleteModelAsync(int id)
        {
            var entity = await _context.VehicleModels.FindAsync(id);
            if (entity != null)
            {
                _context.VehicleModels.Remove(entity);
                await _context.SaveChangesAsync();
            }
            else
            {
                // Optionally handle the case where the entity is not found
                throw new KeyNotFoundException("Vehicle Model not found.");
            }
        }

        #endregion
    }
}
