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
    public class DoctorController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static DoctorController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44316/api/");

        }

        // GET: Doctor/List
        public ActionResult List()
        {
            //objective: communicate with our Doctor data api to retrive a list of Doctors
            //curl: https://localhost:44316/api/DoctorData/ListDoctors

            string url = "DoctorData/ListDoctors";
            HttpResponseMessage response = client.GetAsync(url).Result;


            Debug.WriteLine("the response code is: ");
            Debug.WriteLine(response.StatusCode);

            IEnumerable<DoctorDto> doctors = response.Content.ReadAsAsync<IEnumerable<DoctorDto>>().Result;

            Debug.WriteLine("Number of Doctors recieved:");
            Debug.WriteLine(doctors.Count());

            return View(doctors);
        }

        // GET: Doctor/Details/5
        public ActionResult Details(int id)
        {
            //objective: communicate with our doctor data api to retrive one doctor
            //curl: https://localhost:44316/api/DoctorData/FindDoctor/{id}

            string url = "DoctorData/FindDoctor/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;


            //Debug.WriteLine("the response code is: ");
            // Debug.WriteLine(response.StatusCode);

            DoctorDto SelectedDoctor = response.Content.ReadAsAsync<DoctorDto>().Result;

            //Debug.WriteLine(" Doctor recieved:");
            // Debug.WriteLine(SelectedDoctor.DoctorFName);


            return View(SelectedDoctor);
        }

        public ActionResult Error()
        {
            return View();
        }

        // GET: Doctor/New
        public ActionResult New()
        {
            //objective: communicate with our Department data api to retrive a list of Departments
            //curl: https://localhost:44316/api/departmentdata/listdepartments

            string url = "departmentdata/listdepartments";
            HttpResponseMessage response = client.GetAsync(url).Result;


            Debug.WriteLine("the response code is: ");
            Debug.WriteLine(response.StatusCode);

            IEnumerable<Department> departments = response.Content.ReadAsAsync<IEnumerable<Department>>().Result;

            Debug.WriteLine("Number of Departments recieved:");
            Debug.WriteLine(departments.Count());

            return View(departments);
        }

        // POST: Doctor/Create
        [HttpPost]
        public ActionResult Create(Doctor doctor)
        {
            Debug.WriteLine("the json payload is:");
            // Debug.WriteLine(Doctor.DoctorFName);
            //objective: add a new Doctor into our system using the API

            //curl -H "Content-Type:appliation/json" -d doctor.json https://localhost:44316/api/doctordata/adddoctor
            string url = "DoctorData/AddDoctor";


            string jsonpayload = jss.Serialize(doctor);

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

        // GET: Doctor/Edit/5
        public ActionResult Edit(int id)
        {
            DetailsDoctor ViewModel = new DetailsDoctor();
            //the existing doctor information
            string url = "DoctorData/FindDoctor/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            DoctorDto SelectedDoctor = response.Content.ReadAsAsync<DoctorDto>().Result;
            ViewModel.SelectedDoctor = SelectedDoctor;


            url = "departmentdata/listdepartments";
            response = client.GetAsync(url).Result;


            Debug.WriteLine("the response code is: ");
            Debug.WriteLine(response.StatusCode);

            IEnumerable<Department> departments = response.Content.ReadAsAsync<IEnumerable<Department>>().Result;

            ViewModel.DepartmentList = departments;

            return View(ViewModel);
        }

        // POST: Doctor/Edit/5
        [HttpPost]
        public ActionResult Update(int id, Doctor doctor)
        {
            string url = "DoctorData/UpdateDoctor/" + id;
            string jsonpayload = jss.Serialize(doctor);
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

        // GET: Doctor/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "DoctorData/FindDoctor/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            DoctorDto SelectedDoctor = response.Content.ReadAsAsync<DoctorDto>().Result;
            return View(SelectedDoctor);
        }

        // POST: Doctor/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, Doctor doctor)
        {
            string url = "DoctorData/DeleteDoctor/" + id;
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
