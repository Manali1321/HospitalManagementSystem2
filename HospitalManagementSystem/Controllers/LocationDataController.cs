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

        // GET: api/LocationData/ListLocations
        [HttpGet]
        public IQueryable<Location> ListLocations()
        {
            return db.Locations;
        }

        /// <summary>
        /// Returns all locations in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all animals in the database, including their associated species.
        /// </returns>
        /// <example>
        /// GET: api/AnimalData/ListAnimals
        /// </example>
        [HttpGet]
        [ResponseType(typeof(Location))]
        [Route("api/LocationData/ListLocationsPage/{StartIndex}/{PerPage}")]
        public IHttpActionResult ListLocationsPage(int StartIndex, int PerPage)
        {
            List<Location> Locations = db.Locations.OrderBy(a => a.LocationId).Skip(StartIndex).Take(PerPage).ToList();
            return Ok(Locations);
        }

        // GET: api/LocationData/ListLocationsForDepartment
        [HttpGet]
        public IHttpActionResult ListLocationsForDepartment(int id)
        {
            List<Location> Locations = db.Locations.Where(
                  a => a.Departments.Any(
                      l => l.DepartmentId==id
                  )).ToList();


            return Ok(Locations);

        }

        // GET: api/LocationData/ListLocationsNotHavingThisDepartment
        [HttpGet]
        public IHttpActionResult ListLocationsNotHavingThisDepartment(int id)
        {
            List<Location> Locations = db.Locations.Where(
                  a => !a.Departments.Any(
                      l => l.DepartmentId==id
                  )).ToList();

            return Ok(Locations);

        }

        // GET: api/LocationData/ListLocationsForService
        [HttpGet]
        public IHttpActionResult ListLocationsForService(int id)
        {
            List<Location> Locations = db.Locations.Where(
                  a => a.Services.Any(
                      l => l.ServiceId==id
                  )).ToList();

            return Ok(Locations);

        }

        // GET: api/LocationData/ListLocationsNotHavingThisService
        [HttpGet]
        public IHttpActionResult ListLocationsNotHavingThisService(int id)
        {
            List<Location> Locations = db.Locations.Where(
                  a => !a.Services.Any(
                      l => l.ServiceId==id
                  )).ToList();

            return Ok(Locations);

        }



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

            Debug.WriteLine("input department id is: " + locationid);
            Debug.WriteLine("selected department name is: " + SelectedLocation.LocationName);
            Debug.WriteLine("input location id is: " + departmentid);
            Debug.WriteLine("selected location name is: " + SelectedDepartment.DepartmentName);



            SelectedLocation.Departments.Add(SelectedDepartment);
            db.SaveChanges();

            return Ok();
        }


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

            Debug.WriteLine("input department id is: " + locationid);
            Debug.WriteLine("selected department name is: " + SelectedLocation.LocationName);
            Debug.WriteLine("input location id is: " + departmentid);
            Debug.WriteLine("selected location name is: " + SelectedDepartment.DepartmentName);


            SelectedLocation.Departments.Remove(SelectedDepartment);
            db.SaveChanges();

            return Ok();
        }



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

            Debug.WriteLine("input department id is: " + locationid);
            Debug.WriteLine("selected department name is: " + SelectedLocation.LocationName);
            Debug.WriteLine("input location id is: " + serviceid);
            Debug.WriteLine("selected location name is: " + SelectedService.ServiceName);



            SelectedLocation.Services.Add(SelectedService);
            db.SaveChanges();

            return Ok();
        }


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

            Debug.WriteLine("input department id is: " + locationid);
            Debug.WriteLine("selected department name is: " + SelectedLocation.LocationName);
            Debug.WriteLine("input location id is: " + serviceid);
            Debug.WriteLine("selected location name is: " + SelectedService.ServiceName);


            SelectedLocation.Services.Remove(SelectedService);
            db.SaveChanges();

            return Ok();
        }




        // GET: api/LocationData/FindLocation/5
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

        // POST: api/LocationData/UpdateLocation/5
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

        // POST: api/LocationData/AddLocation
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

        // POST: api/LocationData/DeleteLocation/5
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
