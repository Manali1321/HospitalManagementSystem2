using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalManagementSystem.Models.ViewModels
{
    public class DetailsDoctor
    {
        public DoctorDto SelectedDoctor { get; set; }
        public IEnumerable<Department> DepartmentList { get; set; }
    }
}