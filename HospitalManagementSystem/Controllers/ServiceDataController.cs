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

    public class ServiceDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/ServiceData/ListService
        [HttpGet]
        public IQueryable<Service> ListServices()
        {
            return db.Services;
        }

        /// <summary>
        /// Returns all animals in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all animals in the database, including their associated species.
        /// </returns>
        /// <example>
        /// GET: api/AnimalData/ListAnimals
        /// </example>
        [HttpGet]
        [ResponseType(typeof(Service))]
        [Route("api/ServiceData/ListServicesPage/{StartIndex}/{PerPage}")]
        public IHttpActionResult ListServicesPage(int StartIndex, int PerPage)
        {
            List<Service> Services = db.Services.OrderBy(a => a.ServiceId).Skip(StartIndex).Take(PerPage).ToList();

            return Ok(Services);
        }

        // GET: api/ServiceData/FindService/5
        [ResponseType(typeof(Service))]
        [HttpGet]
        public IHttpActionResult FindService(int id)
        {
            Service service = db.Services.Find(id);
            if (service == null)
            {
                return NotFound();
            }

            return Ok(service);
        }

        [HttpGet]
        [ResponseType(typeof(Service))]
        public IHttpActionResult ListServicesForLocation(int id)
        {
            List<Service> Services = db.Services.Where(
                k => k.Locations.Any(
                    a => a.LocationId==id)
                ).ToList();
            return Ok(Services);
        }

        [HttpGet]
        [ResponseType(typeof(Service))]
        public IHttpActionResult ListOfServicesNotAtThisLocation(int id)
        {
            List<Service> Services = db.Services.Where(
                k => !k.Locations.Any(
                    a => a.LocationId==id)
                ).ToList();
            return Ok(Services);
        }



        [HttpPost]
        [Route("api/ServiceData/AssociateLocationWithService/{serviceid}/{locationid}")]
        public IHttpActionResult AssociateLocationWithService(int serviceid, int locationid)
        {

            Service SelectedService = db.Services.Include(a => a.Locations).Where(a => a.ServiceId == serviceid).FirstOrDefault();
            Location SelectedLocation = db.Locations.Find(locationid);

            if (SelectedService == null || SelectedLocation == null)
            {
                return NotFound();
            }

            Debug.WriteLine("input service id is: " + serviceid);
            Debug.WriteLine("selected service name is: " + SelectedService.ServiceName);
            Debug.WriteLine("input location id is: " + locationid);
            Debug.WriteLine("selected location name is: " + SelectedLocation.LocationName);

            SelectedService.Locations.Add(SelectedLocation);
            db.SaveChanges();

            return Ok();
        }


        [HttpPost]
        [Route("api/ServiceData/UnAssociateLocationWithService/{serviceid}/{locationid}")]
        public IHttpActionResult UnAssociateLocationWithService(int serviceid, int locationid)
        {

            Service SelectedService = db.Services.Include(a => a.Locations).Where(a => a.ServiceId == serviceid).FirstOrDefault();
            Location SelectedLocation = db.Locations.Find(locationid);

            if (SelectedService == null || SelectedLocation == null)
            {
                return NotFound();
            }

            Debug.WriteLine("input service id is: " + serviceid);
            Debug.WriteLine("selected service name is: " + SelectedService.ServiceName);
            Debug.WriteLine("input location id is: " + locationid);
            Debug.WriteLine("selected location name is: " + SelectedLocation.LocationName);


            SelectedService.Locations.Remove(SelectedLocation);
            db.SaveChanges();

            return Ok();
        }


        // POST: api/ServiceData/UpdateService/5
        [ResponseType(typeof(void))]
        [HttpPost]

        public IHttpActionResult UpdateService(int id, Service service)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != service.ServiceId)
            {
                return BadRequest();
            }

            db.Entry(service).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServiceExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/ServiceData/AddService
        [ResponseType(typeof(Service))]
        [HttpPost]

        public IHttpActionResult AddService(Service service)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Services.Add(service);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = service.ServiceId }, service);
        }

        // POST: api/ServiceData/DeleteService/5
        [ResponseType(typeof(Service))]
        [HttpPost]

        public IHttpActionResult DeleteService(int id)
        {
            Service service = db.Services.Find(id);
            if (service == null)
            {
                return NotFound();
            }

            db.Services.Remove(service);
            db.SaveChanges();

            return Ok(service);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ServiceExists(int id)
        {
            return db.Services.Count(e => e.ServiceId == id) > 0;
        }
    }
}