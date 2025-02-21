using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Project.Service.Data.Context;
using Project.Service.Data.DTOs;
using Project.Service.Data.Helpers;
using Project.Service.Interfaces;
using Project.Service.Models;
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

        public async Task<PaginatedList<VehicleMakeDTO>> GetMakesAsync(int page, int pageSize, string sortBy, string sortOrder, string search)
        {
            var query = _context.VehicleMakes.AsNoTracking();

            // Filtering
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(m => m.Name.Contains(search) || m.Abrv.Contains(search));
            }

            // Sorting
            var property = typeof(VehicleMake).GetProperty(sortBy ?? "Name") ?? typeof(VehicleMake).GetProperty("Name");
            if (property != null)
            {
                query = sortOrder == "desc"
                    ? query.OrderByDescending(m => EF.Property<object>(m, property.Name))
                    : query.OrderBy(m => EF.Property<object>(m, property.Name));
            }

            // Pagination
            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ProjectTo<VehicleMakeDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return new PaginatedList<VehicleMakeDTO>(items, totalCount, page, pageSize);
        }

        public async Task<VehicleMakeDTO?> GetMakeByIdAsync(int id)
        {
            var make = await _context.VehicleMakes.FindAsync(id);
            return make == null ? null : _mapper.Map<VehicleMakeDTO>(make);
        }

        public async Task CreateMakeAsync(VehicleMakeDTO makeDto)
        {
            var make = _mapper.Map<VehicleMake>(makeDto);
            _context.VehicleMakes.Add(make);
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
                throw new KeyNotFoundException("Vehicle Make not found.");
            }
        }

        #endregion

        #region Models

        public async Task<PaginatedList<VehicleModelDTO>> GetModelsAsync(int page, int pageSize, string sortBy, string sortOrder, string search, int? makeId)
        {
            var query = _context.VehicleModels
                .Include(m => m.Make)
                .AsNoTracking();

            // Filtering
            if (makeId.HasValue)
            {
                query = query.Where(m => m.MakeId == makeId.Value);
            }

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(m => m.Name.Contains(search) || m.Abrv.Contains(search));
            }

            // Sorting
            var property = typeof(VehicleModel).GetProperty(sortBy ?? "Name") ?? typeof(VehicleModel).GetProperty("Name");
            if (property != null)
            {
                query = sortOrder == "desc"
                    ? query.OrderByDescending(m => EF.Property<object>(m, property.Name))
                    : query.OrderBy(m => EF.Property<object>(m, property.Name));
            }

            // Pagination
            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ProjectTo<VehicleModelDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return new PaginatedList<VehicleModelDTO>(items, totalCount, page, pageSize);
        }

        public async Task<VehicleModelDTO?> GetModelByIdAsync(int id)
        {
            var model = await _context.VehicleModels
                .Include(m => m.Make)
                .FirstOrDefaultAsync(m => m.Id == id);
            return model == null ? null : _mapper.Map<VehicleModelDTO>(model);
        }

        public async Task CreateModelAsync(VehicleModelDTO modelDto)
        {
            var model = _mapper.Map<VehicleModel>(modelDto);
            _context.VehicleModels.Add(model);
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
                throw new KeyNotFoundException("Vehicle Model not found.");
            }
        }

        #endregion
    }
}
