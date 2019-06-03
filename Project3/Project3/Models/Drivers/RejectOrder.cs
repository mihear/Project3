using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project3.Models.Drivers
{
    public class RejectOrder
    {
        public int DriverKey { get; set; }
        public int Reject { get; set; }
        public string Name { get; set; }
        public int Accepted { get; set; }

    }
}