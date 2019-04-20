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
    public class TimesDriver
    {
        public Nullable<System.DateTime> ConfirmationTime { get; set; }
        public Nullable<System.DateTime> DeliveredTime { get; set; }
        public Nullable<System.DateTime> PickedupTime { get; set; }
        public TimesDriver(FactBill fact)
        {
            this.ConfirmationTime = fact.ConfirmationTime;
            this.DeliveredTime = fact.DeliveredTime;
            this.PickedupTime = fact.PickedupTime;
        }
    }
    public class DriversCharts
    {
        public int id { get; set; }
        public string Name { get; set; }
        public List<TimesDriver> Times { get; set; }
        public DriversCharts(string name, int id, List<FactBill> facts)
        {
            this.id = id;
            this.Name = name;
            Times = new List<TimesDriver>();
            foreach (var item in facts)
            {
                Times.Add(new TimesDriver(item));
            }
        }
    }
    public class DriverFilter
    {
        public DateTime from { get; set; }
        public DateTime to { get; set; }
        public int id { get; set; }
    }
    public class DriversType
    {
        public int Size { get; set; }
        public string Type { get; set; }
    }
    public class DriversOrder
    {
        public string Name { get; set; }
        public int allOrder { get; set; }
        public int cancelOrder { get; set; }
    }
    public class DimDriversController : ApiController
    {
        private Entities db = new Entities();
        public IHttpActionResult getAllDrivers()
        {
            return Ok(db.DimDrivers.Select(m => new
            {
                id = m.DriverKey,
                nameof = m.Name
            }).ToList());
        }
        public IHttpActionResult getTotalDrivers()
        {
            return Ok(db.DimDrivers.Count());
        }
        public IHttpActionResult getTotalProviderDrivers()
        {
            return Ok(db.DimDrivers.Where(m => m.Provider == "beeorder").Count());
        }
        // GET: api/DimDrivers
        public IHttpActionResult GetDriver()
        {
            List<DriversCharts> list = new List<DriversCharts>();
            foreach (var item in db.DimDrivers.ToList())
            {
                list.Add(new DriversCharts(item.Name, item.DriverKey, item.FactBills.Where(m =>
                 (DateTime.Compare(new DateTime(((DateTime)m.DeliveredTime).Year, ((DateTime)m.DeliveredTime).Month, ((DateTime)m.DeliveredTime).Day),
                 new DateTime(2017, 12, 2)) == 0)).ToList()));
            }
            return Ok(list.Where(m => m.Times.Count() >= 1).ToList());

        }
        [HttpPost]
        [ResponseType(typeof(DimDriver))]
        // GET: api/DimDrivers
        public IHttpActionResult GetDrivers(DriverFilter filter)
        {
            if (filter.from == null || filter.to == null)
            {
                return BadRequest();
            }
            List<DriversCharts> list = new List<DriversCharts>();
            if (filter.id == 0)
            {
                foreach (var item in db.DimDrivers.ToList())
                {
                    list.Add(new
                        DriversCharts(item.Name, item.DriverKey, item.FactBills.Where(m => m.ConfirmationTime != DateTime.MinValue &&
                        (DateTime.Compare((DateTime)m.PickedupTime, filter.from) >= 0 && DateTime.Compare((DateTime)m.PickedupTime, filter.to) <= 1)).ToList()));
                }
            }
            else
            {
                var item = db.DimDrivers.FirstOrDefault(m => m.DriverKey == filter.id);
                if (item == null)
                    return NotFound();
                list.Add(new
                DriversCharts(item.Name, item.DriverKey, item.FactBills.Where(m =>
                (DateTime.Compare((DateTime)m.PickedupTime, filter.from) >= 0 && DateTime.Compare((DateTime)m.PickedupTime, filter.to) <= 1)).ToList()));

            }
            return Ok(list.Where(m => m.Times.Count() > 0));

        }
        [HttpGet]
        //[Route("api/DriverType")]
        // GET: api/DimDrivers/5
        [ResponseType(typeof(DimDriver))]
        public IHttpActionResult DriverType()
        {
            List<DriversType> list = new List<DriversType>();
            //list.Add(new DriversType()
            //{
            //    Size = db.DimDrivers.GroupBy(m => m.VehicleType).Count(),
            //});
            var test = db.DimDrivers.GroupBy(m => m.VehicleType).Select(x => new { name = x.FirstOrDefault().VehicleType, y = x.Count() });

            return Ok(test);
        }
        [HttpGet]
        public IHttpActionResult driverOrder()
        {
            var list = new List<DriversOrder>();
            foreach (var item in db.DimDrivers.ToList())
            {
                var ordercancel = item.FactBills.Where(m => m.ConfirmationTime != DateTime.MinValue &&
                 (DateTime.Compare((DateTime)m.PickedupTime, DateTime.Parse("2017/2/12 00:00:00")) >= 0
                 && DateTime.Compare((DateTime)m.PickedupTime, DateTime.Parse("2017/2/12 00:00:00")) <= 1)).Count();

                var order = item.FactBills.Where(m => (DateTime.Compare((DateTime)m.PickedupTime, DateTime.Parse("2017/2/12 00:00:00")) >= 0
                 && DateTime.Compare((DateTime)m.PickedupTime, DateTime.Parse("2017/2/12 00:00:00")) <= 1)).Count();

                list.Add(new DriversOrder()
                {
                    allOrder = order,
                    cancelOrder = ordercancel,
                    Name = item.Name
                });
            }
            return Ok(list.Where(m => m.allOrder >= 1).ToList());

        }
        [HttpPost]
        public IHttpActionResult driverOrder(DriverFilter filter)
        {
            if (filter == null || filter.from == null || filter.from == null)
            {
                return BadRequest();
            }
            var list = new List<DriversOrder>();
            if (filter.id == 0)
            {
                foreach (var item in db.DimDrivers.ToList())
                {
                    var ordercancel = item.FactBills.Where(m => m.ConfirmationTime != DateTime.MinValue &&
                     (DateTime.Compare((DateTime)m.PickedupTime, filter.from) >= 0
                     && DateTime.Compare((DateTime)m.PickedupTime, filter.to) <= 1)).Count();

                    var order = item.FactBills.Where(m => (DateTime.Compare((DateTime)m.PickedupTime, filter.from) >= 0
                     && DateTime.Compare((DateTime)m.PickedupTime, filter.to) <= 1)).Count();

                    list.Add(new DriversOrder()
                    {
                        allOrder = order,
                        cancelOrder = ordercancel,
                        Name = item.Name
                    });
                }
            }
            else
            {
                var item = db.DimDrivers.FirstOrDefault(m => m.DriverKey == filter.id);
                if (item == null)
                    return NotFound();
                var ordercancel = item.FactBills.Where(m => m.ConfirmationTime != DateTime.MinValue &&
                  (DateTime.Compare((DateTime)m.PickedupTime, filter.from) >= 0
                  && DateTime.Compare((DateTime)m.PickedupTime, filter.to) <= 1)).Count();

                var order = item.FactBills.Where(m => (DateTime.Compare((DateTime)m.PickedupTime, filter.from) >= 0
                 && DateTime.Compare((DateTime)m.PickedupTime, filter.to) <= 1)).Count();

                list.Add(new DriversOrder()
                {
                    allOrder = order,
                    cancelOrder = ordercancel,
                    Name = item.Name
                });
            }
            return Ok(list.Where(m => m.allOrder >= 1));
        }
        // PUT: api/DimDrivers/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutDimDriver(int id, DimDriver dimDriver)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != dimDriver.DriverKey)
            {
                return BadRequest();
            }

            db.Entry(dimDriver).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DimDriverExists(id))
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

        // POST: api/DimDrivers
        [ResponseType(typeof(DimDriver))]
        public IHttpActionResult PostDimDriver(DimDriver dimDriver)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.DimDrivers.Add(dimDriver);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = dimDriver.DriverKey }, dimDriver);
        }

        // DELETE: api/DimDrivers/5
        [ResponseType(typeof(DimDriver))]
        public IHttpActionResult DeleteDimDriver(int id)
        {
            DimDriver dimDriver = db.DimDrivers.Find(id);
            if (dimDriver == null)
            {
                return NotFound();
            }

            db.DimDrivers.Remove(dimDriver);
            db.SaveChanges();

            return Ok(dimDriver);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DimDriverExists(int id)
        {
            return db.DimDrivers.Count(e => e.DriverKey == id) > 0;
        }
    }
}