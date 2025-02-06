// Project.Service/Services/IVehicleService.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using Project.Service.Models;

namespace Project.Service.Services
{
    public interface IVehicleService
    {
        // VehicleMake metode
        Task<IEnumerable<VehicleMake>> GetVehicleMakesAsync(string sortOrder, string filter, int page, int pageSize);
        Task<VehicleMake> GetVehicleMakeByIdAsync(int id);
        Task CreateVehicleMakeAsync(VehicleMake make);
        Task UpdateVehicleMakeAsync(VehicleMake make);
        Task DeleteVehicleMakeAsync(int id);
        
        // VehicleModel metode
        Task<IEnumerable<VehicleModel>> GetVehicleModelsAsync(string sortOrder, int makeId, int page, int pageSize);
        Task<VehicleModel> GetVehicleModelByIdAsync(int id);
        Task CreateVehicleModelAsync(VehicleModel model);
        Task UpdateVehicleModelAsync(VehicleModel model);
        Task DeleteVehicleModelAsync(int id);
    }
}
