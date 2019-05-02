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
    public class DimDishesController : ApiController
    {
        private Entities db;
        public DimDishesController()
        {
            db = new Entities();
        }
        [HttpGet]
        public IHttpActionResult getAllDish()
        {
            return Ok(db.DimDishes.Select(m => new
            {
                id = m.DishAltKey,
                nameof = m.DishEngName
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
                    .SqlQuery<OrderRate>(" select TOP (10) count(distinct [BillKey]) as Count ,[DimDish].[DishEngName]as Name   from [FactBill] inner join[DimDish] on" +
                     "[FactBill].[DishKey] = [DimDish].[DishAltKey] where [FactBill].[OpenTime] >= @day and [FactBill].[OpenTime] <= @day2" +
                     " group by [FactBill].[DishKey],[DimDish].[DishEngName] order by  count(distinct[BillKey]) desc"
                 , new SqlParameter("@day", filter.from), new SqlParameter("@day2", filter.to)).ToList();
                return Ok(date);
            }
            else
            {
                var date = db.Database
                     .SqlQuery<OrderRate>(" select TOP (10) count(distinct [BillKey]) as Count ,[DimDish].[DishEngName] as Name  from [FactBill] inner join[DimDish] on" +
                     "[FactBill].[DishKey] = [DimDish].[DishAltKey] where [FactBill].[DishKey]=@id and [FactBill].[OpenTime] >= @day and[FactBill].[OpenTime] <= @day2" +
                     " group by[FactBill].[DishKey],[DimDish].[DishEngName] order by  count(distinct[BillKey]) desc"
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
                    .SqlQuery<OrderRate>(" select TOP (10) count(distinct [BillKey]) as Count ,[DimDish].[DishEngName] as Name  from [FactBill] inner join[DimDish] on" +
                     "[FactBill].[DishKey] = [DimDish].[DishAltKey] where [FactBill].[OpenTime] >= @day and[FactBill].[OpenTime] <= @day2" +
                     " group by[FactBill].[DishKey],[DimDish].[DishEngName] order by  count(distinct[BillKey]) "
                 , new SqlParameter("@day", filter.from), new SqlParameter("@day2", filter.to)).ToList();
                return Ok(date);
            }
            else
            {
                var date = db.Database
                     .SqlQuery<OrderRate>("  select TOP (10) count(distinct [BillKey]) as Count ,[DimDish].[DishEngName] as Name   from [FactBill] inner join[DimDish] on" +
                     "[FactBill].[DishKey] = [DimDish].[DishAltKey] where [FactBill].[DishKey]=@id and [FactBill].[OpenTime] >= @day and[FactBill].[OpenTime] <= @day2" +
                     " group by[FactBill].[DishKey],[DimDish].[DishEngName] order by  count(distinct[BillKey])"
                  , new SqlParameter("@id", filter.id), new SqlParameter("@day", filter.from), new SqlParameter("@day2", filter.to)).ToList();
                return Ok(date);
            }
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