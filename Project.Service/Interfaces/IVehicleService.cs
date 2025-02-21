using System.Threading.Tasks;
using Project.Service.Data.DTOs;
using Project.Service.Data.Helpers;

namespace Project.Service.Interfaces
{
    public interface IVehicleService
    {
        // Vehicle Model Methods
        Task<PaginatedList<VehicleModelDTO>> GetModelsAsync(int page, int pageSize, string sortOrder, int makeId = 0);
        Task<VehicleModelDTO> GetModelByIdAsync(int id);
        Task AddModelAsync(VehicleModelDTO model);
        Task UpdateModelAsync(VehicleModelDTO model);
        Task DeleteModelAsync(int id);

        // Vehicle Make Methods
        Task<PaginatedList<VehicleMakeDTO>> GetMakesAsync(int page, int pageSize, string sortOrder);
        Task<VehicleMakeDTO> GetMakeByIdAsync(int id);
        Task CreateMakeAsync(VehicleMakeDTO makeDto);
        Task UpdateMakeAsync(VehicleMakeDTO makeDto);
        Task DeleteMakeAsync(int id);
    }
}
