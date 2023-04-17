using HospitalManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace HospitalManagementSystem.Controllers
{
    public class NewsController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static NewsController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44316/api/");
        }

        // GET: News/List
        public ActionResult List()
        {
            //OBJECTIVE: communicate with our issues api to retrieve a list of issues
            // curl https://localhost:44316/api/NewsData/ListNews

            string url = "NewsData/ListNews";
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is ");
            Debug.WriteLine(response.StatusCode);

            IEnumerable<News> News = response.Content.ReadAsAsync<IEnumerable<News>>().Result;
            Debug.WriteLine("Number of News recieved: ");
            Debug.WriteLine(News.Count());

            return View(News);
        }

        // GET: News/Details/5
        public ActionResult Details(int id)
        {
            string url = "NewsData/FindNews/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is ");
            Debug.WriteLine(response.StatusCode);

            News selectedNews = response.Content.ReadAsAsync<News>().Result;
            Debug.WriteLine("Name of issue: ");
            Debug.WriteLine(selectedNews.newsTitle);

            return View(selectedNews);
        }

        public ActionResult Error()
        {
            return View();
        }


        // GET: News/New
        public ActionResult New()
        {
            return View();
        }

        // POST: News/Create
        [HttpPost]
        public ActionResult Create(News news)
        {
            Debug.WriteLine("The jsonpayload is:");
            //Debug.WriteLine(news.newsTitle);
            //OBJECTIVE: add a new news into our system using the API
            //curl -d @news.json -H "Content-type:application/json" https://localhost:44316/api/newsData/AddNews
            string url = "NewsData/AddNews";


            string jsonpayload = jss.Serialize(news);

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

        // GET: News/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "NewsData/FindNews/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            News selectedNews = response.Content.ReadAsAsync<News>().Result;
            return View(selectedNews);
        }

        // POST: News/Update/5
        [HttpPost]
        public ActionResult Update(int id, News news)
        {
            string url = "NewsData/UpdateNews/" + id;
            string jsonpayload = jss.Serialize(news);
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

        // GET: News/DeleteConfirm/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "NewsData/FindNews/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            News selectedNews = response.Content.ReadAsAsync<News>().Result;
            return View(selectedNews);
        }

        // POST: News/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "NewsData/DeleteNews/" + id;
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
