using Microsoft.EntityFrameworkCore;
using Project.Data.Models;
using Project.Service.Interfaces;
using Project.Service.Helpers;
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
            _context = context;
        }

        // VehicleMake Methods
        public async Task<PaginatedList<VehicleMake>> GetMakesPaginatedAsync(int page, int pageSize, string sortOrder, string searchString)
        {
            var query = _context.VehicleMakes.AsQueryable();

            // Filtering
            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(m => m.Name.Contains(searchString));
            }

            // Sorting
            query = sortOrder switch
            {
                "name_desc" => query.OrderByDescending(m => m.Name),
                _ => query.OrderBy(m => m.Name),
            };

            // Pagination
            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedList<VehicleMake>(
                items,
                page,
                (int)Math.Ceiling(totalCount / (double)pageSize),
                totalCount
            );
        }

        public async Task<VehicleMake> GetMakeByIdAsync(int id)
        {
            return await _context.VehicleMakes.FindAsync(id) 
                ?? throw new KeyNotFoundException("VehicleMake not found.");
        }

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

        public async Task DeleteMakeAsync(int id)
        {
            var make = await GetMakeByIdAsync(id);
            _context.VehicleMakes.Remove(make);
            await _context.SaveChangesAsync();
        }

        // VehicleModel Methods
        public async Task<PaginatedList<VehicleModel>> GetModelsPaginatedAsync(int page, int pageSize, string sortOrder, int? makeId)
        {
            var query = _context.VehicleModels.AsQueryable();

            // Filtering by MakeId
            if (makeId.HasValue)
            {
                query = query.Where(m => m.MakeId == makeId.Value);
            }

            // Sorting
            query = sortOrder switch
            {
                "name_desc" => query.OrderByDescending(m => m.Name),
                _ => query.OrderBy(m => m.Name),
            };

            // Pagination
            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedList<VehicleModel>(
                items,
                page,
                (int)Math.Ceiling(totalCount / (double)pageSize),
                totalCount
            );
        }

        public async Task<VehicleModel> GetModelByIdAsync(int id)
        {
            return await _context.VehicleModels.FindAsync(id) 
                ?? throw new KeyNotFoundException("VehicleModel not found.");
        }

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

        public async Task DeleteModelAsync(int id)
        {
            var model = await GetModelByIdAsync(id);
            _context.VehicleModels.Remove(model);
            await _context.SaveChangesAsync();
        }
    }
}
