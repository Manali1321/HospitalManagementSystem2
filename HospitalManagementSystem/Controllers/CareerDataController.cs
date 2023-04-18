using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using HospitalManagementSystem.Migrations;
using HospitalManagementSystem.Models;

namespace HospitalManagementSystem.Controllers
{
    public class CareerDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        /// <summary>
        /// Returns all list in the Careers.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all career in the database, including their associated department and location.
        /// </returns>
        /// <example>
        /// GET: api/CareerData/ListCareers
        /// </example>
        [HttpGet]
        public IEnumerable<CareerDto> ListCareers()
        {

            List<Career> Careers = db.Careers.ToList();
            List<CareerDto> CareerDtos = new List<CareerDto>();

            Careers.ForEach(c => CareerDtos.Add(new CareerDto()
            {
                CareerId = c.CareerId,
                JobName = c.JobName,
                JobId = c.JobId,
                JobDescription = c.JobDescription,
                DepartmentId = c.Department.DepartmentId,
                LocationId = c.Location.LocationId,

            }));

            return CareerDtos;
        }

        /// <summary>
        /// Returns all values from career table
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: An career in the system matching up to the career ID primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The primary key of the career</param>
        /// <example>
        /// GET: api/CareerData/FindCareer/5
        /// </example>
        [ResponseType(typeof(Career))]
        [HttpGet]
        public IHttpActionResult FindCareer(int id)
        {
            Career Career = db.Careers.Find(id);
            CareerDto CareerDto = new CareerDto()
            {
                CareerId = Career.CareerId,
                JobName = Career.JobName,
                JobId = Career.JobId,
                JobDescription = Career.JobDescription,
                DepartmentId = Career.Department.DepartmentId,
                LocationId = Career.Location.LocationId

            };
            if (Career == null)
            {
                return NotFound();
            }

            return Ok(CareerDto);
        }
        /// <summary>
        /// Updates a particular career in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the career ID primary key</param>
        /// <param name="career">JSON FORM DATA of an career</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// FORM DATA: Career JSON Object
        /// POST: api/CareerData/UpdateCareer/5
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateCareer(int id, Career career)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != career.CareerId)
            {
                return BadRequest();
            }

            db.Entry(career).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CareerExists(id))
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
        /// Adds an career to the system
        /// </summary>
        /// <param name="career">JSON FORM DATA of an career</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: Career ID, Career Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// FORM DATA: Animal JSON Object
        /// POST: api/CareerData/AddCareer
        /// </example>
        [ResponseType(typeof(Career))]
        [HttpPost]
        public IHttpActionResult AddCareer(Career career)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Careers.Add(career);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = career.CareerId }, career);
        }

        /// <summary>
        /// Deletes an career from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the career</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/CareerData/DeleteCareer/5
        /// FORM DATA: (empty)
        /// </example>
        [ResponseType(typeof(Career))]
        [HttpPost]
        public IHttpActionResult DeleteCareer(int id)
        {
            Career career = db.Careers.Find(id);
            if (career == null)
            {
                return NotFound();
            }

            db.Careers.Remove(career);
            db.SaveChanges();

            return Ok(career);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CareerExists(int id)
        {
            return db.Careers.Count(e => e.CareerId == id) > 0;
        }
    }
}
