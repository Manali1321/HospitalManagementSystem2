using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalManagementSystem.Models
{
    public class Department
    {

        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }

        //A department can be at many locations.
        public ICollection<Location> Locations { get; set; }    // creating bridging table between location and department entity
    }
}