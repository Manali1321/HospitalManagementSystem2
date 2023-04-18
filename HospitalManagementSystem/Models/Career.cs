using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HospitalManagementSystem.Models
{
    public class Career
    {
        [Key]
        //internal string LocationName;
        public int CareerId { get; set; }
        public string JobName { get; set; }
        public int JobId { get; set; }

        public string JobDescription { get; set; }

        //A job belongs to a specific department
        [ForeignKey("Department")]
        public int DepartmentId { get; set; }

        public virtual Department Department { get; set; }


        //A job belongs to a specific department of specific location
        [ForeignKey("Location")]
        public int LocationId { get; set; }

        public virtual Location Location { get; set; }
       
    }
        public class CareerDto
        {
            public int CareerId { get; set; }
            public string JobName { get; set; }
            public int JobId { get; set; }

            public string JobDescription { get; set; }

            public int DepartmentId { get; set; }

            public int LocationId { get; set; }
    }
    }
