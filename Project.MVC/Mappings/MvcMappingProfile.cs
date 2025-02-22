// Project.MVC/Mappings/MvcMappingProfile.cs
using AutoMapper;
using Project.Service.Data.DTOs;
using Project.MVC.ViewModels;

namespace Project.MVC.Mappings
{
    public class MvcMappingProfile : Profile
    {
        public MvcMappingProfile()
        {
            // Map DTOs to ViewModels
            CreateMap<VehicleMakeDTO, VehicleMakeVM>().ReverseMap();
            CreateMap<VehicleModelDTO, VehicleModelVM>().ReverseMap();

            // PaginatedList mapping
            CreateMap<PaginatedList<VehicleMakeDTO>, PaginatedList<VehicleMakeVM>>()
                .ConvertUsing((src, dest) => new PaginatedList<VehicleMakeVM>(
                    src.Items.Select(dto => new VehicleMakeVM 
                    { 
                        Id = dto.Id, 
                        Name = dto.Name, 
                        Abrv = dto.Abrv 
                    }).ToList(),
                    src.TotalCount,
                    src.PageIndex,
                    src.PageSize
                ));
        }
    }
}