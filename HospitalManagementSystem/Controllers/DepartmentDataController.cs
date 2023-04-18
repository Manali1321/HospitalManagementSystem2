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

    public class DepartmentDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/DepartmentData/ListDepartments
        [HttpGet]
        public IQueryable<Department> ListDepartments()
        {
            return db.Departments;
        }


        /// <summary>
        /// Returns all departments in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all departments in the database.
        /// </returns>
        /// <example>
        /// GET: api/departmentData/Listdepartments
        /// </example>
        [HttpGet]
        [ResponseType(typeof(Department))]
        [Route("api/DepartmentData/ListDepartmentsPage/{StartIndex}/{PerPage}")]
        public IHttpActionResult ListDepartmentsPage(int StartIndex, int PerPage)
        {
            List<Department> Departments = db.Departments.OrderBy(a => a.DepartmentId).Skip(StartIndex).Take(PerPage).ToList();


            return Ok(Departments);
        }

        /// <summary>
        /// Gathers information about departments related to a particular location
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all department in the database, including their associated location matched with a particular Department ID
        /// </returns>
        /// <param name="id">Location ID.</param>
        /// <example>
        /// GET: api/DepartmentData/ListDepartmentForLocation/3
        /// </example>        
        [ResponseType(typeof(Department))]
        [HttpGet]
        public IHttpActionResult ListDepartmentsForLocation(int id)
        {

            List<Department> Departments = db.Departments.Where(
                  a => a.Locations.Any(
                      l => l.LocationId==id
                  )).ToList();


            return Ok(Departments);
        }
        /// <summary>
        /// Gathers information about departments related to a particular location
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all department in the database, including their not associated with location matched with a particular Department ID
        /// </returns>
        /// <param name="id">Location ID.</param>
        /// <example>
        /// GET: api/DepartmentData/ListDepartmentForLocation/3
        /// </example>  
        /// <summary>
        [ResponseType(typeof(Department))]
        [HttpGet]
        public IHttpActionResult ListOfDepartmentsNotAtThisLocation(int id)
        {

            List<Department> Departments = db.Departments.Where(
                  a => !a.Locations.Any(
                      l => l.LocationId==id
                  )).ToList();


            return Ok(Departments);
        }

        /// <summary>
        /// Gathers information about location related to a particular department
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// </returns>
        /// <param name="DepartmentId">Department ID.</param>
        /// <param name="LocationId">Location ID.</param>
        /// <example>
        /// GET: api/DepartmentData/AssociateLocationWithDepartment/{1}/{2}
        /// </example>        
        [HttpPost]
        [Route("api/DepartmentData/AssociateLocationWithDepartment/{departmentid}/{locationid}")]
        public IHttpActionResult AssociateLocationWithDepartment(int departmentid, int locationid)
        {

            Department SelectedDepartment = db.Departments.Include(a => a.Locations).Where(a => a.DepartmentId==departmentid).FirstOrDefault();
            Location SelectedLocation = db.Locations.Find(locationid);

            if (SelectedDepartment==null || SelectedLocation == null)
            {
                return NotFound();
            }

            //Debug.WriteLine("input department id is: " + departmentid);
            //Debug.WriteLine("selected department name is: "+ SelectedDepartment.DepartmentName);
            //Debug.WriteLine("input location id is: " + locationid);
            //Debug.WriteLine("selected location name is: " + SelectedLocation.LocationName);


            SelectedDepartment.Locations.Add(SelectedLocation);
            db.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Gathers information about location which is not assosiated with department
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// </returns>
        /// <param name="DepartmentId">Department ID.</param>
        /// <param name="LocationId">Location ID.</param>
        /// <example>
        /// GET: api/DepartmentData/AssociateLocationWithDepartment/{1}/{2}
        /// </example> 
        [HttpPost]
        [Route("api/DepartmentData/UnAssociateLocationWithDepartment/{departmentid}/{locationid}")]
        public IHttpActionResult UnAssociateLocationWithDepartment(int departmentid, int locationid)
        {

            Department SelectedDepartment = db.Departments.Include(a => a.Locations).Where(a => a.DepartmentId == departmentid).FirstOrDefault();
            Location SelectedLocation = db.Locations.Find(locationid);

            if (SelectedDepartment == null || SelectedLocation == null)
            {
                return NotFound();
            }

            //Debug.WriteLine("input department id is: " + departmentid);
            //Debug.WriteLine("selected department name is: " + SelectedDepartment.DepartmentName);
            //Debug.WriteLine("input location id is: " + locationid);
            //Debug.WriteLine("selected location name is: " + SelectedLocation.LocationName);


            SelectedDepartment.Locations.Remove(SelectedLocation);
            db.SaveChanges();

            return Ok();
        }


        /// <summary>
        /// Returns all Department in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: An Department in the system matching up to the Department ID primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The primary key of the Department</param>
        /// <example>
        // GET: api/DepartmentData/FindDepartment/5
        /// </example>
        [ResponseType(typeof(Department))]
        [HttpGet]
        public IHttpActionResult FindDepartment(int id)
        {
            Department department = db.Departments.Find(id);
            if (department == null)
            {
                return NotFound();
            }

            return Ok(department);
        }

        /// <summary>
        /// Updates a particular Department in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the Department ID primary key</param>
        /// <param name="department">JSON FORM DATA of an Department</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        // POST: api/DepartmentData/UpdateDepartment/5
        /// FORM DATA: Department JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateDepartment(int id, Department department)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != department.DepartmentId)
            {
                return BadRequest();
            }

            db.Entry(department).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DepartmentExists(id))
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
        /// Adds an department to the system
        /// </summary>
        /// <param name="department">JSON FORM DATA of an Department</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: Department ID, Department Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/DepartmentData/AddDepartment
        /// FORM DATA: Animal JSON Object
        /// </example>
        // POST: api/DepartmentData/AddDepartment
        [ResponseType(typeof(Department))]
        [HttpPost]
        public IHttpActionResult AddDepartment(Department department)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Departments.Add(department);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = department.DepartmentId }, department);
        }
        /// <summary>
        /// Deletes an department from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the department</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/DepartmentData/DeleteDepartment/5
        /// FORM DATA: (empty)
        /// </example>
        // DELETE: api/DepartmentData/DeleteDepartment/5
        [ResponseType(typeof(Department))]
        [HttpPost]
        public IHttpActionResult DeleteDepartment(int id)
        {
            Department department = db.Departments.Find(id);
            if (department == null)
            {
                return NotFound();
            }

            db.Departments.Remove(department);
            db.SaveChanges();

            return Ok(department);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DepartmentExists(int id)
        {
            return db.Departments.Count(e => e.DepartmentId == id) > 0;
        }
    }
}