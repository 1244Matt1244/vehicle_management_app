using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Project.Service.Data.Context;
using Project.Service.Data.DTOs;
using Project.Service.Data.Helpers;
using Project.Service.Interfaces;
using Project.Service.Models;

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
                query = query.Where(m => m.Name.Contains(parameters.Search) || m.Abrv.Contains(parameters.Search));

            // Sorting
            query = ApplySorting(query, parameters.SortBy, parameters.SortOrder);

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
            return make == null ? null : _mapper.Map<VehicleMakeDTO>(make);
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
            _mapper.Map(makeDto, entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMakeAsync(int id)
        {
            var entity = await _context.VehicleMakes.FindAsync(id);
            _context.VehicleMakes.Remove(entity);
            await _context.SaveChangesAsync();
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
                query = query.Where(m => m.MakeId == parameters.MakeId);
            
            if (!string.IsNullOrEmpty(parameters.Search))
                query = query.Where(m => m.Name.Contains(parameters.Search) || m.Abrv.Contains(parameters.Search));

            // Sorting
            query = ApplySorting(query, parameters.SortBy, parameters.SortOrder);

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
            var model = await _context.VehicleModels
                .Include(m => m.Make)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            return model == null ? null : _mapper.Map<VehicleModelDTO>(model);
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
            _mapper.Map(modelDto, entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteModelAsync(int id)
        {
            var entity = await _context.VehicleModels.FindAsync(id);
            _context.VehicleModels.Remove(entity);
            await _context.SaveChangesAsync();
        }
        #endregion

        private IQueryable<T> ApplySorting<T>(IQueryable<T> query, string sortBy, string sortOrder)
        {
            var property = typeof(T).GetProperty(sortBy) ?? typeof(T).GetProperty("Id");
            return sortOrder == "desc" 
                ? query.OrderByDescending(x => EF.Property<object>(x, property.Name))
                : query.OrderBy(x => EF.Property<object>(x, property.Name));
        }
    }
}