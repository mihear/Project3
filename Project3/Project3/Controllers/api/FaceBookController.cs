using Project3.Models;
using Project3.Models.Restaurants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Project3.Controllers.api
{
    public class PostActivtity
    {
        public string Post { get; set; }
        public Double Count { get; set; }
    }
    public class FaceBookController : ApiController
    {

        private Entities db = new Entities();
        // POST: api/FaceBook
        [HttpPost]
        public IHttpActionResult TopEffectingUser()
        {
            var date = db.Database
               .SqlQuery<OrderRate>("select top (10) Name,COUNT(Id)  as Count from FacebookPosts group by Name order by Count desc")
               .ToList();
            return Ok(date);
        }
        [HttpPost]
        public IHttpActionResult TopResturnt()
        {
            var date = db.Database
               .SqlQuery<OrderRate>("select top (10) Resturant as Name,COUNT(Id)  as Count from FacebookPosts group by Resturant order by Count desc")
               .ToList();
            date.Add(new OrderRate()
            {
                Name = "Design",
                Count = 0
            });
            return Ok(date.Where(m => m.Name!=""));
        }
        [HttpPost]
        public IHttpActionResult TopActivityPost()
        {
            var date = db.Database
                .SqlQuery<PostActivtity>("select top(10) Post ,(case when IsPositive in(1) then  1 when IsPositive in(0) then  -1 end  *  Cast(Postivity as float) *(case when RIGHT([Like], 1) in ('K','k') then CAST(Left([Like], len([Like]) -1)  as float) *1000 " +
                " when RIGHT([Like], 1) in ('M','m') then CAST(Left([Like], len([Like]) -1) as float)  *1000000 " +
                " else cast([Like] as float )  end +case when RIGHT(Love,1) in ('K','k') then CAST(Left(Love, len(Love) - 1) as float) * 1000 " +
                " when RIGHT(Love, 1) in ('M', 'm') then CAST(Left(Love, len(Love) - 1) as float) * 1000000 " +
                " else cast(Love as float)  end - case when RIGHT(Haha, 1) in ('K', 'k') then CAST(Left(Haha, len(Haha) - 1) as float) * 1000 " +
                " when RIGHT(Haha, 1) in ('M', 'm') then CAST(Left(Haha, len(Haha) - 1) as float) * 1000000 " +
                " else cast(Haha as float)  end +case when RIGHT(Wow, 1) in ('K', 'k') then CAST(Left(Wow, len(Wow) - 1) as float) * 1000 " +
                " when RIGHT(Wow, 1) in ('M', 'm') then CAST(Left(Wow, len(Wow) - 1) as float) * 1000000 else cast(Wow as float)  end - " +
                " case when RIGHT(Angry, 1) in ('K', 'k') then CAST(Left(Angry, len(Angry) - 1) as float) * 1000 " +
                " when RIGHT(Angry, 1) in ('M', 'm') then CAST(Left(Angry, len(Angry) - 1) as float) * 1000000  else cast(Angry as float)  end - " +
                " case when RIGHT(Sad, 1) in ('K', 'k') then CAST(Left(Sad, len(Sad) - 1) as float) * 1000 " +
                " when RIGHT(Sad, 1) in ('M', 'm') then CAST(Left(Sad, len(Sad) - 1) as float) * 1000000 " +
                " else cast(Sad as float)  end)) as Count from FacebookPosts order by Count desc")
                .ToList();
            return Ok(date);
        }
        [HttpPost]
        public IHttpActionResult LessActivityPost()
        {
            var date = db.Database
                 .SqlQuery<PostActivtity>("select top(10) Post ,(case when IsPositive in(1) then  1 when IsPositive in(0) then  -1 end  *  Cast(Postivity as float) *(case when RIGHT([Like], 1) in ('K','k') then CAST(Left([Like], len([Like]) -1)  as float) *1000 " +
                 " when RIGHT([Like], 1) in ('M','m') then CAST(Left([Like], len([Like]) -1) as float)  *1000000 " +
                 " else cast([Like] as float )  end +case when RIGHT(Love,1) in ('K','k') then CAST(Left(Love, len(Love) - 1) as float) * 1000 " +
                 " when RIGHT(Love, 1) in ('M', 'm') then CAST(Left(Love, len(Love) - 1) as float) * 1000000 " +
                 " else cast(Love as float)  end - case when RIGHT(Haha, 1) in ('K', 'k') then CAST(Left(Haha, len(Haha) - 1) as float) * 1000 " +
                 " when RIGHT(Haha, 1) in ('M', 'm') then CAST(Left(Haha, len(Haha) - 1) as float) * 1000000 " +
                 " else cast(Haha as float)  end +case when RIGHT(Wow, 1) in ('K', 'k') then CAST(Left(Wow, len(Wow) - 1) as float) * 1000 " +
                 " when RIGHT(Wow, 1) in ('M', 'm') then CAST(Left(Wow, len(Wow) - 1) as float) * 1000000 else cast(Wow as float)  end - " +
                 " case when RIGHT(Angry, 1) in ('K', 'k') then CAST(Left(Angry, len(Angry) - 1) as float) * 1000 " +
                 " when RIGHT(Angry, 1) in ('M', 'm') then CAST(Left(Angry, len(Angry) - 1) as float) * 1000000  else cast(Angry as float)  end - " +
                 " case when RIGHT(Sad, 1) in ('K', 'k') then CAST(Left(Sad, len(Sad) - 1) as float) * 1000 " +
                 " when RIGHT(Sad, 1) in ('M', 'm') then CAST(Left(Sad, len(Sad) - 1) as float) * 1000000 " +
                 " else cast(Sad as float)  end)) as Count from FacebookPosts order by Count")
               .ToList();
            return Ok(date);
        }
        [HttpPost]
        public IHttpActionResult Positive()
        {
            var date = db.FacebookPosts.Where(m => m.IsPositive == "1").Count();
            return Ok(date);
        }
        [HttpPost]
        public IHttpActionResult Negative()
        {
            var date = db.FacebookPosts.Where(m => m.IsPositive == "0").Count();
            return Ok(date);
        }

    }
}
