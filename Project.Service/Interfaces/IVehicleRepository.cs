using Project.Data.Models;
using Project.Service.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Project.Service.Interfaces
{
    public interface IVehicleRepository
    {
        // VehicleMake Operations
        Task<PaginatedList<VehicleMake>> GetMakesPaginatedAsync(int page, int pageSize, string sortOrder, string searchString);
        Task<VehicleMake> GetMakeByIdAsync(int id);
        Task AddMakeAsync(VehicleMake make);
        Task UpdateMakeAsync(VehicleMake make);
        Task DeleteMakeAsync(int id);

        // VehicleModel Operations
        Task<PaginatedList<VehicleModel>> GetModelsPaginatedAsync(int page, int pageSize, string sortOrder, string searchString, int? makeId);
        Task<VehicleModel> GetModelByIdAsync(int id);
        Task AddModelAsync(VehicleModel model);
        Task UpdateModelAsync(VehicleModel model);
        Task DeleteModelAsync(int id);
    }
}