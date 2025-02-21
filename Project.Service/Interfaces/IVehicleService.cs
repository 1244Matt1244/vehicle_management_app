using Project.Service.Data.DTOs;
using Project.Service.Data.Helpers;
using System.Threading.Tasks;

namespace Project.Service.Interfaces
{
    public interface IVehicleService
    {
        // VehicleMake Methods
        Task<PaginatedList<VehicleMakeDTO>> GetMakesPaginatedAsync(int page, int pageSize, string sortOrder, string searchString);
        Task<VehicleMakeDTO> GetMakeByIdAsync(int id);
        Task CreateMakeAsync(VehicleMakeDTO makeDto);
        Task UpdateMakeAsync(VehicleMakeDTO makeDto);
        Task DeleteMakeAsync(int id);

        // VehicleModel Methods
        Task<PaginatedList<VehicleModelDTO>> GetModelsPaginatedAsync(int page, int pageSize, string sortOrder, string searchString, int? makeId);
        Task<VehicleModelDTO> GetModelByIdAsync(int id);
        Task CreateModelAsync(VehicleModelDTO modelDto);
        Task UpdateModelAsync(VehicleModelDTO modelDto);
        Task DeleteModelAsync(int id);
    }
}