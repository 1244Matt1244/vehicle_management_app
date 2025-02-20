using Project.Service.Data;
using Project.Service.Interfaces;
using Project.Service.Models;
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

        public async Task<IEnumerable<VehicleModel>> GetAllVehicleModelsAsync()
        {
            return await Task.FromResult(_context.VehicleModels.ToList());
        }

        public async Task<VehicleModel?> GetVehicleModelByIdAsync(int id)
        {
            return await Task.FromResult(_context.VehicleModels.FirstOrDefault(vm => vm.Id == id));
        }

        public async Task AddVehicleModelAsync(VehicleModel vehicleModel)
        {
            _context.VehicleModels.Add(vehicleModel);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteVehicleModelAsync(int id)
        {
            var vehicleModel = _context.VehicleModels.FirstOrDefault(vm => vm.Id == id);
            if (vehicleModel != null)
            {
                _context.VehicleModels.Remove(vehicleModel);
                await _context.SaveChangesAsync();
            }
        }
    }
}
