using System.Collections.Generic;
using System.Threading.Tasks;
using Project.Service.Data.Helpers;
using Project.Service.Models;

namespace Project.Service.Interfaces
{
    public interface IVehicleRepository
    {
        // VehicleMake
        Task<PaginatedList<VehicleMake>> GetMakesPaginatedAsync(
            int page, int pageSize, string sortBy, string sortOrder, string searchString);
        Task<VehicleMake?> GetMakeByIdAsync(int id);
        Task AddMakeAsync(VehicleMake make);
        Task UpdateMakeAsync(VehicleMake make);
        Task DeleteMakeAsync(VehicleMake make);

        // VehicleModel
        Task<PaginatedList<VehicleModel>> GetModelsPaginatedAsync(
            int page, int pageSize, string sortBy, string sortOrder, 
            string searchString, int? makeId);
        Task<VehicleModel?> GetModelByIdAsync(int id);
        Task AddModelAsync(VehicleModel model);
        Task UpdateModelAsync(VehicleModel model);
        Task DeleteModelAsync(VehicleModel model);

        // Common
        Task<List<VehicleMake>> GetAllMakesAsync();
        Task<List<VehicleModel>> GetAllModelsAsync();
    }
}
