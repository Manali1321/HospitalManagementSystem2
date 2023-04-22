using HospitalManagementSystem.Models.ViewModels;
using HospitalManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace HospitalManagementSystem.Controllers
{
    public class AppointmentController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static AppointmentController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44316/api/");

        }
        // GET: Appointment/List
        public ActionResult List()
        {
            //objective: communicate with appointment data api to retrive a list of Appointments
            //curl: https://localhost:44316/api/AppointmentData/ListAppointments

            string url = "AppointmentData/ListAppointments";
            HttpResponseMessage response = client.GetAsync(url).Result;


            Debug.WriteLine("the response code is: ");
            Debug.WriteLine(response.StatusCode);

            IEnumerable<AppointmentDto> appointments = response.Content.ReadAsAsync<IEnumerable<AppointmentDto>>().Result;

            Debug.WriteLine("Number of Appointments recieved:");
            Debug.WriteLine(appointments.Count());

            return View(appointments);
        }

        // GET: Appointment/Details/5
        public ActionResult Details(int id)
        {
            //objective: communicate with our Appointment data api to retrive one Appointment
            //curl: https://localhost:44316/api/AppointmentData/FindAppointment/{id}

            string url = "AppointmentData/FindAppointment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;


            //Debug.WriteLine("the response code is: ");
            // Debug.WriteLine(response.StatusCode);

            AppointmentDto SelectedAppointment = response.Content.ReadAsAsync<AppointmentDto>().Result;

            //Debug.WriteLine(" Appointments recieved:");
            // Debug.WriteLine(SelectedAppointment.AppointmentId);


            return View(SelectedAppointment);
        }

        // GET: Appointment/New
        public ActionResult New()
        {
            string url = "DoctorData/ListDoctors";
            HttpResponseMessage response = client.GetAsync(url).Result;


            Debug.WriteLine("the response code is: ");
            Debug.WriteLine(response.StatusCode);

            IEnumerable<DoctorDto> doctors = response.Content.ReadAsAsync<IEnumerable<DoctorDto>>().Result;

            Debug.WriteLine("Number of Doctors recieved:");
            Debug.WriteLine(doctors.Count());

            return View(doctors);
        }

        // POST: Appointment/Create
        [HttpPost]
        public ActionResult Create(Appointment appointment)
        {
            Debug.WriteLine("the json payload is:");
            // Debug.WriteLine(Appointment.AppointmentId);
            //objective: add a new Appointment into our system using the API

            //curl -H "Content-Type:appliation/json" -d doctor.json https://localhost:44316/api/AppointmentData/AddAppointment
            string url = "AppointmentData/AddAppointment";


            string jsonpayload = jss.Serialize(appointment);

            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Appointment/Edit/5
        public ActionResult Edit(int id)
        {
            AppointmentDetails ViewModel = new AppointmentDetails();
            //the existing appointment information
            string url = "AppointmentData/FindAppointment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            AppointmentDto SelectedAppointment = response.Content.ReadAsAsync<AppointmentDto>().Result;
            ViewModel.SelectedAppointment = SelectedAppointment;

            //the existing doctor information
            url = "DoctorData/ListDoctor/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<DoctorDto> DoctorList = response.Content.ReadAsAsync<IEnumerable<DoctorDto>>().Result;
            ViewModel.DoctorList = DoctorList;
            return View(ViewModel);
        }

        // POST: Appointment/Update/5
        [HttpPost]
        public ActionResult Update(int id, Appointment appointment)
        {
            string url = "AppointmentData/UpdateAppointment/" + id;
            string jsonpayload = jss.Serialize(appointment);
            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        public ActionResult Error()
        {
            return View();
        }
        // GET: Appointment/DeleteConfirm/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "AppointmentData/FindAppointment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            AppointmentDto SelectedAppointment = response.Content.ReadAsAsync<AppointmentDto>().Result;
            return View(SelectedAppointment);
        }

        // POST: Appointment/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, Appointment appointment)
        {
            string url = "AppointmentData/DeleteAppointment/" + id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
    }
}

