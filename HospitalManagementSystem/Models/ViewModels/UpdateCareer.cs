using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalManagementSystem.Models.ViewModels
{
    public class UpdateCareer
    {
        public CareerDto SelectedCareer { get; set; }
        public IEnumerable<DepartmentDto> DepartmentOptions { get; set; }
        public IEnumerable<LocationDto> LocationOptions { get; set; }
    }
}