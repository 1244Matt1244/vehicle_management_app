using Microsoft.EntityFrameworkCore;
using Project.Service.Data.Context;
using Project.Service.Data.Helpers;
using Project.Service.Interfaces;
using Project.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<PaginatedList<VehicleMake>> GetMakesPaginatedAsync(
            int pageIndex, int pageSize, string sortBy, string sortOrder, string searchString)
        {
            var query = _context.VehicleMakes.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(m =>
                    m.Name.Contains(searchString) ||
                    m.Abbreviation.Contains(searchString));
            }

            var totalCount = await query.CountAsync();
            var orderedQuery = query.OrderByProperty(sortBy, sortOrder);

            var items = await orderedQuery
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedList<VehicleMake>(items, totalCount, pageIndex, pageSize);
        }

        public async Task<VehicleMake?> GetMakeByIdAsync(int id) =>
            await _context.VehicleMakes.FindAsync(id);

        public async Task CreateMakeAsync(VehicleMake make)
        {
            await _context.VehicleMakes.AddAsync(make);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateMakeAsync(VehicleMake make)
        {
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

        public async Task<List<VehicleMake>> GetAllMakesAsync() =>
            await _context.VehicleMakes.ToListAsync();

        #endregion

        #region VehicleModel Implementation

        public async Task<PaginatedList<VehicleModel>> GetModelsPaginatedAsync(
            int pageIndex, int pageSize, string sortBy, string sortOrder, string searchString, int? makeId)
        {
            var query = _context.VehicleModels
                .Include(m => m.VehicleMake)
                .AsQueryable();

            if (makeId.HasValue)
                query = query.Where(m => m.VehicleMakeId == makeId.Value); // Updated FK property

            if (!string.IsNullOrEmpty(searchString))
                query = query.Where(m => m.Name.Contains(searchString));

            var totalCount = await query.CountAsync();
            var orderedQuery = query.OrderByProperty(sortBy, sortOrder);

            var items = await orderedQuery
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedList<VehicleModel>(items, totalCount, pageIndex, pageSize);
        }

        public async Task<VehicleModel?> GetModelByIdAsync(int id) =>
            await _context.VehicleModels
                .Include(m => m.VehicleMake)
                .FirstOrDefaultAsync(m => m.Id == id);

        public async Task CreateModelAsync(VehicleModel model)
        {
            await _context.VehicleModels.AddAsync(model);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateModelAsync(VehicleModel model)
        {
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

        public async Task<List<VehicleModel>> GetAllModelsAsync() =>
            await _context.VehicleModels.ToListAsync();

        public async Task<IEnumerable<VehicleModel>> GetModelsByMakeIdAsync(int makeId)
        {
            return await _context.VehicleModels
                .Where(vm => vm.VehicleMakeId == makeId) // Updated FK property
                .ToListAsync();
        }

        #endregion
    }
}
