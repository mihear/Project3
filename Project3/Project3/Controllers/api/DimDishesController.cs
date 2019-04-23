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
    public class DimDishesController : ApiController
    {
        private Entities db = new Entities();

        // GET: api/DimDishes
        public IHttpActionResult GetDimDishes()
        {
            return Ok(db.DimDishes.Where(t => t.DishPrice != 0).Select(m => new
            {
                name = m.DishName,
                data = m.DishPrice
            }).Take(20));
        }

        // GET: api/DimDishes/5
        [ResponseType(typeof(DimDish))]
        public IHttpActionResult GetDimDish(int id)
        {
            DimDish dimDish = db.DimDishes.Find(id);
            if (dimDish == null)
            {
                return NotFound();
            }

            return Ok(dimDish);
        }

        // PUT: api/DimDishes/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutDimDish(int id, DimDish dimDish)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != dimDish.DishKey)
            {
                return BadRequest();
            }

            db.Entry(dimDish).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DimDishExists(id))
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

        // POST: api/DimDishes
        [ResponseType(typeof(DimDish))]
        public IHttpActionResult PostDimDish(DimDish dimDish)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.DimDishes.Add(dimDish);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = dimDish.DishKey }, dimDish);
        }

        // DELETE: api/DimDishes/5
        [ResponseType(typeof(DimDish))]
        public IHttpActionResult DeleteDimDish(int id)
        {
            DimDish dimDish = db.DimDishes.Find(id);
            if (dimDish == null)
            {
                return NotFound();
            }

            db.DimDishes.Remove(dimDish);
            db.SaveChanges();

            return Ok(dimDish);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DimDishExists(int id)
        {
            return db.DimDishes.Count(e => e.DishKey == id) > 0;
        }
    }
}