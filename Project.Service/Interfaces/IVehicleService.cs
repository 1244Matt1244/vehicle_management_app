using Project.Service.Data.DTOs;
using Project.Service.Data.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Project.Service.Interfaces
{
    public interface IVehicleService
    {
        // VehicleMake operations
        Task<PaginatedList<VehicleMakeDTO>> GetMakesAsync(int pageIndex, int pageSize, string sortBy, string sortOrder, string searchString);
        Task<VehicleMakeDTO> GetMakeByIdAsync(int id);
        Task AddMakeAsync(VehicleMakeDTO makeDto);
        Task UpdateMakeAsync(VehicleMakeDTO makeDto);
        Task DeleteMakeAsync(int id);
        
        // VehicleModel operations
        Task<PaginatedList<VehicleModelDTO>> GetModelsAsync(int pageIndex, int pageSize, string sortBy, string sortOrder, string searchString, int? makeId);
        Task<VehicleModelDTO> GetModelByIdAsync(int id);
        Task AddModelAsync(VehicleModelDTO modelDto);
        Task UpdateModelAsync(VehicleModelDTO modelDto);
        Task DeleteModelAsync(int id);
        
        // Lists for dropdowns
        Task<List<VehicleMakeDTO>> GetAllMakesAsync();
        Task<List<VehicleModelDTO>> GetAllModelsAsync();
    }
}