using System.Collections.Generic;
using System.Threading.Tasks;
using Project.Service.Data.DTOs;

namespace Project.Service.Interfaces
{
    public interface IVehicleService
    {
        // VehicleMake methods
        Task<IEnumerable<VehicleMakeDTO>> GetAllMakesAsync();
        Task<VehicleMakeDTO> GetMakeByIdAsync(int id);
        Task CreateMakeAsync(VehicleMakeDTO makeDto);
        Task UpdateMakeAsync(VehicleMakeDTO makeDto);
        Task DeleteMakeAsync(int id);

        // VehicleModel methods
        Task<IEnumerable<VehicleModelDTO>> GetAllModelsAsync();
        Task<VehicleModelDTO> GetModelByIdAsync(int id);
        Task CreateModelAsync(VehicleModelDTO modelDto);
        Task UpdateModelAsync(VehicleModelDTO modelDto);
        Task DeleteModelAsync(int id);

        // Pagination and Filtering
        Task<PaginatedList<VehicleModelDTO>> GetModelsAsync(string sortOrder, string searchString, int pageNumber, int pageSize);
    }
}