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
using Project3.Models.Filters;
using Project3.Models.Restaurants;
using System.Data.SqlClient;

namespace Project3.Controllers.api
{
    public class DimUsersController : ApiController
    {
        private Entities db;
        public DimUsersController()
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
        public IHttpActionResult UserRateH(TimeFilter filter)
        {
            if (filter.from == null || filter.to == null)
            {
                return BadRequest();
            }
            List<OrderRate> list = new List<OrderRate>();
            if (filter.id == 0)
            {
                var date = db.Database
                    .SqlQuery<OrderRate>("select distinct Top(10) [DimUser].Name  ,count(distinct[BillKey]) as count , [DimUser].UserAltKey " +
                    " from [FactBill]  inner hash join [DimUser] on [DimUser].UserAltKey = [FactBill].UserKey " +
                    " where [FactBill].[OpenTime] >= @day and[FactBill].[OpenTime] <= @day2 " +
                    " group by[DimUser].UserAltKey, [DimUser].Name " +
                    " order by count desc"
                 , new SqlParameter("@day", filter.from), new SqlParameter("@day2", filter.to)).ToList();
                return Ok(date);
            }
            return BadRequest();
        }
        [HttpPost]
        public IHttpActionResult UserRateL(TimeFilter filter)
        {
            if (filter.from == null || filter.to == null)
            {
                return BadRequest();
            }
            List<OrderRate> list = new List<OrderRate>();
            if (filter.id == 0)
            {
                var date = db.Database
                    .SqlQuery<OrderRate>(" select distinct Top(10) [DimUser].Name  ,count(distinct[BillKey]) as count , [DimUser].UserAltKey " +
                    " from[FactBill]   inner hash join[DimUser] on[DimUser].UserAltKey = [FactBill].UserKey " +
                    " where[FactBill].[OpenTime] >= @day and[FactBill].[OpenTime] <= @day2 " +
                    " group by[DimUser].UserAltKey, [DimUser].Name " +
                    " order by count "
                 , new SqlParameter("@day", filter.from), new SqlParameter("@day2", filter.to)).ToList();
                return Ok(date);
            }
            return BadRequest();
        }
        [HttpPost]
        public IHttpActionResult AreaOrderH(TimeFilter filter)
        {
            if (filter.from == null || filter.to == null)
            {
                return BadRequest();
            }
            List<OrderRate> list = new List<OrderRate>();
            if (filter.id == 0)
            {
                var date = db.Database
                    .SqlQuery<OrderRate>(" select  top(10) count(distinct[BillKey]) as count , [DimUser].AreaEngName as Name " +
                    " from[FactBill]   inner hash join[DimUser] on[DimUser].UserAltKey = [FactBill].UserKey " +
                    "  where[FactBill].[OpenTime] >= @day and[FactBill].[OpenTime] <= @day2 " +
                    " group by[DimUser].AreaEngName " +
                    "order by count desc"
                 , new SqlParameter("@day", filter.from), new SqlParameter("@day2", filter.to)).ToList();
                return Ok(date);
            }
            return BadRequest();
        }
        [HttpPost]
        public IHttpActionResult AreaOrderL(TimeFilter filter)
        {
            if (filter.from == null || filter.to == null)
            {
                return BadRequest();
            }
            List<OrderRate> list = new List<OrderRate>();
            if (filter.id == 0)
            {
                var date = db.Database
                    .SqlQuery<OrderRate>(" select  top(10) count(distinct[BillKey]) as count , [DimUser].AreaEngName as Name " +
                    " from[FactBill]   inner hash join[DimUser] on[DimUser].UserAltKey = [FactBill].UserKey " +
                    "  where[FactBill].[OpenTime] >= @day and[FactBill].[OpenTime] <= @day2 " +
                    " group by[DimUser].AreaEngName " +
                    "order by count "
                 , new SqlParameter("@day", filter.from), new SqlParameter("@day2", filter.to)).ToList();
                return Ok(date);
            }
            return BadRequest();
        }

        public IHttpActionResult UserAreaH()
        {
            List<OrderRate> list = new List<OrderRate>();
            var date = db.Database
                .SqlQuery<OrderRate>(" select top(10) count([DimUser].[UserAltKey]) as count ,[DimUser].AreaEngName as Name from [DimUser] " +
                " group by[DimUser].AreaEngName order by count desc").ToList();
            return Ok(date);
        }
        public IHttpActionResult UserAreaL()
        {
            List<OrderRate> list = new List<OrderRate>();
            var date = db.Database
                .SqlQuery<OrderRate>(" select top(10) count([DimUser].[UserAltKey]) as count ,[DimUser].AreaEngName as Name from [DimUser] " +
                " group by[DimUser].AreaEngName order by count").ToList();
            return Ok(date);
        }
        //Data Mining 
        [HttpGet]
        public IHttpActionResult UserForMining()
        {
            var date = db.Database
                .SqlQuery<UserMining>("select distinct UDM.id , d.name from DimUser d inner join UserDataMining UDM on d.[UserAltKey] = UDM.id").ToList();
            return Ok(date);
        }
        public IHttpActionResult UserDataMining(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var date = db.Database
                .SqlQuery<UserDataMining>("select * from UserDataMining UDM where id=@id", new SqlParameter("@id", id)).ToList();
            List<DataMining> list = new List<DataMining>();
            foreach(var item in date)
            {
                list.Add(new DataMining()
                {
                    id = item.id,
                    Count = item.Conut,
                    Date = item.Date.ToShortDateString(),
                    prediction = item.prediction
                });
            }
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