using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Project.Service.Data.Context;
using Project.Service.Data.DTOs;
using Project.Service.Interfaces;

namespace Project.Service.Data.Repositories
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public VehicleRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // Vehicle Model Methods
        public async Task<IEnumerable<VehicleModelDTO>> GetModelsAsync(int page, int pageSize, string sortOrder, int makeId = 0)
        {
            var query = _context.VehicleModels.AsNoTracking();

            if (makeId > 0)
                query = query.Where(m => m.MakeId == makeId);

            query = sortOrder == "desc" 
                ? query.OrderByDescending(m => m.Name) 
                : query.OrderBy(m => m.Name);

            return await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ProjectTo<VehicleModelDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<VehicleModelDTO> GetModelByIdAsync(int id)
        {
            return await _context.VehicleModels
                .AsNoTracking()
                .Where(m => m.Id == id)
                .ProjectTo<VehicleModelDTO>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }

        public async Task AddModelAsync(VehicleModelDTO model)
        {
            var entity = _mapper.Map<VehicleModel>(model);
            _context.VehicleModels.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateModelAsync(VehicleModelDTO model)
        {
            var entity = await _context.VehicleModels.FindAsync(model.Id);
            if (entity == null) throw new KeyNotFoundException($"Vehicle model with ID {model.Id} not found.");

            _mapper.Map(model, entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteModelAsync(int id)
        {
            var entity = await _context.VehicleModels.FindAsync(id);
            if (entity == null) throw new KeyNotFoundException($"Vehicle model with ID {id} not found.");

            _context.VehicleModels.Remove(entity);
            await _context.SaveChangesAsync();
        }

        // Vehicle Make Methods
        public async Task<IEnumerable<VehicleMakeDTO>> GetMakesAsync(int page, int pageSize, string sortOrder)
        {
            var query = _context.VehicleMakes.AsNoTracking();

            query = sortOrder == "desc"
                ? query.OrderByDescending(m => m.Name)
                : query.OrderBy(m => m.Name);

            return await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ProjectTo<VehicleMakeDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<VehicleMakeDTO> GetMakeByIdAsync(int id)
        {
            return await _context.VehicleMakes
                .AsNoTracking()
                .Where(m => m.Id == id)
                .ProjectTo<VehicleMakeDTO>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
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
            if (entity == null) throw new KeyNotFoundException($"Vehicle make with ID {makeDto.Id} not found.");

            _mapper.Map(makeDto, entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMakeAsync(int id)
        {
            var entity = await _context.VehicleMakes.FindAsync(id);
            if (entity == null) throw new KeyNotFoundException($"Vehicle make with ID {id} not found.");

            _context.VehicleMakes.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
