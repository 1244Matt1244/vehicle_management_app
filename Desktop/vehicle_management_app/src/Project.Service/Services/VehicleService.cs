using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Project.Service.Data;
using Project.Service.Data.DTOs;
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

        public async Task<IEnumerable<VehicleMakeDTO>> GetAllMakesAsync()
        {
            var makes = await _context.VehicleMakes.ToListAsync();
            return _mapper.Map<IEnumerable<VehicleMakeDTO>>(makes);
        }

        public async Task<VehicleMakeDTO> GetMakeByIdAsync(int id)
        {
            var make = await _context.VehicleMakes.FindAsync(id);
            if (make == null) throw new KeyNotFoundException($"Vehicle make with ID {id} not found");
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
            if (entity == null) throw new KeyNotFoundException($"Vehicle make with ID {makeDto.Id} not found");
            _mapper.Map(makeDto, entity);
            _context.VehicleMakes.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMakeAsync(int id)
        {
            var entity = await _context.VehicleMakes.FindAsync(id);
            if (entity == null) throw new KeyNotFoundException($"Vehicle make with ID {id} not found");
            _context.VehicleMakes.Remove(entity);
            await _context.SaveChangesAsync();
        }

        // Similar methods for VehicleModel...

        public async Task<IEnumerable<VehicleModelDTO>> GetAllModelsAsync()
        {
            var models = await _context.VehicleModels.Include(m => m.Make).ToListAsync();
            return _mapper.Map<IEnumerable<VehicleModelDTO>>(models);
        }

        public async Task<VehicleModelDTO> GetModelByIdAsync(int id)
        {
            var model = await _context.VehicleModels.Include(m => m.Make).FirstOrDefaultAsync(m => m.Id == id);
            if (model == null) throw new KeyNotFoundException($"Vehicle model with ID {id} not found");
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
            if (entity == null) throw new KeyNotFoundException($"Vehicle model with ID {modelDto.Id} not found");
            _mapper.Map(modelDto, entity);
            _context.VehicleModels.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteModelAsync(int id)
        {
            var entity = await _context.VehicleModels.FindAsync(id);
            if (entity == null) throw new KeyNotFoundException($"Vehicle model with ID {id} not found");
            _context.VehicleModels.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}