using AutoMapper;
using Project.Service.Shared.DTOs;
using Project.Service.Shared.Helpers;
using Project.MVC.ViewModels;

public class WebMappings : Profile
{
    public WebMappings()
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
