using Microsoft.EntityFrameworkCore;
using Project.Service.Models;
using System.Collections.Generic;
using Project.Service.Data.Helpers;
using Project.Service.Data.DTOs;

namespace Project.Service.Data.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public required DbSet<VehicleMake> VehicleMakes { get; set; }
        public required DbSet<VehicleModel> VehicleModels { get; set; }

        // Optional: Add fluent API configurations for relationships/constraints
        protected override void OnModelCreating(ModelBuilder modelBuilder)
{
            modelBuilder.Entity<VehicleModel>()
        .HasOne(vm => vm.VehicleMake)
        .WithMany(vm => vm.VehicleModels)
        .HasForeignKey(vm => vm.MakeId)
        .OnDelete(DeleteBehavior.Cascade);

        // Seed data - initialize navigation properties
            modelBuilder.Entity<VehicleMake>().HasData(
        new VehicleMake 
        { 
            Id = 1, 
            Name = "BMW", 
            Abrv = "B",
            VehicleModels = new List<VehicleModel>() // Explicit initialization
        }
    );

            modelBuilder.Entity<VehicleModel>().HasData(
        new VehicleModel 
        { 
            Id = 1, 
            Name = "X5", 
            Abrv = "X5",
            MakeId = 1 
        }
    );
}
    }
}