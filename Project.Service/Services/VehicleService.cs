// Project.Service/Services/VehicleService.cs
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Project.Service.Data.Context;
using Project.Service.DTOs;
using Project.Service.Interfaces;
using Project.Service.Models;
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

        // Vehicle Make Methods
        public async Task<PaginatedList<VehicleMakeDTO>> GetMakesAsync(QueryParams parameters)
        {
            var query = _context.VehicleMakes.AsQueryable();

            // Filtering
            if (!string.IsNullOrEmpty(parameters.Search))
            {
                query = query.Where(m => m.Name.Contains(parameters.Search) || m.Abrv.Contains(parameters.Search));
            }

            // Sorting
            query = parameters.SortOrder == "desc"
                ? query.OrderByDescending(e => EF.Property<object>(e, parameters.SortBy ?? "Name"))
                : query.OrderBy(e => EF.Property<object>(e, parameters.SortBy ?? "Name"));

            // Pagination
            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((parameters.Page - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ProjectTo<VehicleMakeDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return new PaginatedList<VehicleMakeDTO>(items, totalCount, parameters.Page, parameters.PageSize);
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
            var make = _mapper.Map<VehicleMake>(makeDto);
            _context.VehicleMakes.Update(make);
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

        // Vehicle Model Methods
        public async Task<PaginatedList<VehicleModelDTO>> GetModelsAsync(QueryParams parameters)
        {
            var query = _context.VehicleModels.Include(m => m.Make).AsQueryable();

            // Filtering
            if (!string.IsNullOrEmpty(parameters.Search))
            {
                query = query.Where(m => m.Name.Contains(parameters.Search) || m.Abrv.Contains(parameters.Search));
            }

            if (parameters.MakeId.HasValue)
            {
                query = query.Where(m => m.VehicleMakeId == parameters.MakeId.Value);
            }

            // Sorting
            query = parameters.SortOrder == "desc"
                ? query.OrderByDescending(e => EF.Property<object>(e, parameters.SortBy ?? "Name"))
                : query.OrderBy(e => EF.Property<object>(e, parameters.SortBy ?? "Name"));

            // Pagination
            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((parameters.Page - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ProjectTo<VehicleModelDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return new PaginatedList<VehicleModelDTO>(items, totalCount, parameters.Page, parameters.PageSize);
        }

        public async Task<VehicleModelDTO> GetModelByIdAsync(int id)
        {
            var model = await _context.VehicleModels.Include(m => m.Make).FirstOrDefaultAsync(m => m.Id == id);
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
            var model = _mapper.Map<VehicleModel>(modelDto);
            _context.VehicleModels.Update(model);
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
    }

    // Supporting Classes
    public class QueryParams
    {
        public string Search { get; set; }
        public int? MakeId { get; set; } // For filtering by Make
        public string SortBy { get; set; } = "Name";
        public string SortOrder { get; set; } = "asc";
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class PaginatedList<T>
    {
        public List<T> Items { get; }
        public int TotalCount { get; }
        public int PageIndex { get; }
        public int PageSize { get; }

        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            Items = items;
            TotalCount = count;
            PageIndex = pageIndex;
            PageSize = pageSize;
        }

        // Additional properties for pagination can be added here
    }
}
