using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalManagementSystem.Models.ViewModels
{
    public class UpdateDepartment
    {
        //This viewmodel is a class which stores information that we need to present to /Department/Update/{}

        //the existing department information

        public Department SelectedDepartment { get; set; }

        // all species to choose from when updating this department

        public IEnumerable<Location> LocationOptions { get; set; }
    }
}