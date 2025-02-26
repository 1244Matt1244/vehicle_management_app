using Project.Service.Data.DTOs;
using Project.Service.Data.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Project.Service.Interfaces
{
    public interface IVehicleService
    {
        // VehicleMake
        Task<PaginatedList<VehicleMakeDTO>> GetMakesAsync(int pageIndex, int pageSize, string sortBy, string sortOrder, string searchString);
        Task<VehicleMakeDTO> GetMakeByIdAsync(int id);
        Task AddMakeAsync(VehicleMakeDTO makeDto); // Existing method
        Task UpdateMakeAsync(VehicleMakeDTO makeDto); // Existing method
        Task DeleteMakeAsync(int id); // Existing method
        Task<List<VehicleMakeDTO>> GetAllMakesAsync(); // Existing method
        
        // New method definitions
        Task CreateMakeAsync(VehicleMakeDTO make); // New method
        Task CreateModelAsync(VehicleModelDTO model); // New method

        // VehicleModel
        Task<PaginatedList<VehicleModelDTO>> GetModelsAsync(int pageIndex, int pageSize, string sortBy, string sortOrder, string searchString, int? makeId);
        Task<VehicleModelDTO> GetModelByIdAsync(int id);
        Task AddModelAsync(VehicleModelDTO modelDto); // Existing method
        Task UpdateModelAsync(VehicleModelDTO modelDto); // Existing method
        Task DeleteModelAsync(int id); // Existing method
        Task<List<VehicleModelDTO>> GetAllModelsAsync(); // Existing method
    }
}
