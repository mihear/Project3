<<<<<<< HEAD
﻿using System;
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
using System.Data.SqlClient;

namespace Project3.Controllers.api
{
    public class TimesDriver
    {
        public Nullable<System.DateTime> ConfirmationTime { get; set; }
        public int DriverKey { get; set; }
        public Nullable<System.DateTime> DeliveredTime { get; set; }
        public Nullable<System.DateTime> PickedupTime { get; set; }
        public TimesDriver(TimesDriver fact)
        {
            this.ConfirmationTime = fact.ConfirmationTime;
            this.DeliveredTime = fact.DeliveredTime;
            this.PickedupTime = fact.PickedupTime;
        }
        public TimesDriver()
        {

        }
    }
    public class DriversCharts
    {
        public int id { get; set; }
        public string Name { get; set; }
        public List<TimesDriver> Times { get; set; }
        public DriversCharts(string name, int id, List<TimesDriver> facts)
        {
            this.id = id;
            this.Name = name;
            Times = new List<TimesDriver>();
            Times = facts;
            //foreach (var item in facts)
            //{
            //    Times.Add(new TimesDriver(item));
            //}
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
            //db.FactBills.Select(m => new TimesDriver()
            //{
            //    DeliveredTime = m.DeliveredTime,
            //    ConfirmationTime = m.ConfirmationTime,
            //    PickedupTime = m.PickedupTime,
            //    DriverKey = m.DriverKey
            //}).Where(m => m.DriverKey == item.DriverKey
            //&& m.ConfirmationTime != DateTime.MinValue &&
            //((DateTime)m.DeliveredTime) >= day && ((DateTime)m.DeliveredTime) <= day2).ToList()
            //var x = from m in db.FactBills
            //        where m.DriverKey == item.DriverKey && m.ConfirmationTime != DateTime.MinValue && ((DateTime)m.DeliveredTime) >= day && ((DateTime)m.DeliveredTime) <= day2
            //        select new TimesDriver()
            //        {
            //            DeliveredTime = m.DeliveredTime,
            //            ConfirmationTime = m.ConfirmationTime,
            //            PickedupTime = m.PickedupTime,
            //            DriverKey = m.DriverKey
            //        };
            List<DriversCharts> list = new List<DriversCharts>();
            var drivers = db.DimDrivers.Select(m => new
            {
                DriverKey = m.DriverKey,
                Name = m.Name
            }).ToList();
            var day = new DateTime(2019, 1, 6);
            var day2 = new DateTime(2019, 2, 7);
            //(DateTime.Compare(new DateTime(((DateTime)m.DeliveredTime).Year, ((DateTime)m.DeliveredTime).Month, ((DateTime)m.DeliveredTime).Day),    day) == 0)
            var date = db.Database
                .SqlQuery<TimesDriver>("select [ConfirmationTime],[PickedupTime],[DeliveredTime],[DriverKey] from [beeorderWH].[dbo].[FactBill] where PickedupTime >= '2019-01-06 10:47:54' and PickedupTime <= '2019-01-07 10:47:54'")
                .ToList();
            //db.Database
            //    .SqlQuery<TimesDriver>
            //    ("select [ConfirmationTime],[PickedupTime],[DeliveredTime],[DriverKey] from [beeorderWH].[dbo].[FactBill] with(index(chartIndex)) where [DriverKey]=@id and PickedupTime >= '2019-01-06 10:47:54' and PickedupTime <= '2019-01-07 10:47:54'"
            //    , new SqlParameter("@id", item.DriverKey)
            foreach (var item in drivers)
            {
                list.Add(new DriversCharts(item.Name, item.DriverKey, date.Where(m => m.DriverKey == item.DriverKey)
                .ToList()));
            }
            return Ok(list.Where(m => m.Times.Count() >= 1));
            //return Ok(date);

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
                var drivers = db.Database
                .SqlQuery<DimDriver>("select * from [beeorderWH].[dbo].[DimDriver]")
                .ToList();
                var date = db.Database
                    .SqlQuery<TimesDriver>("select [ConfirmationTime],[PickedupTime],[DeliveredTime],[DriverKey] from [beeorderWH].[dbo].[FactBill] where DeliveredTime >= @day and DeliveredTime <= @day2"
                 , new SqlParameter("@day", filter.from), new SqlParameter("@day2", filter.to)).ToList();
                foreach (var item in drivers)
                {
                    list.Add(new DriversCharts(item.Name, item.DriverKey, date.Where(m => m.DriverKey == item.DriverKey)
                 .ToList()));
                }
            }
            else
            {
                var item = db.DimDrivers.Select(m=>new
                {
                    DriverKey= m.DriverKey,
                    Name= m.Name
                }).FirstOrDefault(m => m.DriverKey == filter.id);
                if (item == null)
                    return NotFound();
                var date = db.Database
                .SqlQuery<TimesDriver>("select [ConfirmationTime],[PickedupTime],[DeliveredTime],[DriverKey] from [beeorderWH].[dbo].[FactBill] where DriverKey=@id and DeliveredTime >= @day and DeliveredTime <= @day2"
                , new SqlParameter("@id", item.DriverKey), new SqlParameter("@day", filter.from), new SqlParameter("@day2", filter.to)).ToList();
                list.Add(new DriversCharts(item.Name, item.DriverKey, date.Where(m => m.DriverKey == item.DriverKey)
                .ToList()));
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
        public IHttpActionResult driverOrde()
        {
            var list = new List<DriversOrder>();
            //var drivers = db.DimDrivers.Select(m => new
            //{
            //    DriverKey = m.DriverKey,
            //    Name = m.Name
            //}).ToList();
            var drivers = db.Database
                .SqlQuery<DimDriver>("select * from [beeorderWH].[dbo].[DimDriver]")
                .ToList();
            var day = new DateTime(2019, 1, 6);
            var day2 = new DateTime(2019, 2, 7);
            var date = db.Database
                .SqlQuery<TimesDriver>("select [ConfirmationTime],[PickedupTime],[DeliveredTime],[DriverKey] from [beeorderWH].[dbo].[FactBill] where DeliveredTime >= '2019-01-06 00:00:00' and DeliveredTime <= '2019-02-07 00:00:00'")
                .ToList();
            foreach (var item in drivers)
            {
                //var ordercancel = db.FactBills.Select(m => new TimesDriver
                //{
                //    DeliveredTime = m.DeliveredTime,
                //    ConfirmationTime = m.ConfirmationTime,
                //    PickedupTime = m.PickedupTime,
                //    DriverKey = m.DriverKey
                //}).Where(m => m.DriverKey == item.DriverKey && m.ConfirmationTime != DateTime.MinValue && ((DateTime)m.DeliveredTime) >= day && ((DateTime)m.DeliveredTime) <= day2).Count();

                //var order = db.FactBills.Select(m => new TimesDriver
                //{
                //    DeliveredTime = m.DeliveredTime,
                //    ConfirmationTime = m.ConfirmationTime,
                //    PickedupTime = m.PickedupTime,
                //    DriverKey = m.DriverKey
                //}).Where(m => m.DriverKey == item.DriverKey&&((DateTime)m.DeliveredTime) >= day && ((DateTime)m.DeliveredTime) <= day2).Count();
                //list.Add(new DriversOrder()
                //{
                //    allOrder = order,
                //    cancelOrder = ordercancel,
                //    Name = item.Name
                //});
                //var min = DateTime.MinValue == DateTime.Parse("0001-01-01 00:00:00.0000000");
                list.Add(new DriversOrder()
                {
                    allOrder = date.Where(m => m.DriverKey == item.DriverKey).Count(),
                    cancelOrder = date.Where(m => m.DriverKey == item.DriverKey && m.ConfirmationTime == DateTime.MinValue).Count(),
                    Name = item.Name
                });

            }
            return Ok(list.Where(m => m.allOrder >= 1).OrderBy(m => (m.allOrder - m.cancelOrder) / m.allOrder).Take(10).ToList());
            //return Ok(list);

        }
        [HttpPost]
        public IHttpActionResult driverOrder(DriverFilter filter)
        {
            if (filter == null || filter.from == null || filter.from == null)
            {
                return BadRequest();
            }
            if (filter.to == filter.from)
            {
                filter.to.AddDays(1);
            }
            var list = new List<DriversOrder>();
            if (filter.id == 0)
            {
                var drivers = db.Database
                 .SqlQuery<DimDriver>("select * from [beeorderWH].[dbo].[DimDriver]")
                 .ToList();
                var date = db.Database
                    .SqlQuery<TimesDriver>("select [ConfirmationTime],[PickedupTime],[DeliveredTime],[DriverKey] from [beeorderWH].[dbo].[FactBill] where DeliveredTime >= @day and DeliveredTime <= @day2"
                 , new SqlParameter("@day", filter.from), new SqlParameter("@day2", filter.to)).ToList();
                foreach (var item in drivers)
                {
                    list.Add(new DriversOrder()
                    {
                        allOrder = date.Where(m => m.DriverKey == item.DriverKey).Count(),
                        cancelOrder = date.Where(m => m.DriverKey == item.DriverKey && m.ConfirmationTime == DateTime.MinValue).Count(),
                        Name = item.Name
                    });

                }
            }
            else
            {
                var item = db.DimDrivers.Select(m => new
                {
                    DriverKey = m.DriverKey,
                    Name = m.Name
                }).FirstOrDefault(m => m.DriverKey == filter.id);
                if (item == null)
                    return NotFound();
                var date = db.Database
                 .SqlQuery<TimesDriver>("select [ConfirmationTime],[PickedupTime],[DeliveredTime],[DriverKey] from [beeorderWH].[dbo].[FactBill] where DriverKey=@id and DeliveredTime >= @day and DeliveredTime <= @day2"
                 , new SqlParameter("@id", item.DriverKey), new SqlParameter("@day", filter.from), new SqlParameter("@day2", filter.to)).ToList();
                list.Add(new DriversOrder()
                {
                    allOrder = date.Count(),
                    cancelOrder = date.Where(m => m.ConfirmationTime == DateTime.MinValue).Count(),
                    Name = item.Name
                });
            }
            return Ok(list.Where(m => m.allOrder >= 1).OrderBy(m => (m.allOrder - m.cancelOrder) / m.allOrder).Take(10).ToList());
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
=======
﻿using System;
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
                        (DateTime.Compare((DateTime)m.PickedupTime, filter.from) >= 0 && DateTime.Compare((DateTime)m.PickedupTime, filter.to) <= 0)).ToList()));
                }
            }
            else
            {
                var item = db.DimDrivers.FirstOrDefault(m => m.DriverKey == filter.id);
                if (item == null)
                    return NotFound();
                list.Add(new
                DriversCharts(item.Name, item.DriverKey, item.FactBills.Where(m =>
                (DateTime.Compare((DateTime)m.PickedupTime, filter.from) >= 0 && DateTime.Compare((DateTime)m.PickedupTime, filter.to) <= 0)).ToList()));

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
        //[HttpGet]
        //public IHttpActionResult driverOrder()
        //{
        //    var list = new List<DriversOrder>();
        //    foreach (var item in db.DimDrivers.ToList())
        //    {
        //        var ordercancel = item.FactBills.Where(m => m.ConfirmationTime == DateTime.MinValue &&
        //         (DateTime.Compare((DateTime)m.PickedupTime, DateTime.Parse("2017/2/12 00:00:00")) >= 0
        //         && DateTime.Compare((DateTime)m.PickedupTime, DateTime.Parse("2017/2/12 00:00:00")) <= 1)).Count();

        //        var order = item.FactBills.Where(m => (DateTime.Compare((DateTime)m.PickedupTime, DateTime.Parse("2017/2/12 00:00:00")) >= 0
        //         && DateTime.Compare((DateTime)m.PickedupTime, DateTime.Parse("2017/2/12 00:00:00")) <= 1)).Count();

        //        list.Add(new DriversOrder()
        //        {
        //            allOrder = order,
        //            cancelOrder = ordercancel,
        //            Name = item.Name
        //        });
        //    }
        //    return Ok(list.Where(m => m.allOrder >= 1).OrderBy(m => m.cancelOrder / m.allOrder).Take(10).ToList());

        //}
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
                    var ordercancel = item.FactBills.Where(m => m.ConfirmationTime == DateTime.MinValue &&
                     (DateTime.Compare((DateTime)m.PickedupTime, filter.from) >= 0
                     && DateTime.Compare((DateTime)m.PickedupTime, filter.to) <= 0)).Count();

                    var order = item.FactBills.Where(m => (DateTime.Compare((DateTime)m.PickedupTime, filter.from) >= 0
                     && DateTime.Compare((DateTime)m.PickedupTime, filter.to) <= 0)).Count();

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
                  && DateTime.Compare((DateTime)m.PickedupTime, filter.to) <=0)).Count();

                var order = item.FactBills.Where(m => (DateTime.Compare((DateTime)m.PickedupTime, filter.from) >= 0
                 && DateTime.Compare((DateTime)m.PickedupTime, filter.to) <= 0)).Count();

                list.Add(new DriversOrder()
                {
                    allOrder = order,
                    cancelOrder = ordercancel,
                    Name = item.Name
                });
            }
            return Ok(list.Where(m => m.allOrder >= 1).OrderBy(m => m.cancelOrder / m.allOrder).Take(10).ToList());
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
>>>>>>> 5fe50263b6ce9d1f41c37ca1e175be454ba6c888
}