
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using System.Net.Http;
using HospitalManagementSystem.Models;
using HospitalManagementSystem.Models.ViewModels;
using System.Web.Script.Serialization;

namespace HospitalManagementSystem.Controllers
{
    public class DepartmentController : Controller
    {

        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static DepartmentController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44316/api/");

        }

        // GET: Department/List?PageNum={PageNum}
        public ActionResult List(int PageNum = 0)
        {
            //objective: communicate with our Department data api to retrive a list of Departments
            //curl: https://localhost:44316/api/departmentdata/listdepartments

            DepartmentList ViewModel = new DepartmentList();

            string url = "departmentdata/listdepartments";
            HttpResponseMessage response = client.GetAsync(url).Result;


            //Debug.WriteLine("the response code is: ");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<Department> departments = response.Content.ReadAsAsync<IEnumerable<Department>>().Result;

            //Debug.WriteLine("Number of Departments recieved:");
            //Debug.WriteLine(departments.Count());\


            // -- Start of Pagination Algorithm --

            // Find the total number of departments
            int DepartmentCount = departments.Count();
            // Number of departments to display per page
            int PerPage = 4;
            // Determines the maximum number of pages (rounded up), assuming a page 0 start.
            int MaxPage = (int)Math.Ceiling((decimal)DepartmentCount / PerPage) - 1;

            // Lower boundary for Max Page
            if (MaxPage < 0) MaxPage = 0;
            // Lower boundary for Page Number
            if (PageNum < 0) PageNum = 0;
            // Upper Bound for Page Number
            if (PageNum > MaxPage) PageNum = MaxPage;

            // The Record Index of our Page Start
            int StartIndex = PerPage * PageNum;

            //Helps us generate the HTML which shows "Page 1 of ..." on the list view
            ViewData["PageNum"] = PageNum;
            ViewData["PageSummary"] = " " + (PageNum + 1) + " of " + (MaxPage + 1) + " ";

            // -- End of Pagination Algorithm --

            //Send another request to get the page slice of the full list
            url = "DepartmentData/ListDepartmentsPage/" + StartIndex + "/" + PerPage;
            response = client.GetAsync(url).Result;

            // Retrieve the response of the HTTP Request
            IEnumerable<Department> SelectedDepartmentsPage = response.Content.ReadAsAsync<IEnumerable<Department>>().Result;

            ViewModel.Departments = SelectedDepartmentsPage;

            return View(ViewModel);
        }

        // GET: Department/Details/5
        public ActionResult Details(int id)

        {
            DepartmentDetails ViewModel = new DepartmentDetails();
            //objective: communicate with our department data api to retrive one department
            //curl: https://localhost:44316/api/departmentdata/finddepartment/{id}

            string url = "departmentdata/finddepartment/"+id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            Department SelectedDepartment = response.Content.ReadAsAsync<Department>().Result;
            ViewModel.SelectedDepartment = SelectedDepartment;

            //show associated locations with this department
            url = "locationdata/listlocationsfordepartment/"+id;
            response = client.GetAsync(url).Result;
            IEnumerable<Location> RelatedLocations = response.Content.ReadAsAsync<IEnumerable<Location>>().Result;

            ViewModel.RelatedLocations = RelatedLocations;


            url = "locationdata/listlocationsnothavingthisdepartment/"+id;
            response = client.GetAsync(url).Result;
            IEnumerable<Location> AvailableLocations = response.Content.ReadAsAsync<IEnumerable<Location>>().Result;

            ViewModel.AvailableLocations = AvailableLocations;


            //show associated careers with this department
            url = "careerdata/listcareersfordepartment/"+id;
            response = client.GetAsync(url).Result;
            IEnumerable<Career> RelatedCareers = response.Content.ReadAsAsync<IEnumerable<Career>>().Result;

            ViewModel.RelatedCareers = RelatedCareers;

            // url = "locationdata/listoflocationsnothavingthisdepartment/" + id;
            // response = client.GetAsync(url).Result;
            //IEnumerable<Location> AvailableLocations = response.Content.ReadAsAsync<IEnumerable<Location>>().Result;

            //ViewModel.AvailableLocations = AvailableLocations;

            return View(ViewModel);

        }


        //POST: Department/Associate/{departmentid}
        [HttpPost]
        public ActionResult Associate(int id, int LocationId)
        {
            Debug.WriteLine("Attempting to associate location :"+id+ " with department "+LocationId);

            //call our api to associate department with location
            string url = "departmentdata/associatelocationwithdepartment/" + id + "/" + LocationId;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("Details/" + id);
        }


        //Get: Department/UnAssociate/{id}?LocationId={locationID}
        [HttpGet]
        public ActionResult UnAssociate(int id, int LocationId)
        {
            Debug.WriteLine("Attempting to unassociate location:" + id + " with department: " + LocationId);

            //call our api to associate department with location
            string url = "departmentdata/unassociatelocationwithdepartment/" + id + "/" + LocationId;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("Details/" + id);
        }



        public ActionResult Error()
        {
            return View();
        }

        // GET: Department/New
        public ActionResult New()
        {
            //information about all locations in the system.
            //GET api/locationdata/listlocations

            string url = "locationdata/listlocations";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<Location> LocationOptions = response.Content.ReadAsAsync<IEnumerable<Location>>().Result;

            return View(LocationOptions);

        }

        // POST: Department/Create
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(Department department)
        {
            Debug.WriteLine("the json payload is:");
            // Debug.WriteLine(Department.DepartmentName);
            //objective: add a new Department into our system using the API

            //curl -H "Content-Type:appliation/json" -d department.json https://localhost:44316/api/departmentdata/adddepartment
            string url = "departmentdata/adddepartment";


            string jsonpayload = jss.Serialize(department);

            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType= "application/json";

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

        // GET: Department/Edit/5
        public ActionResult Edit(int id)
        {

            UpdateDepartment ViewModel = new UpdateDepartment();
            //the existing department information
            string url = "departmentdata/finddepartment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            Department SelectedDepartment = response.Content.ReadAsAsync<Department>().Result;
            ViewModel.SelectedDepartment = SelectedDepartment;

            // all locations to choose from when updating this department
            //the existing department information
            url = "locationdata/listlocations/";
            response = client.GetAsync(url).Result;
            IEnumerable<Location> LocationOptions = response.Content.ReadAsAsync<IEnumerable<Location>>().Result;

            ViewModel.LocationOptions = LocationOptions;

            return View(ViewModel);
        }

        // POST: Department/Edit/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Update(int id, Department department)
        {

            string url = "departmentdata/updatedepartment/"+id;
            string jsonpayload = jss.Serialize(department);
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

        // GET: Department/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "departmentdata/finddepartment/"+id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            Department SelectedDepartment = response.Content.ReadAsAsync<Department>().Result;
            return View(SelectedDepartment);
        }

        // POST: Department/Delete/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id, Department department)
        {
            string url = "departmentdata/deletedepartment/"+id;
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
