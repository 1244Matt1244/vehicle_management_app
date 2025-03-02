using AutoMapper;
using AutoMapper.QueryableExtensions;
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

        // VehicleMake Methods
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
                query.ProjectTo<VehicleMakeDTO>(_mapper.ConfigurationProvider),
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
            try
            {
                var make = _mapper.Map<VehicleMake>(makeDto);
                await _context.VehicleMakes.AddAsync(make);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error adding vehicle make", ex);
            }
        }

        public async Task UpdateMakeAsync(VehicleMakeDTO makeDto)
        {
            try
            {
                var make = await _context.VehicleMakes.FindAsync(makeDto.Id);
                if (make == null) throw new KeyNotFoundException($"No VehicleMake found with ID {makeDto.Id}");

                _mapper.Map(makeDto, make);
                _context.VehicleMakes.Update(make);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error updating vehicle make", ex);
            }
        }

        public async Task DeleteMakeAsync(int id)
        {
            try
            {
                var make = await _context.VehicleMakes.FindAsync(id);
                if (make == null) throw new KeyNotFoundException($"No VehicleMake found with ID {id}");

                _context.VehicleMakes.Remove(make);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error deleting vehicle make", ex);
            }
        }

        public async Task<List<VehicleMakeDTO>> GetAllMakesAsync()
        {
            return await _context.VehicleMakes
                .AsNoTracking()
                .ProjectTo<VehicleMakeDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        // VehicleModel Methods
        public async Task<PaginatedList<VehicleModelDTO>> GetModelsAsync(int pageIndex, int pageSize, 
            string sortBy, string sortOrder, string searchString, int? makeId)
        {
            var query = _context.VehicleModels
                .AsNoTracking()
                .Include(m => m.VehicleMake)
                .AsQueryable();

            if (makeId.HasValue)
                query = query.Where(m => m.VehicleMakeId == makeId.Value);

            if (!string.IsNullOrWhiteSpace(searchString))
                query = query.Where(m => m.Name.Contains(searchString));

            query = query.OrderByProperty(sortBy, sortOrder);
            
            return await PaginatedList<VehicleModelDTO>.CreateAsync(
                query.ProjectTo<VehicleModelDTO>(_mapper.ConfigurationProvider),
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
            try
            {
                var model = _mapper.Map<VehicleModel>(modelDto);
                await _context.VehicleModels.AddAsync(model);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error adding vehicle model", ex);
            }
        }

        public async Task UpdateModelAsync(VehicleModelDTO modelDto)
        {
            try
            {
                var model = await _context.VehicleModels.FindAsync(modelDto.Id);
                if (model == null) throw new KeyNotFoundException($"No VehicleModel found with ID {modelDto.Id}");

                _mapper.Map(modelDto, model);
                _context.VehicleModels.Update(model);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error updating vehicle model", ex);
            }
        }

        public async Task DeleteModelAsync(int id)
        {
            try
            {
                var model = await _context.VehicleModels.FindAsync(id);
                if (model == null) throw new KeyNotFoundException($"No VehicleModel found with ID {id}");

                _context.VehicleModels.Remove(model);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error deleting vehicle model", ex);
            }
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
