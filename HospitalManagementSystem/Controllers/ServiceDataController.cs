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
        /// Returns all service. on one page it will be 4 list of service
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all list of services. one page it will contain 4 list of service and automatic another page will be added when contain increase.
        /// </returns>
        /// <example>
        /// GET: api/ServiceData/ListServices
        /// </example>
        [HttpGet]
        [ResponseType(typeof(Service))]
        [Route("api/ServiceData/ListServicesPage/{StartIndex}/{PerPage}")]
        public IHttpActionResult ListServicesPage(int StartIndex, int PerPage)
        {
            List<Service> Services = db.Services.OrderBy(a => a.ServiceId).Skip(StartIndex).Take(PerPage).ToList();

            return Ok(Services);
        }

        /// <summary>
        /// Returns all detail Services in the system when match the param Serviceid.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: An Service in the system matching up to the Service ID primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The primary key of the Service</param>
        /// <example>
        /// GET: api/ServiceData/FindService/5
        /// </example>
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
        /// <summary>
        /// Gathers information about Services related to a particular location
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Services in the database, including their associated location that match to a particular location id
        /// </returns>
        /// <param name="id">location ID.</param>
        /// <example>
        /// GET: api/ServiceData/ListServicesForLocation/1
        /// </example>
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
        /// <summary>
        /// Gathers information about Services is not related to a particular location
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Services in the database, including their is not at location that match to a particular location id
        /// </returns>
        /// <param name="id">Location ID.</param>
        /// <example>
        /// GET: api/LocationData/ListOfServicesNotAtThisLocation/1
        /// </example>
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


        /// <summary>
        /// Associates a particular Location with a particular Service
        /// </summary>
        /// <param name="Serviceid">The Service ID primary key</param>
        /// <param name="Location">The Location ID primary key</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST api/ServiceData/AssociateLocationWithService/9/1
        /// </example>
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

            //Debug.WriteLine("input service id is: " + serviceid);
            //Debug.WriteLine("selected service name is: " + SelectedService.ServiceName);
            //Debug.WriteLine("input location id is: " + locationid);
            //Debug.WriteLine("selected location name is: " + SelectedLocation.LocationName);

            SelectedService.Locations.Add(SelectedLocation);
            db.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Removes an association between a particular location and a particular Service
        /// </summary>
        /// <param name="Serviceid">The Service ID primary key</param>
        /// <param name="Locationid">The Location ID primary key</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST api/ServiceData/AssociateServiceWithLocation/9/1
        /// </example>
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

            //Debug.WriteLine("input service id is: " + serviceid);
            //Debug.WriteLine("selected service name is: " + SelectedService.ServiceName);
            //Debug.WriteLine("input location id is: " + locationid);
            //Debug.WriteLine("selected location name is: " + SelectedLocation.LocationName);


            SelectedService.Locations.Remove(SelectedLocation);
            db.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Updates a particular Service in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the Service ID primary key</param>
        /// <param name="Service">JSON FORM DATA of an Service</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/ServiceData/UpdateService/5
        /// FORM DATA: Service JSON Object
        /// </example>
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
        /// <summary>
        /// Adds an Service to the system
        /// </summary>
        /// <param name="Service">JSON FORM DATA of an Service</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: Service ID, Service Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/ServiceData/AddService
        /// FORM DATA: Service JSON Object
        /// </example>
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

        /// <summary>
        /// Deletes an Service from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the Service</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/ServiceData/DeleteService/5
        /// FORM DATA: (empty)
        /// </example>
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