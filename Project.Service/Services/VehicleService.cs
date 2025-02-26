using Microsoft.EntityFrameworkCore;
using Project.Service.Data.Context;
using Project.Service.Data.DTOs;
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

        // Updated model query with Include for navigation property
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

            // Filtering logic
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                query = query.Where(m => m.Name.Contains(searchString) || 
                                         m.Abbreviation.Contains(searchString));
            }

            // Sorting logic
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                query = sortOrder.ToLower() == "desc" 
                    ? query.OrderByDescending(m => EF.Property<object>(m, sortBy)) 
                    : query.OrderBy(m => EF.Property<object>(m, sortBy));
            }

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
