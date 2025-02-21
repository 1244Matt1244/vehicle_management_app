using AutoMapper;
using Project.MVC.ViewModels;
using Project.Service.Data.DTOs;

namespace Project.MVC.Mappings
{
    public class MvcMappingProfile : Profile
    {
        public MvcMappingProfile()
        {
            CreateMap<VehicleModelDTO, VehicleModelVM>()
                .ForMember(dest => dest.MakeName, opt => opt.MapFrom(src => src.Make.Name));

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