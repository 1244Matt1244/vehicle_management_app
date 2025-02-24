using Microsoft.EntityFrameworkCore;
using Project.Service.Data.Helpers;
using Project.Service.Interfaces;
using Project.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Project.Service.Data.Context;

namespace Project.Service.Repositories
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly ApplicationDbContext _context;

        public VehicleRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        #region VehicleMake Implementation

        public async Task<PaginatedList<VehicleMake>> GetMakesPaginatedAsync(int page, int pageSize, string sortBy, string sortOrder, string searchString)
        {
            IQueryable<VehicleMake> query = _context.VehicleMakes;

            if (!string.IsNullOrEmpty(searchString))
                query = query.Where(m => m.Name.Contains(searchString));

            query = sortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase) 
                ? query.OrderByDescending(m => EF.Property<object>(m, sortBy))
                : query.OrderBy(m => EF.Property<object>(m, sortBy));

            return await PaginatedList<VehicleMake>.CreateAsync(query, page, pageSize);
        }

        public async Task<VehicleMake?> GetMakeByIdAsync(int id) => 
            await _context.VehicleMakes.FindAsync(id);

        public async Task AddMakeAsync(VehicleMake make)
        {
            await _context.VehicleMakes.AddAsync(make);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateMakeAsync(VehicleMake make)
        {
            _context.VehicleMakes.Update(make);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMakeAsync(VehicleMake make)
        {
            _context.VehicleMakes.Remove(make);
            await _context.SaveChangesAsync();
        }

        // New method to return IQueryable for makes
        public IQueryable<VehicleMake> GetMakesQueryable()
        {
            return _context.VehicleMakes; // Return IQueryable for further querying
        }

        #endregion

        #region VehicleModel Implementation

        public async Task<PaginatedList<VehicleModel>> GetModelsPaginatedAsync(int page, int pageSize, string sortBy, string sortOrder, string searchString, int? makeId)
        {
            IQueryable<VehicleModel> query = _context.VehicleModels;

            if (makeId.HasValue)
                query = query.Where(m => m.MakeId == makeId.Value);

            if (!string.IsNullOrEmpty(searchString))
                query = query.Where(m => m.Name.Contains(searchString));

            query = sortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase) 
                ? query.OrderByDescending(m => EF.Property<object>(m, sortBy))
                : query.OrderBy(m => EF.Property<object>(m, sortBy));

            return await PaginatedList<VehicleModel>.CreateAsync(query, page, pageSize);
        }

        public async Task<VehicleModel?> GetModelByIdAsync(int id) => 
            await _context.VehicleModels.FindAsync(id);

        public async Task AddModelAsync(VehicleModel model)
        {
            await _context.VehicleModels.AddAsync(model);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateModelAsync(VehicleModel model)
        {
            _context.VehicleModels.Update(model);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteModelAsync(VehicleModel model)
        {
            _context.VehicleModels.Remove(model);
            await _context.SaveChangesAsync();
        }

        #endregion

        #region Common Implementation

        public async Task<List<VehicleMake>> GetAllMakesAsync() => 
            await _context.VehicleMakes.ToListAsync();

        public async Task<List<VehicleModel>> GetAllModelsAsync() => 
            await _context.VehicleModels.ToListAsync();

        #endregion
    }
}
