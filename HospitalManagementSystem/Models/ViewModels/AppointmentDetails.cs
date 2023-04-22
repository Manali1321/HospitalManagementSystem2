using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalManagementSystem.Models.ViewModels
{
    public class AppointmentDetails
    {
        public AppointmentDto SelectedAppointment { get; set; }
        public IEnumerable<DoctorDto> DoctorList { get; set; }
    }
}