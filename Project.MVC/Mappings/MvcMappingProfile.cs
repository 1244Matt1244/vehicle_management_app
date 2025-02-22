using AutoMapper;
using Project.MVC.ViewModels;
using Project.Service.Data.DTOs;
using Project.Service.Data.Helpers; // Add this

namespace Project.MVC.Mappings
{
    public class MvcMappingProfile : Profile
    {
        public MvcMappingProfile()
        {
            // VehicleModel mappings
            CreateMap<VehicleModelDTO, VehicleModelVM>()
                .ForMember(dest => dest.MakeName, opt => opt.MapFrom(src => src.MakeName));

            // VehicleMake mappings
            CreateMap<VehicleMakeDTO, VehicleMakeVM>();

            // PaginatedList mappings
            CreateMap<PaginatedList<VehicleModelDTO>, VehicleModelIndexVM>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items))
                .ForMember(dest => dest.Pagination, opt => opt.MapFrom(src => new PaginationVM
                {
                    Page = src.PageIndex,
                    PageSize = src.Items.Count,
                    TotalItems = src.TotalCount
                }));
        }
    }
}