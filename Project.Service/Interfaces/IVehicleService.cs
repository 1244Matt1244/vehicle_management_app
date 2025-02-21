using System.Threading.Tasks;
using Project.Service.Data.DTOs;
using Project.Service.Data.Helpers;

namespace Project.Service.Interfaces
{
    public interface IVehicleService
    {
        // Makes
        Task<PaginatedList<VehicleMakeDTO>> GetMakesAsync(QueryParams parameters);
        Task<VehicleMakeDTO> GetMakeByIdAsync(int id);
        Task CreateMakeAsync(VehicleMakeDTO makeDto);
        Task UpdateMakeAsync(VehicleMakeDTO makeDto);
        Task DeleteMakeAsync(int id);

        // Models
        Task<PaginatedList<VehicleModelDTO>> GetModelsAsync(QueryParams parameters);
        Task<VehicleModelDTO> GetModelByIdAsync(int id);
        Task CreateModelAsync(VehicleModelDTO modelDto);
        Task UpdateModelAsync(VehicleModelDTO modelDto);
        Task DeleteModelAsync(int id);
    }
}