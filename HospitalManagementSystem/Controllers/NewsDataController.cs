using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using HospitalManagementSystem.Models;

namespace HospitalManagementSystem.Controllers
{
    public class NewsDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        /// <summary>
        /// Returns all news in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all news in the system
        /// </returns>
        /// <example>
        /// GET: api/NewsData/ListNews
        /// </example>
        [HttpGet]
        public IQueryable<News> ListNews()
        {
            return db.News;
        }

        /// <summary>
        /// Find particular news in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: Find particular news in the system
        /// </returns>
        /// <example>
        /// GET: api/NewsData/FindNews/5
        /// </example>
        [ResponseType(typeof(News))]
        [HttpGet]
        public IHttpActionResult FindNews(int id)
        {
            News news = db.News.Find(id);
            if (news == null)
            {
                return NotFound();
            }

            return Ok(news);
        }

        /// <summary>
        /// Update particular news in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: Update particular news in the system
        /// </returns>
        /// <example>
        /// GET: api/NewsData/UpdateNews/5
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateNews(int id, News news)
        {
            Debug.WriteLine("I have reached the update news method.");
            if (!ModelState.IsValid)
            {
                Debug.WriteLine("Model state is invalid");
                return BadRequest(ModelState);
            }

            if (id != news.newsID)
            {
                Debug.WriteLine("ID mismatched");
                return BadRequest();
            }

            db.Entry(news).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NewsExists(id))
                {
                    Debug.WriteLine("News does not exist");
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            Debug.WriteLine("None of the conditions triggered");
            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Add news in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: Add news in the system
        /// </returns>
        /// <example>
        /// GET: api/NewsData/AddNews
        /// </example>
        [ResponseType(typeof(News))]
        [HttpPost]
        public IHttpActionResult AddNews(News news)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.News.Add(news);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = news.newsID }, news);
        }

        /// <summary>
        /// Delete particular news in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: delete particular news in the system
        /// </returns>
        /// <example>
        /// GET: api/NewsData/DeleteNews/5
        /// </example>
        [ResponseType(typeof(News))]
        public IHttpActionResult DeleteNews(int id)
        {
            News news = db.News.Find(id);
            if (news == null)
            {
                return NotFound();
            }

            db.News.Remove(news);
            db.SaveChanges();

            return Ok(news);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool NewsExists(int id)
        {
            return db.News.Count(e => e.newsID == id) > 0;
        }
    }
}