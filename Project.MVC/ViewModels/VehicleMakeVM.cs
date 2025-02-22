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
        public string Name { get; set; }
        public string Abrv { get; set; }
        public PaginatedList<VehicleMakeVM>? PaginatedList { get; set; }
        public string CurrentSort { get; set; }
        public string SortOrder { get; set; }
        public string SearchString { get; set; }
    }
}