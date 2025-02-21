// Project.Service/Interfaces/IVehicleService.cs
using Project.Service.Data.DTOs;

namespace Project.Service.Interfaces
{
    public interface IVehicleService
    {
        // Vehicle Make Methods
        Task<IEnumerable<VehicleMakeDTO>> GetAllMakesAsync();
        Task<VehicleMakeDTO> GetMakeByIdAsync(int id);
        Task CreateMakeAsync(VehicleMakeDTO makeDto);
        Task UpdateMakeAsync(VehicleMakeDTO makeDto);
        Task DeleteMakeAsync(int id);

        // Vehicle Model Methods
        Task<IEnumerable<VehicleModelDTO>> GetAllModelsAsync();
        Task<VehicleModelDTO> GetModelByIdAsync(int id);
        Task CreateModelAsync(VehicleModelDTO modelDto);
        Task UpdateModelAsync(VehicleModelDTO modelDto);
        Task DeleteModelAsync(int id);
    }
}
