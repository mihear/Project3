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
using Project3.Models.Filters;
using Project3.Models.Drivers;

using System.Data.SqlClient;

namespace Project3.Controllers.api
{
    public class DimDriversController : ApiController
    {
        private Entities db = new Entities();

        //report 1 
        [HttpPost]
        [ResponseType(typeof(DimDriver))]
        public IHttpActionResult DriverType(TimeFilter filter)
        {
            List<DriversType> list = new List<DriversType>();

            if (filter == null || filter.from == null || filter.from == null)
            {
                return BadRequest();
            }
            var date = db.Database
               .SqlQuery<DriversType>("select  count(distinct[BillKey]) as 'Size' , [DimDriver].VehicleType as 'Type' " +
               " from  [FactBill] inner hash join [DimDriver] on [DimDriver].DriverKey = [FactBill].DriverKey " +
               " where [FactBill].[OpenTime] >= @day and [FactBill].[OpenTime] <= @day2 " +
               " group by [DimDriver].VehicleType"
                 , new SqlParameter("@day", filter.from), new SqlParameter("@day2", filter.to))
               .ToList();
            return Ok(date);
            //list.Add(new DriversType()
            //{
            //    Size = db.DimDrivers.GroupBy(m => m.VehicleType).Count(),
            //});
            //var test = db.DimDrivers.GroupBy(m => m.VehicleType).Select(x => new { name = x.FirstOrDefault().VehicleType, y = x.Count() });
            //return Ok(test);
        }

        //report 2
        [HttpPost]
        [ResponseType(typeof(DimDriver))]
        public IHttpActionResult SuccefulOrderDriver(TimeFilter filter)
        {
            List<DriversType> list = new List<DriversType>();

            if (filter == null || filter.from == null || filter.id == 0)
            {
                return BadRequest();
            }
            var date = db.Database
               .SqlQuery<SuccefullOrder>("DECLARE @FirstDate AS datetime " +
               " DECLARE @SecondDate AS datetime " + " DECLARE @AllDate AS int " + " SET @FirstDate = (select top(1) PickedupTime from FactBill where [DriverKey] = @id and  OpenTime >= @day and OpenTime <= @day2 and PickedupTime != '0001-01-01 00:00:00.0000000'order by[FactBill].BillKey)" +
               "SET @SecondDate = (select  top(1) CloseTime from FactBill where [DriverKey] =@id and OpenTime>= @day and OpenTime<= @day2 and PickedupTime != '0001-01-01 00:00:00.0000000' order by[FactBill].BillKey DESC )" +
               "SET @AllDate = (select COUNT(distinct BillKey) from FactBill where [DriverKey] =@id and OpenTime>= @day and OpenTime<= @day2 and PickedupTime != '0001-01-01 00:00:00.0000000')" +
               "select @AllDate as NumberOfOrder , DATEDIFF(MINUTE, @FirstDate, @SecondDate) as WorkHours,@FirstDate as 'Start' ,@SecondDate as 'End'"
                 , new SqlParameter("@id", filter.id), new SqlParameter("@day", filter.from), new SqlParameter("@day2", filter.from.AddDays(1))).FirstOrDefault();
            if (date != null)
            {
                date.AV = date.NumberOfOrder / (date.WorkHours / 60.0);
            }
            return Ok(date);
        }
        //report 3
        [HttpPost]
        [ResponseType(typeof(DimDriver))]
        public IHttpActionResult FreeActive(TimeFilter filter)
        {
            List<DriversType> list = new List<DriversType>();

            if (filter == null || filter.from == null || filter.id == 0)
            {
                return BadRequest();
            }
            var date = db.Database
               .SqlQuery<FreeActive>("DECLARE @FirstDate AS datetime " +
               " DECLARE @SecondDate AS datetime " + " DECLARE @AllDate AS int " +
               " SET @FirstDate = (select top(1) PickedupTime from FactBill where [DriverKey] = @id and  OpenTime >= @day and OpenTime <= @day2 and PickedupTime != '0001-01-01 00:00:00.0000000'order by[FactBill].BillKey)" +
               "SET @SecondDate = (select  top(1) CloseTime from FactBill where [DriverKey] =@id and OpenTime>= @day and OpenTime<= @day2 and PickedupTime != '0001-01-01 00:00:00.0000000' order by[FactBill].BillKey DESC )" +
               "SET @AllDate = (select sum(DATEDIFF(MINUTE, PickedupTime, CloseTime))  from (select distinct FactBill.BillKey, PickedupTime, CloseTime from FactBill where [DriverKey] =@id and OpenTime>= @day and OpenTime<= @day2 and PickedupTime != '0001-01-01 00:00:00.0000000') as p)" +
               "select @AllDate as Active,DATEDIFF(MINUTE, @FirstDate, @SecondDate) - @AllDate as Free ,DATEDIFF(MINUTE, @FirstDate, @SecondDate) as 'All' "
                 , new SqlParameter("@id", filter.id), new SqlParameter("@day", filter.from), new SqlParameter("@day2", filter.from.AddDays(1))).FirstOrDefault();
            return Ok(date);
        }
        //report 4
        [HttpPost]
        [ResponseType(typeof(DimDriver))]
        public IHttpActionResult AcceptReject(TimeFilter filter)
        {

            if (filter == null || filter.from == null)
            {
                return BadRequest();
            }
            var date = db.Database
               .SqlQuery<RejectOrder>("select  d.DriverKey,d.Name,COUNT(distinct[BillKey])  as Reject  from FactBill f  inner hash join DimDriver d on  f.DriverKey =d.DriverKey " +
                    " where  f.ConfirmationTime = '0001-01-01 00:00:00.0000000' and OpenTime >= @day and OpenTime <= @day2 " +
                    " group by d.DriverKey, d.Name ",
                    new SqlParameter("@day", filter.from), new SqlParameter("@day2", filter.from.AddDays(1))).ToList();
            var date2 = db.Database
              .SqlQuery<RejectOrder>("select  d.DriverKey,d.Name,COUNT(distinct[BillKey])  as Accepted  from FactBill  f inner hash join DimDriver d on  f.DriverKey =d.DriverKey " +
                   " where  f.ConfirmationTime != '0001-01-01 00:00:00.0000000' and OpenTime >= @day and OpenTime <= @day2 " +
                   " group by d.DriverKey, d.Name ",
                   new SqlParameter("@day", filter.from), new SqlParameter("@day2", filter.from.AddDays(1))).ToList();
            date.AddRange(date2);
            List<RejectOrder> list = new List<RejectOrder>();
            foreach (var item in date)
            {
                var temp = date.FindLast(m => m.DriverKey == item.DriverKey);
                if (temp != null)
                {
                    item.Accepted = temp.Accepted;
                }
                if (list.FirstOrDefault(m => m.DriverKey == item.DriverKey) == null)
                    list.Add(item);
            }
            return Ok(list);
          
        }
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
        public IHttpActionResult GetDrivers(TimeFilter filter)
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
                    .SqlQuery<TimesDriver>("select  distinct [BillKey],[OpenTime],[DispatchTime],[CloseTime], [ConfirmationTime],[PickedupTime],[DeliveredTime],[DriverKey]" +
                    " from [beeorderWH].[dbo].[FactBill] where OpenTime >= @day and OpenTime <= @day2 and PickedupTime !='0001-01-01 00:00:00.0000000'"
                 , new SqlParameter("@day", filter.from), new SqlParameter("@day2", filter.to)).ToList();
                foreach (var item in drivers)
                {
                    list.Add(new DriversCharts(item.Name, item.DriverKey, date.Where(m => m.DriverKey == item.DriverKey)
                 .ToList()));
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
                .SqlQuery<TimesDriver>("select  distinct [BillKey],[OpenTime],[DispatchTime],[CloseTime], [ConfirmationTime],[PickedupTime],[DeliveredTime],[DriverKey]" +
                " from [beeorderWH].[dbo].[FactBill] where DriverKey=@id and OpenTime >= @day and OpenTime <= @day2 and PickedupTime !='0001-01-01 00:00:00.0000000'"
                , new SqlParameter("@id", item.DriverKey), new SqlParameter("@day", filter.from), new SqlParameter("@day2", filter.to)).ToList();
                list.Add(new DriversCharts(item.Name, item.DriverKey, date.Where(m => m.DriverKey == item.DriverKey)
                .ToList()));
            }
            return Ok(list.Where(m => m.Times.Count() > 0));

        }

        public IHttpActionResult getAvailable()
        {
            return Ok(db.DimDrivers.Where(x => x.Available == 1).Count());
        }
        public IHttpActionResult getActive()
        {
            return Ok(db.DimDrivers.Where(x => x.Active == 1).Count());
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
                .SqlQuery<TimesDriver>("select  distinct [BillKey], [ConfirmationTime],[PickedupTime],[DeliveredTime],[DriverKey] " +
                "from [beeorderWH].[dbo].[FactBill] where DeliveredTime >= '2019-01-06 00:00:00' and DeliveredTime <= '2019-02-07 00:00:00'")
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
        public IHttpActionResult driverOrder(TimeFilter filter)
        {
            if (filter == null || filter.to == null || filter.from == null)
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
                    .SqlQuery<TimesDriver>("select  distinct [BillKey], [ConfirmationTime],[PickedupTime],[DeliveredTime],[DriverKey] " +
                    "from [beeorderWH].[dbo].[FactBill] where DeliveredTime >= @day and DeliveredTime <= @day2"
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
                 .SqlQuery<TimesDriver>("select  distinct [BillKey], [ConfirmationTime],[PickedupTime],[DeliveredTime],[DriverKey] " +
                 "from [beeorderWH].[dbo].[FactBill] where DriverKey=@id and DeliveredTime >= @day and DeliveredTime <= @day2"
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
}