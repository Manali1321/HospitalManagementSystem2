using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HospitalManagementSystem.Models
{
    public class Doctor
    {
      
            public int DoctorId { get; set; }
            public string DoctorFName { get; set; }
            public string DoctorLName { get; set; }
            public string Speciality { get; set; }
            public string Email { get; set; }
            public string ContactNumber { get; set; }

            //A doctor belongs to a location
            [ForeignKey("Department")]
            public int DepartmentId { get; set; }

            public virtual Department Department { get; set; }


        }

        public class DoctorDto
        {
            public int DoctorId { get; set; }
            public string DoctorFName { get; set; }
            public string DoctorLName { get; set; }
            public string Speciality { get; set; }
            public string Email { get; set; }
            public string ContactNumber { get; set; }
            public string DepartmentName { get; set; }
        }
}
