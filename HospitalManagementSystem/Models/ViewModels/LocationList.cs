using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalManagementSystem.Models.ViewModels
{
    public class LocationList
    {
        public IEnumerable<Location> Locations { get; set; }
    }
}