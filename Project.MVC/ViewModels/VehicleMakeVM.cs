using System.Collections.Generic;
using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Project.Service.Data.Helpers;

namespace Project.MVC.ViewModels
{
    public class VehicleMakeVM
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string Abrv { get; set; }
        public required PaginatedList<VehicleMakeVM>? PaginatedList { get; set; }
        public required string CurrentSort { get; set; }
        public required string SortOrder { get; set; }
        public required string SearchString { get; set; }
    }
}