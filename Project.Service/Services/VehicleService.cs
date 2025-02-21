using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Project.Service.Data.Context;
using Project.Service.Data.DTOs;
using Project.Service.Data.Helpers;
using Project.Service.Interfaces;
using Project.Service.Models;
using System;
using System.Linq; // Add this for LINQ methods
using System.Threading.Tasks;
using System.Collections.Generic; // Add this for KeyNotFoundException

namespace Project.Service.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public VehicleService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<PaginatedList<VehicleMakeDTO>> GetMakesAsync(int page, int pageSize, string sortBy, string sortOrder, string search)
        {
            if (page < 1) throw new ArgumentOutOfRangeException(nameof(page), "Page must be greater than 0.");
            if (pageSize < 1) throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size must be greater than 0.");
            if (string.IsNullOrEmpty(sortBy)) throw new ArgumentException("SortBy cannot be null or empty.", nameof(sortBy));
            if (sortOrder != "asc" && sortOrder != "desc") throw new ArgumentException("SortOrder must be either 'asc' or 'desc'.", nameof(sortOrder));

            var query = _context.VehicleMakes.AsNoTracking();

            if (!string.IsNullOrEmpty(search))
                query = query.Where(m => m.Name.Contains(search));

            query = sortOrder == "desc" 
                ? query.OrderByDescending(m => EF.Property<object>(m, sortBy))
                : query.OrderBy(m => EF.Property<object>(m, sortBy));

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ProjectTo<VehicleMakeDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return new PaginatedList<VehicleMakeDTO>(items, totalCount, page, pageSize);
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
            if (make == null)
                throw new KeyNotFoundException("Vehicle make not found.");

            _mapper.Map(makeDto, make);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMakeAsync(int id)
        {
            var make = await _context.VehicleMakes.FindAsync(id);
            if (make == null) throw new KeyNotFoundException("Vehicle make not found.");
            _context.VehicleMakes.Remove(make);
            await _context.SaveChangesAsync();
        }

        public async Task<PaginatedList<VehicleModelDTO>> GetModelsAsync(int page, int pageSize, string sortBy, string sortOrder, string search, int? makeId)
        {
            if (page < 1) throw new ArgumentOutOfRangeException(nameof(page), "Page must be greater than 0.");
            if (pageSize < 1) throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size must be greater than 0.");
            if (string.IsNullOrEmpty(sortBy)) throw new ArgumentException("SortBy cannot be null or empty.", nameof(sortBy));
            if (sortOrder != "asc" && sortOrder != "desc") throw new ArgumentException("SortOrder must be either 'asc' or 'desc'.", nameof(sortOrder));

            var query = _context.VehicleModels.AsNoTracking();

            if (makeId.HasValue)
                query = query.Where(m => m.MakeId == makeId.Value);

            if (!string.IsNullOrEmpty(search))
                query = query.Where(m => m.Name.Contains(search));

            query = sortOrder == "desc" 
                ? query.OrderByDescending(m => EF.Property<object>(m, sortBy))
                : query.OrderBy(m => EF.Property<object>(m, sortBy));

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ProjectTo<VehicleModelDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return new PaginatedList<VehicleModelDTO>(items, totalCount, page, pageSize);
        }

        public async Task<VehicleModelDTO> GetModelByIdAsync(int id)
        {
            var model = await _context.VehicleModels.FindAsync(id);
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
            if (model == null)
                throw new KeyNotFoundException("Vehicle model not found.");

            _mapper.Map(modelDto, model);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteModelAsync(int id)
        {
            var model = await _context.VehicleModels.FindAsync(id);
            if (model == null) throw new KeyNotFoundException("Vehicle model not found.");
            _context.VehicleModels.Remove(model);
            await _context.SaveChangesAsync();
        }
    }
}
