using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project3.Models.Filters
{
    public class TimeFilter
    {
        public DateTime from { get; set; }
        public DateTime to { get; set; }
        public int id { get; set; }
    }
}