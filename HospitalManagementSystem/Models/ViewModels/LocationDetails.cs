using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalManagementSystem.Models.ViewModels
{
    public class LocationDetails
    {
        //the locations itself that we want to display
        public Location SelectedLocation { get; set; }

        //all of the related departments to that particular location
        public IEnumerable<Department> RelatedDepartments { get; set; }

        public IEnumerable<Department> AvailableDepartments { get; set; }

        //all of the related services to that particular location
        public IEnumerable<Service> RelatedServices { get; set; }

        public IEnumerable<Service> AvailableServices { get; set; }

        //all of the related careers to that particular location
        public IEnumerable<Career> RelatedCareers { get; set; }

        public IEnumerable<Career> AvailableCareers { get; set; }
    }
}