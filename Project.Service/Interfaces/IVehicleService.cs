using Project.Service.Data.DTOs;
using Project.Service.Data.Helpers; // Critical for PaginatedList<T>
using System.Threading.Tasks;

namespace Project.Service.Interfaces
{
    public interface IVehicleService
    {
        Task<PaginatedList<VehicleMakeDTO>> GetMakesAsync(
            int pageIndex, 
            int pageSize, 
            string sortBy, 
            string sortOrder, 
            string searchString);
        
        // Other methods...
        
        Task<PaginatedList<VehicleModelDTO>> GetModelsAsync(
            int pageNumber, 
            int pageSize, 
            string sortBy, 
            string sortOrder, 
            string searchString);
        
        Task<VehicleMakeDTO> GetMakeByIdAsync(int id);
        Task AddMakeAsync(VehicleMakeDTO vehicleMake);
        Task UpdateMakeAsync(VehicleMakeDTO vehicleMake);
        Task DeleteMakeAsync(int id);
    }
}
