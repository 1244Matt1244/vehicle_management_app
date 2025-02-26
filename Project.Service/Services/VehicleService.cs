// Project.Service/Services/VehicleService.cs
using Microsoft.EntityFrameworkCore;
using Project.Service.Data.Context;
using Project.Service.Data.DTOs;
using Project.Service.Data.Helpers;
using Project.Service.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Service.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly ApplicationDbContext _context;

        public VehicleService(ApplicationDbContext context)
        {
            _context = context;
        }

        // VehicleMake methods
        public async Task<VehicleMakeDTO> GetMakeByIdAsync(int id)
        {
            var make = await _context.VehicleMakes.FindAsync(id);
            return make == null ? null : new VehicleMakeDTO 
            { 
                Id = make.Id,
                Name = make.Name,
                Abbreviation = make.Abbreviation
            };
        }

        public async Task UpdateMakeAsync(VehicleMakeDTO makeDto)
        {
            var make = await _context.VehicleMakes.FindAsync(makeDto.Id) 
                ?? throw new KeyNotFoundException("Make not found");
            
            make.Name = makeDto.Name;
            make.Abbreviation = makeDto.Abbreviation;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMakeAsync(int id)
        {
            var make = await _context.VehicleMakes.FindAsync(id) 
                ?? throw new KeyNotFoundException("Make not found");
            
            _context.VehicleMakes.Remove(make);
            await _context.SaveChangesAsync();
        }

        // VehicleModel methods
        public async Task<VehicleModelDTO> GetModelByIdAsync(int id)
        {
            var model = await _context.VehicleModels
                .Include(m => m.VehicleMake)
                .FirstOrDefaultAsync(m => m.Id == id);
                
            return model == null ? null : new VehicleModelDTO 
            {
                Id = model.Id,
                Name = model.Name,
                Abbreviation = model.Abbreviation,
                MakeId = model.MakeId,
                MakeName = model.VehicleMake?.Name ?? string.Empty
            };
        }

        public async Task AddModelAsync(VehicleModelDTO modelDto)
        {
            var entity = new Models.VehicleModel 
            {
                Name = modelDto.Name,
                Abbreviation = modelDto.Abbreviation,
                MakeId = modelDto.MakeId
            };

            _context.VehicleModels.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateModelAsync(VehicleModelDTO modelDto)
        {
            var model = await _context.VehicleModels.FindAsync(modelDto.Id) 
                ?? throw new KeyNotFoundException("Model not found");
            
            model.Name = modelDto.Name;
            model.Abbreviation = modelDto.Abbreviation;
            model.MakeId = modelDto.MakeId;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteModelAsync(int id)
        {
            var model = await _context.VehicleModels.FindAsync(id) 
                ?? throw new KeyNotFoundException("Model not found");
            
            _context.VehicleModels.Remove(model);
            await _context.SaveChangesAsync();
        }
    }
}