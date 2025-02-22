using AutoMapper;
using Project.Service.Data.DTOs;
using Project.Service.Data.Helpers;
using System.Linq; // Ensure this is included

namespace Project.MVC.Mappings
{
    public class MvcMappingProfile : Profile
    {
        public MvcMappingProfile()
        {
            CreateMap<PaginatedList<VehicleModelDTO>, PaginatedList<VehicleModelDTO>>()
                .ConvertUsing((src, dest) => new PaginatedList<VehicleModelDTO>(
                    src.Items.ToList(), // Use ToList() from LINQ
                    src.TotalCount,
                    src.PageIndex,
                    src.PageSize
                ));

            CreateMap<PaginatedList<VehicleMakeDTO>, PaginatedList<VehicleMakeDTO>>()
                .ConvertUsing((src, dest) => new PaginatedList<VehicleMakeDTO>(
                    src.Items.ToList(), // Use ToList() from LINQ
                    src.TotalCount,
                    src.PageIndex,
                    src.PageSize
                ));
        }
    }
}
