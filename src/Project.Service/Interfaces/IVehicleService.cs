// Project.Service/Interfaces/IVehicleService.cs
using Project.Service.Data.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Project.Service.Interfaces
{
    public interface IVehicleService
    {
        // Vehicle Model Operations
        Task<IEnumerable<VehicleModelDTO>> GetModelsAsync();
        Task<VehicleModelDTO> GetModelByIdAsync(int id);
        Task CreateModelAsync(VehicleModelDTO model);
        Task UpdateModelAsync(VehicleModelDTO model);
        Task DeleteModelAsync(int id);

        // Vehicle Make Operations
        Task<IEnumerable<VehicleMakeDTO>> GetMakesAsync();
        Task<VehicleMakeDTO> GetMakeByIdAsync(int id);
        Task CreateMakeAsync(VehicleMakeDTO make);
        Task UpdateMakeAsync(VehicleMakeDTO make);
        Task DeleteMakeAsync(int id);
    }
}
