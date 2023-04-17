using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalManagementSystem.Models.ViewModels
{
    public class DepartmentDetails
    {
        //the departments itself that we want to display
        public Department SelectedDepartment { get; set; }

        //all of the related locations to that particular department
        public IEnumerable<Location> RelatedLocations { get; set; }

        //all of the available locations to that particular department
        public IEnumerable<Location> AvailableLocations { get; set; }

        public IEnumerable<Career> RelatedCareers { get; set; }
    }
}