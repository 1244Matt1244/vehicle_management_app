// Project.Service/Services/VehicleService.cs
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Project.Service.Data;
using Project.Service.Models;

namespace Project.Service.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly VehicleContext _context;

        public VehicleService(VehicleContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<VehicleMake>> GetVehicleMakesAsync(string sortOrder, string filter, int page, int pageSize)
        {
            var query = _context.VehicleMakes.AsQueryable();

            if (!string.IsNullOrEmpty(filter))
            {
                query = query.Where(m => m.Name.Contains(filter));
            }

            query = sortOrder == "name_desc" ? query.OrderByDescending(m => m.Name)
                                              : query.OrderBy(m => m.Name);

            return await query.Skip((page - 1) * pageSize)
                              .Take(pageSize)
                              .ToListAsync();
        }

        public async Task<VehicleMake> GetVehicleMakeByIdAsync(int id)
        {
            return await _context.VehicleMakes.FindAsync(id);
        }

        public async Task CreateVehicleMakeAsync(VehicleMake make)
        {
            _context.VehicleMakes.Add(make);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateVehicleMakeAsync(VehicleMake make)
        {
            _context.Entry(make).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteVehicleMakeAsync(int id)
        {
            var make = await _context.VehicleMakes.FindAsync(id);
            if (make != null)
            {
                _context.VehicleMakes.Remove(make);
                await _context.SaveChangesAsync();
            }
        }

        // Metode za VehicleModel
        public async Task<IEnumerable<VehicleModel>> GetVehicleModelsAsync(string sortOrder, int makeId, int page, int pageSize)
        {
            var query = _context.VehicleModels.AsQueryable();
            if (makeId > 0)
            {
                query = query.Where(m => m.MakeId == makeId);
            }
            query = sortOrder == "name_desc" ? query.OrderByDescending(m => m.Name)
                                              : query.OrderBy(m => m.Name);
            return await query.Skip((page - 1) * pageSize)
                              .Take(pageSize)
                              .ToListAsync();
        }

        public async Task<VehicleModel> GetVehicleModelByIdAsync(int id)
        {
            return await _context.VehicleModels.FindAsync(id);
        }

        public async Task CreateVehicleModelAsync(VehicleModel model)
        {
            _context.VehicleModels.Add(model);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateVehicleModelAsync(VehicleModel model)
        {
            _context.Entry(model).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteVehicleModelAsync(int id)
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
