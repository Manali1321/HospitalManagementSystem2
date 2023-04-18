using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HospitalManagementSystem.Controllers;
using HospitalManagementSystem.Models.ViewModels;

namespace HospitalManagementSystem.Models.ViewModels
{
    public class AddCareers
    {
        public IEnumerable<DepartmentDto>DepartmentOptions { get; set; }
        public IEnumerable<LocationDto>LocationOptions { get; set; }
    }
}