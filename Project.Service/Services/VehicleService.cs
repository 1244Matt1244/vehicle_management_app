using AutoMapper;
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
    public class VehicleService : IVehicleService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public VehicleService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // VehicleMake Implementation ------------------------------------------

        public async Task<PaginatedList<VehicleMakeDTO>> GetMakesAsync(int pageIndex, int pageSize, string sortBy, string sortOrder, string searchString)
        {
            var query = _context.VehicleMakes.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                query = query.Where(m => 
                    m.Name.Contains(searchString) || 
                    m.Abbreviation.Contains(searchString));
            }

            query = query.OrderByProperty(sortBy, sortOrder);
            return await PaginatedList<VehicleMakeDTO>.CreateAsync(
                query.Select(m => _mapper.Map<VehicleMakeDTO>(m)),
                pageIndex,
                pageSize
            );
        }

        public async Task<VehicleMakeDTO> GetMakeByIdAsync(int id)
        {
            var make = await _context.VehicleMakes.FindAsync(id);
            return _mapper.Map<VehicleMakeDTO>(make);
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
            if (make == null) throw new ArgumentException("VehicleMake not found");
            
            _mapper.Map(makeDto, make);
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

        public async Task<List<VehicleMakeDTO>> GetAllMakesAsync()
        {
            var makes = await _context.VehicleMakes.ToListAsync();
            return _mapper.Map<List<VehicleMakeDTO>>(makes);
        }

        // New method to create a vehicle make
        public async Task CreateMakeAsync(VehicleMakeDTO makeDto)
        {
            var make = _mapper.Map<VehicleMake>(makeDto);
            _context.VehicleMakes.Add(make);
            await _context.SaveChangesAsync();
        }

        // VehicleModel Implementation -----------------------------------------

        public async Task<PaginatedList<VehicleModelDTO>> GetModelsAsync(int pageIndex, int pageSize, string sortBy, string sortOrder, string searchString, int? makeId)
        {
            var query = _context.VehicleModels
                .Include(m => m.VehicleMake)
                .AsQueryable();

            if (makeId.HasValue)
                query = query.Where(m => m.MakeId == makeId.Value);

            if (!string.IsNullOrWhiteSpace(searchString))
                query = query.Where(m => m.Name.Contains(searchString) || m.Abbreviation.Contains(searchString));

            query = query.OrderByProperty(sortBy, sortOrder);
            return await PaginatedList<VehicleModelDTO>.CreateAsync(
                query.Select(m => _mapper.Map<VehicleModelDTO>(m)),
                pageIndex,
                pageSize
            );
        }

        public async Task<VehicleModelDTO> GetModelByIdAsync(int id)
        {
            var model = await _context.VehicleModels
                .Include(m => m.VehicleMake)
                .FirstOrDefaultAsync(m => m.Id == id);
            return _mapper.Map<VehicleModelDTO>(model);
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
            if (model != null)
            {
                _mapper.Map(modelDto, model);
                _context.VehicleModels.Update(model);
                await _context.SaveChangesAsync();
            }
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

        public async Task<List<VehicleModelDTO>> GetAllModelsAsync()
        {
            var models = await _context.VehicleModels.ToListAsync();
            return _mapper.Map<List<VehicleModelDTO>>(models);
        }

        // New method to create a vehicle model
        public async Task CreateModelAsync(VehicleModelDTO modelDto)
        {
            var model = _mapper.Map<VehicleModel>(modelDto);
            _context.VehicleModels.Add(model);
            await _context.SaveChangesAsync();
        }
    }
}
