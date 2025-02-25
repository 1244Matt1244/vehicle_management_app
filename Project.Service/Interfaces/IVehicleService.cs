// Project.Service/Interfaces/IVehicleService.cs
using Project.Service.Data.DTOs;
using Project.Service.Data.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Project.Service.Interfaces
{
    public interface IVehicleService
    {
        // VehicleMake
        Task<PaginatedList<VehicleMakeDTO>> GetMakesAsync(int page, int pageSize, string sortBy, string sortOrder, string searchString);
        Task<VehicleMakeDTO> GetMakeByIdAsync(int id);
        Task<VehicleMakeDTO> CreateMakeAsync(VehicleMakeDTO makeDto);
        Task UpdateMakeAsync(int id, VehicleMakeDTO makeDto);
        Task DeleteMakeAsync(int id);
        
        // VehicleModel
        Task<PaginatedList<VehicleModelDTO>> GetModelsAsync(int page, int pageSize, string sortBy, string sortOrder, string searchString, int? makeId);
        Task<VehicleModelDTO> GetModelByIdAsync(int id);
        Task<VehicleModelDTO> CreateModelAsync(VehicleModelDTO modelDto);
        Task UpdateModelAsync(int id, VehicleModelDTO modelDto);
        Task DeleteModelAsync(int id);
        
        // Common
        Task<List<VehicleMakeDTO>> GetAllMakesAsync();
        Task<List<VehicleModelDTO>> GetAllModelsAsync();
    }
}