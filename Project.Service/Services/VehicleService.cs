// Project.Service/Services/VehicleService.cs

using AutoMapper;
using AutoMapper.QueryableExtensions; // Add this namespace
using Microsoft.EntityFrameworkCore;
using Project.Service.Data.Context;
using Project.Service.Data.DTOs;
using Project.Service.Data.Helpers;
using Project.Service.Interfaces;
using Project.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Service.Services
{
    public sealed class VehicleService : IVehicleService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public VehicleService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // VehicleMake Implementation
        public async Task<PaginatedList<VehicleMakeDTO>> GetMakesAsync(int pageIndex, int pageSize, 
            string sortBy, string sortOrder, string searchString)
        {
            var query = _context.VehicleMakes.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                query = query.Where(m => 
                    m.Name.Contains(searchString) || 
                    m.Abbreviation.Contains(searchString));
            }

            query = query.OrderByProperty(sortBy, sortOrder);
            
            return await PaginatedList<VehicleMakeDTO>.CreateAsync(
                query.ProjectTo<VehicleMakeDTO>(_mapper.ConfigurationProvider), // Fixed ProjectTo
                pageIndex,
                pageSize
            );
        }

        public async Task<VehicleMakeDTO?> GetMakeByIdAsync(int id)
        {
            return await _context.VehicleMakes
                .AsNoTracking()
                .ProjectTo<VehicleMakeDTO>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task AddMakeAsync(VehicleMakeDTO makeDto)
        {
            var make = _mapper.Map<VehicleMake>(makeDto);
            await _context.VehicleMakes.AddAsync(make);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateMakeAsync(VehicleMakeDTO makeDto)
        {
            var make = await _context.VehicleMakes.FindAsync(makeDto.Id);
            if (make == null) throw new KeyNotFoundException($"No VehicleMake found with ID {makeDto.Id}");

            _mapper.Map(makeDto, make);
            _context.VehicleMakes.Update(make);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMakeAsync(int id)
        {
            var make = await _context.VehicleMakes.FindAsync(id);
            if (make == null) throw new KeyNotFoundException($"No VehicleMake found with ID {id}");

            _context.VehicleMakes.Remove(make);
            await _context.SaveChangesAsync();
        }

        public async Task<List<VehicleMakeDTO>> GetAllMakesAsync()
        {
            return await _context.VehicleMakes
                .AsNoTracking()
                .ProjectTo<VehicleMakeDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        // VehicleModel Implementation
        public async Task<PaginatedList<VehicleModelDTO>> GetModelsAsync(int pageIndex, int pageSize, 
            string sortBy, string sortOrder, string searchString, int? makeId)
        {
            var query = _context.VehicleModels
                .AsNoTracking()
                .Include(m => m.VehicleMake) // Correct include
                .AsQueryable();

            if (makeId.HasValue)
                query = query.Where(m => m.MakeId == makeId.Value);

            if (!string.IsNullOrWhiteSpace(searchString))
                query = query.Where(m => m.Name.Contains(searchString));

            query = query.OrderByProperty(sortBy, sortOrder);
            
            return await PaginatedList<VehicleModelDTO>.CreateAsync(
                query.ProjectTo<VehicleModelDTO>(_mapper.ConfigurationProvider), // Fixed ProjectTo
                pageIndex,
                pageSize
            );
        }

        public async Task<VehicleModelDTO?> GetModelByIdAsync(int id)
        {
            return await _context.VehicleModels
                .AsNoTracking()
                .Include(m => m.VehicleMake)
                .ProjectTo<VehicleModelDTO>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task AddModelAsync(VehicleModelDTO modelDto)
        {
            var model = _mapper.Map<VehicleModel>(modelDto);
            await _context.VehicleModels.AddAsync(model);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateModelAsync(VehicleModelDTO modelDto)
        {
            var model = await _context.VehicleModels.FindAsync(modelDto.Id);
            if (model == null) throw new KeyNotFoundException($"No VehicleModel found with ID {modelDto.Id}");

            _mapper.Map(modelDto, model);
            _context.VehicleModels.Update(model);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteModelAsync(int id)
        {
            var model = await _context.VehicleModels.FindAsync(id);
            if (model == null) throw new KeyNotFoundException($"No VehicleModel found with ID {id}");

            _context.VehicleModels.Remove(model);
            await _context.SaveChangesAsync();
        }

        public async Task<List<VehicleModelDTO>> GetAllModelsAsync()
        {
            return await _context.VehicleModels
                .AsNoTracking()
                .Include(m => m.VehicleMake)
                .ProjectTo<VehicleModelDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }
    }
}
