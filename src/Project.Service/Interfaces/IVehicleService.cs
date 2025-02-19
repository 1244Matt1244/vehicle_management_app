using Project.Service.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Project.Service.Interfaces
{
    public interface IVehicleService
    {
        Task<IEnumerable<VehicleMakeDTO>> GetAllMakesAsync(string search, string sortOrder, int page, int pageSize);
        Task<IEnumerable<VehicleModelDTO>> GetAllModelsAsync(string search, string sortOrder, int page, int pageSize);
        Task<VehicleMakeDTO> GetMakeByIdAsync(int id);
        Task<VehicleModelDTO> GetModelByIdAsync(int id);
        Task AddMakeAsync(VehicleMakeDTO makeDto);
        Task AddModelAsync(VehicleModelDTO modelDto);
        Task UpdateMakeAsync(VehicleMakeDTO makeDto);
        Task UpdateModelAsync(VehicleModelDTO modelDto);
        Task DeleteMakeAsync(int id);
        Task DeleteModelAsync(int id);
    }
}
