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

    public class LocationDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all locations in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all location in the database.
        /// </returns>
        /// <example>
        // GET: api/LocationData/ListLocations
        /// </example>
        [HttpGet]
        public IQueryable<Location> ListLocations()
        {
            return db.Locations;
        }

        /// <summary>
        /// Gathers information about all location. pagination. One page can have 4 location list
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// </returns>
        /// <param name="StartIndex">id</param>
        /// <param name="PerPage">4</param>
        /// <example>
        /// GET: api/LocationData/ListLocationsPage/3/4
        /// </example>
        [HttpGet]
        [ResponseType(typeof(Location))]
        [Route("api/LocationData/ListLocationsPage/{StartIndex}/{PerPage}")]
        public IHttpActionResult ListLocationsPage(int StartIndex, int PerPage)
        {
            List<Location> Locations = db.Locations.OrderBy(a => a.LocationId).Skip(StartIndex).Take(PerPage).ToList();
            return Ok(Locations);
        }


        /// <summary>
        /// Gathers information about all department which connected with locationID
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// </returns>
        /// <param name="id">locationid</param>
        /// <example>
        // GET: api/LocationData/ListLocationsForDepartment
        /// </example>
        [HttpGet]
        public IHttpActionResult ListLocationsForDepartment(int id)
        {
            List<Location> Locations = db.Locations.Where(
                  a => a.Departments.Any(
                      l => l.DepartmentId==id
                  )).ToList();


            return Ok(Locations);

        }

        /// <summary>
        /// Gathers information about all department which is not connected with locationID
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// </returns>
        /// <param name="id">LocationId</param>
        /// <example>
        // GET: api/LocationData/ListLocationsNotHavingThisDepartment
        /// </example>
        [HttpGet]
        public IHttpActionResult ListLocationsNotHavingThisDepartment(int id)
        {
            List<Location> Locations = db.Locations.Where(
                  a => !a.Departments.Any(
                      l => l.DepartmentId==id
                  )).ToList();

            return Ok(Locations);

        }
        /// <summary>
        /// Gathers information about all location related to a particular service ID
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all location in the database, including their associated species matched with a particular service ID
        /// </returns>
        /// <param name="id">Service ID.</param>
        /// <example>
        // GET: api/LocationData/ListLocationsForService/3
        /// </example>
        [HttpGet]
        public IHttpActionResult ListLocationsForService(int id)
        {
            List<Location> Locations = db.Locations.Where(
                  a => a.Services.Any(
                      l => l.ServiceId==id
                  )).ToList();

            return Ok(Locations);

        }
        /// <summary>
        /// Gathers information about all location which is not related to a particular service ID
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// </returns>
        /// <param name="id">Service ID.</param>
        /// <example>
        // GET: api/LocationData/ListLocationsNotHavingThisService/3
        /// </example>
        [HttpGet]
        public IHttpActionResult ListLocationsNotHavingThisService(int id)
        {
            List<Location> Locations = db.Locations.Where(
                  a => !a.Services.Any(
                      l => l.ServiceId==id
                  )).ToList();

            return Ok(Locations);

        }

        /// <summary>
        /// Gathers information about department which is connected with particular location
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// </returns>
        /// <param name="locationid">Location ID.</param>
        /// <param name="departmentid">Department ID.</param>
        /// <example>
        // GET: api/LocationData/AssociateDepartmentWithLocation/locationid/departmentid
        /// </example>
        [HttpPost]
        [Route("api/LocationData/AssociateDepartmentWithLocation/{locationid}/{departmentid}")]
        public IHttpActionResult AssociateDepartmentWithLocation(int locationid, int departmentid)
        {

            Location SelectedLocation = db.Locations.Include(a => a.Departments).Where(a => a.LocationId==locationid).FirstOrDefault();
            Department SelectedDepartment = db.Departments.Find(departmentid);

            if (SelectedLocation == null || SelectedDepartment == null)
            {
                return NotFound();
            }

            //Debug.WriteLine("input department id is: " + locationid);
            //Debug.WriteLine("selected department name is: " + SelectedLocation.LocationName);
            //Debug.WriteLine("input location id is: " + departmentid);
            //Debug.WriteLine("selected location name is: " + SelectedDepartment.DepartmentName);



            SelectedLocation.Departments.Add(SelectedDepartment);
            db.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Gathers information about department which is not connected with particular location
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// </returns>
        /// <param name="locationid">Location ID.</param>
        /// <param name="departmentid">Department ID.</param>
        /// <example>
        // GET: api/LocationData/UnAssociateDepartmentWithLocation/locationid/departmentid
        /// </example>
        [HttpPost]
        [Route("api/LocationData/UnAssociateDepartmentWithLocation/{locationid}/{departmentid}")]
        public IHttpActionResult UnAssociateDepartmentWithLocation(int locationid, int departmentid)
        {

            Location SelectedLocation = db.Locations.Include(a => a.Departments).Where(a => a.LocationId == locationid).FirstOrDefault();
            Department SelectedDepartment = db.Departments.Find(departmentid);

            if (SelectedLocation == null || SelectedDepartment == null)
            {
                return NotFound();
            }

            //Debug.WriteLine("input department id is: " + locationid);
            //Debug.WriteLine("selected department name is: " + SelectedLocation.LocationName);
            //Debug.WriteLine("input location id is: " + departmentid);
            //Debug.WriteLine("selected location name is: " + SelectedDepartment.DepartmentName);


            SelectedLocation.Departments.Remove(SelectedDepartment);
            db.SaveChanges();

            return Ok();
        }


        /// <summary>
        /// Gathers information about service which is connected with particular location
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// </returns>
        /// <param name="locationid">Location ID.</param>
        /// <param name="serviceid">Service ID.</param>
        /// <example>
        // GET: api/LocationData/AssociateServiceWithLocation/locationid/departmentid
        /// </example>
        [HttpPost]
        [Route("api/LocationData/AssociateServiceWithLocation/{locationid}/{serviceid}")]
        public IHttpActionResult AssociateServiceWithLocation(int locationid, int serviceid)
        {

            Location SelectedLocation = db.Locations.Include(a => a.Services).Where(a => a.LocationId==locationid).FirstOrDefault();
            Service SelectedService = db.Services.Find(serviceid);

            if (SelectedLocation == null || SelectedService == null)
            {
                return NotFound();
            }

            //Debug.WriteLine("input department id is: " + locationid);
            //Debug.WriteLine("selected department name is: " + SelectedLocation.LocationName);
            //Debug.WriteLine("input location id is: " + serviceid);
            //Debug.WriteLine("selected location name is: " + SelectedService.ServiceName);



            SelectedLocation.Services.Add(SelectedService);
            db.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Gathers information about service which is not connected with particular location
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// </returns>
        /// <param name="locationid">Location ID.</param>
        /// <param name="serviceid">Service ID.</param>
        /// <example>
        // GET: api/LocationData/UnAssociateServiceWithLocation/locationid/servceid
        /// </example>
        [HttpPost]
        [Route("api/LocationData/UnAssociateServiceWithLocation/{locationid}/{serviceid}")]
        public IHttpActionResult UnAssociateServiceWithLocation(int locationid, int serviceid)
        {

            Location SelectedLocation = db.Locations.Include(a => a.Services).Where(a => a.LocationId == locationid).FirstOrDefault();
            Service SelectedService = db.Services.Find(serviceid);

            if (SelectedLocation == null || SelectedService == null)
            {
                return NotFound();
            }

            //Debug.WriteLine("input department id is: " + locationid);
            //Debug.WriteLine("selected department name is: " + SelectedLocation.LocationName);
            //Debug.WriteLine("input location id is: " + serviceid);
            //Debug.WriteLine("selected location name is: " + SelectedService.ServiceName);


            SelectedLocation.Services.Remove(SelectedService);
            db.SaveChanges();

            return Ok();
        }


        /// <summary>
        /// Returns all details of particular location in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: An location in the system matching up to the location ID primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The primary key of the location</param>
        /// <example>
        // GET: api/LocationData/FindLocation/5
        /// </example>
        [ResponseType(typeof(Location))]
        [HttpGet]
        public IHttpActionResult FindLocation(int id)
        {
            Location location = db.Locations.Find(id);
            if (location == null)
            {
                return NotFound();
            }

            return Ok(location);
        }

        /// <summary>
        /// Updates a particular location in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the location ID primary key</param>
        /// <param name="location">JSON FORM DATA of an location</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        // POST: api/LocationData/UpdateLocation/5
        /// FORM DATA: Location JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateLocation(int id, Location location)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != location.LocationId)
            {
                return BadRequest();
            }

            db.Entry(location).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LocationExists(id))
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
        /// Adds an location to the system
        /// </summary>
        /// <param name="location">JSON FORM DATA of an location</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: Location ID, Location Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        // POST: api/LocationData/AddLocation
        /// FORM DATA: Location JSON Object
        /// </example>
        [ResponseType(typeof(Location))]
        [HttpPost]
        public IHttpActionResult AddLocation(Location location)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Locations.Add(location);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = location.LocationId }, location);
        }
        /// <summary>
        /// Deletes an location from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the location</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        // POST: api/LocationData/DeleteLocation/5
        /// FORM DATA: (empty)
        /// </example>
        [ResponseType(typeof(Location))]
        [HttpPost]
        public IHttpActionResult DeleteLocation(int id)
        {
            Location location = db.Locations.Find(id);
            if (location == null)
            {
                return NotFound();
            }

            db.Locations.Remove(location);
            db.SaveChanges();

            return Ok(location);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LocationExists(int id)
        {
            return db.Locations.Count(e => e.LocationId == id) > 0;
        }
    }
}
