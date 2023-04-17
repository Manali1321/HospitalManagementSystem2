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

            string url = "findcareer/"+id;
            HttpResponseMessage response = client.GetAsync(url).Result;


            //Debug.WriteLine("the response code is: ");
            // Debug.WriteLine(response.StatusCode);

            CareerDto SelectedCareer = response.Content.ReadAsAsync<CareerDto>().Result;

            //Debug.WriteLine(" Career recieved:");
            // Debug.WriteLine(SelectedCareer.CareerName);


            return View(SelectedCareer);
        }

        public ActionResult Error()
        {
            return View();
        }

        // GET: Career/New
        public ActionResult New()
        {
            return View();
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

            //the existing career information
            string url = "findcareer/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            Career SelectedCareer = response.Content.ReadAsAsync<Career>().Result;

            return View(SelectedCareer);
        }

        // POST: Career/Edit/5
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