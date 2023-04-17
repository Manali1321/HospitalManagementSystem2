using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalManagementSystem.Models.ViewModels
{
    public class ServiceDetails
    {

        public Service SelectedService { get; set; }
        public IEnumerable<Location> AtLocations { get; set; }

        public IEnumerable<Location> AvailableLocations { get; set; }
    }
}