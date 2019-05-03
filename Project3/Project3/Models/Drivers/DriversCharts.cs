using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project3.Models.Drivers
{
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
        }
    }
}