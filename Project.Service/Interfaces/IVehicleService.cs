using Project.Service.Data.DTOs;
using Project.Service.Data.Helpers;
using System.Threading.Tasks;

namespace Project.Service.Interfaces
{
    public interface IVehicleService
    {
        // VehicleMake Methods
        Task<PaginatedList<VehicleMakeDTO>> GetMakesAsync(int page, int pageSize, string sortOrder, string searchString);
        Task AddMakeAsync(VehicleMakeDTO makeDto);
        Task UpdateMakeAsync(VehicleMakeDTO makeDto);
        Task DeleteMakeAsync(int id);

        // VehicleModel Methods
        Task<PaginatedList<VehicleModelDTO>> GetModelsAsync(int page, int pageSize, string sortOrder, string searchString, int? makeId);
        Task AddModelAsync(VehicleModelDTO modelDto);
        Task UpdateModelAsync(VehicleModelDTO modelDto);
        Task DeleteModelAsync(int id);
    }
}
