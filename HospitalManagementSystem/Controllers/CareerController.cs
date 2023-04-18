using HospitalManagementSystem.Models;
using HospitalManagementSystem.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.IO;
using System.Xml.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HospitalManagementSystem.Controllers
{
    public class CareerController : Controller
    {

        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static CareerController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44316/api/careerdata/");

        }

        // GET: Career/List
        public ActionResult List()
        {
            //objective: communicate with our Career data api to retrive a list of Careers
            //curl: https://localhost:44316/api/careerdata/listcareers

            string url = "listcareers";
            HttpResponseMessage response = client.GetAsync(url).Result;


            Debug.WriteLine("the response code is: ");
            Debug.WriteLine(response.StatusCode);

            IEnumerable<CareerDto> careers = response.Content.ReadAsAsync<IEnumerable<CareerDto>>().Result;

            Debug.WriteLine("Number of Careers recieved:");
            Debug.WriteLine(careers.Count());

            return View(careers);
        }

        // GET: Career/Details/5
        public ActionResult Details(int id)

        {
            //objective: communicate with our career data api to retrive one career
            //curl: https://localhost:44316/api/careerdata/findcareer/{id}
            CareerDetails ViewModel = new CareerDetails();

            string url = "findcareer/"+id;
            HttpResponseMessage response = client.GetAsync(url).Result;


            //Debug.WriteLine("the response code is: ");
            // Debug.WriteLine(response.StatusCode);

            CareerDto SelectedCareer = response.Content.ReadAsAsync<CareerDto>().Result;

            //Debug.WriteLine(" Career recieved:");
            // Debug.WriteLine(SelectedCareer.CareerName);

            ViewModel.SelectedCareer= SelectedCareer;

            return View(ViewModel);
        }

        public ActionResult Error()
        {
            return View();
        }

        // GET: Career/New
        public async Task<ActionResult> New()
        {
            string url = "https://localhost:44316/api/departmentdata/listdepartments";
            HttpResponseMessage response = await client.GetAsync(url);
            string json = await response.Content.ReadAsStringAsync();
            IEnumerable<DepartmentDto> DepartmentOptions = JsonConvert.DeserializeObject<IEnumerable<DepartmentDto>>(json);

            string url1 = "https://localhost:44316/api/locationdata/listlocations";
            HttpResponseMessage response1 = await client.GetAsync(url1);
            string jsonn = await response1.Content.ReadAsStringAsync();
            IEnumerable<LocationDto> LocationOptions = JsonConvert.DeserializeObject<IEnumerable<LocationDto>>(jsonn);

            var viewModel = new AddCareers
            {
                DepartmentOptions = DepartmentOptions.ToList(),
                LocationOptions = LocationOptions.ToList()
            };
            return View(viewModel);
        }


        // POST: Career/Create
        [HttpPost]
        public ActionResult Create(Career career)
        {
            Debug.WriteLine("the json payload is:");
            // Debug.WriteLine(Career.CareerName);
            //objective: add a new Career into our system using the API

            //curl -H "Content-Type:appliation/json" -d career.json https://localhost:44316/api/careerdata/addcareer
            string url = "addcareer";


            string jsonpayload = jss.Serialize(career);

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

        // GET: Career/Edit/5
        public ActionResult Edit(int id)
        {
            UpdateCareer ViewModel = new UpdateCareer();

            //the existing career information
            string url = "findcareer/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            CareerDto SelectedCareer = response.Content.ReadAsAsync<CareerDto>().Result;

            string url2 = "https://localhost:44316/api/departmentdata/listdepartments";
            response = client.GetAsync(url2).Result;
            IEnumerable<DepartmentDto> DepartmentOptions = response.Content.ReadAsAsync<IEnumerable<DepartmentDto>>().Result;

            string url1 = "https://localhost:44316/api/locationdata/listlocations";
            response = client.GetAsync(url1).Result;
            IEnumerable<LocationDto> LocationOptions = response.Content.ReadAsAsync<IEnumerable<LocationDto>>().Result;



            ViewModel.SelectedCareer = SelectedCareer;
            ViewModel.DepartmentOptions = DepartmentOptions;
            ViewModel.LocationOptions = LocationOptions;


            return View(ViewModel);
        }

        // POST: Career/Update/5
        [HttpPost]
        public ActionResult Update(int id, Career career)
        {

            string url = "updatecareer/"+id;
            string jsonpayload = jss.Serialize(career);
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

        // GET: Career/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "findcareer/"+id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            CareerDto SelectedCareer = response.Content.ReadAsAsync<CareerDto>().Result;
            return View(SelectedCareer);
        }

        // POST: Career/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, Career career)
        {
            string url = "deletecareer/"+id;
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