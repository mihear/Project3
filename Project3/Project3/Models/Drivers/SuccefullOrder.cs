using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project3.Models.Drivers
{
    public class SuccefullOrder
    {
        public int NumberOfOrder { get; set; }
        public int WorkHours { get; set; }
        public double AV { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}