using HospitalManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using HospitalManagementSystem.Models.ViewModels;
using System.Runtime.InteropServices;
using HospitalManagementSystem.Migrations;

namespace HospitalManagementSystem.Controllers
{
    public class ServiceController : Controller
    {

        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static ServiceController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44316/api/");

        }

        // GET: Service/List?PageNum={PageNum}
        public ActionResult List(int PageNum = 0)
        {
            //objective: communicate with our Service data api to retrive a list of Services
            //curl: https://localhost:44316/api/servicedata/listservices

            ServiceList ViewModel = new ServiceList();

            string url = "servicedata/listservices";
            HttpResponseMessage response = client.GetAsync(url).Result;


            Debug.WriteLine("the response code is: ");
            Debug.WriteLine(response.StatusCode);

            IEnumerable<Service> services = response.Content.ReadAsAsync<IEnumerable<Service>>().Result;

            // Debug.WriteLine("Number of Services recieved:");
            // Debug.WriteLine(services.Count());


            // -- Start of Pagination Algorithm --

            // Find the total number of services
            int ServiceCount = services.Count();
            // Number of services to display per page
            int PerPage = 4;
            // Determines the maximum number of pages (rounded up), assuming a page 0 start.
            int MaxPage = (int)Math.Ceiling((decimal)ServiceCount / PerPage) - 1;

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
            url = "ServiceData/ListServicesPage/" + StartIndex + "/" + PerPage;
            response = client.GetAsync(url).Result;

            // Retrieve the response of the HTTP Request
            IEnumerable<Service> SelectedServicesPage = response.Content.ReadAsAsync<IEnumerable<Service>>().Result;

            ViewModel.Services = SelectedServicesPage;

            return View(ViewModel);
        }

        // GET: Service/Details/5
        public ActionResult Details(int id)
        {
            ServiceDetails ViewModel = new ServiceDetails();
            //objective: communicate with our service data api to retrive one service
            //curl: https://localhost:44316/api/servicedata/findservice/{id}

            string url = "servicedata/findservice/"+id;
            HttpResponseMessage response = client.GetAsync(url).Result;


            //Debug.WriteLine("the response code is: ");
            // Debug.WriteLine(response.StatusCode);

            Service SelectedService = response.Content.ReadAsAsync<Service>().Result;

            //Debug.WriteLine(" Service recieved:");
            // Debug.WriteLine(SelectedService.ServiceName);

            ViewModel.SelectedService = SelectedService;

            //show all locations who provide these services
            url = "locationdata/listlocationsforservice/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<Location> AtLocations = response.Content.ReadAsAsync<IEnumerable<Location>>().Result;

            ViewModel.AtLocations = AtLocations;

            url = "locationdata/listlocationsnothavingthisservice/"+id;
            response = client.GetAsync(url).Result;
            IEnumerable<Location> AvailableLocations = response.Content.ReadAsAsync<IEnumerable<Location>>().Result;

            ViewModel.AvailableLocations = AvailableLocations;


            return View(ViewModel);
        }



        //POST: Service/AssociateLocation/{serviceid}
        [HttpPost]
        public ActionResult AssociateLocation(int id, int locationid)
        {
            Debug.WriteLine("Attempting to associate location :"+id+ " with service "+ locationid);

            //call our api to associate  service with location
            string url = "servicedata/associatelocationwithservice/" + id + "/" + locationid;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("Details/" + id);
        }


        //Get: Service/UnAssociateLocation/{id}?LocationId={LocationId}
        [HttpGet]
        public ActionResult UnAssociateLocation(int id, int LocationId)
        {
            Debug.WriteLine("Attempting to unassociate location :" + id + " with service: " + LocationId);

            //call our api to associate service with location
            string url = "servicedata/unassociatelocationwithservice/" + id + "/" + LocationId;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("Details/" + id);
        }


        public ActionResult Error()
        {
            return View();
        }

        // GET: Service/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Service/Create
        [HttpPost]

        public ActionResult Create(Service service)
        {
            Debug.WriteLine("the json payload is:");
            // Debug.WriteLine(Service.ServiceName);
            //objective: add a new Service into our system using the API

            //curl -H "Content-Type:appliation/json" -d service.json https://localhost:44316/api/servicedata/addservice
            string url = "servicedata/addservice";


            string jsonpayload = jss.Serialize(service);

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

        // GET: Service/Edit/5
        public ActionResult Edit(int id)
        {

            //the existing service information
            string url = "servicedata/findservice/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            Service SelectedService = response.Content.ReadAsAsync<Service>().Result;

            return View(SelectedService);
        }

        // POST: Service/Edit/5
        [HttpPost]

        public ActionResult Update(int id, Service service)
        {

            string url = "servicedata/updateservice/"+id;
            string jsonpayload = jss.Serialize(service);
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

        // GET: Service/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "servicedata/findservice/"+id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            Service SelectedService = response.Content.ReadAsAsync<Service>().Result;
            return View(SelectedService);
        }

        // POST: Service/Delete/5
        [HttpPost]

        public ActionResult Delete(int id, Service service)
        {
            string url = "servicedata/deleteservice/"+id;
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