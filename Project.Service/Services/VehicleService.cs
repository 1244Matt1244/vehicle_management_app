\// Project.Service/Services/VehicleService.cs
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Project.Service.Data.Context;
using Project.Service.DTOs;
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

        // Vehicle Make Methods
        public async Task<IEnumerable<VehicleMakeDTO>> GetAllMakesAsync()
        {
            var makes = await _context.VehicleMakes.ToListAsync();
            return _mapper.Map<IEnumerable<VehicleMakeDTO>>(makes);
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
        public async Task<IEnumerable<VehicleModelDTO>> GetAllModelsAsync()
        {
            var models = await _context.VehicleModels.Include(m => m.Make).ToListAsync();
            return _mapper.Map<IEnumerable<VehicleModelDTO>>(models);
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
}
