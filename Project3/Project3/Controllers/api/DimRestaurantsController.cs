using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Project3.Models;
using Project3.Models.Filters;
using Project3.Models.Restaurants;
using System.Data.SqlClient;

namespace Project3.Controllers.api
{
    public class DimRestaurantsController : ApiController
    {
        private Entities db;
        public DimRestaurantsController()
        {
            db = new Entities();
        }
        [HttpGet]
        public IHttpActionResult getAllRestaurants()
        {
            return Ok(db.DimRestaurants.Select(m => new
            {
                id = m.RestaurantKey,
                nameof = m.Name
            }).ToList());
        }
        [HttpPost]
        public IHttpActionResult OrderRateH(TimeFilter filter)
        {
            if (filter.from == null || filter.to == null)
            {
                return BadRequest();
            }
            List<OrderRate> list = new List<OrderRate>();
            if (filter.id == 0)
            {
                var date = db.Database
                    .SqlQuery<OrderRate>(" select TOP (10) count(distinct [BillKey])  as Count,DimRestaurant.[Name]  from [FactBill] inner join[DimRestaurant] on [FactBill].[ResturentKey] = [DimRestaurant].[RestaurantKey]" +
                    "where [FactBill].[OpenTime] >=@day and [FactBill].[OpenTime]<= @day2 group by[ResturentKey],[DimRestaurant].[Name]" +
                    "order by  count(distinct[BillKey]) desc"
                 , new SqlParameter("@day", filter.from), new SqlParameter("@day2", filter.to)).ToList();
                return Ok(date);
            }
            else
            {
                var date = db.Database
                     .SqlQuery<OrderRate>(" select TOP (10) count(distinct [BillKey]) as Count,DimRestaurant.[Name]  from [FactBill] inner join[DimRestaurant] on [FactBill].[ResturentKey] = [DimRestaurant].[RestaurantKey]" +
                     "where [FactBill].[ResturentKey]=@id and [FactBill].[OpenTime] >=@day and [FactBill].[OpenTime]<= @day2 group by[ResturentKey],[DimRestaurant].[Name]" +
                     "order by  count(distinct[BillKey]) desc"
                  , new SqlParameter("@id", filter.id), new SqlParameter("@day", filter.from), new SqlParameter("@day2", filter.to)).ToList();
                return Ok(date);
            }
        }
        [HttpPost]
        public IHttpActionResult OrderRateL(TimeFilter filter)
        {
            if (filter.from == null || filter.to == null)
            {
                return BadRequest();
            }
            List<OrderRate> list = new List<OrderRate>();
            if (filter.id == 0)
            {
                var date = db.Database
                    .SqlQuery<OrderRate>(" select TOP (10) count(distinct [BillKey])  as Count,DimRestaurant.[Name]  from [FactBill] inner join[DimRestaurant] on [FactBill].[ResturentKey] = [DimRestaurant].[RestaurantKey]" +
                    "where [FactBill].[OpenTime] >=@day and [FactBill].[OpenTime]<= @day2 group by[ResturentKey],[DimRestaurant].[Name]" +
                    "order by  count(distinct[BillKey])"
                 , new SqlParameter("@day", filter.from), new SqlParameter("@day2", filter.to)).ToList();
                return Ok(date);
            }
            else
            {
                var date = db.Database
                     .SqlQuery<OrderRate>(" select TOP (10) count(distinct [BillKey])  as Count,DimRestaurant.[Name]  from [FactBill] inner join[DimRestaurant] on [FactBill].[ResturentKey] = [DimRestaurant].[RestaurantKey]" +
                     "where [FactBill].[ResturentKey]=@id and [FactBill].[OpenTime] >=@day and [FactBill].[OpenTime]<= @day2 group by[ResturentKey],[DimRestaurant].[Name]" +
                     "order by  count(distinct[BillKey])"
                  , new SqlParameter("@id", filter.id), new SqlParameter("@day", filter.from), new SqlParameter("@day2", filter.to)).ToList();
                return Ok(date);
            }
        }
        [HttpPost]
        public IHttpActionResult OnOffOrderTop(TimeFilter filter)
        {
            if (filter.from == null || filter.to == null)
            {
                return BadRequest();
            }
            List<OnOffOrder> list = new List<OnOffOrder>();
            if (filter.id == 0)
            {
                list = db.Database
                    .SqlQuery<OnOffOrder>(" select TOP (10)  count(distinct [BillKey]) as 'On' ,DimRestaurant.[Name] from [FactBill]  inner hash   join [DimRestaurant] " +
                    "on [DimRestaurant].RestaurantKey = [FactBill].[ResturentKey] " +
                    "where [FactBill].[OpenTime] >= @day and [FactBill].[OpenTime] <=  @day2 and(DATEPART(HOUR,[DimRestaurant].StartDelivery) <= DATEPART(HOUR,[FactBill].[OpenTime]) and DATEPART(HOUR,[FactBill].[OpenTime]) <= DATEPART(HOUR,[DimRestaurant].EndDelivery))" +
                    "group by[ResturentKey],[DimRestaurant].[Name]" +
                    "order by[ResturentKey]"
                 , new SqlParameter("@day", filter.from), new SqlParameter("@day2", filter.to)).ToList();
                var date = db.Database
                   .SqlQuery<int>(" select TOP (10)  count(distinct [BillKey]) as 'Off' from [FactBill]  inner hash   join [DimRestaurant] " +
                   "on[DimRestaurant].RestaurantKey = [FactBill].[ResturentKey] Where [FactBill].[OpenTime] >= @day and[FactBill].[OpenTime] <=  @day2 and (DATEPART(HOUR,[DimRestaurant].StartDelivery) > DATEPART(HOUR,[FactBill].[OpenTime]) or DATEPART(HOUR,[FactBill].[OpenTime]) > DATEPART(HOUR,[DimRestaurant].EndDelivery))" +
                   "group by[ResturentKey],[DimRestaurant].[Name]" +
                   "order by[ResturentKey]"
                , new SqlParameter("@day", filter.from), new SqlParameter("@day2", filter.to)).ToList();
                int i = 0;
                foreach (var item in list)
                {
                    item.Off = date[i++];
                }
                return Ok(list);
            }
            return BadRequest();
        }
        [HttpPost]
        public IHttpActionResult OnOffOrder(TimeFilter filter)
        {
            if (filter.from == null || filter.to == null)
            {
                return BadRequest();
            }
            List<OnOffOrder> list = new List<OnOffOrder>();
            if (filter.id == 0)
            {
                list = db.Database
                    .SqlQuery<OnOffOrder>(" select count(distinct [BillKey]) as 'On' ,DimRestaurant.[Name] from [FactBill]  inner hash   join [DimRestaurant] " +
                    "on [DimRestaurant].RestaurantKey = [FactBill].[ResturentKey] " +
                    "where [FactBill].[OpenTime] >= @day and [FactBill].[OpenTime] <=  @day2 and(DATEPART(HOUR,[DimRestaurant].StartDelivery) <= DATEPART(HOUR,[FactBill].[OpenTime]) and DATEPART(HOUR,[FactBill].[OpenTime]) <= DATEPART(HOUR,[DimRestaurant].EndDelivery))" +
                    "group by[ResturentKey],[DimRestaurant].[Name]" +
                    "order by[ResturentKey]"
                 , new SqlParameter("@day", filter.from), new SqlParameter("@day2", filter.to)).ToList();
                var date = db.Database
                   .SqlQuery<int>(" select count(distinct [BillKey]) as 'Off' from [FactBill]  inner hash   join [DimRestaurant] " +
                   "on[DimRestaurant].RestaurantKey = [FactBill].[ResturentKey] Where [FactBill].[OpenTime] >= @day and[FactBill].[OpenTime] <=  @day2 and (DATEPART(HOUR,[DimRestaurant].StartDelivery) > DATEPART(HOUR,[FactBill].[OpenTime]) or DATEPART(HOUR,[FactBill].[OpenTime]) > DATEPART(HOUR,[DimRestaurant].EndDelivery))" +
                   "group by[ResturentKey],[DimRestaurant].[Name]" +
                   "order by[ResturentKey]"
                , new SqlParameter("@day", filter.from), new SqlParameter("@day2", filter.to)).ToList();
                int i = 0;
                foreach (var item in list)
                {
                    item.Off = date[i++];
                }
                return Ok(list);
            }
            return BadRequest();
        }
        [HttpGet]
        public IHttpActionResult RestTypeOrder()
        {
            var list = db.Database
                 .SqlQuery<OrderRate>("select count(distinct[BillKey]) as count ,[DimRestaurant].RestaurantType as name from [FactBill]   inner hash   join [DimRestaurant] " +
                 "on [DimRestaurant].RestaurantKey = [FactBill].[ResturentKey]" +
                 //"where [FactBill].[OpenTime] >= @day and [FactBill].[OpenTime] <= @day2  "+
                 "group by[DimRestaurant].RestaurantType"
              //, new SqlParameter("@day", filter.from), new SqlParameter("@day2", filter.to)
              ).ToList();
            return Ok(list);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
