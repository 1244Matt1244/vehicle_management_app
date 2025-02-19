using Project.Service.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Project.Service.Interfaces
{
    public interface IVehicleService
    {
        Task<IEnumerable<VehicleModel>> GetAllVehicleModelsAsync();
        Task<VehicleModel?> GetVehicleModelByIdAsync(int id); // Nullable return type
        Task AddVehicleModelAsync(VehicleModel vehicleModel);
        Task DeleteVehicleModelAsync(int id);
    }
}
