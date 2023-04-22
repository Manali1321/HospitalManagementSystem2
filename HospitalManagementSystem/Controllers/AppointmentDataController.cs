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
    public class AppointmentDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        /// <summary>
        /// List all Appointments available in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: List of appointments in the system.
        /// </returns>
        /// <example>
       /// GET: api/AppointmentData/ListAppointments
        /// </example>
        [HttpGet]
        public IEnumerable<AppointmentDto> ListAppointments()
        {
            List<Appointment> Appointments = db.Appointments.ToList();
            List<AppointmentDto> AppointmentDtos = new List<AppointmentDto>();

            Appointments.ForEach(c => AppointmentDtos.Add(new AppointmentDto()
            {
                AppointmentId = c.AppointmentId,
                PatientName = c.PatientName,
                ConsultingHours = c.ConsultingHours,
                DoctorFName = c.Doctor.DoctorFName,
                DoctorLName = c.Doctor.DoctorLName
            }));

            return AppointmentDtos;
        }

        /// <summary>
        /// Find Appointment in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: Find appointment in the system.
        /// </returns>
        /// <example>
        ///GET: api/AppointmentData/FindAppointment/5
        /// </example>     
        [ResponseType(typeof(Appointment))]
        [HttpGet]
        public IHttpActionResult FindAppointment(int id)
        {
            Appointment Appointment = db.Appointments.Find(id);
            AppointmentDto AppointmentDto = new AppointmentDto()
            {
                AppointmentId = Appointment.AppointmentId,
                PatientName = Appointment.PatientName,
                ConsultingHours = Appointment.ConsultingHours,
                DoctorFName = Appointment.Doctor.DoctorFName,
                DoctorLName = Appointment.Doctor.DoctorLName
            };
            if (Appointment == null)
            {
                return NotFound();
            }

            return Ok(AppointmentDto);
        }

        /// <summary>
        /// Update Appointment in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: Update Appointment in the system.
        /// </returns>
        /// <example>
        /// POST: api/AppointmentData/UpdateDoctor/5
        /// </example>
        /// 
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateAppointment(int id, Appointment appointment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != appointment.AppointmentId)
            {
                return BadRequest();
            }

            db.Entry(appointment).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppointmentExists(id))
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
        /// Add Appointment in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: Add Appointment in the system.
        /// </returns>
        /// <example>
        /// POST: api/AppointmentData/AddAppointment
        /// </example>

        [ResponseType(typeof(Appointment))]
        [HttpPost]

        public IHttpActionResult AddAppointment(Appointment appointment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Appointments.Add(appointment);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = appointment.AppointmentId }, appointment);
        }

        

        /// <summary>
        /// Delete Appointment in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: Delete Appointment in the system.
        /// </returns>
        /// <example>
        /// POST: api/AppointmentData/DeleteAppointment/5
        /// </example>
        [ResponseType(typeof(Appointment))]
        [HttpPost]
        public IHttpActionResult DeleteAppointment(int id)
        {
            Appointment appointment = db.Appointments.Find(id);
            if (appointment == null)
            {
                return NotFound();
            }

            db.Appointments.Remove(appointment);
            db.SaveChanges();

            return Ok(appointment);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AppointmentExists(int id)
        {
            return db.Appointments.Count(e => e.AppointmentId == id) > 0;
        }
    }
}