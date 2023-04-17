using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
using System.Web.Mvc;
using System.Net.Http;
using HospitalManagementSystem.Models;
using HospitalManagementSystem.Models.ViewModels;
using System.Web.Script.Serialization;
using HospitalManagementSystem.Migrations;

namespace HospitalManagementSystem.Controllers
{
    public class LocationController : Controller
    {

        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static LocationController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44316/api/");

        }

        // GET: Location/List?PageNum={PageNum}
        public ActionResult List(int PageNum = 0)
        {
            //objective: communicate with our location data api to retrive a list of locations
            //curl: https://localhost:44316/api/locationdata/listlocations

            LocationList ViewModel = new LocationList();

            string url = "locationdata/listlocations";
            HttpResponseMessage response = client.GetAsync(url).Result;


            Debug.WriteLine("the response code is: ");
            Debug.WriteLine(response.StatusCode);

            IEnumerable<Location> locations = response.Content.ReadAsAsync<IEnumerable<Location>>().Result;

            Debug.WriteLine("Number of Locations recieved:");
            Debug.WriteLine(locations.Count());



            // -- Start of Pagination Algorithm --

            // Find the total number of locations
            int LocationCount = locations.Count();
            // Number of locations to display per page
            int PerPage = 4;
            // Determines the maximum number of pages (rounded up), assuming a page 0 start.
            int MaxPage = (int)Math.Ceiling((decimal)LocationCount / PerPage) - 1;

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
            url = "LocationData/ListLocationsPage/" + StartIndex + "/" + PerPage;
            response = client.GetAsync(url).Result;

            // Retrieve the response of the HTTP Request
            IEnumerable<Location> SelectedLocationsPage = response.Content.ReadAsAsync<IEnumerable<Location>>().Result;

            ViewModel.Locations = SelectedLocationsPage;

            return View(ViewModel);
        }

        // GET: Location/Details/5
        public ActionResult Details(int id)
        {
            LocationDetails ViewModel = new LocationDetails();
            //objective: communicate with our location data api to retrive one location
            //curl: https://localhost:44316/api/locationdata/findloation/{id}

            string url = "locationdata/findlocation/"+id;
            HttpResponseMessage response = client.GetAsync(url).Result;


            //Debug.WriteLine("the response code is: ");
            // Debug.WriteLine(response.StatusCode);

            Location SelectedLocation = response.Content.ReadAsAsync<Location>().Result;
            ViewModel.SelectedLocation = SelectedLocation;
            //Debug.WriteLine(" Location recieved:");
            // Debug.WriteLine(SelectedLocation.LocationName);

            //show associated locations with this department
            url = "departmentdata/listdepartmentsforlocation/"+id;
            response = client.GetAsync(url).Result;
            IEnumerable<Department> RelatedDepartments = response.Content.ReadAsAsync<IEnumerable<Department>>().Result;

            ViewModel.RelatedDepartments = RelatedDepartments;

            //show unassociated locations with this department
            url = "departmentdata/listofdepartmentsnotatthislocation/"+id;
            response = client.GetAsync(url).Result;
            IEnumerable<Department> AvailableDepartments = response.Content.ReadAsAsync<IEnumerable<Department>>().Result;

            ViewModel.AvailableDepartments = AvailableDepartments;

            //show associated careers with this location
            //url = "careerdata/listcareersforlocation/"+id;
            //response = client.GetAsync(url).Result;
            //IEnumerable<Career> RelatedCareers = response.Content.ReadAsAsync<IEnumerable<Career>>().Result;

            //ViewModel.RelatedCareers = RelatedCareers;


            //show unassociated careers with this location
           // url = "careerdata/listcareersnotatthislocation/"+id;
            //response = client.GetAsync(url).Result;
            //IEnumerable<Career> AvailableCareers = response.Content.ReadAsAsync<IEnumerable<Career>>().Result;

            //ViewModel.AvailableCareers = AvailableCareers;


            //show associated locations with this department
            url = "servicedata/listservicesforlocation/"+id;
            response = client.GetAsync(url).Result;
            IEnumerable<Service> RelatedServices = response.Content.ReadAsAsync<IEnumerable<Service>>().Result;

            ViewModel.RelatedServices = RelatedServices;

            //show unassociated locations with this department
            url = "servicedata/listofservicesnotatthislocation/"+id;
            response = client.GetAsync(url).Result;
            IEnumerable<Service> AvailableServices = response.Content.ReadAsAsync<IEnumerable<Service>>().Result;

            ViewModel.AvailableServices = AvailableServices;


            return View(ViewModel);


        }



        //POST: Location/Associate/{locationid}
        [HttpPost]
        public ActionResult Associate(int id, int departmentid)
        {
            Debug.WriteLine("Attempting to associate department :"+id+ " with location "+departmentid);

            //call our api to associate location with department
            string url = "locationdata/associatedepartmentwithlocation/" + id + "/" + departmentid;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("Details/" + id);
        }


        //Get: Location/UnAssociate/{id}?DepartmentId={departmentId}
        [HttpGet]
        public ActionResult UnAssociate(int id, int DepartmentId)
        {
            Debug.WriteLine("Attempting to unassociate department :" + id + " with location: " + DepartmentId);

            //call our api to associate location with department
            string url = "locationdata/unassociatedepartmentwithlocation/" + id + "/" + DepartmentId;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("Details/" + id);
        }



        //POST: Location/AssociateService/{locationid}
        [HttpPost]
        public ActionResult AssociateService(int id, int serviceid)
        {
            Debug.WriteLine("Attempting to associate service :"+id+ " with location "+ serviceid);

            //call our api to associate location with service
            string url = "locationdata/associateservicewithlocation/" + id + "/" + serviceid;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("Details/" + id);
        }


        //Get: Location/UnAssociateService/{id}?ServiceId={ServiceId}
        [HttpGet]
        public ActionResult UnAssociateService(int id, int ServiceId)
        {
            Debug.WriteLine("Attempting to unassociate service :" + id + " with location: " + ServiceId);

            //call our api to associate location with service
            string url = "locationdata/unassociateservicewithlocation/" + id + "/" + ServiceId;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("Details/" + id);
        }



        public ActionResult Error()
        {
            return View();
        }

        // GET: Location/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Location/Create
        [HttpPost]
        public ActionResult Create(Location location)
        {
            Debug.WriteLine("the json payload is:");
            // Debug.WriteLine(location.LocationName);
            //objective: add a new loation into our system using the API

            //curl -H "Content-Type:appliation/json" -d location.json https://localhost:44316/api/locationdata/addlocation
            string url = "locationdata/addlocation";


            string jsonpayload = jss.Serialize(location);

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

        // GET: Location/Edit/5
        public ActionResult Edit(int id)
        {
            //the existing location information
            string url = "locationdata/findlocation/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            Location SelectedLocation = response.Content.ReadAsAsync<Location>().Result;

            return View(SelectedLocation);
        }

        // POST: Location/Edit/5
        [HttpPost]
        public ActionResult Update(int id, Location location)
        {
            string url = "locationdata/updatelocation/"+id;
            string jsonpayload = jss.Serialize(location);
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

        // GET: Location/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "locationdata/findlocation/"+id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            Location SelectedLocation = response.Content.ReadAsAsync<Location>().Result;
            return View(SelectedLocation);
        }

        // POST: Location/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "locationdata/deletelocation/"+id;
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