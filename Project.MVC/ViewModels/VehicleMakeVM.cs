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
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public PaginationVM? Pagination { get; set; }
        public string CurrentSort { get; set; } = string.Empty;
        public string SortOrder { get; set; } = string.Empty;
        public string SearchString { get; set; } = string.Empty;
    }
}