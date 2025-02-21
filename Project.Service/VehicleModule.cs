using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Ninject.Modules;
using Project.Service.Data.Context;
using Project.Service.Interfaces;
using Project.Service.Mapping; // Ensure this namespace is correct
using Project.Service.Services;

namespace Project.Service
{
    public class VehicleModule : NinjectModule

    {
        public override void Load()
        {
            // Database Context (scoped per request)
            Bind<ApplicationDbContext>()
                .ToSelf()
                .InTransientScope()  // For EF Core, let DI handle the scope
                .WithConstructorArgument("options", CreateDbContextOptions());

            // Service Layer
            Bind<IVehicleService>()
                .To<VehicleService>()
                .InSingletonScope();

            // AutoMapper Configuration
            Bind<IMapper>().ToMethod(ctx => 
                new MapperConfiguration(cfg => {
                    cfg.AddProfile<ServiceProfile>(); // Corrected reference
                }).CreateMapper()
            ).InSingletonScope();
        }

        private DbContextOptions<ApplicationDbContext> CreateDbContextOptions()
        {
            return new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite("Data Source=vehicle_management.db")
                .Options;
        }
    }
}
