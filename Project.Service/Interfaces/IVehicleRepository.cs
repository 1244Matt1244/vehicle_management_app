using Project.Service.Data.Helpers;
using Project.Service.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Project.Service.Interfaces
{
    public interface IVehicleRepository
    {
        // VehicleMake Methods
        Task<PaginatedList<VehicleMake>> GetMakesPaginatedAsync(
            int pageIndex, int pageSize, string sortBy, string sortOrder, string searchString);
        Task<VehicleMake?> GetMakeByIdAsync(int id);
        Task CreateMakeAsync(VehicleMake make);
        Task UpdateMakeAsync(VehicleMake make);
        Task DeleteMakeAsync(int id);
        Task<List<VehicleMake>> GetAllMakesAsync();

        // VehicleModel Methods
        Task<PaginatedList<VehicleModel>> GetModelsPaginatedAsync(
            int pageIndex, int pageSize, string sortBy, string sortOrder, string searchString, int? makeId);
        Task<VehicleModel?> GetModelByIdAsync(int id);
        Task CreateModelAsync(VehicleModel model);
        Task UpdateModelAsync(VehicleModel model);
        Task DeleteModelAsync(int id);
        Task<List<VehicleModel>> GetAllModelsAsync();
    }
}