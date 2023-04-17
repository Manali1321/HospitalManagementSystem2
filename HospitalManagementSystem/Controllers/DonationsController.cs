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
    public class DonationsController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static DonationsController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44316/api/DonationsData/");
        }


        // GET: Donations/List
        public ActionResult List()
        {
            string url = "ListDonations";
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is ");
            Debug.WriteLine(response.StatusCode);

            IEnumerable<Donation> donations = response.Content.ReadAsAsync<IEnumerable<Donation>>().Result;
            Debug.WriteLine(message: "Number of donations recieved: ");
            Debug.WriteLine(donations.Count());

            return View(donations);
        }

        // GET: Donations/Details/5
        public ActionResult Details(int id)
        {
            string url = "FindDonation/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is ");
            Debug.WriteLine(response.StatusCode);

            Donation selectedDonation = response.Content.ReadAsAsync<Donation>().Result;
            Debug.WriteLine("Name of Doner: ");
            Debug.WriteLine(selectedDonation.firstName);

            return View(selectedDonation);
        }

        public ActionResult Error()
        {
            return View();
        }

        // GET: Donations/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Donations/Create
        [HttpPost]
        public ActionResult Create(Donation donations)
        {
            Debug.WriteLine("The jsonpayload is:");
            //Debug.WriteLine(donations.firstName);
            //OBJECTIVE: add a new donation into our system using the API
            //curl -d @donations.json -H "Content-type:application/json" https://localhost:44316/api/DonationsData/AddDonation
            string url = "AddDonation";

            string jsonpayload = jss.Serialize(donations);

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

        // GET: Donations/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "FindDonation/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            Donation selectedDonation = response.Content.ReadAsAsync<Donation>().Result;
            return View(selectedDonation);
        }

        // POST: Donations/Update/5
        [HttpPost]
        public ActionResult Update(int id, Donation donations)
        {
            string url = "UpdateDonation/" + id;
            string jsonpayload = jss.Serialize(donations);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }

        }

        // GET: Donations/DeleteConfirm/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "FindDonation/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            Donation selectedDonation = response.Content.ReadAsAsync<Donation>().Result;
            return View(selectedDonation);
        }

        // POST: Donations/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "DeleteDonation/" + id;
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
