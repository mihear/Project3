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
using Project3.Models;

namespace Project3.Controllers.api
{
    public class DimUsersController : ApiController
    {
        private Entities db = new Entities();

        // GET: api/DimUsers
        public IHttpActionResult GetDimUsers()
        {
            return Ok(db.DimUsers.GroupBy(m => m.AreaEngName).Select(x => new { name = x.FirstOrDefault().AreaEngName, y = x.Count() }));
        }

        // GET: api/DimUsers/5
        [ResponseType(typeof(DimUser))]
        public IHttpActionResult GetDimUser(int id)
        {
            DimUser dimUser = db.DimUsers.Find(id);
            if (dimUser == null)
            {
                return NotFound();
            }

            return Ok(dimUser);
        }

        // PUT: api/DimUsers/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutDimUser(int id, DimUser dimUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != dimUser.UserKey)
            {
                return BadRequest();
            }

            db.Entry(dimUser).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DimUserExists(id))
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

        // POST: api/DimUsers
        [ResponseType(typeof(DimUser))]
        public IHttpActionResult PostDimUser(DimUser dimUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.DimUsers.Add(dimUser);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = dimUser.UserKey }, dimUser);
        }

        // DELETE: api/DimUsers/5
        [ResponseType(typeof(DimUser))]
        public IHttpActionResult DeleteDimUser(int id)
        {
            DimUser dimUser = db.DimUsers.Find(id);
            if (dimUser == null)
            {
                return NotFound();
            }

            db.DimUsers.Remove(dimUser);
            db.SaveChanges();

            return Ok(dimUser);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DimUserExists(int id)
        {
            return db.DimUsers.Count(e => e.UserKey == id) > 0;
        }
    }
}