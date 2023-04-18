using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HospitalManagementSystem.Controllers;
using HospitalManagementSystem.Models;
using HospitalManagementSystem.Models.ViewModels;

namespace HospitalManagementSystem.Models.ViewModels
{
    public class AddCareers
    {
        public List<DepartmentDto>DepartmentOptions { get; set; }
        public List<LocationDto>LocationOptions { get; set; }
    }
}