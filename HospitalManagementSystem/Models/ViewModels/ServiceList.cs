using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalManagementSystem.Models.ViewModels
{
    public class ServiceList
    {
        public IEnumerable<Service> Services { get; set; }
    }
}