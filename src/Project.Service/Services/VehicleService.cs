// Project.Service/Services/VehicleService.cs
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Project.Service.Data;
using Project.Service.Data.DTOs;
using Project.Service.Data.Context;
using Project.Service.Interfaces;
using System.Collections.Generic;
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

        // Vehicle Model Methods
        public async Task<IEnumerable<VehicleModelDTO>> GetModelsAsync()
        {
            return await _context.VehicleModels
                .Include(m => m.Make)
                .Select(m => _mapper.Map<VehicleModelDTO>(m))
                .ToListAsync();
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

        // Vehicle Make Methods
        public async Task<IEnumerable<VehicleMakeDTO>> GetMakesAsync()
        {
            return await _context.VehicleMakes
                .Select(m => _mapper.Map<VehicleMakeDTO>(m))
                .ToListAsync();
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
    }
}
