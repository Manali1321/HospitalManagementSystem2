using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalManagementSystem.Models
{
    public class Location
    {


        public int LocationId { get; set; }
        public string LocationName { get; set; }
        public string LocationAddress { get; set; }
        public string LocationContactNo { get; set; }

        //A location can have many departments
        public ICollection<Department> Departments { get; set; }    // creating bridging table between location and department entity

        //A location can have many services.
        public ICollection<Service> Services { get; set; }    // creating bridging table between location and services entity
    }
}