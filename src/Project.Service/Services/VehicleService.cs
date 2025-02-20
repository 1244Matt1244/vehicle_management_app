using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Project.Service.Data.Context;
using Project.Service.Shared.DTOs;
using Project.Service.Shared.Helpers;
using Project.Service.Interfaces;
using Project.Service.Shared.Models;

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

        public async Task<PaginatedList<VehicleMakeDTO>> GetMakesAsync(int pageIndex, int pageSize, 
            string sortBy, string sortOrder, string search)
        {
            var query = _context.VehicleMakes.AsNoTracking();

            if (!string.IsNullOrEmpty(search))
                query = query.Where(m => m.Name.Contains(search) || m.Abrv.Contains(search));

            query = sortOrder == "desc" 
                ? query.OrderByDescending(m => EF.Property<object>(m, sortBy))
                : query.OrderBy(m => EF.Property<object>(m, sortBy));

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ProjectTo<VehicleMakeDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return new PaginatedList<VehicleMakeDTO>(items, totalCount, pageIndex, pageSize);
        }

        public async Task<PaginatedList<VehicleModelDTO>> GetModelsAsync(int pageIndex, int pageSize, 
            string sortBy, string sortOrder, string search, int? makeId)
        {
            var query = _context.VehicleModels
                .Include(m => m.Make)
                .AsNoTracking();

            if (makeId.HasValue)
                query = query.Where(m => m.MakeId == makeId.Value);

            if (!string.IsNullOrEmpty(search))
                query = query.Where(m => m.Name.Contains(search) || m.Make.Name.Contains(search));

            query = sortOrder == "desc" 
                ? query.OrderByDescending(m => EF.Property<object>(m, sortBy))
                : query.OrderBy(m => EF.Property<object>(m, sortBy));

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ProjectTo<VehicleModelDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return new PaginatedList<VehicleModelDTO>(items, totalCount, pageIndex, pageSize);
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
            if (entity == null) return;

            _mapper.Map(makeDto, entity);
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
            if (entity == null) return;

            _mapper.Map(modelDto, entity);
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
}
