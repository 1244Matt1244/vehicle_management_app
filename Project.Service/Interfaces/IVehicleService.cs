// Project.Service/Interfaces/IVehicleService.cs
using Project.Service.DTOs;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Project.Service.Interfaces
{
    public interface IVehicleService
    {
        // Vehicle Make Methods
        Task<PaginatedList<VehicleMakeDTO>> GetMakesAsync(QueryParams parameters);
        Task<VehicleMakeDTO> GetMakeByIdAsync(int id);
        Task CreateMakeAsync(VehicleMakeDTO makeDto);
        Task UpdateMakeAsync(VehicleMakeDTO makeDto);
        Task DeleteMakeAsync(int id);

        // Vehicle Model Methods
        Task<PaginatedList<VehicleModelDTO>> GetModelsAsync(QueryParams parameters);
        Task<VehicleModelDTO> GetModelByIdAsync(int id);
        Task CreateModelAsync(VehicleModelDTO modelDto);
        Task UpdateModelAsync(VehicleModelDTO modelDto);
        Task DeleteModelAsync(int id);
    }
}
