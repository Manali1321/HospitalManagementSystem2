using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalManagementSystem.Models
{
    public class Donation
    {
        public int donationID { get; set; }

        public string firstName { get; set; }

        public string lastName { get; set; }

        public int amount { get; set; }
        
        public string date { get; set; }
    }
}