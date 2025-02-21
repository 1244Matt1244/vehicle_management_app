using AutoMapper;
using AutoMapper.QueryableExtensions;
using Project.Service.Data.DTOs;
using Project.Service.Helpers;
using Project.MVC.ViewModels;

public class MvcMappingProfile : Profile
{
    public MvcMappingProfile()
    {
        // DTO <-> ViewModel conversions
        CreateMap<VehicleMakeDTO, VehicleMakeVM>()
            .ForMember(dest => dest.ModelCount, 
                    opt => opt.MapFrom(src => src.Models.Count))
            .ReverseMap();
        
        CreateMap<VehicleModelDTO, VehicleModelVM>()
            .ForMember(dest => dest.MakeName, 
                    opt => opt.MapFrom(src => src.Make.Name))
            .ReverseMap();

        // Paginated list support
        CreateMap<PaginatedList<VehicleMakeDTO>, PaginatedList<VehicleMakeVM>>()
            .ConvertUsing<PaginatedListConverter<VehicleMakeDTO, VehicleMakeVM>>();
        
        CreateMap<PaginatedList<VehicleModelDTO>, PaginatedList<VehicleModelVM>>()
            .ConvertUsing<PaginatedListConverter<VehicleModelDTO, VehicleModelVM>>();
    }
}
