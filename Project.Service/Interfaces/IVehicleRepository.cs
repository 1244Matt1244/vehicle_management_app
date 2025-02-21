using System.Collections.Generic;
using System.Threading.Tasks;
using Project.Service.Models;

namespace Vehicle.Web.Repositories
{
    public interface IVehicleRepository
    {
        Task<IEnumerable<VehicleMake>> GetMakesAsync();
        Task<VehicleMake> GetMakeByIdAsync(int id);
        Task AddMakeAsync(VehicleMake make);
        Task UpdateMakeAsync(VehicleMake make);
        Task DeleteMakeAsync(int id);

        Task<IEnumerable<VehicleModel>> GetModelsAsync();
        Task<VehicleModel> GetModelByIdAsync(int id);
        Task AddModelAsync(VehicleModel model);
        Task UpdateModelAsync(VehicleModel model);
        Task DeleteModelAsync(int id);
    }
}
