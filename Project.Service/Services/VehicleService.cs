using Microsoft.EntityFrameworkCore;
using Project.Service.Data.Context;
using Project.Service.Data.DTOs;
using Project.Service.Data.Helpers; // Add this using directive
using Project.Service.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Service.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly ApplicationDbContext _context;

        public VehicleService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Implement GetMakesAsync to fulfill IVehicleService
        public async Task<PaginatedList<VehicleMakeDTO>> GetMakesAsync(
            int pageIndex, 
            int pageSize, 
            string sortBy, 
            string sortOrder, 
            string searchString)
        {
            var query = _context.VehicleMakes.AsQueryable();

            // Filtering
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                query = query.Where(m => 
                    m.Name.Contains(searchString) || 
                    m.Abbreviation.Contains(searchString)
                );
            }

            // Sorting
            query = query.OrderByProperty(sortBy, sortOrder);

            return await PaginatedList<VehicleMakeDTO>.CreateAsync(
                query.Select(m => new VehicleMakeDTO 
                { 
                    Id = m.Id,
                    Name = m.Name,
                    Abbreviation = m.Abbreviation
                }), 
                pageIndex, 
                pageSize
            );
        }

        // Existing GetModelsAsync remains unchanged
        public async Task<PaginatedList<VehicleModelDTO>> GetModelsAsync(
            int pageNumber, 
            int pageSize, 
            string sortBy, 
            string sortOrder, 
            string searchString)
        {
            var query = _context.VehicleModels
                .Include(m => m.VehicleMake)  // Eager load navigation property
                .AsQueryable();

            // Add filtering/sorting logic here

            return await PaginatedList<VehicleModelDTO>.CreateAsync(
                query.Select(m => new VehicleModelDTO 
                { 
                    Id = m.Id,
                    Name = m.Name,
                    Abbreviation = m.Abbreviation,
                    MakeName = m.VehicleMake.Name  // Correct navigation access
                }), 
                pageNumber, 
                pageSize
            );
        }
    }
}
