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
using HospitalManagementSystem.Models;

namespace HospitalManagementSystem.Controllers
{
    public class DoctorDataController : ApiController
    {

        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/DoctorData/ListDoctors

        /// <summary>
        /// List all Doctors available in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: List of Doctors in the system.
        /// </returns>
        /// <example>
          /// GET: api/DoctorData/ListDoctors
        /// </example>
        [HttpGet]
        public IEnumerable<DoctorDto> ListDoctors()
        {
            List<Doctor> Doctors = db.Doctors.ToList();
            List<DoctorDto> DoctorDtos = new List<DoctorDto>();

            Doctors.ForEach(c => DoctorDtos.Add(new DoctorDto()
            {
                DoctorId = c.DoctorId,
                DoctorFName = c.DoctorFName,
                DoctorLName = c.DoctorLName,
                Speciality = c.Speciality,
                Email = c.Email,
                ContactNumber = c.ContactNumber,
                DepartmentName = c.Department.DepartmentName
            }));

            return DoctorDtos;
        }

   

        /// <summary>
        /// Find Doctor in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: Find doctor in the system.
        /// </returns>
        /// <example>
        /// GET: api/DoctorData/FindDoctor/5
        /// </example>    
        [ResponseType(typeof(Doctor))]
        [HttpGet]
        public IHttpActionResult FindDoctor(int id)
        {
            Doctor Doctor = db.Doctors.Find(id);
            DoctorDto DoctorDto = new DoctorDto()
            {
                DoctorId = Doctor.DoctorId,
                DoctorFName = Doctor.DoctorFName,
                DoctorLName = Doctor.DoctorLName,
                Speciality = Doctor.Speciality,
                Email = Doctor.Email,
                ContactNumber = Doctor.ContactNumber,
                DepartmentName = Doctor.Department.DepartmentName
            };

            if (Doctor == null)
            {
                return NotFound();
            }

            return Ok(DoctorDto);
        }

        // POST: api/DoctorData/UpdateDoctor/5

        /// <summary>
        /// Update Doctor in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: Update  in the Doctor system.
        /// </returns>
        /// <example>
        /// POST: api/DoctorData/UpdateDoctor/5
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateDoctor(int id, Doctor doctor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != doctor.DoctorId)
            {
                return BadRequest();
            }

            db.Entry(doctor).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DoctorExists(id))
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
        /// Add Doctor in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: Add Dcotor in the system.
        /// </returns>
        /// <example>
        /// POST: api/DoctorData/AddDoctor
        /// </example>
        [ResponseType(typeof(Doctor))]
        [HttpPost]
        public IHttpActionResult AddDoctor(Doctor doctor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Doctors.Add(doctor);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = doctor.DoctorId }, doctor);
        }


        /// <summary>
        /// Delete a Doctor in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: Delete Doctor in the system.
        /// </returns>
        /// <example>
        /// POST: api/DoctorData/DeleteDoctor/5
        /// </example>

    
        [ResponseType(typeof(Doctor))]
        [HttpPost]
        public IHttpActionResult DeleteDoctor(int id)
        {
            Doctor doctor = db.Doctors.Find(id);
            if (doctor == null)
            {
                return NotFound();
            }

            db.Doctors.Remove(doctor);
            db.SaveChanges();

            return Ok(doctor);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DoctorExists(int id)
        {
            return db.Doctors.Count(e => e.DoctorId == id) > 0;
        }
    }
}