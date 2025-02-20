using Project.MVC.ViewModels;

namespace Project.Service.Interfaces
{
    public interface IVehicleService
    {
        Task<IEnumerable<VehicleModelVM>> GetModelsAsync();
        Task CreateModelAsync(VehicleModelVM model);
        Task<VehicleModelVM> GetModelByIdAsync(int id);
        Task UpdateModelAsync(VehicleModelVM model);
        Task DeleteModelAsync(int id);

        Task<IEnumerable<VehicleMakeVM>> GetAllMakesAsync();
        Task CreateMakeAsync(VehicleMakeVM make);
        Task<VehicleMakeVM> GetMakeByIdAsync(int id);
        Task UpdateMakeAsync(VehicleMakeVM make);
        Task DeleteMakeAsync(int id);
    }
}
