using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalManagementSystem.Models.ViewModels
{
    public class DepartmentList
    {
        public IEnumerable<Department> Departments { get; set; }

        public IEnumerable<Location> Locations { get; set; }
    }
}