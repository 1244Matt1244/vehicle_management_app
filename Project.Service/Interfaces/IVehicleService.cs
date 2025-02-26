using Project.Service.Data.DTOs;
using Project.Service.Data.Helpers;
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
    }
}