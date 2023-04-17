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
    public class DonationsDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// List all donations available in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: List of donations in the system.
        /// </returns>
        /// <example>
        /// GET: api/DonationsData/ListDonations
        /// </example>
        [HttpGet]
        public IQueryable<Donation> ListDonations()
        {
            return db.Donations;
        }

        /// <summary>
        /// Find donation in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: Find donation in the system.
        /// </returns>
        /// <example>
        /// GET: api/DonationsData/FindDonation/5
        /// </example>
        [ResponseType(typeof(Donation))]
        [HttpGet]
        public IHttpActionResult FindDonation(int id)
        {
            Donation donation = db.Donations.Find(id);
            if (donation == null)
            {
                return NotFound();
            }

            return Ok(donation);
        }

        /// <summary>
        /// Update donations in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: Update donations in the system.
        /// </returns>
        /// <example>
        /// PUT: api/DonationsData/UpdateDonation/5
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateDonation(int id, Donation donation)
        {
            Debug.WriteLine("I have reached the update donation method.");
            if (!ModelState.IsValid)
            {
                Debug.WriteLine("Model state is invalid");
                return BadRequest(ModelState);
            }

            if (id != donation.donationID)
            {
                Debug.WriteLine("ID mismatched");
                return BadRequest();
            }

            db.Entry(donation).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DonationExists(id))
                {
                    Debug.WriteLine("Donation does not exist");
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
        /// Add donation in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: Add donation in the system.
        /// </returns>
        /// <example>
        /// POST: api/DonationsData/AddDonation
        /// </example>
        [ResponseType(typeof(Donation))]
        [HttpPost]
        public IHttpActionResult AddDonation(Donation donation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Donations.Add(donation);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = donation.donationID }, donation);
        }

        /// <summary>
        /// Delete Donation in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: Delete donation in the system.
        /// </returns>
        /// <example>
        /// DELETE: api/DonationsData/DeleteDonation/5
        /// </example>
        [ResponseType(typeof(Donation))]
        [HttpPost]
        public IHttpActionResult DeleteDonation(int id)
        {
            Donation donation = db.Donations.Find(id);
            if (donation == null)
            {
                return NotFound();
            }

            db.Donations.Remove(donation);
            db.SaveChanges();

            return Ok(donation);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DonationExists(int id)
        {
            return db.Donations.Count(e => e.donationID == id) > 0;
        }
    }
}