using Project.Service.Data.Helpers;
using Project.Service.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Project.Service.Interfaces
{
    public interface IVehicleRepository
    {
        // Vehicle Make Methods
        Task<(List<VehicleMake> Makes, int TotalCount)> GetMakesPaginatedAsync(int page, int pageSize, string sortBy, string sortOrder, string searchString);
        Task<VehicleMake?> GetMakeByIdAsync(int id);
        Task AddMakeAsync(VehicleMake make);
        Task UpdateMakeAsync(VehicleMake make);
        Task DeleteMakeAsync(VehicleMake make);
        Task<List<VehicleMake>> GetAllMakesAsync();

        // Vehicle Model Methods
        Task<(List<VehicleModel> Models, int TotalCount)> GetModelsPaginatedAsync(int page, int pageSize, string sortBy, string sortOrder, string searchString, int? makeId);
        Task<VehicleModel?> GetModelByIdAsync(int id);
        Task AddModelAsync(VehicleModel model);
        Task UpdateModelAsync(VehicleModel model);
        Task DeleteModelAsync(VehicleModel model);
        Task<List<VehicleModel>> GetAllModelsAsync();
    }
}
